var target = Argument("target", "Publish");
var configuration = Argument("configuration", "Release");
var framework = Argument("framework", "net5.0-windows");
var publishDir = Argument("publishDir", $"/../Publish/FPD/{framework}");
var remoteDir = Argument("remoteDir", "%remote_location%");

var repoDir = Context.Environment.WorkingDirectory + Directory("/../");
var publishLocation = repoDir + Directory(publishDir);
//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(context =>
{
	context.Information(repoDir);
    CleanDirectory($"{repoDir}/bin/{configuration}");
});

Task("Build")
    .IsDependentOn("Clean")
    .Does(() =>
{
    DotNetCoreBuild($"{repoDir}/FinancePortfolioDatabase.sln", new DotNetCoreBuildSettings
    {
        Configuration = configuration,
		NoRestore = false
    });
});

Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
{
    DotNetCoreTest($"{repoDir}/FinancePortfolioDatabase.sln", new DotNetCoreTestSettings
    {
        Configuration = configuration,
        NoBuild = true,
    });
});

Task("Publish")
	.IsDependentOn("Test")
	.Does(context =>
{
	var settings = new DotNetCorePublishSettings
	{
		Configuration = configuration,
		Framework = framework,
		NoBuild = true,
		OutputDirectory = publishLocation,
		SelfContained = true
	};
	DotNetCorePublish($"{repoDir}/FinancePortfolioDatabase/FinancePortfolioDatabase.csproj", settings);
});

Task("CopyPublish")
	.IsDependentOn("Publish")
	.Does(context => 
{
	var files = GetFiles($"{publishLocation}");
	CopyFiles(files, $"{remoteDir}");
});

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);