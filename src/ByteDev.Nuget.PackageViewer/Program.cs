using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ByteDev.Nuget.Contract;
using NuGet.Packaging;

namespace ByteDev.Nuget.PackageViewer
{
    class Program
    {
        private static async Task Main(string[] args)
        {
            const string searchTerm = "bytedev";

            await CheckPackages(searchTerm);

            // await CheckPackages(new List<string>
            //  {
            //      "ByteDev.ArgValidation",
            //      "ByteDev.Testing.NUnit",
            //      "ByteDev.Azure.KeyVault"
            //  });
        }

        private static async Task CheckPackages(string searchTerm)
        {
            var client = new NugetPackageClient();

            Console.WriteLine($"Searching for: '{searchTerm}'.");
            Console.WriteLine();

            var packages = await client.SearchAsync(new SearchRequest(searchTerm)
            {
                Take = 50,
                IncludePreRelease = false
            });

            foreach (var sp in packages.OrderBy(p => p.Identity.Id))
            {
                Console.WriteLine($"{sp.Identity}");

                var package = await client.GetAsync(sp.Identity);

                foreach (var ds in package.DependencySets)
                {
                    WriteDependencySet(ds);
                }

                Console.WriteLine();
            }
        }

        private static async Task CheckPackages(IList<string> packageIds)
        {
            var client = new NugetPackageClient();

            var notFoundCount = 0;

            foreach (var packageId in packageIds.OrderBy(p => p))
            {
                var package = await client.GetLatestAsync(packageId);

                if (package == null)
                {
                    Console.WriteLine($"{packageId} (not found).");
                    notFoundCount++;
                }
                else
                {
                    Console.WriteLine($"{packageId}");

                    foreach (var ds in package.DependencySets)
                    {
                        WriteDependencySet(ds);
                    }
                }

                Console.WriteLine();
            }

            WriteFooter(packageIds, notFoundCount);
        }

        private static void WriteDependencySet(PackageDependencyGroup ds)
        {
            Console.WriteLine($"-> {ds.TargetFramework}");

            foreach (var dsPackage in ds.Packages)
            {
                Console.WriteLine($"---> {dsPackage}");
            }
        }

        private static void WriteFooter(IList<string> packageIds, int notFoundCount)
        {
            Console.WriteLine($"Found: {packageIds.Count - notFoundCount}");
            Console.WriteLine($"Not found: {notFoundCount}");
            Console.WriteLine();
            Console.WriteLine("Done.");
        }
    }
}
