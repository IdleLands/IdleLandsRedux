using System;
using WebSocketSharp;
using Newtonsoft.Json;
using IdleLandsRedux.Contracts.API;
using IdleLandsRedux.DataAccess;
using NUnit.Framework;

namespace IdleLandsRedux.WebService.Test
{
	[TestFixture]
	public class RegisterTest
	{
		private Bootstrapper _bootstrapper = null;
		private bool? correct = null;
		private ResponseMessage response = null;
		private WebSocket ws = null;

		[TestFixtureSetUp]
		public void TestSetup()
		{
			_bootstrapper = new Bootstrapper(log4net.LogManager.GetLogger(typeof(RegisterTest)));
			ws = new WebSocket("ws://localhost:2345/IdleLands");
			ws.Log.Level = LogLevel.Trace;

			ResetResponse();

			ws.OnMessage += (sender, e) => {
				Console.WriteLine("Response: " + e.Data);

				if(string.IsNullOrEmpty(e.Data))
				{
					correct = false;
					return;
				}

				response = JsonConvert.DeserializeObject<ResponseMessage>(e.Data);

				if(response == null)
				{
					correct = false;
					return;
				}
				correct = true;
			};

			ws.Connect();
		}

		[TestFixtureTearDown]
		public void TestCleanup()
		{
			if (_bootstrapper != null) {
				_bootstrapper.Dispose();
				_bootstrapper = null;
			}

			if (ws != null) {
				ws.Close();
				ws = null;
			}
		}

		private void WaitForResponse()
		{
			int time = 0;
			while (!correct.HasValue) {
				System.Threading.Thread.Sleep(100);
				time += 100;

				if (time > 15000)
					Assert.That(false);
			}
		}

		private void ResetResponse()
		{
			response = null;
			correct = null;
		}

		[Test]
		public void SimpleRegisterTest()
		{
			var msg = JsonConvert.SerializeObject(new RegisterMessage {
				Path = "/register",
				Password = "test",
				Username = "test"
			});

			ws.Send (msg);

			WaitForResponse();

			Assert.That(correct.Value == true);
			Assert.That(response.Success == true);
			Assert.That(response.Token != null);
		}

		[Test]
		public void CheckDoubleRegisterTest()
		{
			var msg = JsonConvert.SerializeObject(new RegisterMessage {
				Path = "/register",
				Password = "test",
				Username = "test"
			});

			ws.Send (msg);

			WaitForResponse();

			Assert.That(correct.Value == true);
			Assert.That(response.Success == true);
			Assert.That(response.Token != null);

			ResetResponse();

			ws.Send (msg);

			WaitForResponse();

			Assert.That(correct.Value == true);
			Assert.That(response.Success == false);
			Assert.That(response.Token == null);
			Assert.That(response.Error == "Username already exists.");
		}
	}
}

