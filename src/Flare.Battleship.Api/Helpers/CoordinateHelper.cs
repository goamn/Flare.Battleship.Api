using Flare.Battleship.Api.Features.SetupFeature.Dto;
using System.Collections.Generic;

namespace Flare.Battleship.Api.Helpers
{
    public static class CoordinateHelper
    {
        public static List<Coordinate> GetCoordinates(Coordinate topLeft, int length, Alignment alignment)
        {
            var coordinates = new List<Coordinate>();
            var horizontal = alignment == Alignment.Horizontal;
            for (int i = 0; i < length; i++)
            {
                var newCoordinate = (Coordinate)topLeft.Clone();
                if (horizontal)
                {
                    newCoordinate.XCoordinate += i;
                }
                else
                {
                    newCoordinate.YCoordinate += i;
                }
                coordinates.Add(newCoordinate);
            }
            return coordinates;
        }
    }
}
