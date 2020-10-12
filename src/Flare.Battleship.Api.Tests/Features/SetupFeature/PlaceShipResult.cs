using System.Net.Http;

namespace Flare.Battleship.Api.Tests.Features.SetupFeature
{
    public class PlaceShipResult
    {
        public HttpResponseMessage Response { get; set; }
        public bool DeserialisedResponse { get; set; }
    }
}
