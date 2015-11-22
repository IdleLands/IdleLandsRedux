using IdleLandsRedux.GameLogic.DataEntities.Interfaces.Effects;

namespace IdleLandsRedux.GameLogic.DataEntities.Effects
{
    public class PoisonEffect : ISpecificEffect
    {
        public int RoundsLeft { get; set; }
        public int Intensity { get; set; }

        public void ApplyTo(SpecificCharacter character)
        {
            if (RoundsLeft > 0)
            {
                character.TransientStats.HitPointsDrained += Intensity;
            }
        }
        
        public void Cleanup(SpecificCharacter character)
        {
            RoundsLeft--;
        }
    }
}