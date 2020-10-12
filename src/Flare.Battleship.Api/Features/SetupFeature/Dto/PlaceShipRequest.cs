using System;

namespace Flare.Battleship.Api.Features.SetupFeature.Dto
{
    public class PlaceShipRequest
    {
        public Guid BoardId { get; set; }
        public int ShipLength { get; set; }
        public Alignment Alignment { get; set; }
        public Coordinate TopLeftCoordinate { get; set; }
    }    
}
