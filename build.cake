#tool nuget:?package=vswhere

var target					= Argument("target", "Default");
var solutionDir				= System.IO.Directory.GetCurrentDirectory();
var testResultDir			= Argument("testResultDir", System.IO.Path.Combine(solutionDir, "test-results"));   // ./build.sh --target test -testResultsDir="somedir"
var artifactDir				= Argument("artifactDir", System.IO.Path.Combine(solutionDir, "artifacts"));		// ./build.sh --target pack -artifactDir="somedir"
var apiKey					= Argument<string>("apiKey", null);													// ./build.sh --target push -apiKey="nuget api key"
var testFailed				= false;

var peNetProj = System.IO.Path.Combine(solutionDir, "src", "PeNet.Asn1", "PeNet.Asn1.csproj");

// Get the latest VS installation to find the latest MSBuild tools.
DirectoryPath vsLatest  = VSWhereLatest();
FilePath msBuildPathX64 = (vsLatest==null)
                            ? null
                            : vsLatest.CombineWithFilePath("./MSBuild/15.0/Bin/amd64/MSBuild.exe");


Information("Solution Directory: {0}", solutionDir);
Information("Test Results Directory: {0}", testResultDir);


Task("Clean")
	.Does(() =>
	{
		var delSettings = new DeleteDirectorySettings { Recursive = true, Force = true };
			
		if(DirectoryExists(testResultDir))
			DeleteDirectory(testResultDir, delSettings);

		if(DirectoryExists(artifactDir))
			DeleteDirectory(artifactDir, delSettings);

		var binDirs = GetDirectories("./**/bin");
		var objDirs = GetDirectories("./**/obj");
		var testResDirs = GetDirectories("./**/TestResults");
		
		DeleteDirectories(binDirs, delSettings);
		DeleteDirectories(objDirs, delSettings);
		DeleteDirectories(testResDirs, delSettings);
	});


Task("PrepareDirectories")
	.IsDependentOn("Clean")
	.Does(() =>
	{
		EnsureDirectoryExists(testResultDir);
		EnsureDirectoryExists(artifactDir);
	});


Task("Restore")
	.IsDependentOn("PrepareDirectories")
	.Does(() =>
	{
		DotNetCoreRestore();	  
	});

Task("Build")
	.IsDependentOn("Restore")
	.Does(() =>
	{
		MSBuild(solutionDir, new MSBuildSettings {
			ToolPath = msBuildPathX64,
			Verbosity = Verbosity.Minimal,
			Configuration = "Release"
		});
	});


Task("Test")
	.IsDependentOn("Build")
	.ContinueOnError()
	.Does(() =>
	{
		var tests = GetTestProjectFiles();
		
		foreach(var test in tests)
		{
			var projectFolder = System.IO.Path.GetDirectoryName(test.FullPath);

			try
			{
				DotNetCoreTest(test.FullPath, new DotNetCoreTestSettings
				{
					ArgumentCustomization = args => args.Append("-l trx"),
					WorkingDirectory = projectFolder
				});
			}
			catch(Exception e)
			{
				testFailed = true;
				Error(e.Message.ToString());
			}
		}

		// Copy test result files.
		var tmpTestResultFiles = GetFiles("./**/*.trx");
		CopyFiles(tmpTestResultFiles, testResultDir);
	});


Task("Pack")
	.IsDependentOn("Test")
	.Does(() =>
	{
		if(testFailed)
		{
			Information("Do not pack because tests failed");
			return;
		}
		
		Information($"Pack {peNetProj}");
		var settings = new DotNetCorePackSettings
		{
			Configuration = "Release",
			OutputDirectory = artifactDir
		};
		DotNetCorePack(peNetProj, settings);
	});

Task("Push")
    .IsDependentOn("Pack")
    .Does(() => {
        var package = GetFiles($"{artifactDir}/PeNet.Asn1.*.nupkg").ElementAt(0);
        var source = "https://www.nuget.org/api/v2/package";

        if(apiKey==null)
            throw new ArgumentNullException(nameof(apiKey), "The \"apiKey\" argument must be set for this task.");

        Information($"Push {package} to {source}");

        NuGetPush(package, new NuGetPushSettings {
            Source = source,
            ApiKey = apiKey
        });
    });

Task("Default")
	.IsDependentOn("Test")
	.Does(() =>
	{
		Information("Build and test the whole solution.");
		Information("To push the PeNet.Asn1 library to nuget.org use the cake build argument: -target push -apiKey=\"nuget api key\"");
	});


FilePathCollection GetSrcProjectFiles()
{
	return GetFiles("./src/**/*.csproj");
}

FilePathCollection GetTestProjectFiles()
{
	return GetFiles("./test/**/*Test/*.csproj");
}

RunTarget(target);