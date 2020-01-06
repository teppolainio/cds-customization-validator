using CdsCustomizationValidator.Domain;
using CdsCustomizationValidator.Domain.Rule;
using System;
using System.Linq;
using System.Reflection;
using Xunit;
using DTO = CdsCustomizationValidator.Infrastructure.DTO;

namespace CdsCustomizationValidator.Test.Domain
{

    /// <summary>
    /// Unit tests for <see cref="RuleRepository"/>.
    /// </summary>
    public class RuleRepositoryTest
    {

        [Fact(DisplayName = "RuleRepository: DTO.DisallowSolutionToOwnManagedEntitiesRule implicit false.")]
        public void DtoDisallowSolutionToOwnManagedEntitiesRuleImplicitFalse()
        {
            var dtoRules = new DTO.CustomizationRule()
            {
                DisallowSolutionToOwnManagedEntitiesRule = new DTO.DisallowSolutionToOwnManagedEntitiesRule()
            };

            var rules = RuleRepository.GetRules(dtoRules);

            var rule = rules.Single() as DisallowSolutionToOwnManagedEntitiesRule;

            bool allowed = (bool)GetInstanceField(rule.GetType(),
                                                  rule,
                                                  "_allowSolutionToOwnManagedEntities");

            Assert.False(allowed);
        }

        [Fact(DisplayName = "RuleRepository: DTO.DisallowSolutionToOwnManagedEntitiesRule explicit false.")]
        public void DtoDisallowSolutionToOwnManagedEntitiesRuleExplicitFalse()
        {
            var dtoRules = new DTO.CustomizationRule()
            {
                DisallowSolutionToOwnManagedEntitiesRule = new DTO.DisallowSolutionToOwnManagedEntitiesRule()
                {
                    allow = false
                }
            };

            var rules = RuleRepository.GetRules(dtoRules);

            var rule = rules.Single() as DisallowSolutionToOwnManagedEntitiesRule;

            bool allowed = (bool)GetInstanceField(rule.GetType(),
                                                  rule,
                                                  "_allowSolutionToOwnManagedEntities");

            Assert.False(allowed);
        }

        [Fact(DisplayName = "RuleRepository: DTO.DisallowSolutionToOwnManagedEntitiesRule explicit true.")]
        public void DtoDisallowSolutionToOwnManagedEntitiesRuleExplicitTrue()
        {
            var dtoRules = new DTO.CustomizationRule()
            {
                DisallowSolutionToOwnManagedEntitiesRule = new DTO.DisallowSolutionToOwnManagedEntitiesRule()
                {
                    allow = true
                }
            };

            var rules = RuleRepository.GetRules(dtoRules);

            var rule = rules.Single() as DisallowSolutionToOwnManagedEntitiesRule;

            bool allowed = (bool)GetInstanceField(rule.GetType(),
                                                  rule,
                                                  "_allowSolutionToOwnManagedEntities");

            Assert.True(allowed);
        }

        [Fact(DisplayName = "RuleRepository: DTO.EntityPrefixRule value set to \"sar\".")]
        public void DtoEntityPrefixRuleSet()
        {
            var dtoRules = new DTO.CustomizationRule()
            {
                EntityPrefixRule = new DTO.EntityPrefixRule() { 
                    schemaPrefix = "sar"
                }
            };

            var rules = RuleRepository.GetRules(dtoRules);

            var rule = rules.Single() as EntityPrefixRule;

            string prefix = GetInstanceField(rule.GetType(),
                                             rule,
                                             "_schemaPrefix") as string;

            Assert.Equal("sar", prefix);
        }

        [Theory(DisplayName = "RuleRepository: DTO.EntityPrefixRule with illegal values.")]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void DtoEntityPrefixRuleIllegalValues(string prefix)
        {
            var dtoRules = new DTO.CustomizationRule()
            {
                EntityPrefixRule = new DTO.EntityPrefixRule()
                {
                    schemaPrefix = prefix
                }
            };

            var ex = Assert.Throws<ArgumentException>(
                () => RuleRepository.GetRules(dtoRules));

            Assert.StartsWith("Schema prefix isn't allowed to be null, empty or whitespace.",
                              ex.Message);
            Assert.Equal("schemaPrefix", ex.ParamName);
        }

        /// <summary>
        /// Uses reflection to get the field value from an object.
        /// </summary>
        /// <param name="type">The instance type.</param>
        /// <param name="instance">The instance object.</param>
        /// <param name="fieldName">The field's name which is to be fetched.</param>
        /// <returns>The field value from the object.</returns>
        /// <remarks>
        /// https://stackoverflow.com/a/3303182
        /// </remarks>
        private static object GetInstanceField(Type type, object instance, string fieldName)
        {
            BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                | BindingFlags.Static;
            FieldInfo field = type.GetField(fieldName, bindFlags);
            return field.GetValue(instance);
        }
    }
}
