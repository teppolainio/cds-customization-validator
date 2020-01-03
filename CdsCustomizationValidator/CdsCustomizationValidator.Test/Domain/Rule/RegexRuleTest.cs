using CdsCustomizationValidator.Domain;
using CdsCustomizationValidator.Domain.Rule;
using FakeXrmEasy.Extensions;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;
using Xunit;

namespace CdsCustomizationValidator.Test.Domain.Rule
{

    /// <summary>
    /// Unit tests for rule <see cref="RegexRule"/>
    /// </summary>
    public class RegexRuleTests
    {

        [Fact(DisplayName = "RegexRule: Rule description must show pattern.")]
        public void RuleDescriptionMustShowPatternTest()
        {
            var regexPattern = @"^[A-Za-z]+_[A-Z]{1}[a-z]{1}[A-Za-z]*$";
            var scope = RuleScope.Entity;

            var ruleToTest = new RegexRule(regexPattern, scope);

            Assert.EndsWith($" must match to regular expression pattern {regexPattern}.",
                            ruleToTest.Description);
        }

        [Fact(DisplayName = "RegexRule: Invalid pattern must throw an exception.")]
        public void InvalidRegexPatternMustFailOnObjectInitialization()
        {

            var invalidPattern = "[";

            var ex = Assert.Throws<ArgumentException>(
                () => new RegexRule(invalidPattern, RuleScope.Entity));

            Assert.Equal("Given pattern \"[\" was invalid regular expression.",
                         ex.Message);
            Assert.NotNull(ex.InnerException);
            Assert.IsType<ArgumentException>(ex.InnerException);

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

    }

}
