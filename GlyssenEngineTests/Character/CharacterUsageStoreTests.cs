﻿using System.Collections.Generic;
using Glyssen.Shared;
using GlyssenEngine.Character;
using GlyssenEngineTests.Properties;
using NUnit.Framework;
using SIL.Scripture;

namespace GlyssenEngineTests.Character
{
	[TestFixture]
	class CharacterUsageStoreTests
	{
		[Test]
		public void GetStandardCharacterName_KnownCharacterWithSingleDelivery_ReturnsCharacterNameAndDelivery()
		{
			var store = new CharacterUsageStore(ScrVers.English, 
				ControlCharacterVerseData.Singleton, GetLocalizedVariants);
			Assert.AreEqual("Jesus", store.GetStandardCharacterName("Jesus", BCVRef.BookToNumber("MRK"),
				6, new[] {new Verse("38")},  out var delivery, out var defaultCharacter));
			Assert.AreEqual("questioning", delivery);
			Assert.IsNull(defaultCharacter);
		}

		[Test]
		public void GetStandardCharacterName_KnownCharacterInVerseBridgeStartingInVerseBefore_ReturnsCharacterName()
		{
			var store = new CharacterUsageStore(ScrVers.English, 
				ControlCharacterVerseData.Singleton, GetLocalizedVariants);
			Assert.AreEqual("Jesus", store.GetStandardCharacterName("Jesus", BCVRef.BookToNumber("MRK"),
				6, new[] {new Verse("36-38")},  out var delivery, out var defaultCharacter));
			Assert.IsNull(delivery);
			Assert.IsNull(defaultCharacter);
		}

		[Test]
		public void GetStandardCharacterName_LocalizedCharacterWithNoDelivery_ReturnsEnglishCharacterName()
		{
			var store = new CharacterUsageStore(ScrVers.English, 
				ControlCharacterVerseData.Singleton, GetLocalizedVariants);
			Assert.AreEqual("Andrew", store.GetStandardCharacterName("Andrés", BCVRef.BookToNumber("MRK"),
				6, new[] {new Verse("38")}, out var delivery, out var defaultCharacter));
			Assert.IsNull(delivery);
			Assert.IsNull(defaultCharacter);
		}

		/// <summary>
		/// This test case is for a highly improbable scenario, but just in case there were
		/// ever two localized names (in the same or different languages) that happened to
		/// be translations of two different character IDs used in the same verse, we want
		/// it to be treated as ambiguous.
		/// </summary>
		[Test]
		public void GetStandardCharacterName_LocalizedCharacterNameCorrespondsToMultipleCharacters_ReturnsNull()
		{
			var store = new CharacterUsageStore(ScrVers.English, 
				ControlCharacterVerseData.Singleton, GetLocalizedVariants);
			Assert.IsNull(store.GetStandardCharacterName("Unrealistic scenario",
				BCVRef.BookToNumber("MRK"), 6, new[] {new Verse("38")},
				out var delivery, out var defaultCharacter));
			Assert.IsNull(delivery);
			Assert.IsNull(defaultCharacter);
		}

		[Test]
		public void GetStandardCharacterName_LocalizedCharacterWithSingleDelivery_ReturnsEnglishCharacterName()
		{
			var store = new CharacterUsageStore(ScrVers.English, 
				ControlCharacterVerseData.Singleton, GetLocalizedVariants);
			Assert.AreEqual("Jesus", store.GetStandardCharacterName("Jésus", BCVRef.BookToNumber("MRK"),
				6, new[] {new Verse("38")}, out var delivery, out var defaultCharacter));
			Assert.AreEqual("questioning", delivery);
			Assert.IsNull(defaultCharacter);
		}

		[TestCase("ZEC", 1, "10-12", "Zacarías (en una visión)", ExpectedResult = "Zechariah the prophet, son of Berechiah")]
		public string GetStandardCharacterName_LocalizedAliasWithNoDelivery_ReturnsEnglishCharacterName(
			string bookCode, int chapterNum, string verse, string alias)
		{
			var store = new CharacterUsageStore(ScrVers.English, 
				ControlCharacterVerseData.Singleton, GetLocalizedVariants);

			var result = store.GetStandardCharacterName(alias, BCVRef.BookToNumber(bookCode),
				chapterNum, new[] { new Verse(verse) }, out var delivery, out var defaultCharacter);

			Assert.IsNull(delivery);
			Assert.IsNull(defaultCharacter);

			return result;
		}

