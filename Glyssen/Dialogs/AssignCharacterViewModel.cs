﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using DesktopAnalytics;
using Glyssen.Character;
using Glyssen.Shared;
using Glyssen.Shared.Bundle;
using Glyssen.Utilities;
using L10NSharp;
using SIL.Scripture;
using ScrVers = Paratext.ScrVers;

namespace Glyssen.Dialogs
{
	public class AssignCharacterViewModel : BlockNavigatorViewModel
	{
		#region Data members and events
		private readonly ProjectCharacterVerseData m_projectCharacterVerseData;
		private readonly CombinedCharacterVerseData m_combinedCharacterVerseData;
		private readonly CharacterIdComparer m_characterComparer = new CharacterIdComparer();
		private readonly DeliveryComparer m_deliveryComparer = new DeliveryComparer();
		private readonly AliasComparer m_aliasComparer = new AliasComparer();
		private readonly Dictionary<String, CharacterDetail> m_pendingCharacterDetails = new Dictionary<string, CharacterDetail>();
		private readonly HashSet<CharacterVerse> m_pendingCharacterVerseAdditions = new HashSet<CharacterVerse>();
		private HashSet<CharacterVerse> m_currentCharacters;
		private IEnumerable<Character> m_generatedCharacterList;
		private List<Delivery> m_currentDeliveries = new List<Delivery>();

		public delegate void AsssignedBlockIncrementEventHandler(AssignCharacterViewModel sender, int increment);
		public event AsssignedBlockIncrementEventHandler AssignedBlocksIncremented;
		public event EventHandler CurrentBookSaved;
		public delegate void CorrelatedBlockChangedHandler(AssignCharacterViewModel sender, int index);
		public event CorrelatedBlockChangedHandler CorrelatedBlockCharacterAssignmentChanged;

		#endregion

		#region Constructors
		public AssignCharacterViewModel(Project project)
			: this(project, project.Status.AssignCharacterMode != 0 ? project.Status.AssignCharacterMode : BlocksToDisplay.NotYetAssigned, project.Status.AssignCharacterBlock)
		{
		}

		public AssignCharacterViewModel(Project project, BlocksToDisplay mode, BookBlockIndices startingIndices)
			: base(project, mode, startingIndices)
		{
			m_projectCharacterVerseData = project.ProjectCharacterVerseData;
			m_combinedCharacterVerseData = new CombinedCharacterVerseData(project);

			CurrentBlockMatchupChanged += (sender, args) => { m_pendingCharacterVerseAdditions.Clear(); };
		}
		#endregion

		#region Public properties
		public int CompletedBlockCount { get; private set; }

		public override BlocksToDisplay Mode
		{
			get { return base.Mode; }
			set
			{
				base.Mode = value;
				m_project.Status.AssignCharacterMode = value;
			}
		}

		public bool DoingAssignmentTask => (Mode & BlocksToDisplay.NotAssignedAutomatically) > 0;

		public bool DoingAlignmentTask => Mode == BlocksToDisplay.NotAlignedToReferenceText;

		public bool InTaskMode => DoingAssignmentTask || DoingAlignmentTask;

		public bool IsCurrentTaskComplete => InTaskMode && CompletedBlockCount == m_relevantBookBlockIndices.Count;

		public bool IsCurrentBookSingleVoice => CurrentBook.SingleVoice;

		public bool HasSecondaryReferenceText => m_project.ReferenceText.HasSecondaryReferenceText;

		public string PrimaryReferenceTextName => m_project.ReferenceText.LanguageName;
		#endregion

		public void SetUiStrings(string narrator, string bookChapterCharacter, string introCharacter,
			string extraCharacter, string normalDelivery)
		{
			Character.SetUiStrings(narrator, bookChapterCharacter, introCharacter, extraCharacter, () => CurrentBookId, GetCurrentRelevantAlias);
			Delivery.SetNormalDelivery(normalDelivery);
		}

		public void SetCurrentBookSingleVoice(bool singleVoice)
		{
			if (CurrentBook.SingleVoice == singleVoice)
				return;

			CurrentBook.SingleVoice = singleVoice;
			m_project.SaveBook(CurrentBook);

			// REVIEW: Can/should we keep it in rainbow mode even for single-voice books?
			if (singleVoice)
			{
				m_temporarilyIncludedBookBlockIndices = GetCurrentBlockIndices();
				ClearBlockMatchup();
			}
			else // TODO: Ensure test coverage for this
				SetBlockMatchupForCurrentVerse();

			ResetFilter(CurrentBlock);

			OnSaveCurrentBook();

			Analytics.Track("SetSingleVoice", new Dictionary<string, string>
			{
				{ "book", CurrentBookId },
				{ "singleVoice", singleVoice.ToString() },
				{ "method", "AssignCharacterViewModel.SetCurrentBookSingleVoice" }
			});
		}

