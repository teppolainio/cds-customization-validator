using CdsCustomizationValidator.Domain;
using Microsoft.Xrm.Tooling.Connector;
using System;

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
                var filter = new SolutionService(service);

                var solutionEntitities = filter.GetSolutionEntities(solutionName);



                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }

        }
    }
}
