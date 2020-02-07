using CdsCustomizationValidator.Domain;
using CdsCustomizationValidator.Domain.Rule;
using FakeXrmEasy.Extensions;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using System.Collections.Generic;
using Xunit;

namespace CdsCustomizationValidator.Test.Domain.Rule {

  /// <summary>
  /// Unit tests for rule <see cref="RegexRule"/> for entity scope.
  /// </summary>
  public class RegexRuleTest_AttributeScope {

    [Fact(DisplayName = "RegexRule: Rule description must show attribute scope and pattern.")]
    public void RuleDescriptionMustShowScopeAndPatternTest() {
      var regexPattern = @"^[A-Za-z]+_[A-Z]{1}[a-z]{1}[A-Za-z]*$";
      var scope = RuleScope.Attribute;

      var ruleToTest = new RegexRule(regexPattern, scope);

      Assert.Equal($"Schema name of an Attribute must match to regular expression pattern {regexPattern}.",
                   ruleToTest.Description);
    }

    [Fact(DisplayName = "RegexRule: Rule description must show attribute scope and pattern and exlusions.")]
    public void RuleDescriptionMustShowScopeAndPatternAndExlclusionTest() {
      var regexPattern = @"^[A-Za-z]+_[A-Z]{1}[a-z]{1}[A-Za-z]*$";
      var scope = RuleScope.Attribute;
      var excluded = new string[] {
                "new_notConforming.new_FIELd", "new_foo_new_name", "new_BAR.my_fIeld"
            };

      var ruleToTest = new RegexRule(regexPattern, scope, excluded);

      Assert.Equal($"Schema name of an Attribute must match to regular expression pattern {regexPattern}. Attribute new_notConforming.new_FIELd, new_foo_new_name and new_BAR.my_fIeld are excluded.",
                   ruleToTest.Description);
    }

    [Fact(DisplayName = "RegexRule: Managed fields must be skipped on OOB entity.")]
    public void ManagedOobAttributesMustBeSkippedTest() {
      EntityMetadata entity = new EntityMetadata() {
        SchemaName = "Account",
      };
      entity.SetSealedPropertyValue("IsManaged", true);
      entity.SetSealedPropertyValue("IsCustomEntity", false);

      var accountNameMetadata = new StringAttributeMetadata("Name");
      accountNameMetadata.SetSealedPropertyValue("IsManaged", true);
      accountNameMetadata.SetSealedPropertyValue("IsCustomAttribute", false);

      var accountAddress1Line1Metadata = new StringAttributeMetadata("Address1_Line1");
      accountAddress1Line1Metadata.SetSealedPropertyValue("IsManaged", true);
      accountAddress1Line1Metadata.SetSealedPropertyValue("IsCustomAttribute", false);

      var field1Metadata = new StringAttributeMetadata("foo_customField");
      field1Metadata.SetSealedPropertyValue("IsManaged", true);
      field1Metadata.SetSealedPropertyValue("IsCustomAttribute", true);

      var attributes = new List<AttributeMetadata> {
                accountNameMetadata,
                accountAddress1Line1Metadata,
                field1Metadata
            };

      var isOwnedBySolution = false;

      var validSolutionEntity = new SolutionEntity(entity,
                                                   attributes,
                                                   isOwnedBySolution);

      var regexPattern = @"^[A-Za-z]+_[A-Z]{1}[a-z]{1}[A-Za-z]*$";
      var scope = RuleScope.Attribute;

      var ruleToTest = new RegexRule(regexPattern, scope);

      var results = ruleToTest.Validate(validSolutionEntity);

      Assert.True(results.Passed);
    }

