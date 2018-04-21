using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace RandomGroup
{
    class Program
    {
        // 初始化
        public static List<student> boy = new List<student>();// 學生清單
        public static List<student> girl = new List<student>();
        public static List<List<student>> team = new List<List<student>>();
        static string data;// 資料位置
        static Random random = new Random();

        static void Main(string[] args)
        {
            // 得到JSON檔並轉成List
            StreamReader rw = new StreamReader("D:\\VSProject\\Match\\Match\\Data.txt", Encoding.Default);
            data = rw.ReadToEnd();

            int teamCount = 0;

            boy = JsonConvert.DeserializeObject<List<student>>(data);
            boy.RemoveAll(s => s.gender != "男");
            girl = JsonConvert.DeserializeObject<List<student>>(data);
            girl.RemoveAll(s => s.gender != "女");

            double boyP;
            double girlP;

            int all = boy.Count + girl.Count;

            if (boy.Count > girl.Count)
            {
                boyP = (double)boy.Count / girl.Count;
                girlP = 1;
            }
            else
            {
                girlP = (double)girl.Count / boy.Count;
                boyP = 1;
            }
            
            double weights = 6 / (boyP + girlP);
            boyP *= weights;
            girlP *= weights;

            Console.WriteLine(boyP);
            Console.WriteLine(girlP);
            Console.ReadKey();

            int balance = (6 - (6 / 100 * 90));
            
            while(boy.Count>boyP)
            {
                team.Add(new List<student>());

                for(int i = 0; i < boyP; i++)
                {
                    int r = random.Next(0, boy.Count);
                    team[teamCount].Add(boy[r]);
                    boy.Remove(boy[r]);
                }

                for (int i = 0; i < girlP; i++)
                {
                    int r = random.Next(0, girl.Count);
                    team[teamCount].Add(girl[r]);
                    girl.Remove(girl[r]);
                }

                teamCount++;
            }
            

            int groupCount = 0;
            while (girl.Count != 0)
            {
                team.Add(new List<student>());

                if (girl.Count > 0)
                {
                    int r = random.Next(0, girl.Count);
                    team[teamCount].Add(girl[r]);
                    girl.Remove(girl[r]);
                }
                if (girl.Count > 0)
                {
                    int r = random.Next(0, girl.Count);
                    team[teamCount].Add(girl[r]);
                    girl.Remove(girl[r]);
                }
                if (girl.Count > 0)
                {
                    int r = random.Next(0, girl.Count);
                    team[teamCount].Add(girl[r]);
                    girl.Remove(girl[r]);
                }


                while (team[teamCount].Count < balance)
                {
                    if (boy.Count > 0)
                    {
                        int r = random.Next(0, boy.Count);
                        team[teamCount].Add(boy[r]);
                        boy.Remove(boy[r]);
                    }
                    else
                    {
                        team[teamCount].Add(team[groupCount][0]);
                        team[groupCount].Remove(team[groupCount][0]);
                        if (groupCount < teamCount)
                        {
                            groupCount++;
                        }
                        else
                        {
                            groupCount = 0;
                        }
                    }
                }
                teamCount++;
            }

            for (int i = 0; i < team.Count; i++)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("第" + i + "組");
                for (int j = 0; j < team[i].Count; j++)
                {
                    if(team[i][j].gender == "男")
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    Console.WriteLine(team[i][j].name);
                }
                Console.WriteLine("\r\n");
            }



            Console.WriteLine(boyP);
            Console.WriteLine(girlP);
            Console.ReadKey();
        }
    }

    // 學生的class
    public class student
    {
        public string name { get; set; }
        public string studentNumber { get; set; }
        public string gender { get; set; }
        public int height { get; set; }
        public string bloodtype { get; set; }
        public string zodiacSign { get; set; }
    }
}
