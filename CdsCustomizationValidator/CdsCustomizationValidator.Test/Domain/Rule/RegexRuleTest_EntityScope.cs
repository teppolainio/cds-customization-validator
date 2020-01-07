using CdsCustomizationValidator.Domain;
using CdsCustomizationValidator.Domain.Rule;
using FakeXrmEasy.Extensions;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;
using Xunit;

namespace CdsCustomizationValidator.Test.Domain.Rule
{

    /// <summary>
    /// Unit tests for rule <see cref="RegexRule"/> for entity scope.
    /// </summary>
    public class RegexRuleTests
    {

        [Fact(DisplayName = "RegexRule: Rule description must show entity scope and pattern.")]
        public void RuleDescriptionMustShowScopeAndPatternTest()
        {
            var regexPattern = @"^[A-Za-z]+_[A-Z]{1}[a-z]{1}[A-Za-z]*$";
            var scope = RuleScope.Entity;

            var ruleToTest = new RegexRule(regexPattern, scope);

            Assert.Equal($"Schema name of an Entity must match to regular expression pattern {regexPattern}.",
                         ruleToTest.Description);
        }

        [Fact(DisplayName = "RegexRule: Rule description must show entity scope and pattern and exlusions.")]
        public void RuleDescriptionMustShowScopeAndPatternAndExlclusionTest()
        {
            var regexPattern = @"^[A-Za-z]+_[A-Z]{1}[a-z]{1}[A-Za-z]*$";
            var scope = RuleScope.Entity;
            var excluded = new string[] { 
                "new_notConforming", "new_foo", "new_BAR"
            };

            var ruleToTest = new RegexRule(regexPattern, scope, excluded);

            Assert.Equal($"Schema name of an Entity must match to regular expression pattern {regexPattern}. Entity new_notConforming, new_foo and new_BAR are excluded.",
                         ruleToTest.Description);
        }

        [Fact(DisplayName = "RegexRule: Managed OOB entity must be skipped.")]
        public void ManagedOobEntityMustBeSkippedTest()
        {
            EntityMetadata entity = new EntityMetadata()
            {
                SchemaName = "Account",
            };
            entity.SetSealedPropertyValue("IsManaged", true);
            entity.SetSealedPropertyValue("IsCustomEntity", false);

            List<AttributeMetadata> attributes = null;

            var isOwnedBySolution = true;

            var validSolutionEntity = new SolutionEntity(entity,
                                                         attributes,
                                                         isOwnedBySolution);

            var regexPattern = @"^[A-Za-z]+_[A-Z]{1}[a-z]{1}[A-Za-z]*$";
            var scope = RuleScope.Entity;

            var ruleToTest = new RegexRule(regexPattern, scope);

            var results = ruleToTest.Validate(validSolutionEntity);

            Assert.True(results.Passed);
        }

        [Fact(DisplayName = "RegexRule: Managed custom entity must be skipped.")]
        public void ManagedCustomEntityMustBeSkippedTest()
        {
            EntityMetadata entity = new EntityMetadata()
            {
                SchemaName = "new_notconforming",
            };
            entity.SetSealedPropertyValue("IsManaged", true);
            entity.SetSealedPropertyValue("IsCustomEntity", true);

            List<AttributeMetadata> attributes = null;

            var isOwnedBySolution = true;

            var validSolutionEntity = new SolutionEntity(entity,
                                                         attributes,
                                                         isOwnedBySolution);

            var regexPattern = @"^[A-Za-z]+_[A-Z]{1}[a-z]{1}[A-Za-z]*$";
            var scope = RuleScope.Entity;

            var ruleToTest = new RegexRule(regexPattern, scope);

            var results = ruleToTest.Validate(validSolutionEntity);

            Assert.True(results.Passed);
        }

        [Fact(DisplayName = "RegexRule: Entity not owned by solution must be skipped.")]
        public void EntityNotOwnedBySolutionMustBeSkippedTest()
        {
            EntityMetadata entity = new EntityMetadata()
            {
                SchemaName = "new_notconforming",
            };
            entity.SetSealedPropertyValue("IsManaged", false);
            entity.SetSealedPropertyValue("IsCustomEntity", true);

            List<AttributeMetadata> attributes = null;

            var isOwnedBySolution = false;

            var validSolutionEntity = new SolutionEntity(entity,
                                                         attributes,
                                                         isOwnedBySolution);

            var regexPattern = @"^[A-Za-z]+_[A-Z]{1}[a-z]{1}[A-Za-z]*$";
            var scope = RuleScope.Entity;

            var ruleToTest = new RegexRule(regexPattern, scope);

            var results = ruleToTest.Validate(validSolutionEntity);

            Assert.True(results.Passed);
        }

