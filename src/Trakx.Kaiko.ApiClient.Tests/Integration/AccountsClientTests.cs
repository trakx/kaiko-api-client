using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace Trakx.Kaiko.ApiClient.Tests.Integration;

public sealed class AccountsClientTests : KaikoClientTestsBase
{
    private readonly IAggregatesClient _aggregates;
    public AccountsClientTests(KaikoApiFixture apiFixture, ITestOutputHelper output) : base(apiFixture, output)
    {
        _aggregates = ServiceProvider.GetRequiredService<IAggregatesClient>();
    }

    //[Fact]
    //public async Task ListAccounts_and_GetAccount_should_return_results()
    //{
    //    var accounts = (await _aggregates.ListAccountsAsync()).Result;
    //    var first = accounts.First();

    //    Logger.Information("Found accounts: {accounts}",
    //        JsonSerializer.Serialize(accounts));

    //    var account = (await _aggregates.GetAccountAsync(first.Id)).Result;

    //    account.Id.Should().Be(first.Id);
    //    account.Exchange.Should().Be(first.Exchange);
    //    account.IsRebalancing.Should().Be(first.IsRebalancing);
    //}

    //[Fact]
    //public async Task GetBalance_should_work()
    //{
    //    var accounts = (await _aggregates.ListAccountsAsync()).Result;
    //    var binanceAccountId = accounts.Single(a => a.Exchange == Exchange.Binance).Id;

    //    var balances = (await _aggregates.GetBalanceAsync(binanceAccountId)).Result.Balances;
    //    balances.Count.Should().BeGreaterThan(1);
    //    Logger.Information("Binance balances: {balances}",
    //        string.Join(", ", JsonSerializer.Serialize(balances)));

    //    balances.First(b => string.Equals(b.Symbol, "eur", StringComparison.InvariantCultureIgnoreCase)).UsdValue
    //        .Should().NotBe(0);
    //}
}