		public void StoreCharacterDetail(string characterId, CharacterGender gender, CharacterAge age)
		{
			if (m_project.AllCharacterDetailDictionary.ContainsKey(characterId))
			{
				throw new ArgumentException("Project already contains a character with ID " + characterId);
			}
			var detail = new CharacterDetail { CharacterId = characterId, Gender = gender, Age = age, MaxSpeakers = 1 };
			m_pendingCharacterDetails[characterId] = detail;
		}

		public void AddPendingProjectCharacterVerseData(Block block, Character character, Delivery delivery)
		{
			m_pendingCharacterVerseAdditions.Add(new CharacterVerse(GetBlockVerseRef(block, ScrVers.English).BBBCCCVVV,
					character.CharacterId,
					delivery == null ? Delivery.Normal.Text : delivery.Text,
					null,
					true));
			GetCharactersForCurrentReferenceTextMatchup(); // This forces the model's internal list to refresh to just the relevant ones
		}

		private void OnSaveCurrentBook()
		{
			if (CurrentBookSaved != null)
				CurrentBookSaved(this, EventArgs.Empty);
		}

		private void OnAssignedBlocksIncremented(int increment)
		{
			AssignedBlocksIncremented?.Invoke(this, increment);
		}

		#region Overridden methods
		private bool m_inHandleCurrentBlockChanged = false;
		protected override void HandleCurrentBlockChanged()
		{
			if (m_inHandleCurrentBlockChanged)
				return;
			m_inHandleCurrentBlockChanged = true;
			if (CharacterVerseData.IsCharacterExtraBiblical(CurrentBlock.CharacterId))
				throw new InvalidOperationException("Cannot attempt to match an extra-biblical block to a reference text.");
			if (AttemptRefBlockMatchup)
			{
				if (CurrentReferenceTextMatchup == null || !CurrentReferenceTextMatchup.IncludesBlock(CurrentBlock))
				{
					SetBlockMatchupForCurrentVerse();
				}
				else
				{
					CurrentReferenceTextMatchup?.ChangeAnchor(CurrentBlock);
				}
			}
			base.HandleCurrentBlockChanged();
			m_inHandleCurrentBlockChanged = false;
		}

		protected override void PopulateRelevantBlocks()
		{
			CompletedBlockCount = 0;
			base.PopulateRelevantBlocks();
		}

		protected override void RelevantBlockAdded(Block block)
		{
			if (block.UserConfirmed || IsCurrentBookSingleVoice)
				CompletedBlockCount++;
		}

		protected override void StoreCurrentBlockIndices()
		{
			m_project.Status.AssignCharacterBlock = GetCurrentBlockIndices();
		}
		#endregion

		#region Methods to get characters and deliveries
		private HashSet<CharacterVerse> GetUniqueCharacterVerseObjectsForCurrentReference()
		{
			return GetUniqueCharacterVerseObjectsForBlock(CurrentBlock);
		}

		private HashSet<CharacterVerse> GetUniqueCharacterVerseObjectsForBlock(Block block)
		{
			return new HashSet<CharacterVerse>(m_combinedCharacterVerseData.GetCharacters(CurrentBookId,
				block.ChapterNumber, block.InitialStartVerseNumber, block.InitialEndVerseNumber, versification: Versification));
		}

		public IEnumerable<Character> GetCharactersForCurrentReferenceTextMatchup()
		{
			m_currentCharacters = new HashSet<CharacterVerse>();
			foreach (var block in CurrentReferenceTextMatchup.CorrelatedBlocks)
				m_currentCharacters.UnionWith(GetUniqueCharacterVerseObjectsForBlock(block));
			m_currentCharacters.UnionWith(m_pendingCharacterVerseAdditions);

			return GetUniqueCharacters(false);
		}

		public IEnumerable<Character> GetUniqueCharactersForCurrentReference(bool expandIfNone = true)
		{
			m_currentCharacters = GetUniqueCharacterVerseObjectsForCurrentReference();
			return GetUniqueCharacters(expandIfNone);
		}

