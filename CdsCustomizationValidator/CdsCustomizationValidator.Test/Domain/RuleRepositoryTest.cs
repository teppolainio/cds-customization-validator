﻿using CdsCustomizationValidator.Domain;
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