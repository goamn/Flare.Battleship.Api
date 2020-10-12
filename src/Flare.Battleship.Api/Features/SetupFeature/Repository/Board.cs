using System;
using System.Collections.Generic;

namespace Flare.Battleship.Api.Features.SetupFeature.Repository
{
    public class Board
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string PlayerName { get; set; }
        public List<Ship> Ships { get; set; } = new List<Ship>();
    }
}