		private IEnumerable<Character> GetUniqueCharacters(bool expandIfNone = true)
		{
			var listToReturn = new List<Character>(new SortedSet<Character>(
				m_currentCharacters.Select(cv => new Character(cv.Character, cv.LocalizedCharacter, cv.Alias, cv.LocalizedAlias, cv.ProjectSpecific)), m_characterComparer));
			listToReturn.Sort(m_aliasComparer);

			if (listToReturn.All(c => !c.IsNarrator))
				listToReturn.Insert(0, Character.Narrator);

			if (m_currentCharacters.Count == 0 && expandIfNone)
			{
				// This will get any expected characters from other verses in the current block.
				var block = CurrentBlock;
				foreach (var character in m_combinedCharacterVerseData.GetCharacters(CurrentBookId, block.ChapterNumber,
						block.InitialStartVerseNumber, block.LastVerseNum, versification: Versification))
				{
					m_currentCharacters.Add(character);
				}

				var listToAdd = new SortedSet<Character>(m_currentCharacters.Select(cv =>
					new Character(cv.Character, cv.LocalizedCharacter, cv.Alias, cv.LocalizedAlias)), m_characterComparer).Where(c => !listToReturn.Contains(c)).ToList();
				listToAdd.Sort(m_aliasComparer);
				listToReturn.AddRange(listToAdd);
			}

			if (m_currentCharacters.Count == 0 && expandIfNone)
			{
				// This will get any potential or actual characters from surrounding material.
				foreach (var block in ContextBlocksBackward.Union(ContextBlocksForward))
				{
					foreach (var character in m_combinedCharacterVerseData.GetCharacters(CurrentBookId, block.ChapterNumber,
						block.InitialStartVerseNumber, block.InitialEndVerseNumber, versification: Versification))
					{
						m_currentCharacters.Add(character);
					}
				}

				var listToAdd = new SortedSet<Character>(m_currentCharacters.Select(cv =>
					new Character(cv.Character, cv.LocalizedCharacter, cv.Alias, cv.LocalizedAlias)), m_characterComparer).Where(c => !listToReturn.Contains(c)).ToList();
				listToAdd.Sort(m_aliasComparer);
				listToReturn.AddRange(listToAdd);
			}

			return m_generatedCharacterList = listToReturn;
		}

		public IEnumerable<Character> GetUniqueCharacters(string filterText = null)
		{
			var charactersForCurrentRef = GetUniqueCharacterVerseObjectsForCurrentReference();

			if (string.IsNullOrWhiteSpace(filterText))
			{
				m_currentCharacters = new HashSet<CharacterVerse>(m_combinedCharacterVerseData.GetUniqueCharacterAndDeliveries(CurrentBookId));
			}
			else
			{
				m_currentCharacters = new HashSet<CharacterVerse>(m_combinedCharacterVerseData.GetUniqueCharacterAndDeliveries());

				filterText = filterText.Trim();
				m_currentCharacters.RemoveWhere(c => !c.LocalizedCharacter.Contains(filterText, StringComparison.OrdinalIgnoreCase) &&
					(c.LocalizedAlias == null || !c.LocalizedAlias.Contains(filterText, StringComparison.OrdinalIgnoreCase)));
			}

			var listToReturn = new List<Character>(new SortedSet<Character>(
				m_currentCharacters.Select(cv => new Character(cv.Character, cv.LocalizedCharacter, cv.Alias, cv.LocalizedAlias,
					!charactersForCurrentRef.Contains(cv) || cv.ProjectSpecific)), m_characterComparer));
			listToReturn.Sort(m_aliasComparer);

			// PG-88: Now add (at the end of list) any items from charactersForCurrentRef (plus the narrator) that are not in the list.
			listToReturn.AddRange(charactersForCurrentRef.Where(cv => listToReturn.All(ec => ec.CharacterId != cv.Character))
				.Select(cv => new Character(cv.Character, cv.LocalizedCharacter, cv.Alias, cv.LocalizedAlias, cv.ProjectSpecific)));

			if (listToReturn.All(c => !c.IsNarrator))
				listToReturn.Add(Character.Narrator);
			return listToReturn;
		}

