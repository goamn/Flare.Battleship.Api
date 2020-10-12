using Flare.Battleship.Api.Features.SetupFeature.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Flare.Battleship.Api.Features.SetupFeature
{
    [Route("[controller]")]
    [ApiController]
    public class SetupController : ControllerBase
    {
        private readonly ISetupService _setupService;

        public SetupController(ISetupService setupService)
        {
            _setupService = setupService;
        }
        
        [HttpPost("boards/create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Produces("application/json")]
        public ActionResult<Guid> CreateNewBoard([FromBody]NewBoardRequest newBoardRequest)
        {
            //Ideally we want to return a class with more than the boardId
            var boardId = _setupService.CreateNewBoard(newBoardRequest);
            return Created("", boardId);
        }

        [HttpPost("ships")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public ActionResult PlaceShip([FromBody]PlaceShipRequest placeShipRequest)
        {
            _setupService.PlaceShipOnBoard(placeShipRequest);
            return Ok();
        }
    }
}