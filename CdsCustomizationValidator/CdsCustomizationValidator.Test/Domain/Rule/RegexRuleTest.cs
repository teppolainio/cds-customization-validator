using CdsCustomizationValidator.Domain.Rule;
using System;
using Xunit;

namespace CdsCustomizationValidator.Test.Domain.Rule
{

    /// <summary>
    /// Unit tests for rule <see cref="RegexRule"/> which are not bind to any
    /// specific scope or are bind to multiple scopes.
    /// </summary>
    public class RegexRuleTest
    {

        [Theory(DisplayName = "RegexRule: Invalid pattern must throw an exception.")]
        [InlineData(RuleScope.Entity)]
        [InlineData(RuleScope.Attribute)]
        public void InvalidRegexPatternMustFailOnObjectInitialization(
            RuleScope scope)
        {

            var invalidPattern = "[";

            var ex = Assert.Throws<ArgumentException>(
                () => new RegexRule(invalidPattern, scope));

            Assert.Equal("Given pattern \"[\" was invalid regular expression.",
                         ex.Message);
            Assert.NotNull(ex.InnerException);
            Assert.IsType<ArgumentException>(ex.InnerException);

        }

    }
}
