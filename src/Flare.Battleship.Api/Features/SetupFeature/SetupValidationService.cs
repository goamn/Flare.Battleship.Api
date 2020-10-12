using Flare.Battleship.Api.Features.SetupFeature.Dto;
using Flare.Battleship.Api.Features.SetupFeature.Repository;
using Flare.Battleship.Api.Helpers;
using Flare.Battleship.Api.Infrastructure.Exceptions;
using System.Linq;

namespace Flare.Battleship.Api.Features.SetupFeature
{
    public class SetupValidationService : ISetupValidationService
    {
        private readonly IBoardRepository _boardRepository;

        public SetupValidationService(IBoardRepository boardRepository)
        {
            _boardRepository = boardRepository;
        }
        public void ValidateCoordinatePlacement(Coordinate coordinate)
        {
            CoordinateInsideBoard(coordinate);
        }

        public void ValidateShipPlacement(PlaceShipRequest placeShipRequest)
        {
            CoordinateInsideBoard(placeShipRequest.TopLeftCoordinate);
            ShipInsideBoard(placeShipRequest);
            ShipNotConflicting(placeShipRequest);                       
        }

        private void CoordinateInsideBoard(Coordinate coordinates)
        {
            //Ideally we want to abstract the maximum size of 10 so that it's inside the BoardDb model 
            //  and it doesn't have to be 10.
            if (coordinates.XCoordinate < 1 || coordinates.XCoordinate > 10
                || coordinates.YCoordinate < 1 || coordinates.YCoordinate > 10)
            {
                throw new BadRequestException("A coordinate was placed outside the board size of 10 by 10.");
            }
        }
        
        private void ShipInsideBoard(PlaceShipRequest shipRequest)
        {
            var coordinates = shipRequest.TopLeftCoordinate;
            var horizontal = shipRequest.Alignment == Alignment.Horizontal;
            if (horizontal)
            {
                var shipsLastCoordinate = coordinates.XCoordinate + --shipRequest.ShipLength;
                if (shipsLastCoordinate > 10)
                    throw new BadRequestException("A coordinate was placed too close to the edge of the board");
            }
            else
            {
                var shipsLastCoordinate = coordinates.YCoordinate + --shipRequest.ShipLength;
                if (shipsLastCoordinate > 10)
                    throw new BadRequestException("A coordinate was placed too close to the edge of the board");
            }
        }
        private void ShipNotConflicting(PlaceShipRequest placeShipRequest)
        {
            var newShipCoordinates = CoordinateHelper.GetCoordinates(placeShipRequest.TopLeftCoordinate, placeShipRequest.ShipLength, placeShipRequest.Alignment);

            var board = _boardRepository.Find(placeShipRequest.BoardId);
            var ships = board.Ships;
            foreach (var existingShip in ships)
            {
                var existingCoordinates = CoordinateHelper.GetCoordinates(existingShip.TopLeftCoordinate, existingShip.ShipLength, existingShip.Alignment);
                var conflictWithExistingShip = newShipCoordinates.Any(s => existingCoordinates.Any(e => e.Equals(s)));
                if (conflictWithExistingShip)
                {
                    throw new BadRequestException("This coordinate is already occupied by an existing ship.");
                }
            }
        }
    }
    
    public interface ISetupValidationService
    {
        void ValidateShipPlacement(PlaceShipRequest placeShipRequest);
        void ValidateCoordinatePlacement(Coordinate coordinate);
    }

}