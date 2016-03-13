using System;
using System.Linq;
using System.Collections.Generic;
using log4net;
using IdleLandsRedux.DataAccess;
using IdleLandsRedux.DataAccess.Mappings;
using IdleLandsRedux.GameLogic.Actors;
using Akka.Actor;
using Akka.Event;
using NHibernate.SqlCommand;
using IdleLandsRedux.WebService.Services;
using WebSocketSharp.Server;
using WebSocketSharp;

namespace IdleLandsRedux.WebService
{
    public static class Program
    {
        static readonly ILog log = LogManager.GetLogger(typeof(Program));
        private static bool _stop; //defaults to false

        private static void Main()
        {
            log.Info("Starting WebService");

            //new Bootstrapper(log);
           
            Console.CancelKeyPress += (sender, e) =>
            {
                _stop = true;
                log.Info("Ctrl^C received, stopping program.");
                e.Cancel = true;
            };
            
            using (var system = ActorSystem.Create("DispatchSystem"))
            {
				var inbox = Inbox.Create(system);
				var activeUserActor = system.ActorOf(Props.Create(() => new ActiveUsersActor()), "remoteactor");
                inbox.Send(activeUserActor, new InitializeInbox());
                inbox.Send(activeUserActor, new AddUserMessage("Oipo"));

                /*var wssv = new WebSocketServer("ws://localhost:2345");

				wssv.AddWebSocketService<LoginService>("/IdleLands/login", () => new LoginService(activeUserActor, inbox));
				wssv.AddWebSocketService<RegisterService>("/IdleLands/register", () => new RegisterService(activeUserActor, inbox));
                wssv.Log.Level = LogLevel.Trace;
                wssv.Log.File = "IdleLands.Websocket.log";
                wssv.Start();
    
                if (!wssv.IsListening) {
                    log.Error("Service failed to start");
                    return;
                }
    
                log.Info("WebService started");*/

                while (!_stop)
                {
                    try
                    {
                        var message = inbox.Receive();
                        if(message is ExpireUser)
                        {
                            var expiredUser = message as ExpireUser;
                            log.Info("WebServer received expiration timeout: " + expiredUser.User);
                            inbox.Send(activeUserActor, new AddUserMessage("Oipo"));
                        }
                        else if(message is AddUserMessageResponse)
                        {
                            var response = message as AddUserMessageResponse;
                            log.Info("WebServer received response: " + response.Code.ToString() + ":" + response.Token.ToString());
                        }
                        else if(message is String)
                        {
                            log.Info("WebServer received response: " + (message as String));
                        }
                        else
                        {
                            log.Info("WebServer received unknown message of type: " + message.GetType().FullName);
                        }
                    }
                    catch (TimeoutException)
                    {
                        // timeout
                    }
                }
                
                //wssv.Stop();
            }

            log.Info("Stopping IdleLands.WebService websocket.");

            //wssv.Stop();

            log.Info("Quitting");
        }
    }
}