		public IEnumerable<Delivery> GetDeliveriesForCharacter(Character selectedCharacter)
		{
			m_currentDeliveries = new List<Delivery>();
			if (selectedCharacter == null)
				return m_currentDeliveries;
			m_currentDeliveries.Add(Delivery.Normal);
			if (!selectedCharacter.IsNarrator)
			{
				foreach (var cv in m_currentCharacters.Where(c => c.Character == selectedCharacter.CharacterId &&
					!string.IsNullOrEmpty(c.Delivery) &&
					m_currentDeliveries.All(d => d.Text != c.Delivery)))
				{
					m_currentDeliveries.Add(new Delivery(cv.Delivery, cv.ProjectSpecific));
				}
			}
			return m_currentDeliveries;
		}

		public IEnumerable<Delivery> GetUniqueDeliveries(string filterText = null)
		{
			List<Delivery> deliveries;
			if (string.IsNullOrWhiteSpace(filterText))
				deliveries = m_combinedCharacterVerseData.GetUniqueDeliveries()
					.Select(d => m_currentDeliveries.FirstOrDefault(cd => cd.Text == d) ?? new Delivery(d)).ToList();
			else
				deliveries = m_combinedCharacterVerseData.GetUniqueDeliveries()
				.Where(ud => ud.Contains(filterText.Trim(), StringComparison.OrdinalIgnoreCase)).Select(d =>
					m_currentDeliveries.FirstOrDefault(cd => cd.Text == d) ?? new Delivery(d)).ToList();

			deliveries.Sort(m_deliveryComparer);

			deliveries.AddRange(m_currentDeliveries.Where(d => !deliveries.Contains(d)));

			return deliveries;
		}

		public IEnumerable<Delivery> GetDeliveriesForCurrentReferenceTextMatchup()
		{
			m_currentDeliveries = new List<Delivery>();

			foreach (var block in CurrentReferenceTextMatchup.CorrelatedBlocks)
			{
				foreach (var delivery in GetUniqueCharacterVerseObjectsForBlock(block).Where(cv => !String.IsNullOrEmpty(cv.Delivery))
					.Select(cv => new Delivery(cv.Delivery, cv.ProjectSpecific)))
				{
					if (!m_currentDeliveries.Any(d => d.Text == delivery.Text))
						m_currentDeliveries.Add(delivery);
				}
				if (!String.IsNullOrEmpty(block.Delivery) && !m_currentDeliveries.Any(d => d.Text == block.Delivery))
					m_currentDeliveries.Add(new Delivery(block.Delivery));
			}

			m_currentDeliveries.Sort(m_deliveryComparer);

			m_currentDeliveries.Insert(0, Delivery.Normal);

			return m_currentDeliveries;
		}
		#endregion

		#region Character/delivery assignment methods
		public bool IsModified(Character newCharacter, Delivery newDelivery)
		{
			Block currentBlock = CurrentBlockInOriginal;
			if (CharacterVerseData.IsCharacterExtraBiblical(currentBlock.CharacterId))
				return false; // Can't change these.

			if (newCharacter == null)
				return !(currentBlock.CharacterIsUnclear() || currentBlock.CharacterId == null);
			if (newCharacter.IsNarrator)
			{
				if (!currentBlock.CharacterIs(CurrentBookId, CharacterVerseData.StandardCharacter.Narrator))
					return true;
			}
			else if (newCharacter.CharacterId != currentBlock.CharacterId)
				return true;

			if (newDelivery == null)
				return true;

			if (newDelivery.IsNormal)
				return (!string.IsNullOrEmpty(currentBlock.Delivery));

			return newDelivery.Text != currentBlock.Delivery;
		}

		private void SetCharacterAndDelivery(Block block, Character selectedCharacter, Delivery selectedDelivery)
		{
			if (CharacterVerseData.IsCharacterUnclear(selectedCharacter.CharacterId))
				throw new ArgumentException("Character cannot be confirmed as ambigous or unknown.", "selectedCharacter");
			// If the user sets a non-narrator to a block we marked as narrator, we want to track it
			if (!selectedCharacter.IsNarrator && !block.IsQuote)
				Analytics.Track("NarratorToQuote", new Dictionary<string, string>
				{
					{ "book", CurrentBookId },
					{ "chapter", block.ChapterNumber.ToString(CultureInfo.InvariantCulture) },
					{ "initialStartVerse", block.InitialStartVerseNumber.ToString(CultureInfo.InvariantCulture) },
					{ "lastVerse", block.LastVerseNum.ToString(CultureInfo.InvariantCulture) },
					{ "character", selectedCharacter.CharacterId }
				});

			if (selectedCharacter.ProjectSpecific || selectedDelivery.ProjectSpecific)
				AddRecordToProjectCharacterVerseData(block, selectedCharacter, selectedDelivery);

			SetCharacter(block, selectedCharacter);

			block.Delivery = selectedDelivery.IsNormal ? null : selectedDelivery.Text;
		}

