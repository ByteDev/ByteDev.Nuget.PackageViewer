using System.Collections.Generic;
using System.Linq;

namespace ByteDev.Nuget.PackageViewer
{
    public static class StringExtensions
    {
        public static IList<string> ToCsvList(this string csv)
        {
            return csv.Split(",").Select(p => p.Trim()).ToList();
        }
    }
}