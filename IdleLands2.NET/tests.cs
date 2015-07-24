using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace IdleLands2.NET
{
	[TestFixture]
	public class tests
	{
		public tests ()
		{
		}

		[Test]
		public void TestDamageReduction()
		{
			List<ICalcDamageReduction> people = new List<ICalcDamageReduction>();
			people.Add(new Person());
			people.Add(new Monster());

			int retDamageReduction = personalityReduce.PersonalityReduce(personalityReduce.calcDamageReduction, people);

			Assert.That(retDamageReduction == 25);
		}

		[Test]
		public void TestPhysicalAttackTargets()
		{
			Person person = new Person { id = 0 };
			Person person2 = new Person { id = 1 };
			Monster monster = new Monster { id = 2 };

			List<Tuple<IActor, int>> targets = new List<Tuple<IActor, int>>();
			targets.Add(new Tuple<IActor, int>(person, 100));
			targets.Add(new Tuple<IActor, int>(person2, 100));
			targets.Add(new Tuple<IActor, int>(monster, 100));

			List<ICalcPhysicalAttackTargets> actors = new List<ICalcPhysicalAttackTargets>();
			actors.Add(person);
			actors.Add(monster);

			var ret = personalityReduce.PersonalityReduce<Tuple<IActor, int>, ICalcPhysicalAttackTargets, 
			List<Tuple<IActor, int>>, IActor>(personalityReduce.calcPhysicalAttackTargets, targets, actors);

			Assert.That(ret.Count == 2);
			Assert.That(((IActor)ret[0].Item1).id != ret[0].Item2.id);
			Assert.That(((IActor)ret[1].Item1).id != ret[1].Item2.id);
		}
	}
}