		private void SetCharacter(Block block, Character selectedCharacter)
		{
			if (selectedCharacter == null)
			{
				block.CharacterId = CharacterVerseData.kAmbiguousCharacter;
				block.CharacterIdInScript = null;
			}
			else if (selectedCharacter.IsNarrator)
				block.SetStandardCharacter(CurrentBookId, CharacterVerseData.StandardCharacter.Narrator);
			else
				block.SetCharacterAndCharacterIdInScript(selectedCharacter.CharacterId, BCVRef.BookToNumber(CurrentBookId),
					m_project.Versification);
			block.UserConfirmed = !block.CharacterIsUnclear();
		}

		public void SetCharacterAndDelivery(Character selectedCharacter, Delivery selectedDelivery)
		{
			if (!CurrentBlockInOriginal.UserConfirmed)
			{
				CompletedBlockCount++;
				OnAssignedBlocksIncremented(1);
			}

			foreach (Block block in GetAllBlocksWhichContinueTheQuoteStartedByBlock(CurrentBlockInOriginal))
				SetCharacterAndDelivery(block, selectedCharacter, selectedDelivery);

			if (CurrentReferenceTextMatchup != null && CurrentReferenceTextMatchup.HasOutstandingChangesToApply)
			{
				foreach (Block block in GetAllBlocksWhichContinueTheQuoteStartedByBlock(CurrentBlock))
					SetCharacterAndDelivery(block, selectedCharacter, selectedDelivery);
			}

			m_project.SaveBook(CurrentBook);
			OnSaveCurrentBook();
		}

		public override void ApplyCurrentReferenceTextMatchup()
		{
			int numberOfBlocksCompleted = 0;
			if (DoingAssignmentTask)
			{
				numberOfBlocksCompleted = CurrentReferenceTextMatchup.OriginalBlocks.Count(b => !b.UserConfirmed && b.CharacterIsUnclear());
			}
			else if (DoingAlignmentTask)
			{
				numberOfBlocksCompleted = 1;
			}
			base.ApplyCurrentReferenceTextMatchup();

			// PG-805: The block matchup UI does not prevent pairing a delivery with a character to which it does not correspond and
			// also allows addition of new character/delivery pairs, so we need to check to see whether this has happened and, if
			// so, add an appropriate entry to the project CV data.
			foreach (var block in CurrentReferenceTextMatchup.OriginalBlocks.Where(IsBlockAssignedToUnknownCharacterDeliveryPair))
			{
				AddRecordToProjectCharacterVerseData(block,
					GetCharactersForCurrentReferenceTextMatchup().First(c => c.CharacterId == block.CharacterId),
					string.IsNullOrEmpty(block.Delivery) ? Delivery.Normal : 
					GetDeliveriesForCurrentReferenceTextMatchup().First(d => d.Text == block.Delivery));
			}

			m_pendingCharacterVerseAdditions.Clear();

			m_project.SaveBook(CurrentBook);
			OnSaveCurrentBook();
			if (numberOfBlocksCompleted > 0)
			{
				CompletedBlockCount += numberOfBlocksCompleted;
				OnAssignedBlocksIncremented(numberOfBlocksCompleted);
			}
		}

		public bool IsBlockAssignedToUnknownCharacterDeliveryPair(Block block)
		{
			if (block.CharacterIsStandard)
			{
				Debug.Assert(block.Delivery == null);
				return false;
			}
			return !GetUniqueCharacterVerseObjectsForBlock(block).Any(cv => cv.Character == block.CharacterId &&
				((cv.Delivery ?? "") == (block.Delivery ?? "")));
		}

		public void SetReferenceTextMatchupCharacter(int blockIndex, Character selectedCharacter)
		{
			var block = CurrentReferenceTextMatchup.CorrelatedBlocks[blockIndex];
			SetCharacter(block, selectedCharacter);
			if (block.MultiBlockQuote == MultiBlockQuote.Start)
			{
				while (++blockIndex < CurrentReferenceTextMatchup.CorrelatedBlocks.Count && CurrentReferenceTextMatchup.CorrelatedBlocks[blockIndex].IsContinuationOfPreviousBlockQuote)
				{
					SetCharacter(CurrentReferenceTextMatchup.CorrelatedBlocks[blockIndex], selectedCharacter);
					CorrelatedBlockCharacterAssignmentChanged?.Invoke(this, blockIndex);
				} 
			}
		}

