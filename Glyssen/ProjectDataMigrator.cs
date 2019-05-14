﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Glyssen.Character;
using SIL.Extensions;
using SIL.Reporting;
using SIL.Scripture;

namespace Glyssen
{
	internal class ProjectDataMigrator
	{
		private static Project s_lastProjectMigrated;
		private static Migrations s_migrationsRun;

		[Flags]
		private enum Migrations
		{
			ConvertFromDeprecatedPreVersion140 = 1,
		}

		public enum MigrationResult
		{
			NoOp,
			Partial,
			Complete,
		}

		public static MigrationResult MigrateProjectData(Project project, int fromControlFileVersion)
		{
			// There are several places where this can get called prematurely. Unless/until we have a migration
			// step that works with something other than the Books collection, we can just jump out now if we
			// don't have any books. If this changes, and we remove this code, see "HEADS UP" comment below.
			if (!project.Books.Any())
				return MigrationResult.NoOp;

			Logger.WriteEvent("Migrating project " + project.ProjectFilePath);

			if (s_lastProjectMigrated != project)
				s_migrationsRun = 0;

			var result = MigrationResult.Partial;

			if (fromControlFileVersion < 140 && (project != s_lastProjectMigrated || (s_migrationsRun & Migrations.ConvertFromDeprecatedPreVersion140) == 0))
			{
				// HEADS UP! If there are no books, the following method will be a no-op, so we do not want to set the flag. (See comment above.)
				MigrateSpecialCharacterIds(project.Books);
				s_migrationsRun |= Migrations.ConvertFromDeprecatedPreVersion140;
			}

			// We don't need to track runs of Migrations which only occur for projects that are ReadyForDataMigration
			// because we shouldn't call MigrateProjectData again.
			if ((project.ProjectState & ProjectState.ReadyForDataMigration) > 0)
			{
				if (fromControlFileVersion < 96)
					MigrateInvalidMultiBlockQuoteData(project.Books);
				if (fromControlFileVersion < 138)
				{
					// This method was originally called for the < 96 case above, but we found new cases
					// for which it is needed. Therefore, we call it here to keep it in the same order
					// as it was before (though, technically, we aren't sure we care).
					CleanUpOrphanedMultiBlockQuoteStati(project.Books);
				}
				if (fromControlFileVersion < 135)
					MigrateInvalidCharacterIdForScriptData(project.Books);
				if (fromControlFileVersion == 104)
					MigrateInvalidCharacterIdsWithoutCharacterIdInScriptOverrides(project);
				if (fromControlFileVersion < 139)
				{
					MigrateNonContiguousUserSplitsSeparatedByReferenceTextAlignmentSplits(project.Books);
					CleanUpMultiBlockQuotesAssignedToNarrator(project.Books);
					ResolveUnclearCharacterIdsForVernBlocksMatchedToRefBlocks(project.Books, project.Versification);
				}
				MigrateDeprecatedCharacterIds(project);

				result = MigrationResult.Complete;
			}

			s_lastProjectMigrated = project;

			return result;
		}

		// internal for testing
		internal static void MigrateInvalidMultiBlockQuoteData(IReadOnlyList<BookScript> books)
		{
			foreach (var book in books)
			{
				ISet<int> processedSplitIds = new HashSet<int>();
				var blocks = book.GetScriptBlocks();
				foreach (var firstBlockOfSplit in blocks.Where(b => b.SplitId != -1))
				{
					int splitId = firstBlockOfSplit.SplitId;
					if (processedSplitIds.Contains(splitId))
						continue;
					if (firstBlockOfSplit.MultiBlockQuote == MultiBlockQuote.None)
					{
						processedSplitIds.Add(splitId);
						continue;
					}

					List<Block> subsequentBlocksInSplit = new List<Block>(5);
					Block firstBlockAfterSplit = null;
					int iBlock = blocks.IndexOf(firstBlockOfSplit);
					while (++iBlock < blocks.Count)
					{
						if (blocks[iBlock].SplitId == splitId)
							subsequentBlocksInSplit.Add(blocks[iBlock]);
						else
						{
							firstBlockAfterSplit = blocks[iBlock];
							break;
						}
					}

					if (!subsequentBlocksInSplit.Any())
					{
						// This should never happen
						Debug.Fail("Splits should always have more than one block and those blocks should be sequential.");
						processedSplitIds.Add(splitId);
						continue;
					}

					if (subsequentBlocksInSplit.All(b => b.MultiBlockQuote == MultiBlockQuote.None) &&
						(firstBlockAfterSplit == null ||
						firstBlockAfterSplit.IsContinuationOfPreviousBlockQuote))
					{
						if (firstBlockOfSplit.MultiBlockQuote == MultiBlockQuote.Start)
							firstBlockOfSplit.MultiBlockQuote = MultiBlockQuote.None;
						subsequentBlocksInSplit.Last().MultiBlockQuote = MultiBlockQuote.Start;
					}

					processedSplitIds.Add(splitId);
				}
			}
		}

