using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FantasyLib
{
    [Serializable]
    public class Team
    {
        public Goalkeeper Keeper { get; set; }
        public Defender[] Defense { get; set; }
        public MidFielder[] MidField { get; set; }
        public Forward[] Attack { get; set; }
        public Player[] Substitutions { get; set; }
        public Player Capitain { get; set; }
        /// <summary>
        /// Метод для вычисления очков команды
        /// </summary>
        /// <returns></returns>
        public int GetScore()
        {
            int score = 0;
            if (Capitain.Surname == Keeper.Surname) score += 2 * Keeper.Score;
            else score += Keeper.Score;
            foreach (Defender def in Defense)
            {
                if (Capitain.Surname == def.Surname) score += 2 * def.Score;
                else score += def.Score;
            }
            foreach (MidFielder mid in MidField)
            {
                if (Capitain.Surname == mid.Surname) score += 2 * mid.Score;
                else score += mid.Score;
            }
            foreach (Forward fwd in Attack)
            {
                if (Capitain.Surname == fwd.Surname) score += 2 * fwd.Score;
                else score += fwd.Score;
            }
            return score;
        }
        /// <summary>
        /// Метод для вычисления стоимости команды
        /// </summary>
        public double Price
        {
            get
            {
                double price = 0;
                price += Keeper.Price;
                foreach (Defender def in Defense)
                {
                    price += def.Price;
                }
                foreach (MidFielder mid in MidField)
                {
                    price += mid.Price;
                }
                foreach (Forward fwd in Attack)
                {
                    price += fwd.Price;
                }
                return price;
            }
        }
        /// <summary>
        /// Метод, создающий массив основных игроков
        /// </summary>
        /// <returns>Массив игроков основы</returns>
        public Player[] GetTeamArr()
        {
            Player[] res = new Player[9];
            res[0] = Keeper;
            Array.Copy(Defense, 0, res, 1, Defense.Length);
            Array.Copy(MidField, 0, res, Defense.Length + 1, MidField.Length);
            Array.Copy(Attack, 0, res, MidField.Length + Defense.Length + 1, Attack.Length);
            //Array.Copy(Substitutions, 0, res, MidField.Length + Defense.Length + Attack.Length + 1, Substitutions.Length);
            return res;
        }
        /// <summary>
        /// Создает 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static Team CreateTeamFromFile(string fileName, List<Player> list)
        {
            string temp;
            char[] separators = { ' ', ',', '.', ':' };

            using (StreamReader sr = new StreamReader(new FileStream(fileName, FileMode.Open)))
            {
                temp = sr.ReadLine();
                string[] arr = temp.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                Goalkeeper keeper = (Goalkeeper)list.Find(pl => pl.Surname == arr[1]);

                temp = sr.ReadLine();
                arr = temp.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                string[] arrTemp = new string[arr.Length - 1];
                Array.Copy(arr, 1, arrTemp, 0, arrTemp.Length);
                arr = arrTemp;
                Defender[] def = new Defender[arr.Length];
                for (int i = 0; i < def.Length; i++)
                {
                    def[i] = (Defender)list.Find(pl => pl.Surname == arr[i]);
                }

                temp = sr.ReadLine();
                arr = temp.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                arrTemp = new string[arr.Length - 1];
                Array.Copy(arr, 1, arrTemp, 0, arrTemp.Length);
                arr = arrTemp;
                MidFielder[] mid = new MidFielder[arr.Length];
                for (int i = 0; i < mid.Length; i++)
                {
                    mid[i] = (MidFielder)list.Find(pl => pl.Surname == arr[i]);
                }

                temp = sr.ReadLine();
                arr = temp.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                arrTemp = new string[arr.Length - 1];
                Array.Copy(arr, 1, arrTemp, 0, arrTemp.Length);
                arr = arrTemp;
                Forward[] fwd = new Forward[arr.Length];
                for (int i = 0; i < fwd.Length; i++)
                {
                    fwd[i] = (Forward)list.Find(pl => pl.Surname == arr[i]);
                }

                temp = sr.ReadLine();
                arr = temp.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                Player capitain = list.Find(pl => pl.Surname == arr[1]);

                temp = sr.ReadLine();
                arr = temp.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                arrTemp = new string[arr.Length - 1];
                Array.Copy(arr, 1, arrTemp, 0, arrTemp.Length);
                arr = arrTemp;
                Player[] sub = new Player[arr.Length];
                for (int i = 0; i < sub.Length; i++)
                {
                    sub[i] = list.Find(pl => pl.Surname == arr[i]);
                }

                Team team = new Team()
                {
                    Keeper = keeper,
                    Defense = def,
                    MidField = mid,
                    Attack = fwd,
                    Capitain = capitain,
                    Substitutions = sub
                };
                return team;
            }
        }
    }
}
