using System;
using Cake.Frosting;

namespace Build
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            return new CakeHost()
                .InstallTool(new Uri("nuget:?package=NuGet.CommandLine&version=5.9.1"))
                .InstallTool(new Uri("nuget:?package=Cake.VersionReader&version=5.1.0"))
                .UseContext<BuildContext>()
                .Run(args);
        }
    }
}