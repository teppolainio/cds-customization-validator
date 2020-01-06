using CdsCustomizationValidator.Domain.Rule;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using DTO = CdsCustomizationValidator.Infrastructure.DTO;

namespace CdsCustomizationValidator.Domain
{

    /// <summary>
    /// Repository for interacting with rules persisted into some long term
    /// storage (like file system).
    /// </summary>
    public class RuleRepository
    {

        /// <summary>
        /// Gets the customization rules persisted on given file location.
        /// </summary>
        /// <param name="rulesFileLocation">
        /// Location of the file containing rules.
        /// </param>
        /// <returns>
        /// List of rules from <paramref name="rulesFileLocation"/>.
        /// </returns>
        public List<CustomizationRuleBase> GetRules(string rulesFileLocation) {

            var serializer = new XmlSerializer(typeof(DTO.CustomizationRule));

            var filename = Environment.CurrentDirectory + "\\" + rulesFileLocation;

            DTO.CustomizationRule rules = null;

            using (var stream = new FileStream(filename,
                                               FileMode.Open,
                                               FileAccess.Read))
            {
                rules = serializer.Deserialize(stream) as DTO.CustomizationRule;
            }

            var retval = new List<CustomizationRuleBase>();

            var ruleFactory = new RuleFactory();

            if (rules.DisallowSolutionToOwnManagedEntitiesRule != null)
            {
                var rule = ruleFactory.CreateFrom(rules.DisallowSolutionToOwnManagedEntitiesRule);
                retval.Add(rule);
            }
            if (rules.EntityPrefixRule != null)
            {
                var rule = ruleFactory.CreateFrom(rules.EntityPrefixRule);
                retval.Add(rule);
            }

            return retval;
        }

    }

}
