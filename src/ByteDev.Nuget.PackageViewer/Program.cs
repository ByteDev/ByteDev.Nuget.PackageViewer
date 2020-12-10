using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ByteDev.Cmd;
using ByteDev.Cmd.Arguments;
using ByteDev.Nuget.Contract;

namespace ByteDev.Nuget.PackageViewer
{
    internal class Program
    {
        private static readonly Output Output = new Output();

        private static IList<CmdAllowedArg> _cmdAllowedArgs;

        private static async Task Main(string[] args)
        {
            Output.WriteHeader();

            _cmdAllowedArgs = CmdAllowedArgsFactory.CreateCmdAllowedArgs();

            var cmdArgInfo = new CmdArgInfo(args, _cmdAllowedArgs);

            CmdArg searchTermArg = cmdArgInfo.GetArgument('s');
            CmdArg packageIdCsv = cmdArgInfo.GetArgument('i');

            if (searchTermArg != null)
            {
                await SearchAsync(searchTermArg.Value);
            }
            else if (packageIdCsv != null)
            {
                var packageIds = packageIdCsv.Value.ToCsvList();

                await GetByIdsAsync(packageIds);
            }
            else
            {
                HandleError("Provide either a search term or CSV list of package IDs argument.");
            }
        }

        private static async Task SearchAsync(string searchTerm)
        {
            const int takeCount = 50;

            Output.WriteLine($"Searching for first {takeCount} for: '{searchTerm}'.");
            Output.WriteLine();

            var client = new NugetPackageClient();

            var packages = await client.SearchAsync(new SearchRequest(searchTerm)
            {
                Take = takeCount,
                IncludePreRelease = false
            });

            foreach (var sp in packages.OrderBy(p => p.Identity.Id))
            {
                Output.WriteLine($"{sp.Identity}");

                var package = await client.GetAsync(sp.Identity);

                Output.WriteDependencies(package);
                Output.WriteLine();
            }
        }

        private static async Task GetByIdsAsync(IList<string> packageIds)
        {
            Output.WriteLine("Getting packages by ID...");
            Output.WriteLine();

            var client = new NugetPackageClient();
            
            var notFoundCount = 0;

            foreach (var packageId in packageIds.OrderBy(p => p))
            {
                var package = await client.GetLatestAsync(packageId);

                if (package == null)
                {
                    Output.WritePackageNotFound(packageId);
                    notFoundCount++;
                }
                else
                {
                    Output.WriteLine($"{packageId}");
                    Output.WriteDependencies(package);
                }

                Output.WriteLine();
            }

            Output.WriteFooter(packageIds, notFoundCount);
        }

        private static void HandleError(string errorMessage)
        {
            Output.WriteError(errorMessage);
            Output.WriteLine(_cmdAllowedArgs.HelpText());
            Environment.Exit(0);
        }
    }
}
