using System;
using System.Collections.Generic;
using NUnit.Framework;
using IdleLandsRedux.SpecificMappings;
using IdleLandsRedux.DataAccess.Mappings;

namespace IdleLandsRedux
{
	[TestFixture]
	public class ReductionTests
	{
		public ReductionTests ()
		{
		}

		[Test]
		public void TestDamageReduction()
		{
			List<ICalcDamageReduction> people = new List<ICalcDamageReduction>();
			people.Add(new SpecificPlayer());
			people.Add(new SpecificMonster());

			int retDamageReduction = personalityReduce.PersonalityReduce(personalityReduce.calcDamageReduction, people);

			Assert.That(retDamageReduction == 25);
		}

		[Test]
		public void TestPhysicalAttackTargets()
		{
			SpecificPlayer person = new SpecificPlayer { Id = 0, Stats = new StatsObject { Dexterity = 1 } };
			SpecificPlayer person2 = new SpecificPlayer { Id = 1, Stats = new StatsObject { Dexterity = 2 } };
			SpecificMonster monster = new SpecificMonster { Id = 2, Stats = new StatsObject { Dexterity = 3 } };

			List<Tuple<Character, int>> targets = new List<Tuple<Character, int>>();
			targets.Add(new Tuple<Character, int>(person, 100));
			targets.Add(new Tuple<Character, int>(person2, 100));
			targets.Add(new Tuple<Character, int>(monster, 100));

			List<ICalcPhysicalAttackTargets> actors = new List<ICalcPhysicalAttackTargets>();
			actors.Add(person);
			actors.Add(monster);

			var ret = personalityReduce.PersonalityReduce<Tuple<Character, int>, ICalcPhysicalAttackTargets, 
			List<Tuple<Character, int>>, Character>(personalityReduce.calcPhysicalAttackTargets, targets, actors);

			Assert.That(ret.Count == 2);
			Assert.That(((SpecificCharacter)ret[0].Item1).Id != ret[0].Item2.Id);
			Assert.That(((SpecificCharacter)ret[1].Item1).Id != ret[1].Item2.Id);
		}
	}
}