		[Test]
		public void GetStandardCharacterName_LocalizedGroupCharacterWithDefault_ReturnsEnglishCharacterNameAndDefault()
		{
			var store = new CharacterUsageStore(ScrVers.English, 
				ControlCharacterVerseData.Singleton, GetLocalizedVariants);
			Assert.AreEqual("Barnabas/Paul", store.GetStandardCharacterName("Barnabus/Paulus", BCVRef.BookToNumber("ACT"),
				14, new[] {new Verse("15-16"), new Verse("17")}, out var delivery, out var defaultCharacter));
			Assert.AreEqual("preaching", delivery);
			Assert.AreEqual("Paul", defaultCharacter);
		}

		[Test]
		public void GetStandardCharacterName_KnownCharacterWithMultipleDeliveries_ReturnsCharacterNameAndNullDelivery()
		{
			var store = new CharacterUsageStore(ScrVers.English, 
				ControlCharacterVerseData.Singleton, GetLocalizedVariants);
			Assert.AreEqual("Jesus", store.GetStandardCharacterName("Jesus", BCVRef.BookToNumber("MAT"),
				14, new[] {new Verse("19")}, out var delivery, out var defaultCharacter));
			Assert.IsNull(delivery);
			Assert.IsNull(defaultCharacter);
		}

		[Test]
		public void GetStandardCharacterName_LocalizedCharacterWithMultipleDeliveries_ReturnsCharacterNameAndNullDelivery()
		{
			var store = new CharacterUsageStore(ScrVers.English, 
				ControlCharacterVerseData.Singleton, GetLocalizedVariants);
			Assert.AreEqual("Jesus", store.GetStandardCharacterName("Jesucristo", BCVRef.BookToNumber("MAT"),
				14, new[] {new Verse("19")}, out var delivery, out var defaultCharacter));
			Assert.IsNull(delivery);
			Assert.IsNull(defaultCharacter);
		}

		[Test]
		public void GetStandardCharacterName_SpecificCharacterInList_ReturnsFullCharacterListWithSpecifiedCharacterAsDefault()
		{
			var store = new CharacterUsageStore(ScrVers.English, 
				ControlCharacterVerseData.Singleton, GetLocalizedVariants);
			Assert.AreEqual("Pharisees/teachers of religious law", store.GetStandardCharacterName("teachers of religious law", BCVRef.BookToNumber("MRK"),
				7, new[] {new Verse("5")}, out var delivery, out var defaultCharacter));
			Assert.AreEqual("critical", delivery);
			Assert.AreEqual("teachers of religious law", defaultCharacter);
		}

		[TestCase("Pharisees and teachers of religious law")]
		[TestCase("Pharisees/teachers of religious law ")]
		[TestCase(" Pharisees/teachers of religious law")]
		[TestCase("pharisees/teachers of religious law")]
		[TestCase("Pharisees / teachers of religious law")]
		[TestCase("Pharisees/teachers of religious law (bad)")]
		[TestCase("Pharisees/teachers of religious-law")]
		[TestCase("pharisees and Teachers of religious law")]
		[TestCase("Pharisees teachers of religious law")]
		[TestCase("teachers of religious law/Pharisees")]
		[TestCase("pharisees")]
		[TestCase("fariseos")]
		[TestCase("फरीसियों")]
		public void GetStandardCharacterName_CloseMatchToCharacter_ReturnsStandardCharacterName(string close)
		{
			var store = new CharacterUsageStore(ScrVers.English, 
				ControlCharacterVerseData.Singleton, GetLocalizedVariants);
			Assert.AreEqual("Pharisees/teachers of religious law", store.GetStandardCharacterName(close, BCVRef.BookToNumber("MRK"),
				7, new[] {new Verse("5")}, out var delivery, out var defaultCharacter));
			Assert.AreEqual("critical", delivery);
			Assert.AreEqual("Pharisees", defaultCharacter);
		}

