using FluentAssertions;
using NUnit.Framework;
using Pathoschild.Stardew.ModTranslationClassBuilder;

namespace ModTranslationClassBuilder.Tests
{
    /// <summary>Unit tests for <see cref="TranslationClassBuilder"/>.</summary>
    [TestFixture]
    public class TranslationClassBuilderTests
    {
        /// <summary>Assert that <see cref="TranslationClassBuilder.TryParseTranslationFilePath"/></summary>
        [Test]

        // valid default
        [TestCase("i18n/default.json", true, true, true)]
        [TestCase("I18N/DEfauLT.JSON", true, true, true)]
        [TestCase("i18n/default/any-file.json", true, false, true)]
        [TestCase("I18N/deFAULT/any-FILE.JSON", true, false, true)]

        // valid non-default
        [TestCase("i18n/fr.json", true, true, false)]
        [TestCase("I18N/FR.JSON", true, true, false)]
        [TestCase("i18n/fr/default.json", true, false, false)]
        [TestCase("I18N/FR/deFAULT.JSON", true, false, false)]

        // invalid
        [TestCase("any-file.json", false, false, false)]
        [TestCase("i18n/default.cs", false, false, false)]
        [TestCase("i18n/default", false, false, false)]
        [TestCase("i18n/fr", false, false, false)]
        [TestCase("i18n/default/default/default.json", false, false, false)]
        [TestCase("i18n/fr/fr/default.json", false, false, false)]
        public void TryParseTranslationFilePath(string path, bool expectValid, bool expectRoot, bool expectDefaultLocale)
        {
            // act
            bool valid = new TranslationClassBuilder().TryParseTranslationFilePath(path, out bool root, out bool defaultLocale);

            // assert
            valid.Should().Be(expectValid);
            root.Should().Be(expectRoot);
            defaultLocale.Should().Be(expectDefaultLocale);
        }
    }
}
