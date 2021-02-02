using System;
using System.Collections.Generic;
using DeviceUtil;
using HMSDataRepo.Model;

namespace SSHService
{
    public class Program
    {
        public const string RefreshCommand = "Refresh", StopCommand = "Stop", StartCommand = "Start";
        public const int Port = 3030;

        private static void Main(string[] args)
        {
            Server = Hik.Communication.Scs.Server.ScsServerFactory.CreateServer(new Hik.Communication.Scs.Communication.EndPoints.Tcp.ScsTcpEndPoint(Port));
            Server.ClientConnected += Server_ClientConnected;
            Server.ClientDisconnected += Server_ClientDisconnected;
        }

        private static void Server_ClientDisconnected(object sender, Hik.Communication.Scs.Server.ServerClientEventArgs e)
        {
            Client = null;
        }
        private static void Server_ClientConnected(object sender, Hik.Communication.Scs.Server.ServerClientEventArgs e)
        {
            Client = e.Client;
            Client.MessageSent += Client_MessageSent;
        }

        private static void Client_MessageSent(object sender, Hik.Communication.Scs.Communication.Messages.MessageEventArgs e)
        {
            if (e.Message is Hik.Communication.Scs.Communication.Messages.ScsTextMessage text)
            {
                switch (text.Text)
                {
                    case StartCommand: RestartRecognizers(); break;
                    case StopCommand: StopRecognizers(); break;
                    case RefreshCommand: RefreshRecognizersInfo(); break;
                    default:
                        break;
                }
            }
        }



        private static void RefreshRecognizersInfo()
        {
            try
            {

            }
            catch (Exception e)
            {
                RepotError(e);
            }
        }

        private static void RepotError(Exception e)
        {
            Client.SendMessage(new Hik.Communication.Scs.Communication.Messages.ScsTextMessage("Error| " + e.Message));
        }

        private static void StopRecognizers()
        {
            throw new NotImplementedException();
        }

        private static void RestartRecognizers()
        {
            throw new NotImplementedException();
        }

        public static Hik.Communication.Scs.Server.IScsServer Server { get; private set; }
        public static List<DeviceUtil.Recognition.LiveRecognitionUnit> LiveRecognitionUnits { get; } = new List<DeviceUtil.Recognition.LiveRecognitionUnit>();
        public static List<RoomCameraInfo> RoomCameras { get; } = new List<RoomCameraInfo>();
        public static Stack<Records> Records { get; } = new Stack<Records>();
        public static Hik.Communication.Scs.Server.IScsServerClient Client { get; private set; }

    }
}