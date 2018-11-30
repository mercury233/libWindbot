using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using WindBot.Game;
using WindBot.Game.AI;
using YGOSharp.OCGWrapper;

namespace WindBot
{
    public class WindBot
    {
        public static string AssetPath;

        public static void Test1(string test)
        {
            WindBotInfo Info = new WindBotInfo
            {
                Host = "118.178.111.167",
                Port = 9999,
                HostInfo = test
            };
            Thread workThread = new Thread(new ParameterizedThreadStart(Run));
            workThread.Start(Info);
            //Windbot.Run(Info);
        }

        public static string Test2(string test)
        {
            NamedCard card1 = NamedCard.Get(22862454);
            return card1.Name;
        }

        public static void InitAndroid(string assetPath, string databasePath, string confPath)
        {
            Program.Rand = new Random();
            AssetPath = assetPath;
            DecksManager.Init();
            NamedCardsManager.Init(databasePath);
            ReadBots(confPath);
        }

        private static IList<string> ParseArgs(string arg)
        {
            return Regex.Split(arg, "(?<=^[^\']*(?:\'[^\']*\'[^\']*)*) (?=(?:[^\']*\'[^\']*\')*[^\']*$)").ToList(); // https://stackoverflow.com/questions/4780728/regex-split-string-preserving-quotes/
        }

        public static void RunAndroid(string arg)
        {
            IList<string> args = ParseArgs(arg);
            Match match = Regex.Match(arg, "Random=(\\w+)");
            if (match.Success)
            {
                string randomFlag = match.Groups[1].Value;
                string command = GetRandomBot(randomFlag);
                if (command != "")
                {
                    IList<string> randomArgs = ParseArgs(command);
                    foreach (string param in randomArgs)
                    {
                        args.Add(param);
                    }
                }
            }
            WindBotInfo Info = new WindBotInfo();
            foreach (string param in args)
            {
                string[] p = param.Split(new char['=']);
                p[1] = p[1].Replace("'", "");
                if (p[0] == "Name") Info.Name = p[1];
                if (p[0] == "Deck") Info.Deck = p[1];
                if (p[0] == "Dialog") Info.Dialog = p[1];
                if (p[0] == "Port") Info.Port = int.Parse(p[1]);
                if (p[0] == "Hand") Info.Hand = int.Parse(p[1]);
                if (p[0] == "Host") Info.Host = p[1];
                if (p[0] == "HostInfo") Info.HostInfo = p[1];
            }
            Thread workThread = new Thread(new ParameterizedThreadStart(Run));
            workThread.Start(Info);
        }

        private static void Run(object o)
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

        private static IList<BotInfo> Bots = new List<BotInfo>();

        private static void ReadBots(string confPath)
        {
            StreamReader reader = new StreamReader(new FileStream(confPath, FileMode.Open, FileAccess.Read));
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine().Trim();
                if (line.Length > 0 && line[0] == '!')
                {
                    BotInfo newBot = new BotInfo();
                    newBot.name = line;
                    newBot.command = reader.ReadLine().Trim();
                    newBot.desc = reader.ReadLine().Trim();
                    line = reader.ReadLine().Trim();
                    newBot.flags = line.Split(new char[' ']);
                    Bots.Add(newBot);
                }
            }
        }

        private static string GetRandomBot(string flag)
        {
            IList<BotInfo> foundBots = Bots.Where(bot => bot.flags.Contains(flag)).ToList();
            if (foundBots.Count > 0)
            {
                Random rand = new Random();
                BotInfo bot = foundBots[rand.Next(foundBots.Count)];
                return bot.command;
            }
            return "";
        }
    }

    public class BotInfo
    {
        public string name;
        public string command;
        public string desc;
        public string[] flags;
    }

    public class Program
    {
        internal static Random Rand;
    }
}
