using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using RiotApi.Net.RestClient.Configuration;
using RiotApi.Net.RestClient;
using RiotApi.Net.RestClient.Dto.League;
using RiotApi.Net.RestClient.Helpers;
using System.Diagnostics;
using HtmlAgilityPack;

namespace ConsoleApplication2
{
    class Program
    {
       static public RiotApi.Net.RestClient.Dto.Summoner.SummonerDto username;
        static public List<String> GameMembers = new List<String>();
        static public String BlueTeam;
        static public String RedTeam;
        static public String Red1;
        static public String Red2;
        static public int counter;
        static public String Red3;
        static public String Red4;
        static public int BlueTeamELO;
        static public int RedteamELO;
        static public IRiotClient riotClient = new RiotClient("RGAPI-CEE9BFC4-F5B2-45F6-BBBC-0510BA3E0A71");
        static public String Red5;
        static public String Blue1;
        static public Dictionary<string, string> ChampionsRed = new Dictionary<string, string>();
        static public Dictionary<string, string> ChampionsBlue = new Dictionary<string, string>();
        static public String Blue2;
        static public Boolean game = false;
        static public String Blue3;
        static public String Blue4;
        static public String Blue5;

        static public Dictionary<int, long> Fellow = new Dictionary<int, long>();

        static public Dictionary<string, string> Blue1Name = new Dictionary<string, string>();
        static public Dictionary<string, string> Blue2Name = new Dictionary<string, string>();
        static public Dictionary<string, string> Blue3Name = new Dictionary<string, string>();
        static public Dictionary<string, string> Blue4Name = new Dictionary<string, string>();
        static public Dictionary<string, string> Blue5Name = new Dictionary<string, string>();

        static public Dictionary<string, string> Red1Name = new Dictionary<string, string>();
        static public Dictionary<string, string> Red2Name = new Dictionary<string, string>();
        static public Dictionary<string, string> Red3Name = new Dictionary<string, string>();
        static public Dictionary<string, string> Red4Name = new Dictionary<string, string>();
        static public Dictionary<string, string> Red5Name = new Dictionary<string, string>();

        static public String Red1n;
        static public String Red2n;
        static public String Red3n;
        static public String Red4n;
        static public String Red5n;

        static public String Blue1n;
        static public String Blue2n;
        static public String Blue3n;
        static public String Blue4n;
        static public String Blue5n;

        static public long userteam;

        static void Main(string[] args)
    
