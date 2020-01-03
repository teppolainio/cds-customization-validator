using System;
using System.Collections.Generic;
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
            get { return $"Schema name of {_scope} must match to regular expression pattern {Pattern}."; }
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
            : this(regexPattern, scope, new List<string>())
        {   
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
        /// <param name="excludedSchemaNames">
        /// List of schema names which are exluded from the rule.
        /// </param>
        public RegexRule(string regexPattern,
                         RuleScope scope,
                         ICollection<string> excludedSchemaNames)
        {
            if (excludedSchemaNames == null)
            {
                throw new ArgumentNullException(nameof(excludedSchemaNames));
            }

            try
            {
                Pattern = new Regex(regexPattern);
            }
            catch(ArgumentException ex) {
                throw new ArgumentException(
                    $"Given pattern \"{regexPattern}\" was invalid regular expression.",
                    ex);
            }

            _scope = scope;

            _excludedSchemaNames = excludedSchemaNames;
        }

        /// <summary>
        /// See <see cref="CustomizationRuleBase.ValidateRule(SolutionEntity)"/>.
        /// </summary>
        protected override ValidationResult ValidateRule(
            SolutionEntity solutionEntity)
        {

            var entity = solutionEntity.Entity;

            bool validationPassed = false;
            if (solutionEntity.IsOwnedBySolution == false ||
                entity.IsManaged == true ||
                _excludedSchemaNames.Contains(entity.SchemaName))
            {
                validationPassed = true;
            }
            else
            {
                validationPassed = Pattern.IsMatch(entity.SchemaName);
            }       

            var retval = new RegexValidationResult(solutionEntity.Entity,
                                                   validationPassed,
                                                   this);

            return retval as ValidationResult;
        }

        internal Regex Pattern { get; }

        private readonly RuleScope _scope;
        private readonly ICollection<string> _excludedSchemaNames;
    }
}