		//internal for testing
		internal static void CleanUpOrphanedMultiBlockQuoteStati(IReadOnlyList<BookScript> books)
		{
			foreach (var book in books)
			{
				var blocks = book.GetScriptBlocks();
				if (!blocks.Any())
					continue;

				Block previousBlock = blocks[0];
				if (blocks[0].MultiBlockQuote != MultiBlockQuote.None && blocks[0].MultiBlockQuote != MultiBlockQuote.Start)
					previousBlock.MultiBlockQuote = MultiBlockQuote.None;
				MultiBlockQuote previousBlockMultiBlockStatus = previousBlock.MultiBlockQuote;
				foreach (var block in blocks.Skip(1))
				{
					switch (block.MultiBlockQuote)
					{
						case MultiBlockQuote.None:
							if (previousBlockMultiBlockStatus == MultiBlockQuote.Start)
								previousBlock.MultiBlockQuote = MultiBlockQuote.None;

							break;
						case MultiBlockQuote.Start:
							if (previousBlockMultiBlockStatus == MultiBlockQuote.Start)
								previousBlock.MultiBlockQuote = MultiBlockQuote.None;

							break;
						case MultiBlockQuote.Continuation:
							// One could make the case that we should check if we are dealing with a quote and, if so,
							// set to Start here. If is an orphan, it will be cleaned up in the next pass, but if it is
							// followed by one or more Continuations, we will now have those grouped together.
							// However,
							// 1) This is the safer bet.
							//    Marking multiple blocks as not being the same quote if they really are (hopefully) just means
							//    the user might have a little more work to do.
							//    Marking multiple blocks as being in the same quote if they aren't is an actual data problem.
							// 2) We don't think the actual cases we are cleaning up have this problem.
							if (previousBlockMultiBlockStatus == MultiBlockQuote.None)
								block.MultiBlockQuote = MultiBlockQuote.None;

							break;
					}

					previousBlock = block;
					previousBlockMultiBlockStatus = block.MultiBlockQuote;
				}

				if (previousBlock.MultiBlockQuote == MultiBlockQuote.Start)
					previousBlock.MultiBlockQuote = MultiBlockQuote.None;
			}
		}

		// internal for testing
		internal static void MigrateInvalidCharacterIdForScriptData(IReadOnlyList<BookScript> books)
		{
			foreach (var block in books.SelectMany(book => book.GetScriptBlocks().Where(block =>
				block.SpecialCharacter != Block.SpecialCharacters.NormalBiblicalCharacter &&
				block.CharacterIdOverrideForScript != null)))
			{
				block.CharacterIdInScript = null;
				if (!block.CharacterIsStandard)
					block.UserConfirmed = false;
			}
		}

		// internal for testing
		internal static int MigrateInvalidCharacterIdsWithoutCharacterIdInScriptOverrides(Project project)
		{
			int numberOfChangesMade = 0; // For testing

			foreach (BookScript book in project.Books)
			{
				foreach (Block block in book.GetScriptBlocks().Where(block => block.CharacterId != null &&
					block.CharacterId.Contains("/") &&
					block.CharacterIdOverrideForScript == null))
				{
					block.UseDefaultForMultipleChoiceCharacter(project.Versification);
					numberOfChangesMade++;
				}
			}
			return numberOfChangesMade;
		}

