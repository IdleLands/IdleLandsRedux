using IdleLandsRedux.Common;

namespace IdleLandsRedux.GameLogic.DataEntities.Interfaces.Effects
{
	public interface ISpecificEffect : IEffect
	{
		void ApplyTo(SpecificCharacter character);
		void Cleanup(SpecificCharacter character);
	}
}