		[TestCase("teachers of religious (Jewish OT) law")]
		[TestCase("teachers ofreligiouslaw")]
		[TestCase("धार्मिक कानून शिक्षक")]
		public void GetStandardCharacterName_CloseMatchToNonDefaultCharacter_ReturnsStandardCharacterNameAndMatchingDefault(string close)
		{
			var store = new CharacterUsageStore(ScrVers.English, 
				ControlCharacterVerseData.Singleton, GetLocalizedVariants);
			Assert.AreEqual("Pharisees/teachers of religious law", store.GetStandardCharacterName(close, BCVRef.BookToNumber("MRK"),
				7, new[] {new Verse("5")}, out var delivery, out var defaultCharacter));
			Assert.AreEqual("critical", delivery);
			Assert.AreEqual("teachers of religious law", defaultCharacter);
		}

		[TestCase("Enoc", ExpectedResult = "Enoch")]
		[TestCase("Enóch", ExpectedResult = "Enoch")]
		[TestCase("enoc", ExpectedResult = "Enoch")]
		[TestCase("Enocho", ExpectedResult = "Enoch")]
		[TestCase("Enoc (quoted)", ExpectedResult = "Enoch")]
		[TestCase("Enock (quotation)", ExpectedResult = "Enoch")]
		[TestCase("voice of man calling (preparing the way for Christ)", "ISA", 40, "3",
			ExpectedResult = "voice of one calling (preparing way for Christ)")]
		[TestCase("singer in Judea", "ISA", 26, "1-6", "singing",
			ExpectedResult = "singers in Judah")]
		public string GetStandardCharacterName_CloseMatchToOnlyCharacter_ReturnsStandardCharacterName(
			string close, string bookCode = "JUD", int chapterNum = 1, string verse = "14-15",
			string expectedDelivery = null)
		{
			var store = new CharacterUsageStore(ScrVers.English, 
				ControlCharacterVerseData.Singleton, GetLocalizedVariants);
			var result = store.GetStandardCharacterName(close, BCVRef.BookToNumber(bookCode),
				chapterNum, new[] {new Verse(verse)}, out var delivery, out _);
			Assert.AreEqual(expectedDelivery, delivery);
			return result;
		}

		[TestCase("MRK", 6, "15", "people, st others")]
		[TestCase("MRK", 6, "15", "peoples, others")]
		[TestCase("MRK", 6, "15", "people, some others")]
		[TestCase("MRK", 6, "15", "people, others")]
		[TestCase("LUK", 22, "70", "यीशु शिक्षक")] // "Jesus the teacher"
		public void GetStandardCharacterName_CloseMatchToMultipleCharactersWithNoClearWinner_ReturnsNull(
			string bookCode, int chapterNum, string verse, string close)
		{
			var store = new CharacterUsageStore(ScrVers.English, 
				ControlCharacterVerseData.Singleton, GetLocalizedVariants);
			Assert.That(store.GetStandardCharacterName(close, BCVRef.BookToNumber(bookCode),
				chapterNum, new[] {new Verse(verse)}, out _, out _), Is.Null);
		}

		[TestCase("ISA", 41, "6", "islander", null, ExpectedResult = "islanders")]
		[TestCase("ISA", 28, "9", "hearer", "mocking", ExpectedResult = "hearers")]
		public string GetStandardCharacterName_CloseMatchInVerseWithMultipleCharactersWithClearWinner_ReturnsNull(
			string bookCode, int chapterNum, string verse, string close, string expectedDelivery)
		{
			var store = new CharacterUsageStore(ScrVers.English, 
				ControlCharacterVerseData.Singleton, GetLocalizedVariants);
			var result = store.GetStandardCharacterName(close, BCVRef.BookToNumber(bookCode),
				chapterNum, new[] {new Verse(verse)}, out var delivery, out _);
			Assert.AreEqual(expectedDelivery, delivery);
			return result;
		}

