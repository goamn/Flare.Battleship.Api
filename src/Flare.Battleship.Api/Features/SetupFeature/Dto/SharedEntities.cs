using System;

namespace Flare.Battleship.Api.Features.SetupFeature.Dto
{
    public class Coordinate : ICloneable, IEquatable<Coordinate>
    {
        public Coordinate(int x, int y)
        {
            XCoordinate = x;
            YCoordinate = y;
        }
        public int XCoordinate { get; set; }
        public int YCoordinate { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public bool Equals(Coordinate other)
        {
            var coordinatesAreEqual = this.XCoordinate == other.XCoordinate && this.YCoordinate == other.YCoordinate;
            return coordinatesAreEqual;
        }
    }

    public enum Alignment
    {
        Horizontal = 1,
        Vertical = 2
    }
}
