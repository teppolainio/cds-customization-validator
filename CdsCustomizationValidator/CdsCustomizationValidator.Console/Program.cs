using CdsCustomizationValidator.Domain;
using CdsCustomizationValidator.Domain.Rule;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CdsCustomizationValidator.App
{
    class Program
    {
        static void Main(string[] args)
        {

            var solutionName = args[0];
            var connStr = args[1];

            var ruleRepo = new RuleRepository();

            var rulesXml = ruleRepo.GetRules("rules.xml");

            using (var service = new CrmServiceClient(connStr))
            {
                var solutionValidator = new SolutionService(service);

                var solutionEntitities = solutionValidator.GetSolutionEntities(solutionName);

                var rules = new List<CustomizationRuleBase>() {
                    new AllowSolutionToOwnManagedEntitiesRule(false),
                    new EntityPrefixRule("sar"),
                    new AttributePrefixRule("sar"),
                    new RegexRule(@"^[A-Za-z]+_[A-Z]{1}[a-z]{1}[A-Za-z]*$",
                                  RuleScope.Entity)
                };

                var results = solutionValidator.Validate(solutionEntitities, rules);

                foreach (var result in results)
                {
                    Console.Write($"{result.Key.LogicalName}: ");
                    if (result.Value.All(r => r.Passed)) {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Passed validation.");
                        Console.ResetColor();
                        continue;
                    }

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("Failed validation.");
                    Console.ResetColor();
                    Console.WriteLine(" Failures are:");

                    var failures = result.Value
                                         .Where(r => !r.Passed);
                    foreach (var failure in failures)
                    {
                        Console.WriteLine("  * " + failure.FormatValidationResult());
                    }
                }

                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }

        }
    }
}
