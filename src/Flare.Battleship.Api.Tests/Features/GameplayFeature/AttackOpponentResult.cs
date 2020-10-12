using Flare.Battleship.Api.Features.GameplayFeature.Dto;
using System.Net.Http;

namespace Flare.Battleship.Api.Tests.Features.SetupFeature
{
    public class AttackOpponentResult
    {
        public HttpResponseMessage Response { get; set; }
        public AttackResult AttackResult { get; set; }
    }
}
