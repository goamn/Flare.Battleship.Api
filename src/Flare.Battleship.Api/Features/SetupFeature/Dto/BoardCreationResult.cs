using System;

namespace Flare.Battleship.Api.Features.SetupFeature.Dto
{
    public class BoardCreationResult
    {
        public Guid Id { get; set; }
        public Guid Player1Id { get; set; }
        public Guid Player2Id { get; set; }
    }
}