        [Fact(DisplayName = "RegexRule: Entity first letter is a capital letter check succeeds.")]
        public void EnsureEntityFirstLetterIsCapitalLetterSucceedsTest() {
            EntityMetadata entity = new EntityMetadata()
            {
                SchemaName = "foobar_MyMagnificientEntity",
            };
            entity.SetSealedPropertyValue("IsManaged", false);
            entity.SetSealedPropertyValue("IsCustomEntity", true);

            List<AttributeMetadata> attributes = null;

            var isOwnedBySolution = true;

            var validSolutionEntity = new SolutionEntity(entity,
                                                         attributes,
                                                         isOwnedBySolution);

            var regexPattern = @"^[A-Za-z]+_[A-Z]{1}[a-z]{1}[A-Za-z]*$";
            var scope = RuleScope.Entity;

            var ruleToTest = new RegexRule(regexPattern, scope);

            var results = ruleToTest.Validate(validSolutionEntity);

            Assert.True(results.Passed);
        }

        [Fact(DisplayName = "RegexRule: Entity first letter is a capital letter check fails.")]
        public void EnsureEntityFirstLetterIsCapitalLetterFailsTest()
        {
            EntityMetadata entity = new EntityMetadata()
            {
                SchemaName = "foobar_myMagnificientEntity",
            };
            entity.SetSealedPropertyValue("IsManaged", false);
            entity.SetSealedPropertyValue("IsCustomEntity", true);

            List<AttributeMetadata> attributes = null;

            var isOwnedBySolution = true;

            var validSolutionEntity = new SolutionEntity(entity,
                                                         attributes,
                                                         isOwnedBySolution);

            var regexPattern = @"^[A-Za-z]+_[A-Z]{1}[a-z]{1}[A-Za-z]*$";
            var scope = RuleScope.Entity;

            var ruleToTest = new RegexRule(regexPattern, scope);

            var results = ruleToTest.Validate(validSolutionEntity);

            Assert.False(results.Passed);
        }

        [Fact(DisplayName = "RegexRule: Entity first letter is a capital letter check failure description is correct.")]
        public void EntityFirstLetterIsCapitalLetterFailureDescriptionIsCorrectTest()
        {
            EntityMetadata entity = new EntityMetadata()
            {
                SchemaName = "foobar_myMagnificientEntity",
                DisplayName = new Label() {
                    UserLocalizedLabel = new LocalizedLabel("Suurenmoinen entiteetti",
                                                            1035)
                }
            };
            entity.SetSealedPropertyValue("IsManaged", false);
            entity.SetSealedPropertyValue("IsCustomEntity", true);

            List<AttributeMetadata> attributes = null;

            var isOwnedBySolution = true;

            var validSolutionEntity = new SolutionEntity(entity,
                                                         attributes,
                                                         isOwnedBySolution);

            var regexPattern = @"^[A-Za-z]+_[A-Z]{1}[a-z]{1}[A-Za-z]*$";
            var scope = RuleScope.Entity;

            var ruleToTest = new RegexRule(regexPattern, scope);

            var results = ruleToTest.Validate(validSolutionEntity);

            Assert.Equal($"Rule failed: {ruleToTest.Description} Entity schema name {entity.SchemaName} doesn't match given pattern \"{regexPattern}\".",
                         results.FormatValidationResult());
        }

        [Fact(DisplayName = "RegexRule: Entity with incorrectly created schema name can be excluded from validation.")]
        public void ExcludeEntityFromCheck() {
            EntityMetadata entity = new EntityMetadata()
            {
                SchemaName = "foobar_myNotSoMagnificientEntity",
            };
            entity.SetSealedPropertyValue("IsManaged", false);
            entity.SetSealedPropertyValue("IsCustomEntity", true);

            List<AttributeMetadata> attributes = null;

            var isOwnedBySolution = true;

            var validSolutionEntity = new SolutionEntity(entity,
                                                         attributes,
                                                         isOwnedBySolution);

            var regexPattern = @"^[A-Za-z]+_[A-Z]{1}[a-z]{1}[A-Za-z]*$";
            var scope = RuleScope.Entity;
            var excludedEntities = new List<string> {
                "foobar_123",
                "foobar_myNotSoMagnificientEntity",
                "foobar_mySecondNotSoGreatAgainEntity"
            };

            var ruleToTest = new RegexRule(regexPattern, scope,
                                           excludedEntities);

            var results = ruleToTest.Validate(validSolutionEntity);

            Assert.True(results.Passed);
        }

    }

}
