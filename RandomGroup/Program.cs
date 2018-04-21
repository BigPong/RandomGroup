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
        public static List<student> boy = new List<student>();// 男學生清單
        public static List<student> girl = new List<student>();// 女學生清單
        public static List<List<student>> team = new List<List<student>>();
        static string data;// 資料位置
        static Random random = new Random();

        static void Main(string[] args)
        {
            // 得到JSON檔並轉成List
            StreamReader rw = new StreamReader("D:\\VSProject\\Match\\Match\\Data.txt", Encoding.Default);
            data = rw.ReadToEnd();

            // 參數宣告跟決定每組幾人
            int teamCount = 0;
            int MaxPeople;
            Console.WriteLine("請輸入每組要有多少人(根據人數狀況可能會有少許誤差值)");
            Int32.TryParse(Console.ReadLine(), out MaxPeople);
            Console.WriteLine("\r\n\r\n");

            // 踢掉男女
            boy = JsonConvert.DeserializeObject<List<student>>(data);
            boy.RemoveAll(s => s.gender != "男");
            girl = JsonConvert.DeserializeObject<List<student>>(data);
            girl.RemoveAll(s => s.gender != "女");

            double boyP;
            double girlP;

            int all = boy.Count + girl.Count;

            // 平均男女比
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

            // 計算權重
            double weights = MaxPeople / (boyP + girlP);
            boyP *= weights;
            girlP *= weights;

            // 補差額(小數部分的處理)
            double boyBonus = boyP % 1;
            double girlBonus = girlP % 1;
            double boyExtra = 0;
            double girlExtra = 0;
            boyP = (int)boyP;
            girlP = (int)girlP;

            // 人數下限
            int balance = (MaxPeople - (MaxPeople / 100 * 90));

            // 其中一方未歸零就繼續
            while (boy.Count >= boyP && girl.Count >= girlP)
            {
                team.Add(new List<student>());

                // 分組
                for (int i = 0; i < boyP; i++)
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

                boyExtra += boyBonus;
                girlExtra += girlBonus;

                // 補差額  但不要男女補到同一組
                if (boyExtra >= 1 && boy.Count > 0)
                {
                    int r = random.Next(0, boy.Count);
                    team[teamCount].Add(boy[r]);
                    boy.Remove(boy[r]);
                    boyExtra--;
                }
                else if (girlExtra >= 1 && girl.Count > 0)
                {
                    int r = random.Next(0, girl.Count);
                    team[teamCount].Add(girl[r]);
                    girl.Remove(girl[r]);
                    girlExtra--;
                }

                teamCount++;
            }

            // 多於人口處理
            while (boy.Count > 0)
            {
                int minBoy = 0;
                int minBoyTeam = 0;
                for (int i = 0; i < team.Count; i++)
                {
                    // 看哪一組男的比較少就分去那
                    int boyNum = 0;
                    foreach (student s in team[i])
                    {
                        if (s.gender == "男")
                        {
                            boyNum++;
                        }
                    }
                    if (minBoy >= boyNum || minBoy == 0)
                    {
                        minBoy = boyNum;
                        minBoyTeam = i;
                    }
                }
                int r = random.Next(0, boy.Count);
                team[minBoyTeam].Add(boy[r]);
                boy.Remove(boy[r]);
            }

            // 多於人口處理2號 概念同上
            while (girl.Count > 0)
            {
                int minGirl = 0;
                int minGirlTeam = 0;
                for (int i = 0; i < team.Count; i++)
                {
                    int girlNum = 0;
                    foreach (student s in team[i])
                    {
                        if (s.gender == "女")
                        {
                            girlNum++;
                        }
                    }
                    if (minGirl > girlNum || minGirl == 0)
                    {
                        minGirl = girlNum;
                        minGirlTeam = i;
                    }
                }
                int r = random.Next(0, girl.Count);
                team[minGirlTeam].Add(girl[r]);
                girl.Remove(girl[r]);
            }

            string ans ="";

            // 顯示分組結果
            for (int i = 0; i < team.Count; i++)
            {
                int count = 0;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("第" + (i + 1) + "組");
                ans += "第" + (i + 1) + "組\r\n";

                // 寫出人名
                for (int j = 0; j < team[i].Count; j++)
                {
                    // 男女分色方便觀看
                    if (team[i][j].gender == "男")
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    count++;
                    Console.WriteLine("   " + team[i][j].name);
                    ans += "   " + team[i][j].name + "\r\n";
                }

                // 統計人數
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("   " + count + "人");
                Console.WriteLine("\r\n");
                ans += "   " + count + "人\r\n\r\n";
            }

            // 從MSDN幹下來的神奇函數
            string mydocpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            Console.WriteLine("請輸入文件名稱  文件將保存在[使用者\\文件]中");
            string path = Console.ReadLine();

            // 寫出去
            using (StreamWriter outputFile = new StreamWriter(mydocpath + @"\" + path + ".txt"))
            {
                    outputFile.WriteLine(ans);
            }

            Console.WriteLine("保存成功  按任意鍵即可退出程式");
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
