using System.Collections.Generic;
using ByteDev.Cmd.Arguments;

namespace ByteDev.Nuget.PackageViewer
{
    public static class CmdAllowedArgsFactory
    {
        public static IList<CmdAllowedArg> CreateCmdAllowedArgs()
        {
            return new List<CmdAllowedArg>
            {
                new CmdAllowedArg('s', true) {Description = "Search term."},
                new CmdAllowedArg('i', true) {Description = "CSV of package IDs."}
            };
        }
    }
}