        {
            Console.Title = "League Stats";
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            Console.WriteLine("Welcome to ldrpc's League Stats");
            Console.WriteLine("Settings Located at: " + config.FilePath);
            game = false;
            Thread.Sleep(500);
            if (config.AppSettings.Settings["Username"] == null)
            {
                Color("It appears like you haven't yet entered your League of Legends username!", ConsoleColor.Red);
                Thread.Sleep(500);
                String username = Ask("Enter League username: ");
                config.AppSettings.Settings.Add("Username", username);
                config.Save(ConfigurationSaveMode.Modified);
            }
            try
            {
                var testing = riotClient.Summoner.GetSummonersByName(RiotApiConfig.Regions.NA, config.AppSettings.Settings["Username"].Value.ToLower().Replace(" ", string.Empty));
                username = testing[config.AppSettings.Settings["Username"].Value.ToLower().Replace(" ", string.Empty)];

                Color("Successfully connected to user " + username.Name + " with summoner level " + username.SummonerLevel, ConsoleColor.Green);



            }
            catch
            {
                Color("Username " + config.AppSettings.Settings["Username"].Value + " seems to be incorrect", ConsoleColor.Red);
                String strl = Console.ReadLine();

            }

            Color("Loading Game Information...", ConsoleColor.White);
            StartGame();
              
          
           


            }
        public static void StartGame()
        {

            try
            {

                var current = riotClient.CurrentGame.GetCurrentGameInformationForSummonerId(RiotApiConfig.Platforms.NA1, username.Id);

                var ts = TimeSpan.FromSeconds(current.GameLength);

                if (game == false)
                    game = true;


        

                GameMembers.Clear();

                BlueTeam = "";
                RedTeam = "";
                Red1 = "";
                Red2 = "";
                Red3 = "";
                Red4 = "";
                Red5 = "";

                Red1n = "";
                Red2n = "";
                Red3n = "";
                Red4n = "";
                Red5n = "";

                Blue1n = "";
                Blue2n = "";
                Blue3n = "";
                Blue4n = "";
                Blue5n = "";

                Red1Name.Clear();
                Red2Name.Clear();
                Red3Name.Clear();
                Red4Name.Clear();
                Red5Name.Clear();

                Blue1Name.Clear();
                Blue2Name.Clear();
                Blue3Name.Clear();
                Blue4Name.Clear();
                Blue5Name.Clear();

                Blue1 = "";
                userteam = 0;
                Blue2 = "";
                Blue3 = "";
                Blue4 = "";
                Blue5 = "";
                BlueTeamELO = 0;
                RedteamELO = 0;
                ChampionsBlue.Clear();
                ChampionsRed.Clear();

                


                String strq = string.Format("{0}:{1}:{2}", ts.Hours, ts.Minutes, ts.Seconds);
                foreach (var item in current.Participants)
                {
                    if (item.SummonerId == username.Id)
                        userteam = item.TeamId;
                }

                foreach (var item in current.Participants)
                {
                    if (item.TeamId == 200)
                    {
                        string champ = riotClient.LolStaticData.GetChampionById(RiotApiConfig.Regions.NA, (int)item.ChampionId).Name;
                        ChampionsRed.Add(item.SummonerName, riotClient.LolStaticData.GetChampionById(RiotApiConfig.Regions.NA, (int)item.ChampionId).Name);
                        if (Red1 == "" && Red2 == "" && Red3 == "" && Red4 == "" && Red5 == "")
                        {
                            Red1n = item.SummonerName;
                            String rank = Rank(item.SummonerId, item.TeamId);
                            if (item.Spell1Id == 12 || item.Spell2Id == 12)
                            {
                                Red1 = item.SummonerName + " - Top - " + champ + " - " + rank;
                                Red1Name.Add(item.SummonerName, "Top");
                            }
                            else if (item.Spell1Id == 3 || item.Spell2Id == 3)
                            {
                                Red1 = item.SummonerName + " - Support - " + champ + " - " + rank;
                                Red1Name.Add(item.SummonerName, "Support");
                            }
                            else if (item.Spell1Id == 7 || item.Spell2Id == 7)
                            {
                                Red1 = item.SummonerName + " - ADC - " + champ + " - " + rank;
                                Red1Name.Add(item.SummonerName, "ADC");
                            }
                            else if (item.Spell1Id == 14 || item.Spell2Id == 14 || item.Spell1Id == 21 || item.Spell2Id == 21 || item.Spell1Id == 13 || item.Spell2Id == 13)
                            {
                                Red1 = item.SummonerName + " - Mid/Top - " + champ + " - " + rank;
                                Red1Name.Add(item.SummonerName, "Mid/Top");
                            }
                            else if (item.Spell1Id == 11 || item.Spell2Id == 11)
                            {
                                Red1 = item.SummonerName + " - Jungle - " + champ + " - " + rank;
                                Red1Name.Add(item.SummonerName, "Jungle");
                            }
                            else
                            {
                                Red1 = item.SummonerName + " - Unknown - " + champ + " - " + rank;
                                Red1Name.Add(item.SummonerName, "Unknown");
                            }

                        }
                        else if (Red1 != "" && Red2 == "" && Red3 == "" && Red4 == "" && Red5 == "")
                        {
                            Red2n = item.SummonerName;
                            String rank = Rank(item.SummonerId, item.TeamId);
                            if (item.Spell1Id == 12 || item.Spell2Id == 12)
                            {
                                Red2 = item.SummonerName + " - Top - " + champ + " - " + rank;
                                Red2Name.Add(item.SummonerName, "Top");
                            }
                            else if (item.Spell1Id == 3 || item.Spell2Id == 3)
                            {
                                Red2 = item.SummonerName + " - Support - " + champ + " - " + rank;
                                Red2Name.Add(item.SummonerName, "Support");
                            }
                            else if (item.Spell1Id == 7 || item.Spell2Id == 7)
                            {
                                Red2 = item.SummonerName + " - ADC - " + champ + " - " + rank;
                                Red2Name.Add(item.SummonerName, "ADC");
                            }
                            else if (item.Spell1Id == 14 || item.Spell2Id == 14 || item.Spell1Id == 21 || item.Spell2Id == 21 || item.Spell1Id == 13 || item.Spell2Id == 13)
                            {
                                Red2 = item.SummonerName + " - Mid/Top - " + champ + " - " + rank;
                                Red2Name.Add(item.SummonerName, "Mid/Top");
                            }
                            else if (item.Spell1Id == 11 || item.Spell2Id == 11)
                            {
                                Red2 = item.SummonerName + " - Jungle - " + champ + " - " + rank;
                                Red2Name.Add(item.SummonerName, "Jungle");
                            }
                            else
                            {
                                Red2 = item.SummonerName + " - Unknown - " + champ + " - " + rank;
                                Red2Name.Add(item.SummonerName, "Unknown");
                            }
                        }
                        else if (Red1 != "" && Red2 != "" && Red3 == "" && Red4 == "" && Red5 == "")
                        {
                            Red3n = item.SummonerName;
                            String rank = Rank(item.SummonerId, item.TeamId);
                            if (item.Spell1Id == 12 || item.Spell2Id == 12)
                            {
                                Red3 = item.SummonerName + " - Top - " + champ + " - " + rank;
                                Red3Name.Add(item.SummonerName, "Top");
                            }
                            else if (item.Spell1Id == 3 || item.Spell2Id == 3)
                            {
                                Red3 = item.SummonerName + " - Support - " + champ + " - " + rank;
                                Red3Name.Add(item.SummonerName, "Support");
                            }
                            else if (item.Spell1Id == 7 || item.Spell2Id == 7)
                            {
                                Red3 = item.SummonerName + " - ADC - " + champ + " - " + rank;
                                Red3Name.Add(item.SummonerName, "ADC");
                            }
                            else if (item.Spell1Id == 14 || item.Spell2Id == 14 || item.Spell1Id == 21 || item.Spell2Id == 21 || item.Spell1Id == 13 || item.Spell2Id == 13)
                            {
                                Red3 = item.SummonerName + " - Mid/Top - " + champ + " - " + rank;
                                Red3Name.Add(item.SummonerName, "Mid/Top");
                            }
                            else if (item.Spell1Id == 11 || item.Spell2Id == 11)
                            {
                                Red3 = item.SummonerName + " - Jungle - " + champ + " - " + rank;
                                Red3Name.Add(item.SummonerName, "Jungle");
                            }
                            else
                            {
                                Red3 = item.SummonerName + " - Unknown - " + champ + " - " + rank;
                                Red3Name.Add(item.SummonerName, "Unknown");
                            }
                        }
                        else if (Red1 != "" && Red2 != "" && Red3 != "" && Red4 == "" && Red5 == "")
                        {
                            Red4n = item.SummonerName;
                            String rank = Rank(item.SummonerId, item.TeamId);
                            if (item.Spell1Id == 12 || item.Spell2Id == 12)
                            {
                                Red4 = item.SummonerName + " - Top - " + champ + " - " + rank;
                                Red4Name.Add(item.SummonerName, "Top");
                            }
                            else if (item.Spell1Id == 3 || item.Spell2Id == 3)
                            {
                                Red4 = item.SummonerName + " - Support - " + champ + " - " + rank;
                                Red4Name.Add(item.SummonerName, "Support");
                            }
                            else if (item.Spell1Id == 7 || item.Spell2Id == 7)
                            {
                                Red4 = item.SummonerName + " - ADC - " + champ + " - " + rank;
                                Red4Name.Add(item.SummonerName, "ADC");
                            }
                            else if (item.Spell1Id == 14 || item.Spell2Id == 14 || item.Spell1Id == 21 || item.Spell2Id == 21 || item.Spell1Id == 13 || item.Spell2Id == 13)
                            {
                                Red4 = item.SummonerName + " - Mid/Top - " + champ + " - " + rank;
                                Red4Name.Add(item.SummonerName, "Mid/Top");
                            }
                            else if (item.Spell1Id == 11 || item.Spell2Id == 11)
                            {
                                Red4 = item.SummonerName + " - Jungle - " + champ + " - " + rank;
                                Red4Name.Add(item.SummonerName, "Jungle");
                            }
                            else
                            {
                                Red4 = item.SummonerName + " - Unknown - " + champ + " - " + rank;
                                Red4Name.Add(item.SummonerName, "Unknown");
                            }
                        }
                        else if (Red1 != "" && Red2 != "" && Red3 != "" && Red4 != "" && Red5 == "")
                        {
                            Red5n = item.SummonerName;
                            String rank = Rank(item.SummonerId, item.TeamId);
                            if (item.Spell1Id == 12 || item.Spell2Id == 12)
                            {
                                Red5 = item.SummonerName + " - Top - " + champ + " - " + rank;
                                Red5Name.Add(item.SummonerName, "Top");
                            }
                            else if (item.Spell1Id == 3 || item.Spell2Id == 3)
                            {
                                Red5 = item.SummonerName + " - Support - " + champ + " - " + rank;
                                Red5Name.Add(item.SummonerName, "Support");
                            }
                            else if (item.Spell1Id == 7 || item.Spell2Id == 7)
                            {
                                Red5 = item.SummonerName + " - ADC - " + champ + " - " + rank;
                                Red5Name.Add(item.SummonerName, "ADC");
                            }
                            else if (item.Spell1Id == 14 || item.Spell2Id == 14 || item.Spell1Id == 21 || item.Spell2Id == 21 || item.Spell1Id == 13 || item.Spell2Id == 13)
                            {
                                Red5 = item.SummonerName + " - Mid/Top - " + champ + " - " + rank;
                                Red5Name.Add(item.SummonerName, "Mid/Top");
                            }
                            else if (item.Spell1Id == 11 || item.Spell2Id == 11)
                            {
                                Red5 = item.SummonerName + " - Jungle - " + champ + " - " + rank;
                                Red5Name.Add(item.SummonerName, "Jungle");
                            }
                            else
                            {
                                Red5 = item.SummonerName + " - Unknown - " + champ + " - " + rank;
                                Red5Name.Add(item.SummonerName, "Unknown");
                            }
                        }

                    }

                    if (item.TeamId == 100)
                    {
                        string champ = riotClient.LolStaticData.GetChampionById(RiotApiConfig.Regions.NA, (int)item.ChampionId).Name;
                        ChampionsBlue.Add(item.SummonerName, riotClient.LolStaticData.GetChampionById(RiotApiConfig.Regions.NA, (int)item.ChampionId).Name);
                        if (Blue1 == "" && Blue2 == "" && Blue3 == "" && Blue4 == "" && Blue5 == "")
                        {
                            Blue1n = item.SummonerName;
                            String rank = Rank(item.SummonerId, item.TeamId);
                            if (item.Spell1Id == 12 || item.Spell2Id == 12)
                            {
                                Blue1 = item.SummonerName + " - Top - " + champ + " - " + rank;
                                Blue1Name.Add(item.SummonerName, "Top");
                            }
                            else if (item.Spell1Id == 3 || item.Spell2Id == 3)
                            {
                                Blue1 = item.SummonerName + " - Support - " + champ + " - " + rank;
                                Blue1Name.Add(item.SummonerName, "Support");
                            }
                            else if (item.Spell1Id == 7 || item.Spell2Id == 7)
                            {
                                Blue1 = item.SummonerName + " - ADC - " + champ + " - " + rank;
                                Blue1Name.Add(item.SummonerName, "ADC");
                            }
                            else if (item.Spell1Id == 14 || item.Spell2Id == 14 || item.Spell1Id == 21 || item.Spell2Id == 21 || item.Spell1Id == 13 || item.Spell2Id == 13)
                            {
                                Blue1 = item.SummonerName + " - Mid/Top - " + champ + " - " + rank;
                                Blue1Name.Add(item.SummonerName, "Mid/Top");
                            }
                            else if (item.Spell1Id == 11 || item.Spell2Id == 11)
                            {
                                Blue1 = item.SummonerName + " - Jungle - " + champ + " - " + rank;
                                Blue1Name.Add(item.SummonerName, "Jungle");
                            }
                            else
                            {
                                Blue1 = item.SummonerName + " - Unknown - " + champ + " - " + rank;
                                Blue1Name.Add(item.SummonerName, "Unknow");
                            }

                        }
                        else if (Blue1 != "" && Blue2 == "" && Blue3 == "" && Blue4 == "" && Blue5 == "")
                        {
                            Blue2n = item.SummonerName;
                            String rank = Rank(item.SummonerId, item.TeamId);
                            if (item.Spell1Id == 12 || item.Spell2Id == 12)
                            {
                                Blue2 = item.SummonerName + " - Top - " + champ + " - " + rank;
                                Blue2Name.Add(item.SummonerName, "Top");
                            }
                            else if (item.Spell1Id == 3 || item.Spell2Id == 3)
                            {
                                Blue2 = item.SummonerName + " - Support - " + champ + " - " + rank;
                                Blue2Name.Add(item.SummonerName, "Support");
                            }
                            else if (item.Spell1Id == 7 || item.Spell2Id == 7)
                            {
                                Blue2 = item.SummonerName + " - ADC - " + champ + " - " + rank;
                                Blue2Name.Add(item.SummonerName, "ADC");
                            }
                            else if (item.Spell1Id == 14 || item.Spell2Id == 14 || item.Spell1Id == 21 || item.Spell2Id == 21 || item.Spell1Id == 13 || item.Spell2Id == 13)
                            {
                                Blue2 = item.SummonerName + " - Mid/Top - " + champ + " - " + rank;
                                Blue2Name.Add(item.SummonerName, "Mid/Top");
                            }
                            else if (item.Spell1Id == 11 || item.Spell2Id == 11)
                            {
                                Blue2 = item.SummonerName + " - Jungle - " + champ + " - " + rank;
                                Blue2Name.Add(item.SummonerName, "Jungle");

                            }
                            else
                            {
                                Blue2 = item.SummonerName + " - Unknown - " + champ + " - " + rank;
                                Blue2Name.Add(item.SummonerName, "Unknown");
                            }
                        }
                        else if (Blue1 != "" && Blue2 != "" && Blue3 == "" && Blue4 == "" && Blue5 == "")
                        {
                            Blue3n = item.SummonerName;
                            String rank = Rank(item.SummonerId, item.TeamId);
                            if (item.Spell1Id == 12 || item.Spell2Id == 12)
                            {
                                Blue3 = item.SummonerName + " - Top - " + champ + " - " + rank;
                                Blue3Name.Add(item.SummonerName, "Top");
                            }
                            else if (item.Spell1Id == 3 || item.Spell2Id == 3)
                            {
                                Blue3 = item.SummonerName + " - Support - " + champ + " - " + rank;
                                Blue3Name.Add(item.SummonerName, "Support");
                            }
                            else if (item.Spell1Id == 7 || item.Spell2Id == 7)
                            {
                                Blue3 = item.SummonerName + " - ADC - " + champ + " - " + rank;
                                Blue3Name.Add(item.SummonerName, "ADC");
                            }
                            else if (item.Spell1Id == 14 || item.Spell2Id == 14 || item.Spell1Id == 21 || item.Spell2Id == 21 || item.Spell1Id == 13 || item.Spell2Id == 13)
                            {
                                Blue3 = item.SummonerName + " - Mid/Top - " + champ + " - " + rank;
                                Blue3Name.Add(item.SummonerName, "Mid/Top");
                            }
                            else if (item.Spell1Id == 11 || item.Spell2Id == 11)
                            {
                                Blue3 = item.SummonerName + " - Jungle - " + champ + " - " + rank;
                                Blue3Name.Add(item.SummonerName, "Jungle");
                            }
                            else
                            {
                                Blue3 = item.SummonerName + " - Unknown - " + champ + " - " + rank;
                                Blue3Name.Add(item.SummonerName, "Unknow");
                            }
                        }
                        else if (Blue1 != "" && Blue2 != "" && Blue3 != "" && Blue4 == "" && Blue5 == "")
                        {
                            Blue4n = item.SummonerName;
                            String rank = Rank(item.SummonerId, item.TeamId);
                            if (item.Spell1Id == 12 || item.Spell2Id == 12)
                            {
                                Blue4 = item.SummonerName + " - Top - " + champ + " - " + rank;
                                Blue4Name.Add(item.SummonerName, "Top");
                            }
                            else if (item.Spell1Id == 3 || item.Spell2Id == 3)
                            {
                                Blue4 = item.SummonerName + " - Support - " + champ + " - " + rank;
                                Blue4Name.Add(item.SummonerName, "Support");
                            }
                            else if (item.Spell1Id == 7 || item.Spell2Id == 7)
                            {
                                Blue4 = item.SummonerName + " - ADC - " + champ + " - " + rank;
                                Blue4Name.Add(item.SummonerName, "ADC");
                            }
                            else if (item.Spell1Id == 14 || item.Spell2Id == 14 || item.Spell1Id == 21 || item.Spell2Id == 21 || item.Spell1Id == 13 || item.Spell2Id == 13)
                            {
                                Blue4 = item.SummonerName + " - Mid/Top - " + champ + " - " + rank;
                                Blue4Name.Add(item.SummonerName, "Mid/Top");
                            }
                            else if (item.Spell1Id == 11 || item.Spell2Id == 11)
                            {
                                Blue4 = item.SummonerName + " - Jungle - " + champ + " - " + rank;
                                Blue4Name.Add(item.SummonerName, "Jungle");
                            }
                            else
                            {
                                Blue4 = item.SummonerName + " - Unknown - " + champ + " - " + rank;
                                Blue4Name.Add(item.SummonerName, "Unknow");
                            }
                        }
                        else if (Blue1 != "" && Blue2 != "" && Blue3 != "" && Blue4 != "" && Blue5 == "")
                        {
                            Blue5n = item.SummonerName;
                            String rank = Rank(item.SummonerId, item.TeamId);
                            if (item.Spell1Id == 12 || item.Spell2Id == 12)
                            {
                                Blue5 = item.SummonerName + " - Top - " + champ + " - " + rank;
                                Blue5Name.Add(item.SummonerName, "Top");
                            }
                            else if (item.Spell1Id == 3 || item.Spell2Id == 3)
                            {
                                Blue5 = item.SummonerName + " - Support - " + champ + " - " + rank;
                                Blue5Name.Add(item.SummonerName, "Support");
                            }
                            else if (item.Spell1Id == 7 || item.Spell2Id == 7)
                            {
                                Blue5 = item.SummonerName + " - ADC - " + champ + " - " + rank;
                                Blue5Name.Add(item.SummonerName, "ADC");
                            }
                            else if (item.Spell1Id == 14 || item.Spell2Id == 14 || item.Spell1Id == 21 || item.Spell2Id == 21 || item.Spell1Id == 13 || item.Spell2Id == 13)
                            {
                                Blue5 = item.SummonerName + " - Mid/Top - " + champ + " - " + rank;
                                Blue5Name.Add(item.SummonerName, "Mid/Top");
                            }
                            else if (item.Spell1Id == 11 || item.Spell2Id == 11)
                            {
                                Blue5 = item.SummonerName + " - Jungle - " + champ + " - " + rank;
                                Blue5Name.Add(item.SummonerName, "Jungle");
                            }
                            else
                            {
                                Blue5 = item.SummonerName + " - Unknown - " + champ + " - " + rank;
                                Blue5Name.Add(item.SummonerName, "Unknow");
                            }


                        }
                    }
                }

                Thread.Sleep(500);

                Color("----------------------------- [ Game Info ] -----------------------------", ConsoleColor.Magenta);
                Color("Game ID: " + current.GameId, ConsoleColor.Magenta);
                Color("Game Mode: " + current.GameMode, ConsoleColor.Magenta);
                Color("Game Type: " + current.GameType, ConsoleColor.Magenta);
                Color("Current Match Time: " + strq, ConsoleColor.Magenta);
                Color("         ", ConsoleColor.Red);
                Color("Match's Participants: ", ConsoleColor.Magenta);
                if (userteam == 100)
                    Color("Teams:   [BLUE] - Combined ELO: " + BlueTeamELO + " (Your Team!)", ConsoleColor.Cyan);
                if (userteam != 100)
                    Color("Teams:   [BLUE] - Combined ELO: " + BlueTeamELO, ConsoleColor.Cyan);
                Color("         " + Blue1, ConsoleColor.Cyan);
                Color("         " + Blue2, ConsoleColor.Cyan);
                Color("         " + Blue3, ConsoleColor.Cyan);
                Color("         " + Blue4, ConsoleColor.Cyan);
                Color("         " + Blue5, ConsoleColor.Cyan);
                Color("         ", ConsoleColor.Red);
                if (userteam == 200)
                    Color("         [RED] - Combined ELO: " + RedteamELO + " (Your Team!)", ConsoleColor.Red);
                if (userteam != 200)
                    Color("         [RED] - Combined ELO: " + RedteamELO, ConsoleColor.Red);
                Color("         " + Red1, ConsoleColor.Red);
                Color("         " + Red2, ConsoleColor.Red);
                Color("         " + Red3, ConsoleColor.Red);
                Color("         " + Red4, ConsoleColor.Red);
                Color("         " + Red5, ConsoleColor.Red);
                Color("         ", ConsoleColor.Red);
                int sum = BlueTeamELO + RedteamELO;
                decimal divRed = Decimal.Divide(RedteamELO, sum);
                decimal reddec = divRed * 100;
                decimal red = (int)Math.Round(reddec);
                decimal divBlue = Decimal.Divide(BlueTeamELO, sum);
                decimal bluedec = divBlue * 100;
                decimal blue = Math.Round(bluedec);
                Color("Chances of Winning:", ConsoleColor.Magenta);
                if (BlueTeamELO > RedteamELO)
                {
                    Color("Blue Team: " + blue.ToString() + "%" + " (Expected Winner!)", ConsoleColor.Cyan);
                    Color("Red Team:  " + red.ToString() + "%", ConsoleColor.Red);
                }
                else if (BlueTeamELO < RedteamELO)
                {
                    Color("Red Team:  " + red.ToString() + "%" + " (Expected Winner!)", ConsoleColor.Red);
                    Color("Blue Team: " + blue.ToString() + "%", ConsoleColor.Blue);
                }
                else if (BlueTeamELO == RedteamELO)
                {
                    Color("Red Team:  " + red.ToString() + "%" + " (Equal Chances!)", ConsoleColor.Red);
                    Color("Blue Team: " + blue.ToString() + "%" + " (Equal Chances!)", ConsoleColor.Cyan);
                }
                Color("-------------------------------------------------------------------------", ConsoleColor.Magenta);


                Color("         ", ConsoleColor.Red);
                Color("----------------------------- [ Counter Info for Enemy Team ] -----------------------------", ConsoleColor.Magenta);
                try
                {
                    if (userteam == 100)
                    {
                        String champion1 = ChampionsRed[Red1n];
                        string Url = "http://lolcounter.com/champions/" + champion1.ToLower().Replace("'",string.Empty);
                        if (champion1.ToLower() == "kled" || champion1.ToLower() == "jhin")
                        {
                            Color("Enemy player " + Red1n + " - Playing as " + champion1 + " - in " + Red1Name[Red1n] + " lane. Counter Info:", ConsoleColor.Magenta);
                            Color("Kled and Jhin are currently not registed in the database", ConsoleColor.Red);
                        }
                        else
                        {
                            HtmlWeb web = new HtmlWeb();
                            HtmlDocument doc = web.Load(Url);
                            string summary = doc.DocumentNode.SelectNodes("//*[@id=\"championPage\"]/div[1]/div[1]/div[3]/div[1]/div[1]/div[4]/span[1]/text()")[0].InnerText;
                            string summary2 = doc.DocumentNode.SelectNodes("//*[@id=\"championPage\"]/div[1]/div[1]/div[3]/div[1]/div[2]/div[4]/span[1]/text()")[0].InnerText;
                            string summary3 = doc.DocumentNode.SelectNodes("//*[@id=\"championPage\"]/div[1]/div[1]/div[3]/div[1]/div[3]/div[4]/span[1]/text()")[0].InnerText;
                            string summary4 = doc.DocumentNode.SelectNodes("//*[@id=\"championPage\"]/div[1]/div[1]/div[3]/div[1]/div[4]/div[4]/span[1]/text()")[0].InnerText;

                            Color("Enemy player " + Red1n + " - Playing as " + champion1 + " - in " + Red1Name[Red1n] + " lane. Counter Info:", ConsoleColor.Magenta);
                            Color("1. " + summary, ConsoleColor.Yellow);
                            Color("2. " + summary2, ConsoleColor.Yellow);
                            Color("3. " + summary3, ConsoleColor.Yellow);
                            Color("4. " + summary4, ConsoleColor.Yellow);
                        }


                        String champion2 = ChampionsRed[Red2n];
                        if (champion2.ToLower() == "kled" || champion2.ToLower() == "jhin")
                        {
                            Color("Enemy player " + Red2n + " - Playing as " + champion2 + " - in " + Red2Name[Red2n] + " lane. Counter Info:", ConsoleColor.Magenta);
                            Color("Kled and Jhin are currently not registed in the database", ConsoleColor.Red);
                        }
                        else
                        {
                            string Url1 = "http://lolcounter.com/champions/" + champion2.ToLower().Replace("'", string.Empty);
                            HtmlWeb web1 = new HtmlWeb();
                            HtmlDocument doc1 = web1.Load(Url1);
                            string summaryl = doc1.DocumentNode.SelectNodes("//*[@id=\"championPage\"]/div[1]/div[1]/div[3]/div[1]/div[1]/div[4]/span[1]/text()")[0].InnerText;
                            string summary2l = doc1.DocumentNode.SelectNodes("//*[@id=\"championPage\"]/div[1]/div[1]/div[3]/div[1]/div[2]/div[4]/span[1]/text()")[0].InnerText;
                            string summary3l = doc1.DocumentNode.SelectNodes("//*[@id=\"championPage\"]/div[1]/div[1]/div[3]/div[1]/div[3]/div[4]/span[1]/text()")[0].InnerText;
                            string summary4l = doc1.DocumentNode.SelectNodes("//*[@id=\"championPage\"]/div[1]/div[1]/div[3]/div[1]/div[4]/div[4]/span[1]/text()")[0].InnerText;

                            Color("Enemy player " + Red2n + " - Playing as " + champion2 + " - in " + Red2Name[Red2n] + " lane. Counter Info:", ConsoleColor.Magenta);
                            Color("1. " + summaryl, ConsoleColor.Yellow);
                            Color("2. " + summary2l, ConsoleColor.Yellow);
                            Color("3. " + summary3l, ConsoleColor.Yellow);
                            Color("4. " + summary4l, ConsoleColor.Yellow);
                        }


                        String champion3 = ChampionsRed[Red3n];
                        if (champion3.ToLower() == "kled" || champion3.ToLower() == "jhin")
                        {
                            Color("Enemy player " + Red3n + " - Playing as " + champion3 + " - in " + Red3Name[Red3n] + " lane. Counter Info:", ConsoleColor.Magenta);
                            Color("Kled and Jhin are currently not registed in the database", ConsoleColor.Red);
                        }
                        else
                        {
                            string Url2 = "http://lolcounter.com/champions/" + champion3.ToLower().Replace("'", string.Empty);
                            HtmlWeb web2 = new HtmlWeb();
                            HtmlDocument doc2 = web2.Load(Url2);
                            string summary41 = doc2.DocumentNode.SelectNodes("//*[@id=\"championPage\"]/div[1]/div[1]/div[3]/div[1]/div[1]/div[4]/span[1]/text()")[0].InnerText;
                            string summary42 = doc2.DocumentNode.SelectNodes("//*[@id=\"championPage\"]/div[1]/div[1]/div[3]/div[1]/div[2]/div[4]/span[1]/text()")[0].InnerText;
                            string summary43 = doc2.DocumentNode.SelectNodes("//*[@id=\"championPage\"]/div[1]/div[1]/div[3]/div[1]/div[3]/div[4]/span[1]/text()")[0].InnerText;
                            string summary44 = doc2.DocumentNode.SelectNodes("//*[@id=\"championPage\"]/div[1]/div[1]/div[3]/div[1]/div[4]/div[4]/span[1]/text()")[0].InnerText;

                            Color("Enemy player " + Red3n + " - Playing as " + champion3 + " - in " + Red3Name[Red3n] + " lane. Counter Info:", ConsoleColor.Magenta);
                            Color("1. " + summary41, ConsoleColor.Yellow);
                            Color("2. " + summary42, ConsoleColor.Yellow);
                            Color("3. " + summary43, ConsoleColor.Yellow);
                            Color("4. " + summary44, ConsoleColor.Yellow);
                        }


                        String champion4 = ChampionsRed[Red4n];
                        if (champion4.ToLower() == "kled" || champion4.ToLower() == "jhin")
                        {
                            Color("Enemy player " + Red4n + " - Playing as " + champion4 + " - in " + Red4Name[Red4n] + " lane. Counter Info:", ConsoleColor.Magenta);
                            Color("Kled and Jhin are currently not registed in the database", ConsoleColor.Red);
                        }
                        else
                        {
                            string Url3 = "http://lolcounter.com/champions/" + champion4.ToLower().Replace("'", string.Empty);
                            HtmlWeb web3 = new HtmlWeb();
                            HtmlDocument doc3 = web3.Load(Url3);
                            string summary51 = doc3.DocumentNode.SelectNodes("//*[@id=\"championPage\"]/div[1]/div[1]/div[3]/div[1]/div[1]/div[4]/span[1]/text()")[0].InnerText;
                            string summary52 = doc3.DocumentNode.SelectNodes("//*[@id=\"championPage\"]/div[1]/div[1]/div[3]/div[1]/div[2]/div[4]/span[1]/text()")[0].InnerText;
                            string summary53 = doc3.DocumentNode.SelectNodes("//*[@id=\"championPage\"]/div[1]/div[1]/div[3]/div[1]/div[3]/div[4]/span[1]/text()")[0].InnerText;
                            string summary54 = doc3.DocumentNode.SelectNodes("//*[@id=\"championPage\"]/div[1]/div[1]/div[3]/div[1]/div[4]/div[4]/span[1]/text()")[0].InnerText;

                            Color("Enemy player " + Red4n + " - Playing as " + champion4 + " - in " + Red4Name[Red4n] + " lane. Counter Info:", ConsoleColor.Magenta);
                            Color("1. " + summary51, ConsoleColor.Yellow);
                            Color("2. " + summary52, ConsoleColor.Yellow);
                            Color("3. " + summary53, ConsoleColor.Yellow);
                            Color("4. " + summary54, ConsoleColor.Yellow);
                        }


                        String champion5 = ChampionsRed[Red5n];
                        if (champion5.ToLower() == "kled" || champion5.ToLower() == "jhin")
                        {
                            Color("Enemy player " + Red5n + " - Playing as " + champion5 + " - in " + Red5Name[Red5n] + " lane. Counter Info:", ConsoleColor.Magenta);
                            Color("Kled and Jhin are currently not registed in the database", ConsoleColor.Red);
                        }
                        else
                        {
                            string Url4 = "http://lolcounter.com/champions/" + champion5.ToLower().Replace("'", string.Empty);
                            HtmlWeb web4 = new HtmlWeb();
                            HtmlDocument doc4 = web4.Load(Url4);
                            string summary61 = doc4.DocumentNode.SelectNodes("//*[@id=\"championPage\"]/div[1]/div[1]/div[3]/div[1]/div[1]/div[4]/span[1]/text()")[0].InnerText;
                            string summary62 = doc4.DocumentNode.SelectNodes("//*[@id=\"championPage\"]/div[1]/div[1]/div[3]/div[1]/div[2]/div[4]/span[1]/text()")[0].InnerText;
                            string summary63 = doc4.DocumentNode.SelectNodes("//*[@id=\"championPage\"]/div[1]/div[1]/div[3]/div[1]/div[3]/div[4]/span[1]/text()")[0].InnerText;
                            string summary64 = doc4.DocumentNode.SelectNodes("//*[@id=\"championPage\"]/div[1]/div[1]/div[3]/div[1]/div[4]/div[4]/span[1]/text()")[0].InnerText;

                            Color("Enemy player " + Red5n + " - Playing as " + champion5 + " - in " + Red5Name[Red5n] + " lane. Counter Info:", ConsoleColor.Magenta);
                            Color("1. " + summary61, ConsoleColor.Yellow);
                            Color("2. " + summary62, ConsoleColor.Yellow);
                            Color("3. " + summary63, ConsoleColor.Yellow);
                            Color("4. " + summary64, ConsoleColor.Yellow);
                        }
                        Color("------------------------------------------------------------------------------", ConsoleColor.Magenta);
                        Color("Refreshing game status in 120 seconds", ConsoleColor.White);
                        Thread.Sleep(120000);
                        StartGame();
                    }
                    else if (userteam == 200)
                    {
                        String champion1 = ChampionsBlue[Blue1n];
                        if (champion1.ToLower() == "kled" || champion1.ToLower() == "jhin")
                        {
                            Color("Enemy player " + Blue1n + " - Playing as " + champion1 + " - in " + Blue1Name[Blue1n] + " lane. Counter Info:", ConsoleColor.Magenta);
                            Color("Kled and Jhin are currently not registed in the database", ConsoleColor.Red);
                        }
                        else
                        {
                            string Url = "http://lolcounter.com/champions/" + champion1.ToLower().Replace("'", string.Empty);
                            HtmlWeb web = new HtmlWeb();
                            HtmlDocument doc = web.Load(Url);
                            string summary = doc.DocumentNode.SelectNodes("//*[@id=\"championPage\"]/div[1]/div[1]/div[3]/div[1]/div[1]/div[4]/span[1]/text()")[0].InnerText;
                            string summary2 = doc.DocumentNode.SelectNodes("//*[@id=\"championPage\"]/div[1]/div[1]/div[3]/div[1]/div[2]/div[4]/span[1]/text()")[0].InnerText;
                            string summary3 = doc.DocumentNode.SelectNodes("//*[@id=\"championPage\"]/div[1]/div[1]/div[3]/div[1]/div[3]/div[4]/span[1]/text()")[0].InnerText;
                            string summary4 = doc.DocumentNode.SelectNodes("//*[@id=\"championPage\"]/div[1]/div[1]/div[3]/div[1]/div[4]/div[4]/span[1]/text()")[0].InnerText;

                            Color("Enemy player " + Blue1n + " - Playing as " + champion1 + " - in " + Blue1Name[Blue1n] + " lane. Counter Info:", ConsoleColor.Magenta);
                            Color("1. " + summary, ConsoleColor.Yellow);
                            Color("2. " + summary2, ConsoleColor.Yellow);
                            Color("3. " + summary3, ConsoleColor.Yellow);
                            Color("4. " + summary4, ConsoleColor.Yellow);
                        }


                        String champion2 = ChampionsBlue[Blue2n];
                        if (champion2.ToLower() == "kled" || champion2.ToLower() == "jhin")
                        {
                            Color("Enemy player " + Blue2n + " - Playing as " + champion2 + " - in " + Blue2Name[Blue2n] + " lane. Counter Info:", ConsoleColor.Magenta);
                            Color("Kled and Jhin are currently not registed in the database", ConsoleColor.Red);
                        }
                        else
                        {
                            string Url1 = "http://lolcounter.com/champions/" + champion2.ToLower().Replace("'", string.Empty);
                            HtmlWeb web1 = new HtmlWeb();
                            HtmlDocument doc1 = web1.Load(Url1);
                            string summaryl = doc1.DocumentNode.SelectNodes("//*[@id=\"championPage\"]/div[1]/div[1]/div[3]/div[1]/div[1]/div[4]/span[1]/text()")[0].InnerText;
                            string summary2l = doc1.DocumentNode.SelectNodes("//*[@id=\"championPage\"]/div[1]/div[1]/div[3]/div[1]/div[2]/div[4]/span[1]/text()")[0].InnerText;
                            string summary3l = doc1.DocumentNode.SelectNodes("//*[@id=\"championPage\"]/div[1]/div[1]/div[3]/div[1]/div[3]/div[4]/span[1]/text()")[0].InnerText;
                            string summary4l = doc1.DocumentNode.SelectNodes("//*[@id=\"championPage\"]/div[1]/div[1]/div[3]/div[1]/div[4]/div[4]/span[1]/text()")[0].InnerText;

                            Color("Enemy player " + Blue2n + " - Playing as " + champion2 + " - in " + Blue2Name[Blue2n] + " lane. Counter Info:", ConsoleColor.Magenta);
                            Color("1. " + summaryl, ConsoleColor.Yellow);
                            Color("2. " + summary2l, ConsoleColor.Yellow);
                            Color("3. " + summary3l, ConsoleColor.Yellow);
                            Color("4. " + summary4l, ConsoleColor.Yellow);
                        }


                        String champion3 = ChampionsBlue[Blue3n];
                        if (champion3.ToLower() == "kled" || champion3.ToLower() == "jhin")
                        {
                            Color("Enemy player " + Blue3n + " - Playing as " + champion3 + " - in " + Blue3Name[Blue3n] + " lane. Counter Info:", ConsoleColor.Magenta);
                            Color("Kled and Jhin are currently not registed in the database", ConsoleColor.Red);
                        }
                        else
                        {
                            string Url2 = "http://lolcounter.com/champions/" + champion3.ToLower().Replace("'", string.Empty);
                            HtmlWeb web2 = new HtmlWeb();
                            HtmlDocument doc2 = web2.Load(Url2);
                            string summary41 = doc2.DocumentNode.SelectNodes("//*[@id=\"championPage\"]/div[1]/div[1]/div[3]/div[1]/div[1]/div[4]/span[1]/text()")[0].InnerText;
                            string summary42 = doc2.DocumentNode.SelectNodes("//*[@id=\"championPage\"]/div[1]/div[1]/div[3]/div[1]/div[2]/div[4]/span[1]/text()")[0].InnerText;
                            string summary43 = doc2.DocumentNode.SelectNodes("//*[@id=\"championPage\"]/div[1]/div[1]/div[3]/div[1]/div[3]/div[4]/span[1]/text()")[0].InnerText;
                            string summary44 = doc2.DocumentNode.SelectNodes("//*[@id=\"championPage\"]/div[1]/div[1]/div[3]/div[1]/div[4]/div[4]/span[1]/text()")[0].InnerText;

                            Color("Enemy player " + Blue3n + " - Playing as " + champion3 + " - in " + Blue3Name[Blue3n] + " lane. Counter Info:", ConsoleColor.Magenta);
                            Color("1. " + summary41, ConsoleColor.Yellow);
                            Color("2. " + summary42, ConsoleColor.Yellow);
                            Color("3. " + summary43, ConsoleColor.Yellow);
                            Color("4. " + summary44, ConsoleColor.Yellow);
                        }


                        String champion4 = ChampionsBlue[Blue4n];
                        if (champion4.ToLower() == "kled" || champion4.ToLower() == "jhin")
                        {
                            Color("Enemy player " + Blue4n + " - Playing as " + champion4 + " - in " + Blue4Name[Blue4n] + " lane. Counter Info:", ConsoleColor.Magenta);
                            Color("Kled and Jhin are currently not registed in the database", ConsoleColor.Red);
                        }
                        else
                        {
                            string Url3 = "http://lolcounter.com/champions/" + champion4.ToLower().Replace("'", string.Empty);
                            HtmlWeb web3 = new HtmlWeb();
                            HtmlDocument doc3 = web3.Load(Url3);
                            string summary51 = doc3.DocumentNode.SelectNodes("//*[@id=\"championPage\"]/div[1]/div[1]/div[3]/div[1]/div[1]/div[4]/span[1]/text()")[0].InnerText;
                            string summary52 = doc3.DocumentNode.SelectNodes("//*[@id=\"championPage\"]/div[1]/div[1]/div[3]/div[1]/div[2]/div[4]/span[1]/text()")[0].InnerText;
                            string summary53 = doc3.DocumentNode.SelectNodes("//*[@id=\"championPage\"]/div[1]/div[1]/div[3]/div[1]/div[3]/div[4]/span[1]/text()")[0].InnerText;
                            string summary54 = doc3.DocumentNode.SelectNodes("//*[@id=\"championPage\"]/div[1]/div[1]/div[3]/div[1]/div[4]/div[4]/span[1]/text()")[0].InnerText;

                            Color("Enemy player " + Blue4n + " - Playing as " + champion4 + " - in " + Blue4Name[Blue4n] + " lane. Counter Info:", ConsoleColor.Magenta);
                            Color("1. " + summary51, ConsoleColor.Yellow);
                            Color("2. " + summary52, ConsoleColor.Yellow);
                            Color("3. " + summary53, ConsoleColor.Yellow);
                            Color("4. " + summary54, ConsoleColor.Yellow);
                        }


                        String champion5 = ChampionsBlue[Blue5n];
                        if (champion5.ToLower() == "kled" || champion5.ToLower() == "jhin")
                        {
                            Color("Enemy player " + Blue5n + " - Playing as " + champion5 + " - in " + Blue5Name[Blue5n] + " lane. Counter Info:", ConsoleColor.Magenta);
                            Color("Kled and Jhin are currently not registed in the database", ConsoleColor.Red);
                        }
                        else
                        {
                            string Url4 = "http://lolcounter.com/champions/" + champion5.ToLower().Replace("'", string.Empty);
                            HtmlWeb web4 = new HtmlWeb();
                            HtmlDocument doc4 = web4.Load(Url4);
                            string summary61 = doc4.DocumentNode.SelectNodes("//*[@id=\"championPage\"]/div[1]/div[1]/div[3]/div[1]/div[1]/div[4]/span[1]/text()")[0].InnerText;
                            string summary62 = doc4.DocumentNode.SelectNodes("//*[@id=\"championPage\"]/div[1]/div[1]/div[3]/div[1]/div[2]/div[4]/span[1]/text()")[0].InnerText;
                            string summary63 = doc4.DocumentNode.SelectNodes("//*[@id=\"championPage\"]/div[1]/div[1]/div[3]/div[1]/div[3]/div[4]/span[1]/text()")[0].InnerText;
                            string summary64 = doc4.DocumentNode.SelectNodes("//*[@id=\"championPage\"]/div[1]/div[1]/div[3]/div[1]/div[4]/div[4]/span[1]/text()")[0].InnerText;

                            Color("Enemy player " + Blue5n + " - Playing as " + champion5 + " - in " + Blue5Name[Blue5n] + " lane. Counter Info:", ConsoleColor.Magenta);
                            Color("1. " + summary61, ConsoleColor.Yellow);
                            Color("2. " + summary62, ConsoleColor.Yellow);
                            Color("3. " + summary63, ConsoleColor.Yellow);
                            Color("4. " + summary64, ConsoleColor.Yellow);
                        }
                        Color("-------------------------------------------------------------------------", ConsoleColor.Magenta);
                        Color("Refreshing game status in 120 seconds", ConsoleColor.White);
                        Thread.Sleep(120000);
                        StartGame();
                    }
                }
                catch
                {
                    Color("An error has occured", ConsoleColor.Red);
                    String str = Console.ReadLine();
                }
            }
            catch
            {
                if (game == false)
                {
                    Color("Player " + username.Name + " is currently not in a match.", ConsoleColor.Red);
                    Color("Checking again in 15 seconds!", ConsoleColor.White);
                    Thread.Sleep(15000);
                    StartGame();
                } else if (game == true)
                {
                    Color("The current game has finished.", ConsoleColor.White);
                    Thread.Sleep(200);
                    Color("Processing game stats...", ConsoleColor.White);
                    game = false;
                    Thread.Sleep(60000);
                    var games = riotClient.Game.GetRecentGamesBySummonerId(RiotApiConfig.Regions.NA, username.Id).Games.ToList();

                    int i = 1;
                    foreach (var e in games[0].FellowPlayers.ToList())
                    {
                        if (e.TeamId == games[0].TeamId && e.SummonerId != username.Id)
                        {
                            Fellow.Add(i, e.SummonerId);
                            i++;
                        }
                    }
                    var FellowGames1 = riotClient.Game.GetRecentGamesBySummonerId(RiotApiConfig.Regions.NA, Fellow[1]).Games.ToList()[0];
                    var FellowGames2 = riotClient.Game.GetRecentGamesBySummonerId(RiotApiConfig.Regions.NA, Fellow[2]).Games.ToList()[0];
                    var FellowGames3 = riotClient.Game.GetRecentGamesBySummonerId(RiotApiConfig.Regions.NA, Fellow[3]).Games.ToList()[0];
                    var FellowGames4 = riotClient.Game.GetRecentGamesBySummonerId(RiotApiConfig.Regions.NA, Fellow[4]).Games.ToList()[0];
                    Thread.Sleep(200);
                    var user1 = riotClient.Summoner.GetSummonersById(RiotApiConfig.Regions.NA, Fellow[1].ToString())[Fellow[1].ToString()].Name;
                    var user2 = riotClient.Summoner.GetSummonersById(RiotApiConfig.Regions.NA, Fellow[2].ToString())[Fellow[2].ToString()].Name;
                    var user3 = riotClient.Summoner.GetSummonersById(RiotApiConfig.Regions.NA, Fellow[3].ToString())[Fellow[3].ToString()].Name;
                    var user4 = riotClient.Summoner.GetSummonersById(RiotApiConfig.Regions.NA, Fellow[4].ToString())[Fellow[4].ToString()].Name;
                    Thread.Sleep(200);
                    string ten = "▓▓▓▓▓▓▓▓▓▓";



                    Color("----------------------------- [ End of Game Information ] -----------------------------", ConsoleColor.Magenta);
                    Color(" ", ConsoleColor.Magenta);
                    Color("                              -    Match Information    -", ConsoleColor.Magenta);
                    Color(" ", ConsoleColor.Magenta);
                    if (games[0].Stats.Win == false)
                        Color("• Game Status: Lost", ConsoleColor.Magenta);
                    if (games[0].Stats.Win == true)
                        Color("• Game Status: Won", ConsoleColor.Magenta);
                    Color("• IP Gained: " + games[0].IpEarned, ConsoleColor.Magenta);
                    Color("• Level: " + games[0].Stats.Level, ConsoleColor.Magenta);
                    Color(" ", ConsoleColor.Magenta);
                    Color("                              -    Your Stats    -", ConsoleColor.Magenta);
                    Color(" ", ConsoleColor.Magenta);
                    Color("• KDA: " + Math.Round(Decimal.Divide(games[0].Stats.ChampionsKilled, games[0].Stats.NumDeaths), 2), ConsoleColor.Magenta);
                    Color("• Number of Inhibitors Destroyed: " + games[0].Stats.BarracksKilled, ConsoleColor.Magenta);
                    Color("• Number of Towers Destroyed: " + games[0].Stats.TurretsKilled, ConsoleColor.Magenta);
                    Color("• Number of Minions Killed: " + games[0].Stats.MinionsKilled, ConsoleColor.Magenta);
                    Color("• Number of Neutral Minions Killed: " + games[0].Stats.NeutralMinionsKilled, ConsoleColor.Magenta);
                    if (games[0].Stats.NeutralMinionsKilled == 0)
                    {
                        Color("• Friendly Jungle: 0", ConsoleColor.Magenta);
                        Color("• Enemy Jungle: 0", ConsoleColor.Magenta);
                    }
                    else if (games[0].Stats.NeutralMinionsKilled != 0)
                    {
                        Color("• Friendly Jungle: " + games[0].Stats.NeutralMinionsKilledYourJungle + " - " + Decimal.Divide(games[0].Stats.NeutralMinionsKilledYourJungle, games[0].Stats.NeutralMinionsKilled) * 100 + "% of Total Neutral Minions Killed", ConsoleColor.Magenta);
                        Color("• Enemy Jungle: " + games[0].Stats.NeutralMinionsKilledEnemyJungle + " - " + Decimal.Divide(games[0].Stats.NeutralMinionsKilledEnemyJungle, games[0].Stats.NeutralMinionsKilledYourJungle + games[0].Stats.NeutralMinionsKilledEnemyJungle) * 100 + "% of Total Neutral Minions Killed", ConsoleColor.Magenta);
                    }
                    Color("• Amount of Killing Sprees: " + games[0].Stats.KillingSprees, ConsoleColor.Magenta);
                    Color("• Amount of Double Kills: " + games[0].Stats.DoubleKills, ConsoleColor.Magenta);
                    Color("• Amount of Triple Kills: " + games[0].Stats.TripleKills, ConsoleColor.Magenta);
                    Color("• Amount of Quadra Kills: " + games[0].Stats.QuadraKills, ConsoleColor.Magenta);
                    Color("• Amount of Penta Kills: " + games[0].Stats.PentaKills, ConsoleColor.Magenta); Color(" ", ConsoleColor.Magenta);
                    if (games[0].Stats.WardPlaced < 14)
                        Color("• Total Wards Placed: " + games[0].Stats.WardPlaced + " (Low amount detected, try to place more next time!)", ConsoleColor.Magenta);
                    if (games[0].Stats.WardPlaced > 13)
                        Color("• Total Wards Placed: " + games[0].Stats.WardPlaced + " (Good amount of wards placed!)", ConsoleColor.Magenta);
                    Color("• Total Wards Destroyed: " + games[0].Stats.WardKilled, ConsoleColor.Magenta);
                    Color("• Total Wards Bought: " + games[0].Stats.VisionWardsBought + games[0].Stats.SightWardsBought, ConsoleColor.Magenta);
                    Color("• Total Gold Earned: " + games[0].Stats.GoldEarned, ConsoleColor.Magenta);
                    Color("• Total Gold Spent: " + games[0].Stats.GoldSpent, ConsoleColor.Magenta);
                    Color("• Magic Damage Taken: " + games[0].Stats.MagicDamageTaken, ConsoleColor.Magenta);
                    Color("• Physical Damage Taken: " + games[0].Stats.PhysicalDamageTaken, ConsoleColor.Magenta);
                    Color("• True Damage Taken: " + games[0].Stats.TrueDamageTaken, ConsoleColor.Magenta);
                    Color("• Total Damage Taken: " + games[0].Stats.TotalDamageTaken, ConsoleColor.Magenta);
                    Color(" ", ConsoleColor.Magenta);
                    Color("                              -    Damaga Stats    -", ConsoleColor.Magenta);
                    Color(" ", ConsoleColor.Magenta);
                    Color("You (" + username.Name + "):", ConsoleColor.DarkMagenta);
                    Color(" ", ConsoleColor.Magenta);
                    Color("• Magic Damage Dealt: " + games[0].Stats.MagicDamageDealtToChampions + " " + per(games[0].Stats.MagicDamageDealtToChampions, games[0].Stats.TotalDamageDealtToChampions) + "% " + magic(games[0].Stats.MagicDamageDealtToChampions, games[0].Stats.TotalDamageDealtToChampions), ConsoleColor.Magenta);
                    Color("• Physical Damage Dealt: " + games[0].Stats.PhysicalDamageDealtToChampions + " " + per(games[0].Stats.PhysicalDamageDealtToChampions, games[0].Stats.TotalDamageDealtToChampions) + "% " + magic(games[0].Stats.PhysicalDamageDealtToChampions, games[0].Stats.TotalDamageDealtToChampions), ConsoleColor.Magenta);
                    Color("• True Damage Dealt: " + games[0].Stats.TrueDamageDealtToChampions + " " + per(games[0].Stats.TrueDamageDealtToChampions, games[0].Stats.TotalDamageDealtToChampions) + "% " + magic(games[0].Stats.TrueDamageDealtToChampions, games[0].Stats.TotalDamageDealtToChampions), ConsoleColor.Magenta);
                    Color("• Total Damage Dealt: " + games[0].Stats.TotalDamageDealtToChampions + " " + "100" + "% " + ten, ConsoleColor.Magenta);
                    Color(" ", ConsoleColor.Magenta);
                    Color(user1 + ":", ConsoleColor.DarkMagenta);
                    Color(" ", ConsoleColor.Magenta);
                    Color("• Magic Damage Dealt: " + FellowGames1.Stats.MagicDamageDealtToChampions + " " + per(FellowGames1.Stats.MagicDamageDealtToChampions, FellowGames1.Stats.TotalDamageDealtToChampions) + "% " + magic(FellowGames1.Stats.MagicDamageDealtToChampions, FellowGames1.Stats.TotalDamageDealtToChampions), ConsoleColor.Magenta);
                    Color("• Physical Damage Dealt: " + FellowGames1.Stats.PhysicalDamageDealtToChampions + " " + per(FellowGames1.Stats.PhysicalDamageDealtToChampions, FellowGames1.Stats.TotalDamageDealtToChampions) + "% " + magic(FellowGames1.Stats.PhysicalDamageDealtToChampions, FellowGames1.Stats.TotalDamageDealtToChampions), ConsoleColor.Magenta);
                    Color("• True Damage Dealt: " + FellowGames1.Stats.TrueDamageDealtToChampions + " " + per(FellowGames1.Stats.TrueDamageDealtToChampions, FellowGames1.Stats.TotalDamageDealtToChampions) + "% " + magic(FellowGames1.Stats.TrueDamageDealtToChampions, FellowGames1.Stats.TotalDamageDealtToChampions), ConsoleColor.Magenta);
                    Color("• Total Damage Dealt: " + FellowGames1.Stats.TotalDamageDealtToChampions + " " + "100" + "% " + ten, ConsoleColor.Magenta);
                    Color(" ", ConsoleColor.Magenta);
                    Color(user2 + ":", ConsoleColor.DarkMagenta);
                    Color(" ", ConsoleColor.Magenta);
                    Color("• Magic Damage Dealt: " + FellowGames2.Stats.MagicDamageDealtToChampions + " " + per(FellowGames2.Stats.MagicDamageDealtToChampions, FellowGames2.Stats.TotalDamageDealtToChampions) + "% " + magic(FellowGames2.Stats.MagicDamageDealtToChampions, FellowGames2.Stats.TotalDamageDealtToChampions), ConsoleColor.Magenta);
                    Color("• Physical Damage Dealt: " + FellowGames2.Stats.PhysicalDamageDealtToChampions + " " + per(FellowGames2.Stats.PhysicalDamageDealtToChampions, FellowGames2.Stats.TotalDamageDealtToChampions) + "% " + magic(FellowGames2.Stats.PhysicalDamageDealtToChampions, FellowGames2.Stats.TotalDamageDealtToChampions), ConsoleColor.Magenta);
                    Color("• True Damage Dealt: " + FellowGames2.Stats.TrueDamageDealtToChampions + " " + per(FellowGames2.Stats.TrueDamageDealtToChampions, FellowGames2.Stats.TotalDamageDealtToChampions) + "% " + magic(FellowGames2.Stats.TrueDamageDealtToChampions, FellowGames2.Stats.TotalDamageDealtToChampions), ConsoleColor.Magenta);
                    Color("• Total Damage Dealt: " + FellowGames2.Stats.TotalDamageDealtToChampions + " " + "100" + "% " + ten, ConsoleColor.Magenta);
                    Color(" ", ConsoleColor.Magenta);
                    Color(user3 + ":", ConsoleColor.DarkMagenta);
                    Color(" ", ConsoleColor.Magenta);
                    Color("• Magic Damage Dealt: " + FellowGames3.Stats.MagicDamageDealtToChampions + " " + per(FellowGames3.Stats.MagicDamageDealtToChampions, FellowGames3.Stats.TotalDamageDealtToChampions) + "% " + magic(FellowGames3.Stats.MagicDamageDealtToChampions, FellowGames3.Stats.TotalDamageDealtToChampions), ConsoleColor.Magenta);
                    Color("• Physical Damage Dealt: " + FellowGames3.Stats.PhysicalDamageDealtToChampions + " " + per(FellowGames3.Stats.PhysicalDamageDealtToChampions, FellowGames3.Stats.TotalDamageDealtToChampions) + "% " + magic(FellowGames3.Stats.PhysicalDamageDealtToChampions, FellowGames3.Stats.TotalDamageDealtToChampions), ConsoleColor.Magenta);
                    Color("• True Damage Dealt: " + FellowGames3.Stats.TrueDamageDealtToChampions + " " + per(FellowGames3.Stats.TrueDamageDealtToChampions, FellowGames3.Stats.TotalDamageDealtToChampions) + "% " + magic(FellowGames3.Stats.TrueDamageDealtToChampions, FellowGames3.Stats.TotalDamageDealtToChampions), ConsoleColor.Magenta);
                    Color("• Total Damage Dealt: " + FellowGames3.Stats.TotalDamageDealtToChampions + " " + "100" + "% " + ten, ConsoleColor.Magenta);
                    Color(" ", ConsoleColor.Magenta);
                    Color(user4 + ":", ConsoleColor.DarkMagenta);
                    Color(" ", ConsoleColor.Magenta);
                    Color("• Magic Damage Dealt: " + FellowGames4.Stats.MagicDamageDealtToChampions + " " + per(FellowGames4.Stats.MagicDamageDealtToChampions, FellowGames4.Stats.TotalDamageDealtToChampions) + "% " + magic(FellowGames4.Stats.MagicDamageDealtToChampions, FellowGames4.Stats.TotalDamageDealtToChampions), ConsoleColor.Magenta);
                    Color("• Physical Damage Dealt: " + FellowGames3.Stats.PhysicalDamageDealtToChampions + " " + per(FellowGames4.Stats.PhysicalDamageDealtToChampions, FellowGames4.Stats.TotalDamageDealtToChampions) + "% " + magic(FellowGames4.Stats.PhysicalDamageDealtToChampions, FellowGames4.Stats.TotalDamageDealtToChampions), ConsoleColor.Magenta);
                    Color("• True Damage Dealt: " + FellowGames4.Stats.TrueDamageDealtToChampions + " " + per(FellowGames4.Stats.TrueDamageDealtToChampions, FellowGames4.Stats.TotalDamageDealtToChampions) + "% " + magic(FellowGames4.Stats.TrueDamageDealtToChampions, FellowGames4.Stats.TotalDamageDealtToChampions), ConsoleColor.Magenta);
                    Color("• Total Damage Dealt: " + FellowGames4.Stats.TotalDamageDealtToChampions + " " + "100" + "% " + ten, ConsoleColor.Magenta);
                    Color("---------------------------------------------------------------------------------------", ConsoleColor.Magenta);
                    Color("Checking for in-game status in 120 seconds...", ConsoleColor.White);
                    Thread.Sleep(120000);
                    StartGame();
                }
            }

        }
            
