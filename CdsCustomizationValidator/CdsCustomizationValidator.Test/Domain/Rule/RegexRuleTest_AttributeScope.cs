using CdsCustomizationValidator.Domain.Rule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CdsCustomizationValidator.Test.Domain.Rule
{

    /// <summary>
    /// Unit tests for rule <see cref="RegexRule"/> for entity scope.
    /// </summary>
    public class RegexRuleTest_AttributeScope
    {

        [Fact(DisplayName = "RegexRule: Rule description must show attribute scope and pattern.")]
        public void RuleDescriptionMustShowScopeAndPatternTest()
        {
            var regexPattern = @"^[A-Za-z]+_[A-Z]{1}[a-z]{1}[A-Za-z]*$";
            var scope = RuleScope.Attribute;

            var ruleToTest = new RegexRule(regexPattern, scope);

            Assert.Equal($"Schema name of an Attribute must match to regular expression pattern {regexPattern}.",
                         ruleToTest.Description);
        }

    }

}
