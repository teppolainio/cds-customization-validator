using CdsCustomizationValidator.Domain;
using CdsCustomizationValidator.Domain.Rule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;
using DTO = CdsCustomizationValidator.Infrastructure.DTO;

namespace CdsCustomizationValidator.Test.Domain {

  /// <summary>
  /// Unit tests for <see cref="RuleRepository"/>.
  /// </summary>
  public class RuleRepositoryTest {

    [Fact(DisplayName = "RuleRepository: DTO.DisallowSolutionToOwnManagedEntitiesRule implicit false.")]
    public void DtoDisallowSolutionToOwnManagedEntitiesRuleImplicitFalse() {
      var dtoRules = new DTO.CustomizationRule() {
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
    public void DtoDisallowSolutionToOwnManagedEntitiesRuleExplicitFalse() {
      var dtoRules = new DTO.CustomizationRule() {
        DisallowSolutionToOwnManagedEntitiesRule = new DTO.DisallowSolutionToOwnManagedEntitiesRule() {
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
    public void DtoDisallowSolutionToOwnManagedEntitiesRuleExplicitTrue() {
      var dtoRules = new DTO.CustomizationRule() {
        DisallowSolutionToOwnManagedEntitiesRule = new DTO.DisallowSolutionToOwnManagedEntitiesRule() {
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
    public void DtoEntityPrefixRuleSet() {
      var dtoRules = new DTO.CustomizationRule() {
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
    public void DtoEntityPrefixRuleIllegalValues(string prefix) {
      var dtoRules = new DTO.CustomizationRule() {
        EntityPrefixRule = new DTO.EntityPrefixRule() {
          schemaPrefix = prefix
        }
      };

      var ex = Assert.Throws<ArgumentException>(
          () => RuleRepository.GetRules(dtoRules));

      Assert.StartsWith("Schema prefix isn't allowed to be null, empty or whitespace.",
                        ex.Message);
      Assert.Equal("schemaPrefix", ex.ParamName);
    }

    [Fact(DisplayName = "RuleRepository: DTO.AttributePrefixRule value set to \"sar\".")]
    public void DtoAttributePrefixRuleSet() {
      var dtoRules = new DTO.CustomizationRule() {
        AttributePrefixRule = new DTO.AttributePrefixRule() {
          schemaPrefix = "sar"
        }
      };

      var rules = RuleRepository.GetRules(dtoRules);

      var rule = rules.Single() as AttributePrefixRule;

      string prefix = GetInstanceField(rule.GetType(),
                                       rule,
                                       "_schemaPrefix") as string;

      Assert.Equal("sar", prefix);
    }

    [Theory(DisplayName = "RuleRepository: DTO.AttributePrefixRule with illegal values.")]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void DtoAttributePrefixRuleIllegalValues(string prefix) {
      var dtoRules = new DTO.CustomizationRule() {
        AttributePrefixRule = new DTO.AttributePrefixRule() {
          schemaPrefix = prefix
        }
      };

      var ex = Assert.Throws<ArgumentException>(
          () => RuleRepository.GetRules(dtoRules));

      Assert.StartsWith("Schema prefix isn't allowed to be null, empty or whitespace.",
                        ex.Message);
      Assert.Equal("schemaPrefix", ex.ParamName);
    }

    [Fact(DisplayName = "RuleRepository: DTO.RegexRule pattern at entity scope.")]
    public void DtoRegexRulePatternAtEntityScope() {
      var dtoRules = new DTO.CustomizationRule() {
        RegexRules = new DTO.RegexRule[]
          {
                    new DTO.RegexRule() {
                        pattern = @"^[A-Za-z]+_[A-Z]{1}[a-z]{1}[A-Za-z]*$",
                        scope = DTO.RuleScope.Entity
                    }
          }
      };

      var rules = RuleRepository.GetRules(dtoRules);

      var rule = rules.Single() as RegexRule;

      Assert.Equal(@"^[A-Za-z]+_[A-Z]{1}[a-z]{1}[A-Za-z]*$",
                   rule.Pattern.ToString());
      Assert.Equal(RuleScope.Entity, rule.Scope);
    }

    [Fact(DisplayName = "RuleRepository: DTO.RegexRule pattern at entity scope with one exclusion.")]
    public void DtoRegexRulePatternAtEntityScopeWithOneExclusion() {
      var dtoRules = new DTO.CustomizationRule() {
        RegexRules = new DTO.RegexRule[]
          {
                    new DTO.RegexRule() {
                        pattern = @"^[A-Za-z]+_[A-Z]{1}[a-z]{1}[A-Za-z]*$",
                        scope = DTO.RuleScope.Entity,
                        Exclude = new string[] { "new_incorrect_Naming" }
                    }
          }
      };

      var rules = RuleRepository.GetRules(dtoRules);

      var rule = rules.Single() as RegexRule;

      Assert.Equal(@"^[A-Za-z]+_[A-Z]{1}[a-z]{1}[A-Za-z]*$",
                   rule.Pattern.ToString());
      Assert.Equal(RuleScope.Entity, rule.Scope);

      var excluded = GetInstanceField(rule.GetType(),
                                      rule,
                                      "_excludedSchemaNames") as ICollection<string>;
      Assert.Single(excluded);
      Assert.Contains("new_incorrect_Naming", excluded);
    }

    [Fact(DisplayName = "RuleRepository: DTO.RegexRule pattern at entity scope with many exclusions.")]
    public void DtoRegexRulePatternAtEntityScopeWithManyExclusions() {
      var dtoRules = new DTO.CustomizationRule() {
        RegexRules = new DTO.RegexRule[]
          {
                    new DTO.RegexRule() {
                        pattern = @"^[A-Za-z]+_[A-Z]{1}[a-z]{1}[A-Za-z]*$",
                        scope = DTO.RuleScope.Entity,
                        Exclude = new string[] {
                            "new_incorrect_Naming", "new_INcorrect", "not_SoGood"
                        }
                    }
          }
      };

      var rules = RuleRepository.GetRules(dtoRules);

      var rule = rules.Single() as RegexRule;

      Assert.Equal(@"^[A-Za-z]+_[A-Z]{1}[a-z]{1}[A-Za-z]*$",
                   rule.Pattern.ToString());
      Assert.Equal(RuleScope.Entity, rule.Scope);

      var excluded = GetInstanceField(rule.GetType(),
                                      rule,
                                      "_excludedSchemaNames") as ICollection<string>;
      Assert.Equal(3, excluded.Count);
      Assert.Contains("new_incorrect_Naming", excluded);
      Assert.Contains("new_INcorrect", excluded);
      Assert.Contains("not_SoGood", excluded);
    }

    [Fact(DisplayName = "RuleRepository: DTO.RegexRule pattern at attribute scope.")]
    public void DtoRegexRulePatternAtAttributeScope() {
      var dtoRules = new DTO.CustomizationRule() {
        RegexRules = new DTO.RegexRule[]
          {
                    new DTO.RegexRule() {
                        pattern = @"^[A-Za-z]+_[A-Z]{1}[a-z]{1}[A-Za-z]*$",
                        scope = DTO.RuleScope.Attribute
                    }
          }
      };

      var rules = RuleRepository.GetRules(dtoRules);

      var rule = rules.Single() as RegexRule;

      Assert.Equal(@"^[A-Za-z]+_[A-Z]{1}[a-z]{1}[A-Za-z]*$",
                   rule.Pattern.ToString());
      Assert.Equal(RuleScope.Attribute, rule.Scope);
    }

    [Fact(DisplayName = "RuleRepository: DTO.RegexRule pattern at entity scope with one exclusion.")]
    public void DtoRegexRulePatternAtAttributeScopeWithOneExclusion() {
      var dtoRules = new DTO.CustomizationRule() {
        RegexRules = new DTO.RegexRule[]
          {
                    new DTO.RegexRule() {
                        pattern = @"^[A-Za-z]+_[A-Z]{1}[a-z]{1}[A-Za-z]*$",
                        scope = DTO.RuleScope.Attribute,
                        Exclude = new string[] { "new_incorrect_Naming.my_field" }
                    }
          }
      };

      var rules = RuleRepository.GetRules(dtoRules);

      var rule = rules.Single() as RegexRule;

      Assert.Equal(@"^[A-Za-z]+_[A-Z]{1}[a-z]{1}[A-Za-z]*$",
                   rule.Pattern.ToString());
      Assert.Equal(RuleScope.Attribute, rule.Scope);

      var excluded = GetInstanceField(rule.GetType(),
                                      rule,
                                      "_excludedSchemaNames") as ICollection<string>;
      Assert.Single(excluded);
      Assert.Contains("new_incorrect_Naming.my_field", excluded);
    }

    [Fact(DisplayName = "RuleRepository: DTO.RegexRule pattern at attribute scope with many exclusions.")]
    public void DtoRegexRulePatternAtAttributeScopeWithManyExclusions() {
      var dtoRules = new DTO.CustomizationRule() {
        RegexRules = new DTO.RegexRule[]
          {
                    new DTO.RegexRule() {
                        pattern = @"^[A-Za-z]+_[A-Z]{1}[a-z]{1}[A-Za-z]*$",
                        scope = DTO.RuleScope.Attribute,
                        Exclude = new string[] {
                            "new_incorrect_Naming.my_field",
                            "new_incorrect_Naming.my_field2",
                            "new_INcorrect.new_name"
                        }
                    }
          }
      };

      var rules = RuleRepository.GetRules(dtoRules);

      var rule = rules.Single() as RegexRule;

      Assert.Equal(@"^[A-Za-z]+_[A-Z]{1}[a-z]{1}[A-Za-z]*$",
                   rule.Pattern.ToString());
      Assert.Equal(RuleScope.Attribute, rule.Scope);

      var excluded = GetInstanceField(rule.GetType(),
                                      rule,
                                      "_excludedSchemaNames") as ICollection<string>;
      Assert.Equal(3, excluded.Count);
      Assert.Contains("new_incorrect_Naming.my_field", excluded);
      Assert.Contains("new_incorrect_Naming.my_field2", excluded);
      Assert.Contains("new_INcorrect.new_name", excluded);
    }

    [Fact(DisplayName = "RuleRepository: DTO.RegexRule pattern at lookup scope with many exclusions.")]
    public void DtoRegexRulePatternAtLookupScopeWithManyExclusions() {
      var dtoRules = new DTO.CustomizationRule() {
        RegexRules = new DTO.RegexRule[]
          {
                    new DTO.RegexRule() {
                        pattern = @"^[A-Za-z]+_[A-Z]{1}[a-z]{1}[A-Za-z]*Id$",
                        scope = DTO.RuleScope.Lookup,
                        Exclude = new string[] {
                            "new_incorrect_Naming.my_field",
                            "new_incorrect_Naming.my_field2",
                            "new_INcorrect.new_nameid"
                        }
                    }
          }
      };

      var rules = RuleRepository.GetRules(dtoRules);

      var rule = rules.Single() as RegexRule;

      Assert.Equal(@"^[A-Za-z]+_[A-Z]{1}[a-z]{1}[A-Za-z]*Id$",
                   rule.Pattern.ToString());
      Assert.Equal(RuleScope.Lookup, rule.Scope);

      var excluded = GetInstanceField(rule.GetType(),
                                      rule,
                                      "_excludedSchemaNames") as ICollection<string>;
      Assert.Equal(3, excluded.Count);
      Assert.Contains("new_incorrect_Naming.my_field", excluded);
      Assert.Contains("new_incorrect_Naming.my_field2", excluded);
      Assert.Contains("new_INcorrect.new_nameid", excluded);
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
    private static object GetInstanceField(Type type, object instance, string fieldName) {
      BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
          | BindingFlags.Static;
      FieldInfo field = type.GetField(fieldName, bindFlags);
      return field.GetValue(instance);
    }
  }
}