		public void SetReferenceTextMatchupDelivery(int blockIndex, Delivery selectedDelivery)
		{
			CurrentReferenceTextMatchup.CorrelatedBlocks[blockIndex].Delivery = selectedDelivery.IsNormal ? null : selectedDelivery.Text;
		}

		private void AddRecordToProjectCharacterVerseData(Block block, Character character, Delivery delivery)
		{
			CharacterDetail detail;
			if (m_pendingCharacterDetails.TryGetValue(character.CharacterId, out detail))
			{
				m_project.AddProjectCharacterDetail(detail);
				m_project.SaveProjectCharacterDetailData();
				m_pendingCharacterDetails.Remove(detail.CharacterId);
			}

			var cv = new CharacterVerse(
				new BCVRef(GetBlockVerseRef(block, ScrVers.English).BBBCCCVVV),
				character.IsNarrator
						? CharacterVerseData.GetStandardCharacterId(CurrentBookId, CharacterVerseData.StandardCharacter.Narrator)
						: character.CharacterId,
				delivery.IsNormal ? null : delivery.Text,
				character.Alias,
				character.ProjectSpecific || delivery.ProjectSpecific);
			m_projectCharacterVerseData.Add(cv);

			m_project.SaveProjectCharacterVerseData();
		}

		private string GetCurrentRelevantAlias(string characterId)
		{
			if (m_generatedCharacterList == null)
				return null;
			foreach (Character character in m_generatedCharacterList)
			{
				if (character.CharacterId == characterId)
				{
					if (!string.IsNullOrEmpty(character.LocalizedAlias))
						return character.LocalizedAlias;
					break;
				}
			}
			return null;
		}
		#endregion

		#region Block editing methods
		public void SplitBlock(IEnumerable<BlockSplitData> blockSplits, List<KeyValuePair<int, string>> characters)
		{
			// set the character for the first block
			Block currentBlock = CurrentBlock;
			var firstCharacterId = characters.First(c => c.Key == 0).Value;
			if (currentBlock.CharacterId != firstCharacterId)
			{
				currentBlock.CharacterId = firstCharacterId;
				currentBlock.Delivery = null;
			}

			foreach (var groupOfSplits in blockSplits.GroupBy(s => new { s.BlockToSplit }))
			{
				foreach (var blockSplitData in groupOfSplits.OrderByDescending(s => s, BlockSplitData.BlockSplitDataVerseAndOffsetComparer))
				{
					// get the character selected for this split
					var characterId = characters.First(c => c.Key == blockSplitData.Id).Value;

					var newBlock = CurrentBook.SplitBlock(blockSplitData.BlockToSplit, blockSplitData.VerseToSplit, 
						blockSplitData.CharacterOffsetToSplit, true, characterId, m_project.Versification);

					var newBlockIndices = GetBlockIndices(newBlock);
					var blocksIndicesNeedingUpdate = m_relevantBookBlockIndices.Where(
						r => r.BookIndex == newBlockIndices.BookIndex &&
							r.BlockIndex >= newBlockIndices.BlockIndex);
					foreach (var block in blocksIndicesNeedingUpdate)
						block.BlockIndex++;

					AddToRelevantBlocksIfNeeded(newBlock);
				}
			}
			if (AttemptRefBlockMatchup)
			{
				// A split will always require the current matchup to be re-constructed.
				SetBlockMatchupForCurrentVerse();
			}
		}
		#endregion

		#region Character class
		public class Character
		{
			private static Character s_narrator;
			private static string s_bookChapterCharacter;
			private static string s_introCharacter;
			private static string s_extraCharacter;

			private readonly string m_characterId;
			private readonly string m_localizedCharacterId;
			private readonly string m_localizedAlias;
			private readonly string m_alias;
			private readonly bool m_projectSpecific;
			private static Func<string> s_funcToGetBookId;
			private static Func<string, string> s_funcToGetRelevantAlias;

			public static Character Narrator { get { return s_narrator; } }

			public string CharacterId { get { return m_characterId; } }
			public string LocalizedCharacterId { get { return m_localizedCharacterId; } }
			public string Alias { get { return m_alias; } }
			public string LocalizedAlias { get { return m_localizedAlias; } }
			public bool ProjectSpecific { get { return m_projectSpecific; } }
			public bool IsNarrator { get { return Equals(s_narrator); } }

