using Cake.Common.IO;
using Cake.Core;
using Cake.Core.IO;
using Cake.Frosting;

public class BuildContext : FrostingContext
{
    public string BuildConfiguration
    {
        get;
    }

    public string Framework
    {
        get;
    }

    public string Runtime
    {
        get;
    }

    private string PublishDir
    {
        get;
    }

    public string RemoteDir
    {
        get;
    }

    public DirectoryPath RepoDir
    {
        get;
    }

    public DirectoryPath PublishLocation
    {
        get;
        set;
    }

    public DirectoryPath RemoteLocation
    {
        get;
        set;
    }

    public BuildContext(ICakeContext context)
        : base(context)
    {
        BuildConfiguration = context.Arguments.GetArgument("configuration") ?? "Release";
        Framework = context.Arguments.GetArgument("framework") ?? "net6.0-windows";
        PublishDir = context.Arguments.GetArgument("publishDir") ?? $"../Publish/FPD/{Framework}";
        Runtime = context.Arguments.GetArgument("runtime") ?? "win-x64";

        RepoDir = context.MakeAbsolute(context.Directory("../../"));
        PublishLocation = RepoDir + context.Directory($"/{PublishDir}");
        RemoteDir = context.Arguments.GetArgument("remote") ?? string.Empty;
        RemoteLocation = context.MakeAbsolute((DirectoryPath)$"{RemoteDir}/{Framework}");
    }
}