		// internal for testing
		internal static int MigrateDeprecatedCharacterIds(Project project)
		{
			var cvInfo = new CombinedCharacterVerseData(project);
			var characterDetailDictionary = project.AllCharacterDetailDictionary;
			int numberOfChangesMade = 0; // For testing

			foreach (BookScript book in project.Books)
			{
				int bookNum = BCVRef.BookToNumber(book.BookId);

				foreach (var block in book.GetScriptBlocks().Where(block => block.CharacterId != null))
				{
					var unknownCharacter = !characterDetailDictionary.ContainsKey(block.CharacterIdInScript);
					if (unknownCharacter && project.ProjectCharacterVerseData.GetCharacters(bookNum, block.ChapterNumber, block.InitialStartVerseNumber,
						block.InitialEndVerseNumber, block.LastVerseNum).Any(c => c.Character == block.CharacterId && c.Delivery == (block.Delivery ?? "")))
					{
						// PG-471: This is a known character who spoke in an unexpected location and was therefore added to the project CV file,
						// but was subsequently removed or renamed from the master character detail list.
						project.ProjectCharacterVerseData.Remove(bookNum, block.ChapterNumber, block.InitialStartVerseNumber,
							block.InitialEndVerseNumber, block.CharacterId, block.Delivery ?? "");
						block.SpecialCharacter = Block.SpecialCharacters.Unexpected;
						numberOfChangesMade++;
					}
					else
					{
						var characters = cvInfo.GetCharacters(bookNum, block.ChapterNumber, block.InitialStartVerseNumber, block.InitialEndVerseNumber, block.LastVerseNum).ToList();
						if (unknownCharacter || !characters.Any(c => c.Character == block.CharacterId && c.Delivery == (block.Delivery ?? "")))
						{
							if (characters.Count(c => c.Character == block.CharacterId) == 1)
								block.Delivery = characters.First(c => c.Character == block.CharacterId).Delivery;
							else
								block.SetCharacterAndDelivery(characters);
							numberOfChangesMade++;
						}
					}
				}
			}
			return numberOfChangesMade;
		}

		/// <summary>
		/// This migration fixes a problem caused when a manual split operation that broke a block up into multiple pieces (all with the same split ID)
		/// was followed by further programatic splitting (at verse breaks) to align the blocks to the reference text. Those splits should have (and now
		/// do) copied the split id to all intervening blocks because if the user ends up applying the reference text matchup, we need for the whole
		/// sequence to be treated as a single manual split operation. (This is mainly to allow us to re-apply user decisions in the event of a parser
		/// change or an updated data set.)
		/// </summary>
		/// <remarks>internal for testing</remarks>
		internal static void MigrateNonContiguousUserSplitsSeparatedByReferenceTextAlignmentSplits(IReadOnlyList<BookScript> books)
		{
			foreach (var book in books)
			{
				int currentSplitId = -1;
				var contiguousSubsequentBlocksMatchingRefText = new List<Block>();
				foreach (var block in book.GetScriptBlocks())
				{
					if (block.SplitId >= 0)
					{
						if (block.SplitId != currentSplitId)
							currentSplitId = block.SplitId;
						else
						{
							foreach (var b in contiguousSubsequentBlocksMatchingRefText)
								b.SplitId = currentSplitId;
						}
						contiguousSubsequentBlocksMatchingRefText.Clear();
					}
					else if (currentSplitId >= 0)
					{
						if (block.MatchesReferenceText)
							contiguousSubsequentBlocksMatchingRefText.Add(block);
						else
						{
							// Once we hit a block that is neither a manual split nor aligned to the reference text,
							// we should be done processing the current split.
							contiguousSubsequentBlocksMatchingRefText.Clear();
							currentSplitId = -1;
						}
					}
				}
			}
		}

		//internal for testing
		internal static void CleanUpMultiBlockQuotesAssignedToNarrator(IReadOnlyList<BookScript> books)
		{
			foreach (var block in books.SelectMany(book => book.GetScriptBlocks())
				.Where(b => b.MultiBlockQuote != MultiBlockQuote.None && b.CharacterIsStandard))
			{
				block.MultiBlockQuote = MultiBlockQuote.None;
			}
		}

		//internal for testing
		internal static void ResolveUnclearCharacterIdsForVernBlocksMatchedToRefBlocks(IReadOnlyList<BookScript> books, ScrVers scrVers)
		{
			foreach (var book in books)
			{
				Block prevBlock = null;
				foreach (var block in book.GetScriptBlocks().Where(b => b.CharacterIsUnknown))
				{
					if (block.MatchesReferenceText)
						block.SetCharacterAndDeliveryInfo(block.ReferenceBlocks.Single(), scrVers);
					else if (block.IsContinuationOfPreviousBlockQuote)
						block.SetCharacterAndDeliveryInfo(prevBlock, scrVers);
					prevBlock = block;
				}
			}
		}

