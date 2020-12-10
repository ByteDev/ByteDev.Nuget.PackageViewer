using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using ByteDev.Cmd;
using NuGet.Packaging;
using NuGet.Protocol.Core.Types;

namespace ByteDev.Nuget.PackageViewer
{
    internal static class OutputExtensions
    {
        public static void WriteError(this Output source, string errorMessage)
        {
            source.WriteLine(errorMessage, new OutputColor(ConsoleColor.Red, ConsoleColor.Black));
        }

        public static void WriteHeader(this Output source)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var fvi = FileVersionInfo.GetVersionInfo(assembly.Location);

            source.WriteBlankLines();

            source.Write(new MessageBox($" Nuget Package Viewer {fvi.FileVersion} ")
            {
                TextColor = new OutputColor(ConsoleColor.White, ConsoleColor.Blue),
                BorderColor = new OutputColor(ConsoleColor.White, ConsoleColor.Blue)
            });

            source.WriteBlankLines();
        }

        public static void WriteFooter(this Output source, IList<string> packageIds, int notFoundCount)
        {
            source.WriteLine($"Found: {packageIds.Count - notFoundCount}");
            source.WriteLine($"Not found: {notFoundCount}");
            source.WriteLine();
        }

        public static void WritePackageNotFound(this Output source, string packageId)
        {
            source.WriteError($"{packageId} (not found).");
        }

        public static void WriteDependencies(this Output source, IPackageSearchMetadata package)
        {
            foreach (var ds in package.DependencySets)
            {
                source.WriteDependencySet(ds);
            }
        }

        public static void WriteDependencySet(this Output source, PackageDependencyGroup ds)
        {
            source.WriteLine($"-> {ds.TargetFramework}");

            foreach (var dsPackage in ds.Packages)
            {
                source.WriteLine($"---> {dsPackage}");
            }
        }
    }
}