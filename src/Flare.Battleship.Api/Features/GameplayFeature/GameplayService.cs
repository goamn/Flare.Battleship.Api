using Flare.Battleship.Api.Features.GameplayFeature.Dto;
using Flare.Battleship.Api.Features.SetupFeature;
using Flare.Battleship.Api.Features.SetupFeature.Repository;
using Flare.Battleship.Api.Helpers;
using Flare.Battleship.Api.Infrastructure.Exceptions;
using System;
using System.Linq;
using static Flare.Battleship.Api.Features.GameplayFeature.Dto.AttackResult;

namespace Flare.Battleship.Api.Features.GameplayFeature
{
    public class GameplayService : IGameplayService
    {
        private readonly ISetupValidationService _setupValidationService;
        private readonly IBoardRepository _boardRepository;

        public GameplayService(ISetupValidationService setupValidationService, IBoardRepository boardRepository)
        {
            _setupValidationService = setupValidationService;
            _boardRepository = boardRepository;
        }
        public AttackOutcome AttackOpponent(Guid boardId, AttackRequest attackRequest)
        {
            _setupValidationService.ValidateCoordinatePlacement(attackRequest.Coordinate);

            var attackCoordinate = attackRequest.Coordinate;

            var board = _boardRepository.Find(boardId);
            if (board == null)
            {
                throw new BadRequestException($"Board ID '{boardId}' was not found.");
            }
            var ships = board.Ships;
            foreach (var ship in ships)
            {
                var shipCoordinates = CoordinateHelper.GetCoordinates(ship.TopLeftCoordinate, ship.ShipLength, ship.Alignment);
                var conflictWithExistingShip = shipCoordinates.FirstOrDefault(a => a.Equals(attackCoordinate));
                if (conflictWithExistingShip != null)
                {
                    ship.SuccessfulHits.Add(attackCoordinate);
                    return AttackOutcome.Hit;
                }
            }
            return AttackOutcome.Miss;
        }

        public bool PlayerDefeated(Guid boardId)
        {
            var board = _boardRepository.Find(boardId);
            if (board == null)
            {
                throw new BadRequestException($"Board ID '{boardId}' was not found.");
            }
            return board.Ships.All(x => x.IsSunk);
        }
    }
    public interface IGameplayService
    {
        AttackOutcome AttackOpponent(Guid boardId, AttackRequest placeShipRequest);
        bool PlayerDefeated(Guid boardId);
    }
}