		/// <summary>
		/// This migration moves standard, "Unknown" (i.e., uhnexpected) and "Ambiguous") character IDs to SpecialCharacter.
		/// </summary>
		/// <remarks>internal for testing</remarks>
		internal static void MigrateSpecialCharacterIds(IReadOnlyList<BookScript> books)
		{
			// Represents a quote whose character has not been set (usually represents an unexpected quote)</summary>
			const string kUnexpectedCharacter = "Unknown";
			// Represents a quote whose character has not been set. Used when the user needs to disambiguate between multiple potential characters.
			const string kAmbiguousCharacter = "Ambiguous";


			foreach (var book in books)
			{
				foreach (var block in book.GetScriptBlocks())
				{
					switch (GetStandardCharacterType(block.CharacterId))
					{
						case CharacterVerseData.CharacterType.Narrator: block.SpecialCharacter = Block.SpecialCharacters.Narrator; break;
						case CharacterVerseData.CharacterType.ExtraBiblical: block.SpecialCharacter = Block.SpecialCharacters.ExtraBiblical; break;
						case CharacterVerseData.CharacterType.BookOrChapter:
							block.SpecialCharacter = (block.StyleTag == "c" || block.StyleTag == "cl") ?
								Block.SpecialCharacters.ChapterAnnouncement :
								Block.SpecialCharacters.BookTitle;
							break;
						case CharacterVerseData.CharacterType.Intro: block.SpecialCharacter = Block.SpecialCharacters.Intro; break;

						default:
							if (block.CharacterId == kAmbiguousCharacter)
								block.SpecialCharacter = Block.SpecialCharacters.Ambiguous;
							else if (block.CharacterId == kUnexpectedCharacter)
								block.SpecialCharacter = Block.SpecialCharacters.Unexpected;
							break;
					}
				}
			}
		}

		/// <summary>Character ID prefix for material to be read by narrator</summary>
		private const string kNarratorPrefix = "narrator-";
		/// <summary>Character ID prefix for book titles or chapter breaks</summary>
		private const string kBookOrChapterPrefix = "BC-";
		/// <summary>Character ID prefix for extra-biblical material (section heads, etc.)</summary>
		private const string kExtraBiblicalPrefix = "extra-";
		/// <summary>Character ID prefix for intro material</summary>
		private const string kIntroPrefix = "intro-";

		public static CharacterVerseData.CharacterType GetStandardCharacterType(string characterId)
		{
			if (string.IsNullOrEmpty(characterId))
				return CharacterVerseData.CharacterType.NonStandard;

			var i = characterId.IndexOf("-", StringComparison.Ordinal);
			if (i < 0)
				return CharacterVerseData.CharacterType.NonStandard;

			switch (characterId.Substring(0, i + 1))
			{
				case kNarratorPrefix: return CharacterVerseData.CharacterType.Narrator;
				case kIntroPrefix: return CharacterVerseData.CharacterType.Intro;
				case kExtraBiblicalPrefix: return CharacterVerseData.CharacterType.ExtraBiblical;
				case kBookOrChapterPrefix: return CharacterVerseData.CharacterType.BookOrChapter;
			}

			return CharacterVerseData.CharacterType.NonStandard;
		}

		internal static string GetCharacterPrefix(CharacterVerseData.CharacterType standardCharacterType)
		{
			switch (standardCharacterType)
			{
				case CharacterVerseData.CharacterType.Narrator:
					return kNarratorPrefix;
				case CharacterVerseData.CharacterType.BookOrChapter:
					return kBookOrChapterPrefix;
				case CharacterVerseData.CharacterType.ExtraBiblical:
					return kExtraBiblicalPrefix;
				case CharacterVerseData.CharacterType.Intro:
					return kIntroPrefix;
				default:
					throw new ArgumentException("Unexpected standard character type.");
			}
		}

		public static string GetStandardCharacterId(string bookId, CharacterVerseData.CharacterType standardCharacterType)
		{
			return GetCharacterPrefix(standardCharacterType) + bookId;
		}

		public static bool IsCharacterOfType(string characterId, CharacterVerseData.CharacterType standardCharacterType)
		{
			return characterId.StartsWith(GetCharacterPrefix(standardCharacterType), StringComparison.Ordinal);
		}

		public static bool IsCharacterStandard(string characterId)
		{
			if (characterId == null)
				return false;

			return IsCharacterOfType(characterId, CharacterVerseData.CharacterType.Narrator) ||
				IsCharacterOfType(characterId, CharacterVerseData.CharacterType.BookOrChapter) ||
				IsCharacterOfType(characterId, CharacterVerseData.CharacterType.ExtraBiblical) ||
				IsCharacterOfType(characterId, CharacterVerseData.CharacterType.Intro);
			// We could call IsCharacterExtraBiblical instead of the last three lines of this if,
			// but this is speed-critical code and the overhead of the extra method call is
			// expensive.
		}

		public static bool IsCharacterExtraBiblical(string characterId)
		{
			if (characterId == null)
				return false;

			return IsCharacterOfType(characterId, CharacterVerseData.CharacterType.BookOrChapter) ||
				IsCharacterOfType(characterId, CharacterVerseData.CharacterType.ExtraBiblical) ||
				IsCharacterOfType(characterId, CharacterVerseData.CharacterType.Intro);
		}
	}
}