    [Fact(DisplayName = "RegexRule: Check must succeed for custom unmanaged fields matching the pattern.")]
    public void CustomAttributesMatchingPatternMustSucceedTest() {
      EntityMetadata entity = new EntityMetadata() {
        SchemaName = "Account",
      };
      entity.SetSealedPropertyValue("IsManaged", true);
      entity.SetSealedPropertyValue("IsCustomEntity", false);

      var field1Metadata = new StringAttributeMetadata("foo_CustomField");
      field1Metadata.SetSealedPropertyValue("IsManaged", false);
      field1Metadata.SetSealedPropertyValue("IsCustomAttribute", true);

      var attributes = new List<AttributeMetadata> {
                field1Metadata
            };

      var isOwnedBySolution = false;

      var validSolutionEntity = new SolutionEntity(entity,
                                                   attributes,
                                                   isOwnedBySolution);

      var regexPattern = @"^[A-Za-z]+_[A-Z]{1}[a-z]{1}[A-Za-z]*$";
      var scope = RuleScope.Attribute;

      var ruleToTest = new RegexRule(regexPattern, scope);

      var results = ruleToTest.Validate(validSolutionEntity);

      Assert.True(results.Passed);
    }

    [Fact(DisplayName = "RegexRule: Custom fields with incorrect names must fail on OOB entity.")]
    public void UnmanagedAttributesOnOobEntityWithIncorrectNamesMustFailTest() {
      EntityMetadata entity = new EntityMetadata() {
        SchemaName = "Account",
      };
      entity.SetSealedPropertyValue("IsManaged", true);
      entity.SetSealedPropertyValue("IsCustomEntity", false);

      var field1Metadata = new StringAttributeMetadata("foo_customField");
      field1Metadata.SetSealedPropertyValue("IsManaged", false);
      field1Metadata.SetSealedPropertyValue("IsCustomAttribute", true);

      var field2Metadata = new StringAttributeMetadata("foo_c");
      field2Metadata.SetSealedPropertyValue("IsManaged", false);
      field2Metadata.SetSealedPropertyValue("IsCustomAttribute", true);

      var field3Metadata = new StringAttributeMetadata("foo_CustomField");
      field3Metadata.SetSealedPropertyValue("IsManaged", false);
      field3Metadata.SetSealedPropertyValue("IsCustomAttribute", true);

      var attributes = new List<AttributeMetadata> {
                field1Metadata,
                field2Metadata,
                field3Metadata
            };

      var isOwnedBySolution = false;

      var validSolutionEntity = new SolutionEntity(entity,
                                                   attributes,
                                                   isOwnedBySolution);

      var regexPattern = @"^[A-Za-z]+_[A-Z]{1}[a-z]{1}[A-Za-z]*$";
      var scope = RuleScope.Attribute;

      var ruleToTest = new RegexRule(regexPattern, scope);

      var results = ruleToTest.Validate(validSolutionEntity);

      Assert.False(results.Passed);
    }

    [Fact(DisplayName = "RegexRule: Custom fields with incorrect names failure description is correct.")]
    public void UnmanagedAttributesOnOobEntityWithIncorrectNamesFailureDescriptionIsCorrectTest() {
      EntityMetadata entity = new EntityMetadata() {
        SchemaName = "Account",
        DisplayName = new Label() {
          UserLocalizedLabel = new LocalizedLabel("Asiakas", 1035)
        }
      };
      entity.SetSealedPropertyValue("IsManaged", true);
      entity.SetSealedPropertyValue("IsCustomEntity", false);

      var field1Metadata = new StringAttributeMetadata("foo_customField");
      field1Metadata.SetSealedPropertyValue("IsManaged", false);
      field1Metadata.SetSealedPropertyValue("IsCustomAttribute", true);

      var field2Metadata = new StringAttributeMetadata("foo_c");
      field2Metadata.SetSealedPropertyValue("IsManaged", false);
      field2Metadata.SetSealedPropertyValue("IsCustomAttribute", true);

      var field3Metadata = new StringAttributeMetadata("foo_CustomField");
      field3Metadata.SetSealedPropertyValue("IsManaged", false);
      field3Metadata.SetSealedPropertyValue("IsCustomAttribute", true);

      var attributes = new List<AttributeMetadata> {
                field1Metadata,
                field2Metadata,
                field3Metadata
            };

      var isOwnedBySolution = false;

      var validSolutionEntity = new SolutionEntity(entity,
                                                   attributes,
                                                   isOwnedBySolution);

      var regexPattern = @"^[A-Za-z]+_[A-Z]{1}[a-z]{1}[A-Za-z]*$";
      var scope = RuleScope.Attribute;

      var ruleToTest = new RegexRule(regexPattern, scope);

      var results = ruleToTest.Validate(validSolutionEntity);

      Assert.Equal($"Rule failed: {ruleToTest.Description} " +
                   "Following attributes do not match given pattern: " +
                   "foo_customField, foo_c.",
                   results.FormatValidationResult());
    }

