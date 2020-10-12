using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using Flare.Battleship.Api.Features.GameplayFeature;
using Flare.Battleship.Api.Features.SetupFeature.Dto;
using Flare.Battleship.Api.Features.SetupFeature.Repository;
using Flare.Battleship.Api.Tests.Features.SetupFeature;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static Flare.Battleship.Api.Features.GameplayFeature.Dto.AttackResult;
using Flare.Battleship.Api.Features.GameplayFeature.Dto;

namespace Flare.Battleship.Api.Tests.IntegrationTests
{
    public class GameplayControllerTests
    {
        [Test]
        public async Task AttackInsideBoard_ShouldBeOkAndMissOutcome()
        {
            var attackRequest = new AttackRequest()
            {
                Coordinate = new Coordinate(5, 5)
            };
            var ship = new Ship() { Alignment = Alignment.Horizontal, ShipLength = 5, TopLeftCoordinate = new Coordinate(1, 1) };
            var newBoardId = CreateBoardAndPlaceShip(ship);

            var attackResult = await PostAttackOpponent(attackRequest, newBoardId);

            Assert.AreEqual(StatusCodes.Status200OK, (int)attackResult.Response.StatusCode);
            Assert.IsNotNull(attackResult.AttackResult);
            Assert.AreEqual(AttackOutcome.Miss, attackResult.AttackResult.Outcome);
        }

        [Test]
        public async Task AttackOutsideBoard_ShouldBeBadRequest()
        {
            var attackRequest = new AttackRequest()
            {
                Coordinate = new Coordinate(15, 5)
            };
            var ship = new Ship() { Alignment = Alignment.Horizontal, ShipLength = 5, TopLeftCoordinate = new Coordinate(1, 1) };
            var newBoardId = CreateBoardAndPlaceShip(ship);

            var attackResult = await PostAttackOpponent(attackRequest, newBoardId);

            Assert.AreEqual(StatusCodes.Status400BadRequest, (int)attackResult.Response.StatusCode);
            Assert.IsNull(attackResult.AttackResult);
        }

        [Test]
        public async Task PlaceShipAndAttack_ShouldBeOkAndHitOutcome()
        {
            var attackRequest = new AttackRequest()
            {
                Coordinate = new Coordinate(1, 1)
            };
            var ship = new Ship() { Alignment = Alignment.Horizontal, ShipLength = 5, TopLeftCoordinate = new Coordinate(1, 1) };
            var newBoardId = CreateBoardAndPlaceShip(ship);

            var attackResult = await PostAttackOpponent(attackRequest, newBoardId);

            Assert.AreEqual(StatusCodes.Status200OK, (int)attackResult.Response.StatusCode);
            Assert.IsNotNull(attackResult.AttackResult);
            Assert.AreEqual(AttackOutcome.Hit, attackResult.AttackResult.Outcome);
        }

        [Test]
        public void AttackOnShip_ShouldBeOkAndHitOutcome()
        {
            var attackRequest = new AttackRequest()
            {
                Coordinate = new Coordinate(3, 5)
            };
            var boardId = Guid.NewGuid();
            var attackOutcome = AttackOutcome.Hit;
            var gameplayServiceMock = new Mock<IGameplayService>();
            gameplayServiceMock.Setup(m => m.AttackOpponent(boardId,attackRequest)).Returns(attackOutcome);
            var gameplayController = new GameplayController(gameplayServiceMock.Object);

            var response = gameplayController.AttackOpponent(boardId, attackRequest);

            Assert.IsTrue(response.Result is OkObjectResult);
            var responseAttackOutcome = ((response.Result as OkObjectResult)?.Value as AttackResult).Outcome;
            Assert.AreEqual(attackOutcome, responseAttackOutcome);
            gameplayServiceMock.Verify(mock => mock.AttackOpponent(boardId, attackRequest), Times.Once());
        }

        [Test]
        public async Task PlayerWithShipNotSunk_ShouldNotBeDefeated()
        {
            var ship = new Ship() { Alignment = Alignment.Horizontal, ShipLength = 5, TopLeftCoordinate = new Coordinate(1, 1) };

            var newBoardId = CreateBoardAndPlaceShip(ship);
            var playedDefeated = await PlayedHasBeenDefeated(newBoardId);

            Assert.IsFalse(playedDefeated);
        }

        [Test]
        public async Task PlayerWithAllShipsSunk_ShouldBeDefeated()
        {
            var ship = new Ship { Alignment = Alignment.Horizontal, ShipLength = 3, TopLeftCoordinate = new Coordinate(1, 1) };
            var newBoardId = CreateBoardAndPlaceShip(ship);
            var gameplayService = IntegrationFixture.GetGameplayService();
            gameplayService.AttackOpponent(newBoardId, new AttackRequest { Coordinate = new Coordinate(1, 1) });
            gameplayService.AttackOpponent(newBoardId, new AttackRequest { Coordinate = new Coordinate(2, 1) });
            gameplayService.AttackOpponent(newBoardId, new AttackRequest { Coordinate = new Coordinate(3, 1) });

            var playerDefeated = await PlayedHasBeenDefeated(newBoardId);

            Assert.IsTrue(playerDefeated);
        }

        private async Task<AttackOpponentResult> PostAttackOpponent(AttackRequest attackRequest, Guid newBoardId)
        {
            using var client = IntegrationFixture.Server.CreateClient();
            var response = await client.PostAsJsonAsync($"Gameplay/boards/{newBoardId}/attack", attackRequest);
            AttackResult attackResult = null;
            try
            {
                attackResult = await response.Content.ReadAsAsync<AttackResult>();
            }
            catch (Exception) { }

            var attackOpponentResult = new AttackOpponentResult()
            {
                AttackResult = attackResult,
                Response = response
            };
            return attackOpponentResult;
        }

        private Guid CreateBoardAndPlaceShip(Ship ship)
        {
            var newBoardId = Guid.NewGuid();
            ship.BoardId = newBoardId;
            var boardWithShip = new Board() { Id = newBoardId, Ships = new List<Ship>() { ship } };
            MemoryState.Boards.Add(boardWithShip);
            return newBoardId;
        }

        private async Task<bool> PlayedHasBeenDefeated(Guid boardId)
        {
            using var client = IntegrationFixture.Server.CreateClient();
            var response = await client.GetAsync($"Gameplay/boards/{boardId}/defeated");
            var defeated = await response.Content.ReadAsAsync<bool>();
            return defeated;
        }
    }
}