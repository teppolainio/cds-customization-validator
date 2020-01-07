using CdsCustomizationValidator.Domain;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Linq;

namespace CdsCustomizationValidator.App
{
    class Program
    {
        static void Main(string[] args)
        {

            var solutionName = args[0];
            var connStr = args[1];
            var rulesFile = args[2];

            var ruleRepo = new RuleRepository();

            Console.WriteLine($"Reading rules from file \"{rulesFile}\".");
            var rules = ruleRepo.GetRules(rulesFile);

            Console.WriteLine($"Following {rules.Count} rules were found:");
            for (int rule = 0; rule < rules.Count; rule++)
            {
                Console.WriteLine($"  {rule+1}: {rules[rule].Description}");
            }

            using (var service = new CrmServiceClient(connStr))
            {
                var solutionValidator = new SolutionService(service);

                var solutionEntitities = solutionValidator.GetSolutionEntities(solutionName);

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
