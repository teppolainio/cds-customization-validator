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
    }
}