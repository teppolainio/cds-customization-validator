using CdsCustomizationValidator.Domain.Rule;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using DTO = CdsCustomizationValidator.Infrastructure.DTO;

namespace CdsCustomizationValidator.Domain {

  /// <summary>
  /// Repository for interacting with rules persisted into some long term
  /// storage (like file system).
  /// </summary>
  public class RuleRepository {

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

      var filename = rulesFileLocation;
      if(!IsPathValidRootedLocal(filename)) {
        filename = Environment.CurrentDirectory + "\\" + rulesFileLocation;
      }

      DTO.CustomizationRule rules = null;

      using(var stream = new FileStream(filename,
                                         FileMode.Open,
                                         FileAccess.Read)) {
        rules = serializer.Deserialize(stream) as DTO.CustomizationRule;
      }

      return GetRules(rules);
    }

    /// <summary>
    /// Get rules from deserialized rules object.
    /// This is internal so we can get unit tests to run nicely.
    /// </summary>
    /// <param name="rules">
    /// Deserialized rules object.
    /// </param>
    /// <returns>
    /// List of rules from <paramref name="rules"/>.
    /// </returns>
    internal static List<CustomizationRuleBase> GetRules(
        DTO.CustomizationRule rules) {
      if(rules is null) {
        throw new ArgumentNullException(nameof(rules));
      }

      var retval = new List<CustomizationRuleBase>();

      var ruleFactory = new RuleFactory();

      if(rules.DisallowSolutionToOwnManagedEntitiesRule != null) {
        var rule = ruleFactory.CreateFrom(rules.DisallowSolutionToOwnManagedEntitiesRule);
        retval.Add(rule);
      }
      if(rules.EntityPrefixRule != null) {
        var rule = ruleFactory.CreateFrom(rules.EntityPrefixRule);
        retval.Add(rule);
      }
      if(rules.AttributePrefixRule != null) {
        var rule = ruleFactory.CreateFrom(rules.AttributePrefixRule);
        retval.Add(rule);
      }
      if(rules.RegexRules != null) {
        foreach(var dtoRule in rules.RegexRules) {
          var rule = ruleFactory.CreateFrom(dtoRule);
          retval.Add(rule);
        }
      }

      return retval;
    }

    // https://stackoverflow.com/a/11636052
    private bool IsPathValidRootedLocal(string pathString) {
      Uri pathUri;
      var isValidUri = Uri.TryCreate(pathString,
                                     UriKind.Absolute,
                                     out pathUri);
      return isValidUri && pathUri != null && pathUri.IsLoopback;
    }

  }

}