		[TestCase("REV", 1, "17", "someone li", ExpectedResult = "Jesus")]
		[TestCase("REV", 1, "17", "son of man", ExpectedResult = "Jesus")]
		[TestCase("REV", 1, "17", "Son of man", ExpectedResult = "Jesus")]
		[TestCase("ISA", 45, "24", "tongue", ExpectedResult = "every tongue")]
		public string GetStandardCharacterName_OnlyMatchingSubstring_ReturnsStandardCharacterName(
			string bookCode, int chapterNum, string verse, string alias)
		{
			var store = new CharacterUsageStore(ScrVers.English, 
				ControlCharacterVerseData.Singleton, GetLocalizedVariants);
			return store.GetStandardCharacterName(alias, BCVRef.BookToNumber(bookCode),
				chapterNum, new[] {new Verse(verse)}, out _, out _);
		}

		[TestCase("REV", 1, "17", "of")]
		[TestCase("REV", 1, "17", "ome")]
		[TestCase("REV", 1, "17", "Son")]
		[TestCase("REV", 1, "17", "son")]
		public void GetStandardCharacterName_ShortOrPartialWordMatchingSubstring_ReturnsNull(
			string bookCode, int chapterNum, string verse, string alias)
		{
			var store = new CharacterUsageStore(ScrVers.English, 
				ControlCharacterVerseData.Singleton, GetLocalizedVariants);
			Assert.That(store.GetStandardCharacterName(alias, BCVRef.BookToNumber(bookCode),
				chapterNum, new[] {new Verse(verse)}, out _, out _), Is.Null);
		}

		[TestCase("ISA", 63, "4", "garments")]
		[TestCase("ZEC", 1, "10", "angel")]
		public void GetStandardCharacterName_SubstringMatchesMultipleEntries_ReturnsNull(
			string bookCode, int chapterNum, string verse, string alias)
		{
			var store = new CharacterUsageStore(ScrVers.English, 
				ControlCharacterVerseData.Singleton, GetLocalizedVariants);
			Assert.That(store.GetStandardCharacterName(alias, BCVRef.BookToNumber(bookCode),
				chapterNum, new[] {new Verse(verse)}, out _, out _), Is.Null);
		}

		[TestCase("REV", 1, "17", "someone like a son of man", ExpectedResult = "Jesus")]
		[TestCase("ISA", 41, "6", "everyone (islanders or foreigners)", ExpectedResult = "islanders")]
		[TestCase("ISA", 41, "6", "everyone", ExpectedResult = "islanders")]
		[TestCase("ISA", 41, "6", "islanders", ExpectedResult = "islanders")]
		[TestCase("ISA", 41, "6", "foreigners", ExpectedResult = "islanders")]
		public string GetStandardCharacterName_KnownCharacterWithMatchingAlias_ReturnsStandardCharacterName(
			string bookCode, int chapterNum, string verse, string alias)
		{
			var store = new CharacterUsageStore(ScrVers.English, 
				ControlCharacterVerseData.Singleton, GetLocalizedVariants);
			return store.GetStandardCharacterName(alias, BCVRef.BookToNumber(bookCode),
				chapterNum, new[] {new Verse(verse)}, out _, out _);
		}

		[Test]
		public void GetStandardCharacterName_CharacterIsGroupConsistingOfMultipleKnownCharacters_ReturnsNull()
		{
			var store = new CharacterUsageStore(ScrVers.English, 
				ControlCharacterVerseData.Singleton, GetLocalizedVariants);
			Assert.IsNull(store.GetStandardCharacterName("Andrew/Jesus", BCVRef.BookToNumber("MRK"),
				6, new[] {new Verse("38")}, out var delivery, out var defaultCharacter));
			Assert.IsNull(delivery);
			Assert.IsNull(defaultCharacter);
		}

		[Test]
		public void GetStandardCharacterName_CharacterIsInMoreThanOneKnownGroup_ReturnsNull()
		{
			var store = new CharacterUsageStore(ScrVers.English, 
				ControlCharacterVerseData.Singleton, GetLocalizedVariants);
			Assert.IsNull(store.GetStandardCharacterName("Asher", BCVRef.BookToNumber("GEN"),
				6, new[] {new Verse("38")}, out var delivery, out var defaultCharacter));
			Assert.IsNull(delivery);
			Assert.IsNull(defaultCharacter);
		}

