namespace Flare.Battleship.Api.Features.GameplayFeature.Dto
{
    public class AttackResult
    {
        public AttackOutcome Outcome { get; set; }

        public enum AttackOutcome
        {
            Miss = 0,
            Hit = 1
        }
    }
}