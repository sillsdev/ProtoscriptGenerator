﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;
using Glyssen.Properties;
using SIL.Scripture;
using SIL.Xml;

namespace Glyssen.Character
{
	[XmlRoot("NarratorOverrides")]
	public class NarratorOverrides
	{
		private static NarratorOverrides s_singleton;
		private Dictionary<string, List<NarratorOverrideDetail>> m_dictionary;

		public static NarratorOverrides Singleton
		{
			get
			{
				if (s_singleton == null)
				{
					s_singleton = XmlSerializationHelper.DeserializeFromString<NarratorOverrides>(Resources.NarratorOverrides);
					s_singleton.m_dictionary = s_singleton.Books.ToDictionary(b => b.Id, b => b.Overrides);
					foreach (var book in s_singleton.Books)
					{
						var bookNum = BCVRef.BookToNumber(book.Id);
						foreach (var overrideDetail in book.Overrides.Where(o => o.EndVerse == 0))
							overrideDetail.EndVerse = ScrVers.English.GetLastVerse(bookNum, overrideDetail.EndChapter);
					}
				}
				return s_singleton;
			}
		}

		public static IEnumerable<NarratorOverrideDetail> GetNarratorOverridesForBook(string bookId, ScrVers targetVersification = null)
		{
			if (!Singleton.m_dictionary.TryGetValue(bookId, out List<NarratorOverrideDetail> details))
				return new NarratorOverrideDetail[0];
			if (targetVersification == null || targetVersification == ScrVers.English)
				return details;

			var bookNum = BCVRef.BookToNumber(bookId);
			return details.Select(t => t.WithVersification(bookNum, targetVersification));
		}

		/// <summary>
		/// Gets the character to use in the script for a narrator block in the reference range of the given block. Note
		/// that this code does not bother to check whether the given block is actually a narrator block.
		/// </summary>
		public static string GetCharacterOverrideForBlock(int bookNum, Block block, ScrVers versification)
		{
			var details = GetCharacterOverrideDetailsForRefRange(block.StartRef(bookNum, versification), block.LastVerseNum)?.ToList();
			if (details == null)
				return null;
			if (details.Count == 1)
				return details[0].Character;
			throw new NotImplementedException("Handle multiple");
		}

		public static IEnumerable<NarratorOverrideDetail> GetCharacterOverrideDetailsForRefRange(VerseRef startRef, int endVerse)
		{
			int endChapter;
			if (!ChangeToEnglishVersification(ref startRef, ref endVerse, out endChapter))
				return null;

			return GetNarratorOverridesForBook(startRef.Book).Where(o =>
				(o.StartChapter < startRef.ChapterNum || (o.StartChapter == startRef.ChapterNum && o.StartVerse <= startRef.VerseNum)) &&
				(o.EndChapter > endChapter || (o.EndChapter == endChapter && o.EndVerse >= endVerse)));
		}

		public static bool ChangeToEnglishVersification(ref VerseRef startRef, ref int endVerse, out int endChapter)
		{
			if (endVerse < startRef.VerseNum)
				throw new ArgumentOutOfRangeException(nameof(endVerse), "Range must be in a single chapter and end verse must be greater than start verse.");

			bool endAndStartAreSame = endVerse == startRef.VerseNum;
			VerseRef endRef = endAndStartAreSame ? startRef : new VerseRef(startRef) { VerseNum = endVerse };

			startRef.ChangeVersification(ScrVers.English);
			if (startRef.VerseNum == 0) // Currently, we don't support overriding verse 0 (Hebrew subtitle in Psalms) -- this allows us to define overrides with chapter ranges.
			{
				endChapter = -1;
				return false;
			}

			if (endAndStartAreSame) // Calling change versification is kind of expensive, so this is a helpful optimization
				endRef = startRef;
			else
				endRef.ChangeVersification(ScrVers.English);
			endVerse = endRef.VerseNum;
			endChapter = endRef.ChapterNum;
			return true;
		}

		public static IReadOnlyDictionary<string, List<NarratorOverrideDetail>> NarratorOverridesByBookId => Singleton.m_dictionary;

		[XmlElement(ElementName = "Book")]
		public List<BookNarratorOverrides> Books { get; set; }

		[XmlRoot("Book")]
		public class BookNarratorOverrides
		{
			[XmlAttribute("id")]
			public string Id { get; set; }

			[XmlElement(ElementName = "Override")]
			public List<NarratorOverrideDetail> Overrides { get; set; }
		}

		[XmlRoot("Override")]
		public class NarratorOverrideDetail
		{
			public NarratorOverrideDetail()
			{
				StartVerse = 1;
				StartBlock = 0;
				EndBlock = 0;
			}

			[XmlAttribute("startChapter")]
			public int StartChapter { get; set; }

			[XmlAttribute("startVerse")]
			[DefaultValue(1)]
			public int StartVerse { get; set; }

			[XmlAttribute("startBlock")]
			[DefaultValue(0)]
			public int StartBlock { get; set; }

			private int? m_endChapter;
			[XmlAttribute("endChapter")]
			public int EndChapter
			{
				get => m_endChapter ?? StartChapter;
				set => m_endChapter = value;
			}

			[XmlAttribute("endVerse")]
			public int EndVerse { get; set; }

			[XmlAttribute("endBlock")]
			[DefaultValue(0)]
			public int EndBlock { get; set; }

			[XmlAttribute("character")]
			public string Character { get; set; }

			private string StartBlockAsSegmentLetter(bool suppressSegmentA = true)
			{
				if (StartBlock == 0 || (suppressSegmentA && StartBlock == 1))
					return string.Empty;
				return ((char)('a' + StartBlock - 1)).ToString();
			}

			public override string ToString()
			{
				return $"{StartChapter}:{StartVerse}{StartBlockAsSegmentLetter()}-{(EndChapter == StartChapter ? null : EndChapter + ":")}{EndVerse}, {Character}";
			}

			public NarratorOverrideDetail WithVersification(int bookNum, ScrVers targetVersification)
			{
				var clone = (NarratorOverrideDetail)MemberwiseClone();
				var startRef = new VerseRef(bookNum, StartChapter, StartVerse, ScrVers.English);
				startRef.ChangeVersification(targetVersification);
				var endRef = new VerseRef(bookNum, EndChapter, EndVerse, ScrVers.English);
				endRef.ChangeVersification(targetVersification);
				clone.StartChapter = startRef.ChapterNum;
				clone.StartVerse = startRef.VerseNum;
				clone.EndChapter = endRef.ChapterNum;
				clone.EndVerse = endRef.VerseNum;
				return clone;
			}
		}
	}
}