		[TestCase("narrator-MAT")]
		[TestCase("narrator (MAT)")]
		[TestCase("narrador (MAT)")]
		public void GetStandardCharacterName_NarratorScareQuote_ReturnsStandardCharacterName(string narratorChar)
		{
			var store = new CharacterUsageStore(ScrVers.English,
				ControlCharacterVerseData.Singleton, GetLocalizedVariants);
			Assert.AreEqual("narrator-MAT", store.GetStandardCharacterName(narratorChar, BCVRef.BookToNumber("MAT"),
				2, new[] {new Verse("1")}, out var delivery, out var defaultCharacter));
			Assert.IsNull(delivery);
			Assert.IsNull(defaultCharacter);
		}

		[TestCase("JUD", 1, "14-15", "Anacho")]
		[TestCase("MAT", 2, "1", "buggaboo snerfwiddle")]
		public void GetStandardCharacterName_UnknownCharacter_ReturnsNull(string bookCode,
			int chapterNum, string verse, string character)
		{
			var store = new CharacterUsageStore(ScrVers.English,
				ControlCharacterVerseData.Singleton, GetLocalizedVariants);
			Assert.IsNull(store.GetStandardCharacterName(character, BCVRef.BookToNumber(bookCode),
				chapterNum, new[] {new Verse(verse)}, out var delivery, out var defaultCharacter));
			Assert.IsNull(delivery);
			Assert.IsNull(defaultCharacter);
		}

		[Test] public void GetStandardCharacterName_KnownCharacterNotExpectedInVerse_ReturnsNull()
		{
			var store = new CharacterUsageStore(ScrVers.English,
				ControlCharacterVerseData.Singleton, GetLocalizedVariants);
			Assert.IsNull(store.GetStandardCharacterName("Jesus", BCVRef.BookToNumber("MAT"),
				1, new[] {new Verse("1")}, out var delivery, out var defaultCharacter));
			Assert.IsNull(delivery);
			Assert.IsNull(defaultCharacter);
		}

		// Note: In production, we have tried to clean up this kind of C-V data so this can't
		// happen because it seldom makes sense. This test uses an older test version of the
		// CV file to test this condition, in case there are places where it is still possible.
		[Test] public void GetStandardCharacterName_TwoVersesWithSameCharacterGroupButDifferentDefaults_UsesDefaultFromFirstVerse()
		{
			try
			{
				ControlCharacterVerseData.TabDelimitedCharacterVerseData = Resources.TestCharacterVerseOct2015;
				var store = new CharacterUsageStore(ScrVers.English,
					ControlCharacterVerseData.Singleton, GetLocalizedVariants);
				Assert.AreEqual("Peter (Simon)/John", store.GetStandardCharacterName("Peter (Simon)/John", BCVRef.BookToNumber("ACT"),
					4, new[] {new Verse("19"), new Verse("20")}, out var delivery, out var defaultCharacter));
				Assert.IsNull(delivery);
				Assert.AreEqual("Peter (Simon)", defaultCharacter);
			}
			finally
			{
				ControlCharacterVerseData.TabDelimitedCharacterVerseData = null;
			}
		}

		private IEnumerable<string> GetLocalizedVariants(string englishCharId)
		{
			switch (englishCharId)
			{
				case "Andrew":
					yield return "Andrés";
					yield return "Andy";
					yield return "Unrealistic scenario";
					break;

				case "Jesus":
					yield return "Jesucristo";
					yield return "Jésus";
					yield return "यीशु";
					yield return "Unrealistic scenario";
					break;

				case "narrator-MAT":
					yield return "narrator (MAT)";
					yield return "narrador (MAT)";
					break;

				case "Barnabas/Paul":
					yield return "Bernabé/Pablo";
					yield return "Barnabus/Paulus";
					break;

				case "Zechariah (in vision)":
					yield return "Zacarías (en una visión)";
					break;

				case "Pharisees":
					yield return "fariseos";
					yield return "फरीसियों";
					break;

				case "teachers of religious law":
					yield return "धार्मिक कानून के शिक्षक";
					break;
			}
		}
	}
}