			public string LocalizedDisplay { get { return ToLocalizedString(); } }

			public static void SetUiStrings(string narrator, string bookChapterCharacter, string introCharacter,
				string extraCharacter, Func<string> funcToGetBookId, Func<string, string> funcToGetRelevantAlias)
			{
				s_funcToGetBookId = funcToGetBookId;
				s_funcToGetRelevantAlias = funcToGetRelevantAlias;
				s_narrator = new Character(narrator, null, null, null, false);
				s_bookChapterCharacter = bookChapterCharacter;
				s_introCharacter = introCharacter;
				s_extraCharacter = extraCharacter;
			}

			internal Character(string characterId, string localizedCharacterId = null, string alias = null, string localizedAlias = null, bool projectSpecific = true)
			{
				m_characterId = CharacterVerseData.IsCharacterOfType(characterId, CharacterVerseData.StandardCharacter.Narrator) ?
					s_narrator.CharacterId : characterId;
				m_localizedCharacterId = localizedCharacterId ?? characterId;
				m_alias = String.IsNullOrWhiteSpace(alias) ? null : alias;
				m_localizedAlias = String.IsNullOrWhiteSpace(localizedAlias) ? null : localizedAlias;
				m_projectSpecific = projectSpecific;
			}

			public override string ToString()
			{
				if (IsNarrator)
					return String.Format(CharacterId, s_funcToGetBookId());
				return LocalizedAlias ?? CharacterId;
			}

			public string ToLocalizedString()
			{
				if (IsNarrator)
					return ToString();
				return LocalizedAlias ?? LocalizedCharacterId;
			}

			public static string GetCharacterIdForUi(string characterId)
			{
				switch (CharacterVerseData.GetStandardCharacterType(characterId))
				{
					case CharacterVerseData.StandardCharacter.Narrator: return s_narrator.ToString();
					case CharacterVerseData.StandardCharacter.Intro: return String.Format(s_introCharacter, s_funcToGetBookId());
					case CharacterVerseData.StandardCharacter.ExtraBiblical: return String.Format(s_extraCharacter, s_funcToGetBookId());
					case CharacterVerseData.StandardCharacter.BookOrChapter: return String.Format(s_bookChapterCharacter, s_funcToGetBookId());
					default:
						if (characterId == CharacterVerseData.kAmbiguousCharacter || characterId == CharacterVerseData.kUnknownCharacter)
							return "";
						string relevantAlias = s_funcToGetRelevantAlias(characterId);
						characterId = LocalizationManager.GetDynamicString(GlyssenInfo.kApplicationId, "CharacterName." + characterId, characterId);
						if (relevantAlias != null)
							return characterId + " [" + relevantAlias + "]";
						return characterId;
				}
			}

			#region Equality members
			protected bool Equals(Character other)
			{
				return string.Equals(CharacterId, other.CharacterId);
			}

			public override bool Equals(object obj)
			{
				if (ReferenceEquals(null, obj))
					return false;
				if (ReferenceEquals(this, obj))
					return true;
				if (obj.GetType() != GetType())
					return false;
				return Equals((Character)obj);
			}

			public override int GetHashCode()
			{
				return (m_characterId != null ? m_characterId.GetHashCode() : 0);
			}

			public static bool operator ==(Character left, Character right)
			{
				return Equals(left, right);
			}

			public static bool operator !=(Character left, Character right)
			{
				return !Equals(left, right);
			}
			#endregion
		}
		#endregion

		#region CharacterIdComparer class

		public abstract class CharacterComparer
		{
			protected int CompareSpecialCases(Character x, Character y, string xTextToCompare, string yTextToCompare)
			{
				// if the CharacterIds are not the same, check for a special case
				if ((x.CharacterId) != (y.CharacterId))
				{
					// narrator should be first item
					if (x.IsNarrator) return -1;
					if (y.IsNarrator) return 1;

					// Jesus should be second item
					if (x.CharacterId == "Jesus") return -1;
					if (y.CharacterId == "Jesus") return 1;
				}
				
				// this is not a special case
				return string.Compare(xTextToCompare, yTextToCompare, StringComparison.InvariantCultureIgnoreCase);
			}
		}

