using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Flare.Battleship.Api.Features.SetupFeature;
using Flare.Battleship.Api.Features.SetupFeature.Dto;
using Flare.Battleship.Api.Tests.Features.SetupFeature;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Flare.Battleship.Api.Tests.IntegrationTests
{
    public class SetupControllerTests
    {
        [Test]
        public void CreateNewBoardShouldBe201()
        {
            var newBoardRequest = new NewBoardRequest() { BoardName = "FirstBoard", PlayerName = "First Player" };
            var setupServiceMock = new Mock<ISetupService>();
            var generatedGuid = Guid.NewGuid();
            setupServiceMock.Setup(m => m.CreateNewBoard(newBoardRequest)).Returns(generatedGuid);
            var setupController = new SetupController(setupServiceMock.Object);

            var response = setupController.CreateNewBoard(newBoardRequest);

            Assert.IsTrue(response.Result is CreatedResult);
            var responseValue = (response.Result as CreatedResult)?.Value;
            Assert.AreEqual(generatedGuid, responseValue);
            setupServiceMock.Verify(mock => mock.CreateNewBoard(newBoardRequest), Times.Once());
        }

        [Test]
        public async Task ShipSuccessfullyPlacedShouldBeOk()
        {
            var placeShipRequest = new PlaceShipRequest()
            {
                Alignment = Alignment.Horizontal,
                ShipLength = 5,
                TopLeftCoordinate = new Coordinate(6, 3)
            };

            var placeShipResult = await PlaceShipCall(placeShipRequest);

            Assert.AreEqual(StatusCodes.Status200OK, (int)placeShipResult.Response.StatusCode);
        }

        [Test]
        public async Task ShipCoordinateOutsideBoardShouldBeBadRequest()
        {
            var placeShipRequest = new PlaceShipRequest()
            {
                Alignment = Alignment.Vertical,
                ShipLength = 2,
                TopLeftCoordinate = new Coordinate(15, 1)
            };

            var placeShipResult = await PlaceShipCall(placeShipRequest);

            Assert.AreEqual(StatusCodes.Status400BadRequest, (int)placeShipResult.Response.StatusCode);
            Assert.IsFalse(placeShipResult.DeserialisedResponse);
        }

        [Test]
        public async Task ShipOutsideBoardHorizontallyShouldBeBadRequest()
        {
            var placeShipRequest = new PlaceShipRequest()
            {
                Alignment = Alignment.Horizontal,
                ShipLength = 5,
                TopLeftCoordinate = new Coordinate(7, 1)
            };

            var placeShipResult = await PlaceShipCall(placeShipRequest);

            Assert.AreEqual(StatusCodes.Status400BadRequest, (int)placeShipResult.Response.StatusCode);
            Assert.IsFalse(placeShipResult.DeserialisedResponse);
        }

        [Test]
        public async Task ShipOutsideBoardVerticallyShouldBeBadRequest()
        {
            var placeShipRequest = new PlaceShipRequest()
            {
                Alignment = Alignment.Vertical,
                ShipLength = 4,
                TopLeftCoordinate = new Coordinate(1, 8)
            };

            var placeShipResult = await PlaceShipCall(placeShipRequest);

            Assert.AreEqual(StatusCodes.Status400BadRequest, (int)placeShipResult.Response.StatusCode);
            Assert.IsFalse(placeShipResult.DeserialisedResponse);
        }

        [Test]
        public async Task PlaceShipOutsideBoardVerticallyShouldBeBadRequest()
        {
            var placeShipRequest = new PlaceShipRequest()
            {
                Alignment = Alignment.Vertical,
                ShipLength = 4,
                TopLeftCoordinate = new Coordinate(1, 8)
            };

            var placeShipResult = await PlaceShipCall(placeShipRequest);

            Assert.AreEqual(StatusCodes.Status400BadRequest, (int)placeShipResult.Response.StatusCode);
            Assert.IsFalse(placeShipResult.DeserialisedResponse);
        }

        [Test]
        public async Task PlaceShipTwiceOnTheSameCoordinatesShouldBeBadRequest()
        {
            var newBoardId = Guid.NewGuid();
            var placeShipRequest = new PlaceShipRequest()
            {
                BoardId = newBoardId,
                Alignment = Alignment.Vertical,
                ShipLength = 4,
                TopLeftCoordinate = new Coordinate(1, 8)
            };

            await PlaceShipCall(placeShipRequest);
            var placeShipSecondTime = await PlaceShipCall(placeShipRequest);

            Assert.AreEqual(StatusCodes.Status400BadRequest, (int)placeShipSecondTime.Response.StatusCode);
            Assert.IsFalse(placeShipSecondTime.DeserialisedResponse);
        }

        private async Task<PlaceShipResult> PlaceShipCall(PlaceShipRequest placeShipRequest)
        {
            using (var client = IntegrationFixture.Server.CreateClient())
            {
                if (placeShipRequest.BoardId == Guid.Empty)
                {
                    placeShipRequest.BoardId = await CreateNewBoard(client);
                }                

                var response = await client.PostAsJsonAsync("Setup/ships", placeShipRequest);
                var content = await response.Content.ReadAsStringAsync();
                bool.TryParse(content, out bool deserialised);

                var placeShipResult = new PlaceShipResult
                {
                    DeserialisedResponse = deserialised,
                    Response = response
                };
                return placeShipResult;
            }
        }

        private async Task<Guid> CreateNewBoard(HttpClient client)
        {
            var ticks = DateTime.Now.Ticks;
            var newBoardRequest = new NewBoardRequest { BoardName = $"Board_{ticks}", PlayerName = $"Player_{ticks}" };
            var response = await client.PostAsJsonAsync("Setup/boards/create", newBoardRequest);
            var newBoardGuid = await response.Content.ReadAsAsync<Guid>();
            return newBoardGuid;
        }
    }
}