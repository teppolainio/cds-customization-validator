﻿using CdsCustomizationValidator.Domain;
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
            using (var service = new CrmServiceClient(connStr))
            {
                var solutionValidator = new SolutionService(service);

                var solutionEntitities = solutionValidator.GetSolutionEntities(solutionName);

                var rules = new List<CustomizationRule>() {
                    new AllowSolutionToOwnManagedEntitiesRule(false),
                    new EntityPrefixRule("sar"),
                    new AttributePrefixRule("sar")
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
                    Console.WriteLine("Failed validation.");
                    Console.ResetColor();
                }

                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }

        }
    }
}