		public class CharacterIdComparer : CharacterComparer, IComparer<Character>
		{
			int IComparer<Character>.Compare(Character x, Character y)
			{
				return CompareSpecialCases(x, y, x.CharacterId, y.CharacterId);
			}
		}
		#endregion

		#region AliasComparer class
		public class AliasComparer : CharacterComparer, IComparer<Character>
		{
			int IComparer<Character>.Compare(Character x, Character y)
			{
				var xTextToCompare = string.IsNullOrEmpty(x.Alias) ? x.CharacterId : x.Alias;
				var yTextToCompare = string.IsNullOrEmpty(y.Alias) ? y.CharacterId : y.Alias;
				
				var result = CompareSpecialCases(x, y, xTextToCompare, yTextToCompare);
				return result != 0 ? result : string.Compare(x.CharacterId, y.CharacterId, StringComparison.InvariantCultureIgnoreCase);
			}
		}
		#endregion

		#region Delivery class
		public class Delivery
		{
			private static Delivery s_normalDelivery;

			private readonly string m_text;
			private readonly bool m_projectSpecific;

			public string Text { get { return m_text; } }
			public string LocalizedDisplay { get { return ToLocalizedString(); } }

			private string ToLocalizedString()
			{
				// TODO: Enable localization of deliveries
				return Text;
			}
			public bool ProjectSpecific { get { return m_projectSpecific; } }
			public static Delivery Normal { get { return s_normalDelivery; } }
			public bool IsNormal { get { return Equals(s_normalDelivery); } }

			public static void SetNormalDelivery(string normalDelivery)
			{
				s_normalDelivery = new Delivery(normalDelivery, false);
			}

			internal Delivery(string text, bool projectSpecific = true)
			{
				m_text = text;
				m_projectSpecific = projectSpecific;
			}

			public override string ToString()
			{
				return Text;
			}

			#region Equality members
			protected bool Equals(Delivery other)
			{
				return String.Equals(Text, other.Text);
			}

			public override bool Equals(object obj)
			{
				if (ReferenceEquals(null, obj))
					return false;
				if (ReferenceEquals(this, obj))
					return true;
				if (obj.GetType() != GetType())
					return false;
				return Equals((Delivery)obj);
			}

			public override int GetHashCode()
			{
				return (Text != null ? Text.GetHashCode() : 0);
			}

			public static bool operator ==(Delivery left, Delivery right)
			{
				return Equals(left, right);
			}

			public static bool operator !=(Delivery left, Delivery right)
			{
				return !Equals(left, right);
			}
			#endregion
		}
		#endregion

		#region DeliveryComparer class
		public class DeliveryComparer : IComparer<Delivery>
		{
			int IComparer<Delivery>.Compare(Delivery x, Delivery y)
			{
				return String.Compare(x.Text, y.Text, StringComparison.InvariantCultureIgnoreCase);
			}
		}
		#endregion

		public Character GetCharacterToSelectForCurrentBlock(IEnumerable<Character> currentCharacters)
		{
			if (CurrentBlock.CharacterIs(CurrentBookId, CharacterVerseData.StandardCharacter.Narrator))
				return Character.Narrator;
			if (CurrentBlock.CharacterIsUnclear())
			{
				if (!CurrentBlock.ContainsVerseNumber)
				{
					var charactersForCurrentVerse = GetUniqueCharacterVerseObjectsForCurrentReference();
					// ENHANCE: Some "Quotations" in the control file may represent text that is typically rendered as
					// indirect speech (and should therefore be marked as Indirect|Quotation). We really don't want to
					// include these, but in practice it probably won't matter much.
					charactersForCurrentVerse.RemoveWhere(c => !c.IsExpected && c.QuoteType != QuoteType.Quotation);
					if (charactersForCurrentVerse.Count != 2)
						return null;
					var blocks = CurrentBook.GetBlocksForVerse(CurrentBlock.ChapterNumber, CurrentBlock.InitialStartVerseNumber).Where(b => b.UserConfirmed).ToList();
					if (blocks.Count != 1)
						return null;

					charactersForCurrentVerse.RemoveWhere(c => c.Character == blocks[0].CharacterId);

					if (charactersForCurrentVerse.Count != 1)
						return null;

					return currentCharacters.FirstOrDefault(item => item.LocalizedCharacterId == charactersForCurrentVerse.Single().LocalizedCharacter);
				}
			}
			return currentCharacters.FirstOrDefault(item => item.CharacterId == CurrentBlock.CharacterId);
		}
	}
}
