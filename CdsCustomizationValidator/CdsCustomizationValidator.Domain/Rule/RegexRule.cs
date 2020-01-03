using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CdsCustomizationValidator.Domain.Rule
{


    public enum RuleScope { 
        Entity
    };

    /// <summary>
    /// Rule which allows to define rules using regular expression.
    /// Expressions need to be in .NET regular expression format.
    /// </summary>
    public class RegexRule : CustomizationRuleBase
    {

        /// <summary>
        /// See <see cref="CustomizationRuleBase.Description"/>.
        /// </summary>
        public override string Description
        {
            get { return $"Logical name of {_scope} must match to regular expression pattern {_regexPattern}."; }
        }

        /// <summary>
        /// Creates a new rule instance requiring naming to match given
        /// pattern on certain scope.
        /// </summary>
        /// <param name="regexPattern">
        /// Regular expression pattern which must be met on scope given by
        /// parameter <paramref name="scope"/>.
        /// </param>
        /// <param name="scope">
        /// Scope of the rule.
        /// </param>
        public RegexRule(string regexPattern, RuleScope scope)
        {
            try
            {
                _regexPattern = new Regex(regexPattern);
            }
            catch(ArgumentException ex) {
                throw new ArgumentException(
                    $"Given pattern \"{regexPattern}\" was invalid regular expression.",
                    ex);
            }

            _scope = scope;
        }

        protected override ValidationResult ValidateRule(SolutionEntity solutionEntity)
        {
            throw new NotImplementedException();
        }

        private readonly Regex _regexPattern;
        private readonly RuleScope _scope;

    }
}
