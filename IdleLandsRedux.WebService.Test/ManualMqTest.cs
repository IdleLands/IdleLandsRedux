using System;
using System.Threading;
using NUnit.Framework;
using Newtonsoft.Json;
using NetMQ;
using IdleLandsRedux.Contracts.MQ;

namespace IdleLandsRedux.WebService.Test
{
	[TestFixture]
	public class ManualMqTest
	{
		[Test]
		public void _ManualMqTest()
		{
			using (var ctx = NetMQContext.Create()) {
				using (var sender = ctx.CreatePushSocket()) {
					sender.Bind("tcp://localhost:8172");
					Task task = new Task { Type = TaskType.Battle };
					string message = JsonConvert.SerializeObject(task);
					for(int i = 0; i < 200; i++) {
						sender.SendFrame(message);
					}
				}
			}
		}
	}
}