    [Fact(DisplayName = "RegexRule: Custom fields with incorrect names can be excluded from validation.")]
    public void ExcludeUnmanagedAttributesWithIncorrectNamesFromValidationTest() {
      EntityMetadata entity = new EntityMetadata() {
        SchemaName = "Account",
      };
      entity.SetSealedPropertyValue("IsManaged", true);
      entity.SetSealedPropertyValue("IsCustomEntity", false);

      var field1Metadata = new StringAttributeMetadata("foo_customField");
      field1Metadata.SetSealedPropertyValue("IsManaged", false);
      field1Metadata.SetSealedPropertyValue("IsCustomAttribute", true);

      var field2Metadata = new StringAttributeMetadata("foo_c");
      field2Metadata.SetSealedPropertyValue("IsManaged", false);
      field2Metadata.SetSealedPropertyValue("IsCustomAttribute", true);

      var field3Metadata = new StringAttributeMetadata("foo_CustomField");
      field3Metadata.SetSealedPropertyValue("IsManaged", false);
      field3Metadata.SetSealedPropertyValue("IsCustomAttribute", true);

      var attributes = new List<AttributeMetadata> {
                field1Metadata,
                field2Metadata,
                field3Metadata
            };

      var isOwnedBySolution = false;

      var validSolutionEntity = new SolutionEntity(entity,
                                                   attributes,
                                                   isOwnedBySolution);

      var regexPattern = @"^[A-Za-z]+_[A-Z]{1}[a-z]{1}[A-Za-z]*$";
      var scope = RuleScope.Attribute;
      var excludedAttributes = new List<string> {
                "Account.foo_customField",
                "Account.foo_c"
            };

      var ruleToTest = new RegexRule(regexPattern, scope,
                                     excludedAttributes);

      var results = ruleToTest.Validate(validSolutionEntity);

      Assert.True(results.Passed);
    }

    [Fact(DisplayName = "RegexRule: Exclude automatically generated Base-suffixed attributes of Money type from validation.")]
    public void ExcludeAutomaticallyGeneratedBaseAttributesOfMonyFromValidationTest() {

      EntityMetadata entity = new EntityMetadata() {
        SchemaName = "Account",
      };
      entity.SetSealedPropertyValue("IsManaged", true);
      entity.SetSealedPropertyValue("IsCustomEntity", false);

      var field1Metadata = new MoneyAttributeMetadata("foo_CustomField");
      field1Metadata.SetSealedPropertyValue("IsManaged", false);
      field1Metadata.SetSealedPropertyValue("IsCustomAttribute", true);

      var field1MetadataBase = new MoneyAttributeMetadata("foo_customField_Base");
      field1MetadataBase.SetSealedPropertyValue("IsManaged", false);
      field1MetadataBase.SetSealedPropertyValue("IsCustomAttribute", true);
      field1MetadataBase.CalculationOf = "foo_customField";

      var attributes = new List<AttributeMetadata> {
        field1Metadata,
        field1MetadataBase
      };

      var isOwnedBySolution = false;

      var validSolutionEntity = new SolutionEntity(entity,
                                                   attributes,
                                                   isOwnedBySolution);

      var regexPattern = @"^[A-Za-z]+_[A-Z]{1}[a-z]{1}[A-Za-z]*$";
      var scope = RuleScope.Attribute;

      var ruleToTest = new RegexRule(regexPattern, scope);

      var results = ruleToTest.Validate(validSolutionEntity);

      Assert.True(results.Passed);
    }

  }

}
