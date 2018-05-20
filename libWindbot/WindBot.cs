using System;
using System.IO;
using System.Threading;
using System.Net;
using WindBot.Game;
using WindBot.Game.AI;
using YGOSharp.OCGWrapper;

namespace WindBot
{
    public class WindBot
    {
        internal static Random Rand;
        public static string AssetPath;

        public static void Test1(string test)
        {
            WindBotInfo Info = new WindBotInfo();
            Info.Host = "118.178.111.167";
            Info.Port = 9999;
            Info.HostInfo = test;
            Thread workThread = new Thread(new ParameterizedThreadStart(Run));
            workThread.Start(Info);
            //Windbot.Run(Info);
        }

        public static string Test2(string test)
        {
            NamedCard card1 = NamedCard.Get(22862454);
            return card1.Name;
        }

        public static void InitAndroid(string assetPath, string databasePath)
        {
            Rand = new Random();
            AssetPath = assetPath;
            DecksManager.Init();
            NamedCardsManager.Init(databasePath);
        }

        public static void Run(object o)
        {
#if !DEBUG
    try
    {
    //all errors will be catched instead of causing the program to crash.
#endif
            WindBotInfo Info = (WindBotInfo)o;
            GameClient client = new GameClient(Info);
            client.Start();
            Logger.DebugWriteLine(client.Username + " started.");
            while (client.Connection.IsConnected)
            {
#if !DEBUG
        try
        {
#endif
                client.Tick();
                Thread.Sleep(30);
#if !DEBUG
        }
        catch (Exception ex)
        {
            Logger.WriteErrorLine("Tick Error: " + ex);
        }
#endif
            }
            Logger.DebugWriteLine(client.Username + " end.");
#if !DEBUG
    }
    catch (Exception ex)
    {
        Logger.WriteErrorLine("Run Error: " + ex);
    }
#endif
        }
    }
}
