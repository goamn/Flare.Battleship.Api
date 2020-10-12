using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Flare.Battleship.Api.Tests.Infrastructure;
using System.IO;
using Flare.Battleship.Api.Features.GameplayFeature;

namespace Flare.Battleship.Api.Tests.IntegrationTests
{
    [SetUpFixture]
    public class IntegrationFixture
    {
        public static TestServer Server;

        public static IGameplayService GetGameplayService() => Server.Services.GetService<IGameplayService>();

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var serverFixtureConfiguration = new ServerFixtureConfiguration
            {
                StartupType = typeof(Startup)
            };
            var wrappedStartupType = typeof(TestsStartup<>).MakeGenericType(serverFixtureConfiguration.StartupType);

            var host = new WebHostBuilder()
                .UseEnvironment("Test")
                .ConfigureServices(services =>
                {
                    services.AddSingleton(serverFixtureConfiguration);
                })
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup(wrappedStartupType);

            Server = new TestServer(host);
        }
    }
}
