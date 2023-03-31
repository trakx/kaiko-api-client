using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Loader;
using System.Text.RegularExpressions;
using System.Text;
using Trakx.Utils.Attributes;
using Trakx.Utils.Extensions;
using Trakx.Utils.Testing.ReadmeUpdater;

namespace Trakx.Kaiko.ApiClient.Stream.Tests;

public class ReadmeDocumentationUpdater : ReadmeDocumentationUpdaterBase
{
    public ReadmeDocumentationUpdater(ITestOutputHelper output) : base(output)
    {
    }
}