        static public String Rank(long id, long Team)
        {
            try { 
            Dictionary<string, IEnumerable<LeagueDto>> league_response = riotClient.League.GetSummonerLeagueEntriesByIds(RiotApiConfig.Regions.NA, id);

            // While there is only one entry in the response we still need to extract it to use it.
            // This is done with summoner id. Because the response from riot api is coming back as JSON we need to
            // convert our summoner id to a string.
            IEnumerable<LeagueDto> my_league = league_response[id.ToString()];

            // This is converting the object to a list for easy access, since we know there is only one entry.
            List<LeagueDto> league = my_league.ToList();
            LeagueDto ranked = league[0];

            // This is converting to a list again since entries has ranked as its only entry in it.
            List<LeagueDto.LeagueEntryDto> entries = ranked.Entries.ToList();
            LeagueDto.LeagueEntryDto my_entry = entries[0];

            // Now we can finally extract the information your after.
            Enums.Tier tier = ranked.Tier;
            string division = my_entry.Division;

            int playerElo = CalculateElo(tier, division);
            
                if(Team == 200)
                {
                    RedteamELO = RedteamELO + playerElo;
                } else if (Team == 100)
                {
                    BlueTeamELO = BlueTeamELO + playerElo;
                }
            
            return tier + " (" + division + ")";
            }
            catch
            {
                if (Team == 200)
                {
                    RedteamELO = RedteamELO + 4;
                }
                else if (Team == 100)
                {
                    BlueTeamELO = BlueTeamELO + 4;
                }
                return "No Rank";

            }
        }
        static public int CalculateElo(Enums.Tier tier, String division)
        {
            if (tier == Enums.Tier.BRONZE)
            {
                if (division == "I")
                {
                    return 5;
                }
                else if (division == "II")
                {
                    return 4;
                }
                else if (division == "III")
                {
                    return 3;
                }
                else if (division == "IV")
                {
                    return 2;
                }
                else if (division == "V")
                {
                    return 1;
                }
            }
            if (tier == Enums.Tier.SILVER)
            {
                if (division == "I")
                {
                    return 12;
                }
                else if (division == "II")
                {
                    return 11;
                }
                else if (division == "III")
                {
                    return 10;
                }
                else if (division == "IV")
                {
                    return 9;
                }
                else if (division == "V")
                {
                    return 8;
                }
                }
            if (tier == Enums.Tier.GOLD)
            {
                if (division == "I")
                {
                    return 19;
                }
                else if (division == "II")
                {
                    return 18;
                }
                else if (division == "III")
                {
                    return 17;
                }
                else if (division == "IV")
                {
                    return 16;
                }
                else if (division == "V")
                {
                    return 15;
                }
            }
            if (tier == Enums.Tier.PLATINUM)
            {
                if (division == "I")
                {
                    return 26;
                }
                else if (division == "II")
                {
                    return 25;
                }
                else if (division == "III")
                {
                    return 24;
                }
                else if (division == "IV")
                {
                    return 23;
                }
                else if (division == "V")
                {
                    return 22;
                }
            }
            if (tier == Enums.Tier.DIAMOND)
            {
                if (division == "I")
                {
                    return 33;
                }
                else if (division == "II")
                {
                    return 32;
                }
                else if (division == "III")
                {
                    return 31;
                }
                else if (division == "IV")
                {
                    return 30;
                }
                else if (division == "V")
                {
                    return 29;
                }

            }
            if (tier == Enums.Tier.MASTER)
            {
                if (division == "I")
                {
                    return 40;
                }
                else if (division == "II")
                {
                    return 39;
                }
                else if (division == "III")
                {
                    return 38;
                }
                else if (division == "IV")
                {
                    return 37;
                }
                else if (division == "V")
                {
                    return 36;
                }
            }
            if (tier == Enums.Tier.CHALLENGER)
            {
                if (division == "I")
                {
                    return 47;
                }
                else if (division == "II")
                {
                    return 46;
                }
                else if (division == "III")
                {
                    return 45;
                }
                else if (division == "IV")
                {
                    return 44;
                }
                else if (division == "V")
                {
                    return 43;
                }
            }
            return 0;
        }
       static public void Turn()
        {
            counter++;
            switch (counter % 4)
            {
                case 0: Console.Write("/"); counter = 0; break;
                case 1: Console.Write("-"); break;
                case 2: Console.Write("\\"); break;
                case 3: Console.Write("|"); break;
            }
            Thread.Sleep(100);
            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
        }
        static public String magic(int Magic, int Total)
        {
            string zero = "░░░░░░░░░░";

            string one = "▓░░░░░░░░░";

            string two = "▓▓░░░░░░░░";

            string three = "▓▓▓░░░░░░░";

            string four = "▓▓▓▓░░░░░░";

            string five = "▓▓▓▓▓░░░░░";

            string six = "▓▓▓▓▓▓░░░░";

            string seven = "▓▓▓▓▓▓▓▓░░░";

            string eigth = "▓▓▓▓▓▓▓▓░░";

            string nine = "▓▓▓▓▓▓▓▓▓░";

            string ten = "▓▓▓▓▓▓▓▓▓▓";

            String magicda = "";
            decimal magicd = Decimal.Divide(Magic, Total);
            decimal h = magicd * 100;
            decimal g = (int)Math.Round(h);

            if (0 <= g && g <= 10)
                magicda = zero;
            if (10 < g && g <= 20)
                magicda = one;
            if (20 < g && g <= 30)
                magicda = two;
            if (30 < g && g <= 40)
                magicda = three;
            if (40 < g && g <= 50)
                magicda = four;
            if (50 < g && g <= 60)
                magicda = five;
            if (60 < g && g <= 70)
                magicda = six;
            if (70 < g && g <= 80)
                magicda = seven;
            if (80 < g && g <= 90)
                magicda = eigth;
            if (90 < g && g <= 100)
                magicda = nine;
            if (g == 100)
                magicda = ten;

            return magicda;
        }
        static public decimal per(int Magic, int Total)
        {
            string zero = "░░░░░░░░░░";

            string one = "▓░░░░░░░░░";

            string two = "▓▓░░░░░░░░";

            string three = "▓▓▓░░░░░░░";

            string four = "▓▓▓▓░░░░░░";

            string five = "▓▓▓▓▓░░░░░";

            string six = "▓▓▓▓▓▓░░░░";

            string seven = "▓▓▓▓▓▓▓▓░░░";

            string eigth = "▓▓▓▓▓▓▓▓░░";

            string nine = "▓▓▓▓▓▓▓▓▓░";

            string ten = "▓▓▓▓▓▓▓▓▓▓";

            String magicda = "";
            decimal magicd = Decimal.Divide(Magic, Total);
            decimal h = magicd * 100;
            decimal g = (int)Math.Round(h);
          
            if (0 <= g && g <= 10)
                magicda = zero;
            if (10 < g && g <= 20)
                magicda = one;
            if (20 < g && g <= 30)
                magicda = two;
            if (30 < g && g <= 40)
                magicda = three;
            if (40 < g && g <= 50)
                magicda = four;
            if (50 < g && g <= 60)
                magicda = five;
            if (60 < g && g <= 70)
                magicda = six;
            if (70 < g && g <= 80)
                magicda = seven;
            if (80 < g && g <= 90)
                magicda = eigth;
            if (90 < g && g <= 100)
                magicda = nine;
            if (g == 100)
                magicda = ten;


            return g;
        }
        static public void Color(String text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = ConsoleColor.White;
        }
        static public String Ask(String k)
        {

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(k);
            String str = Console.ReadLine();
            return str;
        }
    }

}
internal class ConsoleSpinner
{
    private int _currentAnimationFrame;

    public ConsoleSpinner()
    {
        SpinnerAnimationFrames = new[]
                                 {
                                         '|',
                                         '/',
                                         '-',
                                         '\\'
                                     };
    }

    public char[] SpinnerAnimationFrames { get; set; }

    public void UpdateProgress()
    {

        Console.CursorVisible = false;
        // Store the current position of the cursor
        var originalX = Console.CursorLeft;
        var originalY = Console.CursorTop;

        // Write the next frame (character) in the spinner animation
        Console.Write(SpinnerAnimationFrames[_currentAnimationFrame]);

        // Keep looping around all the animation frames
        _currentAnimationFrame++;
        if (_currentAnimationFrame == SpinnerAnimationFrames.Length)
        {
            _currentAnimationFrame = 0;
        }

        // Restore cursor to original position
        Console.SetCursorPosition(originalX, originalY);
    }
}
