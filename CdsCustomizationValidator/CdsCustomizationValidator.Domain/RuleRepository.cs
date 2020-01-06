using CdsCustomizationValidator.Domain.Rule;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            throw new NotImplementedException();
        }

    }

}
