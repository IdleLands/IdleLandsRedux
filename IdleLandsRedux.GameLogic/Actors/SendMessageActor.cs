using Akka.Actor;
using IdleLandsRedux.Contracts.API;
using log4net;

namespace IdleLandsRedux.GameLogic
{
    public sealed class WebServiceMessage
    {
        public readonly Message Message;

        public WebServiceMessage(Message message)
        {
            this.Message = message;
        }
    }

    public class SendMessageActor : ReceiveActor
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Bootstrapper));

        public SendMessageActor()
        {
            //init socket etc
            
            Receive<WebServiceMessage>(msg =>
            {
                
                //send deserialized message over socket with JsonConvert.SerializeObject(msg)
            });
        }
    }
}

