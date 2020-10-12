using Flare.Battleship.Api.Features.SetupFeature.Dto;
using Flare.Battleship.Api.Features.SetupFeature.Repository;
using Flare.Battleship.Api.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;

namespace Flare.Battleship.Api.Features.SetupFeature
{
    public class SetupService : ISetupService
    {
        private readonly ISetupValidationService _setupValidation;
        private readonly IBoardRepository _boardRepository;

        public SetupService(ISetupValidationService validationService, IBoardRepository boardRepository)
        {
            _setupValidation = validationService;
            _boardRepository = boardRepository;
        }
        public Guid CreateNewBoard(NewBoardRequest newBoardRequest)
        {
            var newBoardId = _boardRepository.AddBoard(newBoardRequest.BoardName, newBoardRequest.PlayerName);

            return newBoardId;
        }

        public void PlaceShipOnBoard(PlaceShipRequest placeShipRequest)
        {
            _setupValidation.ValidateShipPlacement(placeShipRequest);

            var newShip = new Ship()
            {
                BoardId = placeShipRequest.BoardId,
                Alignment = placeShipRequest.Alignment,
                ShipLength = placeShipRequest.ShipLength,
                TopLeftCoordinate = placeShipRequest.TopLeftCoordinate,
                SuccessfulHits = new List<Coordinate>()
            };
            var isSuccessful = _boardRepository.AddShip(newShip);
            if (isSuccessful == false)
            {
                throw new BadRequestException($"Adding ship failed, board ID of '{placeShipRequest.BoardId}' was not found.");
            }
        }
    }

    public interface ISetupService
    {
        Guid CreateNewBoard(NewBoardRequest newBoardRequest);
        void PlaceShipOnBoard(PlaceShipRequest placeShipRequest);
    }
}