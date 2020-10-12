using Flare.Battleship.Api.Features.SetupFeature.Dto;
using System;
using System.Collections.Generic;

namespace Flare.Battleship.Api.Features.SetupFeature.Repository
{
    public class Ship
    {        
        public Guid ShipId { get; set; }
        public int ShipLength { get; set; }
        public Alignment Alignment { get; set; }
        public Coordinate TopLeftCoordinate { get; set; }
        public List<Coordinate> SuccessfulHits { get; set; } = new List<Coordinate>();
        public Guid BoardId { get; set; }
        public bool IsSunk
        {
            get
            {
                var entireShipHit = SuccessfulHits.Count == ShipLength;
                return entireShipHit;
            }
        }
    }
}
