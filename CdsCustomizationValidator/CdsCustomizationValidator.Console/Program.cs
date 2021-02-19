using CdsCustomizationValidator.Domain;
using CommandLine;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CdsCustomizationValidator.App {
  class Program {
    static void Main(string[] args) {
      var exitcode = Parser.Default
                           .ParseArguments<Options>(args)
                           .MapResult(opts => Execute(opts),
                                      errors => HandleArgumentErrors(errors));

      if(exitcode != _EXIT_HELPREQUESTED) {
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
      }
    }

    private const int _EXIT_SUCCESS = 0;
    private const int _EXIT_HELPREQUESTED = -1;
    private const int _EXIT_ARGUMENTERROR = -2;

    private static int Execute(Options commandlineOptions) {
      var solutionName = commandlineOptions.Solution;
      var connStr = commandlineOptions.CdsConnectionString;
      var rulesFile = commandlineOptions.RuleFile;

      var ruleRepo = new RuleRepository();

      Console.WriteLine($"Reading rules from file \"{rulesFile}\".");
      var rules = ruleRepo.GetRules(rulesFile);

      Console.WriteLine($"Following {rules.Count} rules were found:");
      for(int rule = 0; rule < rules.Count; rule++) {
        Console.WriteLine($"  {rule + 1}: {rules[rule].Description}");
      }

      using(var service = new CrmServiceClient(connStr)) {
        var solutionValidator = new SolutionService(service);

        var solutionEntitities = solutionValidator.GetSolutionEntities(solutionName);

        var results = solutionValidator.Validate(solutionEntitities, rules);

        foreach(var result in results) {
          Console.Write($"{result.Key.SchemaName}: ");
          if(result.Value.All(r => r.Passed)) {
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
          foreach(var failure in failures) {
            Console.WriteLine("  * " + failure.FormatValidationResult());
          }
        }

        return _EXIT_SUCCESS;
      }

    }

    private static int HandleArgumentErrors(
        IEnumerable<Error> errors) {

      var exitCode = _EXIT_ARGUMENTERROR;

      if(errors.Any(x => x is HelpRequestedError || x is VersionRequestedError)) {
        exitCode = _EXIT_HELPREQUESTED;
      }
      else {
        Console.WriteLine($"Command line argument errors {errors.Count()}.");
      }

      return exitCode;
    }
  }
}
