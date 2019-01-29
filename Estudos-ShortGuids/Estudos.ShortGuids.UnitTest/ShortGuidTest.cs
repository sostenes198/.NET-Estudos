using System;
using FluentAssertions;
using Xunit;

namespace Estudos.ShortGuids.UnitTest;

public class ShortGuidTest
    {
        private const string GuidTest = "b44ff3da-72bd-40cd-93ae-782fe5fe32a9";
        private const string StringShortGuidTest = "2vNPtL1yzUCTrngv5f4yqQ";

        [Fact(DisplayName = "Should throw exception when guid is empty")]
        public void ShouldThrownExceptionWhenGuidIsEmpty()
        {
            // arrange
            Action act = () => _ = new ShortGuid(Guid.Empty);


            // act
            var exceptionAssertions = act.Should().Throw<ArgumentNullException>();

            // assert
            exceptionAssertions.Which.ParamName.Should().BeEquivalentTo("guid");
            exceptionAssertions.Which.Message.Should().BeEquivalentTo("Value cannot be null. (Parameter 'guid')");
        }

        [Fact(DisplayName = "Should get short guid in string")]
        public void ShouldGetStringShortGuid()
        {
            // arrange
            var shortGuid = new ShortGuid(Guid.Parse(GuidTest));

            // act
            var stringShortGuid = shortGuid.ToString();

            // assert
            stringShortGuid.Should().BeEquivalentTo(stringShortGuid);
        }

        [Fact(DisplayName = "Should get guid")]
        public void ShouldGetGuid()
        {
            // arrange
            var shortGuid = new ShortGuid(Guid.Parse(GuidTest));

            // act
            var guid = shortGuid.ToGuid();

            // assert
            guid.Should().Be(Guid.Parse(GuidTest));
        }

        [Fact(DisplayName = "Should perform convert short guid string to short guid")]
        public void ShouldParseStringToShortGuid()
        {
            // arrange

            // act
            var shortGuid = ShortGuid.Parse(StringShortGuidTest);

            // assert
            shortGuid.ToString().Should().BeEquivalentTo(StringShortGuidTest);
            shortGuid.ToGuid().Should().Be(Guid.Parse(GuidTest));
        }

        [Fact(DisplayName = "Should throw exception when trying to convert string to short guid and string is empty")]
        public void ShouldThrownExceptionWhenParseStringToShortGuidAndStringIsEmpty()
        {
            // arrange
            Action act = () => ShortGuid.Parse(string.Empty);

            // act
            var exceptionAssertions = act.Should().Throw<ArgumentNullException>();

            // assert
            exceptionAssertions.Which.ParamName.Should().BeEquivalentTo("shortGuid");
            exceptionAssertions.Which.Message.Should().BeEquivalentTo("Value cannot be null. (Parameter 'shortGuid')");
        }

        [Fact(DisplayName = "Should throw exception when trying to convert string to short guid and string is not the correct length")]
        public void ShouldThrownExceptionWhenParseStringToShortGuidAndStringIncorrectSize()
        {
            // arrange
            Action act = () => ShortGuid.Parse("none");

            // act
            var exceptionAssertions = act.Should().Throw<FormatException>();

            // assert
            exceptionAssertions.Which.Message.Should().BeEquivalentTo("Input is not in the correct format");
        }

        [Fact(DisplayName = "Should create short guid")]
        public void ShouldCreateShortGuid()
        {
            // arrange
            Action act = () => ShortGuid.NewShortGuid();

            // act - assert
            act.Should().NotThrow();
        }

        [Fact(DisplayName = "Should validate implicit converters")]
        public void ShouldValidateImplictOperator()
        {
            // arrange
            var shortGuid = ShortGuid.Parse(StringShortGuidTest);

            // act
            string stringShortGuid = shortGuid;
            Guid guid = shortGuid;

            // assert
            stringShortGuid.Should().BeEquivalentTo(StringShortGuidTest);
            guid.Should().Be(Guid.Parse(GuidTest));
        }
    }