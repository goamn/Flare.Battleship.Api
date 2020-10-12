using Microsoft.Extensions.DependencyInjection;
using System;

namespace Flare.Battleship.Api.Tests.Infrastructure
{
    public class ServerFixtureConfiguration
    {
        public Type StartupType { get; set; }
        public Action<IServiceCollection> MainServicePostConfigureServices { get; set; }
    }
}
