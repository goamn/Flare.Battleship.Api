using Flare.Battleship.Api.Features.GameplayFeature.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Flare.Battleship.Api.Features.GameplayFeature
{
    [Route("[controller]")]
    [ApiController]
    public class GameplayController : ControllerBase
    {
        private readonly IGameplayService _gameplayService;

        public GameplayController(IGameplayService gameplayService)
        {
            _gameplayService = gameplayService;
        }

        [HttpPost("boards/{boardId}/attack")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public ActionResult<AttackResult> AttackOpponent(Guid boardId, [FromBody]AttackRequest placeShipRequest)
        {
            var attackOutcome = _gameplayService.AttackOpponent(boardId, placeShipRequest);
            var attackResult = new AttackResult() { Outcome = attackOutcome };
            return Ok(attackResult);
        }

        [HttpGet("boards/{boardId}/defeated")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        public ActionResult<AttackResult> PlayerDefeated(Guid boardId)
        {
            var playerDefeated = _gameplayService.PlayerDefeated(boardId);
            return Ok(playerDefeated);
        }
    }
}