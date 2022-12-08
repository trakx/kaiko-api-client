using Xunit.Abstractions;

namespace Trakx.Kaiko.ApiClient.Tests.Integration;

public class OpenApiGeneratedCodeModifier : Trakx.Utils.Testing.OpenApiGeneratedCodeModifier
{
    public OpenApiGeneratedCodeModifier(ITestOutputHelper output) : base(output)
    {
    }
}