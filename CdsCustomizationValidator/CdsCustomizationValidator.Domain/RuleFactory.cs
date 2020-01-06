using CdsCustomizationValidator.Domain.Rule;
using System;
using DTO = CdsCustomizationValidator.Infrastructure.DTO;

namespace CdsCustomizationValidator.Domain
{

    /// <summary>
    /// Factory for creating new rule instances.
    /// </summary>
    internal class RuleFactory
    {

        /// <summary>
        /// Creates a new rule from deserialized rule representation.
        /// </summary>
        /// <param name="deserializedRule">
        /// Deserialized rule.
        /// </param>
        /// <returns>
        /// The rule from <paramref name="deserializedRule"/>.
        /// </returns>
        internal Rule.DisallowSolutionToOwnManagedEntitiesRule CreateFrom(
            DTO.DisallowSolutionToOwnManagedEntitiesRule deserializedRule)
        {
            if (deserializedRule is null)
            {
                throw new ArgumentNullException(nameof(deserializedRule));
            }

            var allow = deserializedRule.allow;
            return new Rule.DisallowSolutionToOwnManagedEntitiesRule(allow);
        }

        /// <summary>
        /// Creates a new rule from deserialized rule representation.
        /// </summary>
        /// <param name="deserializedRule">
        /// Deserialized rule.
        /// </param>
        /// <returns>
        /// The rule from <paramref name="deserializedRule"/>.
        /// </returns>
        internal EntityPrefixRule CreateFrom(
            DTO.EntityPrefixRule entityPrefixRule)
        {
            if (entityPrefixRule is null)
            {
                throw new ArgumentNullException(nameof(entityPrefixRule));
            }

            var prefix = entityPrefixRule.schemaPrefix;
            return new EntityPrefixRule(prefix);
        }

        /// <summary>
        /// Creates a new rule from deserialized rule representation.
        /// </summary>
        /// <param name="deserializedRule">
        /// Deserialized rule.
        /// </param>
        /// <returns>
        /// The rule from <paramref name="deserializedRule"/>.
        /// </returns>
        internal AttributePrefixRule CreateFrom(
            DTO.AttributePrefixRule deserializedRule)
        {
            if (deserializedRule is null)
            {
                throw new ArgumentNullException(nameof(deserializedRule));
            }

            var prefix = deserializedRule.schemaPrefix;
            return new Rule.AttributePrefixRule(prefix);
        }

        /// <summary>
        /// Creates a new rule from deserialized rule representation.
        /// </summary>
        /// <param name="deserializedRule">
        /// Deserialized rule.
        /// </param>
        /// <returns>
        /// The rule from <paramref name="deserializedRule"/>.
        /// </returns>
        internal Rule.RegexRule CreateFrom(DTO.RegexRule deserializedRule)
        {
            RuleScope? scope = null;
            switch (deserializedRule.scope)
            {
                case DTO.RuleScope.Entity:
                    scope = RuleScope.Entity;
                    break;
                case DTO.RuleScope.Attribute:
                    scope = RuleScope.Attribute;
                    break;
                default:
                    throw new NotImplementedException(
                        $"No mapping for DTO rule scope {deserializedRule.scope}."
                        );
            }

            var rule = new RegexRule(deserializedRule.pattern,
                                     scope.Value);
            return rule;
        }
    }
}