using Trakx.Common.Testing.Documentation.GenerateApiClient;

namespace Trakx.Kaiko.ApiClient.Tests;

public class GenerateApiClientChecker : GenerateApiClientCheckerBase<KaikoApiConfiguration>
{
    public GenerateApiClientChecker(ITestOutputHelper output) : base(output)
    {
    }
}
