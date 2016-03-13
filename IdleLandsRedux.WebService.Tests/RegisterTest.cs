using System;
using System.Diagnostics.CodeAnalysis;
using WebSocketSharp;
using Newtonsoft.Json;
using IdleLandsRedux.Contracts;
using IdleLandsRedux.Contracts.API;
using IdleLandsRedux.DataAccess;
using NUnit.Framework;
using FluentAssertions;

namespace IdleLandsRedux.WebService.Test
{
	[SuppressMessage("Gendarme.Rules.Performance", "UseStringEmptyRule", Justification = "FluentAssertions uses a default value, outside of our control.")]
	[SuppressMessage("Gendarme.Rules.Design", "TypesWithDisposableFieldsShouldBeDisposableRule", Justification = "Gets disposed in the test teardown.")]
	[TestFixture]
	public class RegisterTest
	{
		private Bootstrapper _bootstrapper;
		private bool? correct;
		private ResponseMessage response;
		private WebSocket ws;

		[OneTimeSetUpAttribute]
		public void TestSetup()
		{
			_bootstrapper = new Bootstrapper(log4net.LogManager.GetLogger(typeof(RegisterTest)));
			ws = new WebSocket("ws://localhost:2345/IdleLands/register");
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

		[OneTimeTearDownAttribute]
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
		
		private string GetRegisterMessage()
		{
			return JsonConvert.SerializeObject(new RegisterMessage {
				Password = "test",
				Username = "test"
			});
		}

		[SuppressMessage("Gendarme.Rules.Smells", "AvoidCodeDuplicatedInSameClassRule", Justification = "Correct usage in a test.")]
		[Test]
        [Category("this")]
		public void SimpleRegisterTest()
		{
			var msg = GetRegisterMessage();

			ws.Send (msg);

			WaitForResponse();
			
			
			correct.Value.Should().Be(true);
			response.Success.Should().Be(true);
			response.Token.Should().NotBeNull();
            
            while(true)
            {
                System.Threading.Thread.Sleep(100);
            }
		}

		[Test]
		public void CheckDoubleRegisterTest()
		{
			var msg = GetRegisterMessage();

			ws.Send (msg);

			WaitForResponse();

			correct.Value.Should().Be(true);
			response.Success.Should().Be(true);
			response.Token.Should().NotBeNull();

			ResetResponse();

			ws.Send (msg);

			WaitForResponse();

			correct.Value.Should().Be(true);
			response.Success.Should().Be(false);
			response.Token.Should().BeNull();
			response.Error.Should().Be("Username already exists.");
		}
	}
}

