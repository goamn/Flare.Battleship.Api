using System;
using System.Collections.Generic;
using System.Linq;

namespace Flare.Battleship.Api.Features.SetupFeature.Repository
{
    public class BoardRepository : IBoardRepository
    {
        public Guid AddBoard(string boardName, string playerName)
        {
            var newBoard = new Board()
            {
                Id = Guid.NewGuid(),
                Name = boardName,
                PlayerName = playerName,
                Ships = new List<Ship>()
            };
            MemoryState.Boards.Add(newBoard);
            return newBoard.Id;
        }

        public Board Find(Guid boardId)
        {
            var board = MemoryState.Boards.FirstOrDefault(x => x.Id == boardId);
            return board;
        }

        public bool AddShip(Ship ship)
        {
            var board = Find(ship.BoardId);
            if (board == null)
            {
                return false;
            }
            board.Ships.Add(ship);
            return true;
        }
    }

    public interface IBoardRepository
    {
        Guid AddBoard(string boardName, string playerName);
        Board Find(Guid boardId);
        bool AddShip(Ship ship);
    }
}
