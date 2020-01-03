using System;
using System.Text.RegularExpressions;

namespace CdsCustomizationValidator.Domain.Rule
{


    /// <summary>
    /// Scope of the rule.
    /// </summary>
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

        /// <summary>
        /// See <see cref="CustomizationRuleBase.ValidateRule(SolutionEntity)"/>.
        /// </summary>
        protected override ValidationResult ValidateRule(SolutionEntity solutionEntity)
        {

            bool validationPassed = false;
            if (solutionEntity.IsOwnedBySolution == false ||
                solutionEntity.Entity.IsManaged == true)
            {
                validationPassed = true;
            }
            else
            {
                validationPassed = _regexPattern.IsMatch(solutionEntity.Entity
                                                                       .SchemaName);
            }       

            return new ValidationResult(solutionEntity.Entity,
                                        validationPassed,
                                        this);
        }

        private readonly Regex _regexPattern;
        private readonly RuleScope _scope;

    }
}
