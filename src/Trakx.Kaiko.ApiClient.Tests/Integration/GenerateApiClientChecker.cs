using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Trakx.Kaiko.ApiClient.Tests;

public class GenerateApiClientChecker
{
    public ITestOutputHelper Output { get; }

    public GenerateApiClientChecker(ITestOutputHelper output)
    {
        Output = output;
    }

    [Fact]
    public async Task GenerateApiClient_should_be_disabled_before_commit()
    {
        var src = new DirectoryInfo(Environment.CurrentDirectory);
        while (!IsNullOrSrc(src))
        {
            src = src!.Parent;
        }
        src.Should().NotBeNull(because: "src folder is always expected");

        var testProject = Assembly.GetExecutingAssembly().GetName().Name!;
        var clientProject = testProject[..^".Tests".Length] + ".csproj";
        var csproj = src!
            .EnumerateFiles(clientProject, SearchOption.AllDirectories)
            .FirstOrDefault();
        csproj.Should().NotBeNull(because: $"project {clientProject} should exist");

        var content = await File.ReadAllTextAsync(csproj!.FullName);

        var generateApiClient = content.Contains("<GenerateApiClient>true", StringComparison.OrdinalIgnoreCase);
        generateApiClient.Should().BeFalse(because: "API client generation should be disabled when not needed");
    }

    private static bool IsNullOrSrc(DirectoryInfo? directory)
    {
        return directory == null
            || directory.Name.Equals("src", StringComparison.OrdinalIgnoreCase);
    }

}
