using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.Office.Interop.Excel;
using System.IO;

namespace FantasyLib
{
    public class ExcelParser
    { 
        private Workbook WorkBookExcel;
        private Worksheet WorkSheetExcel;

        /// <summary>
        /// Возвращает список игроков, считанный из excel-файла
        /// </summary>
        /// <returns></returns>
        public List<Player> InitializeList()
        {
            var ExcelApp = new Application();
            string path = System.IO.Path.GetFullPath(@"..\..\..\Resources\Fantasy.xlsx");
            //Книга.
            WorkBookExcel = ExcelApp.Workbooks.Open(path); 
            //Таблица.
            WorkSheetExcel = (Worksheet)WorkBookExcel.Sheets[1];

            var lastCell = WorkSheetExcel.Cells.SpecialCells(XlCellType.xlCellTypeLastCell);
            List<Player> players = new List<Player>();

            for (int i = 1; i < lastCell.Row; i++)
            {
                string surname = WorkSheetExcel.Cells[i + 1, 1].Text.ToString();
                string club = WorkSheetExcel.Cells[i + 1, 3].Text.ToString();
                double price = Convert.ToDouble(WorkSheetExcel.Cells[i + 1, 4].Text.ToString());

                switch (WorkSheetExcel.Cells[i + 1, 2].Text.ToString())
                {
                    case "ВР": 
                        Goalkeeper gk = new Goalkeeper();
                        gk.Surname = surname;
                        gk.Price = price;
                        players.Add(gk);
                        break;
                    case "ЗЩ": Defender def = new Defender();
                        def.Surname = surname;
                        def.Price = price;
                        players.Add(def);
                        break;
                    case "ПЗ": MidFielder mf = new MidFielder();
                        mf.Surname = surname;
                        mf.Price = price;
                        players.Add(mf);
                        break;
                    case "НП": Forward fr = new Forward();
                        fr.Surname = surname;
                        fr.Price = price;
                        players.Add(fr);
                        break;
                    default: throw new Exception("Unknown error");
                }

                Statistics st = new Statistics();
                st.Goals = GetIntCellValue(i + 1, 5);
                st.Assists = GetIntCellValue(i + 1, 6);
                st.PenaltyMiss = GetIntCellValue(i + 1, 7);
                st.YellowCard = GetIntCellValue(i + 1, 8);
                st.RedCard = GetIntCellValue(i + 1, 9);
                st.OwnGoal = GetIntCellValue(i + 1, 10);
                st.GoalsConc = GetIntCellValue(i + 1, 11);
                st.CleanSheet = GetIntCellValue(i + 1, 12);
                st.PenaltySave = GetIntCellValue(i + 1, 13);
                st.TeamOfWeek = GetIntCellValue(i + 1, 14);
                players[i - 1].Stat = st;
            }

            WorkBookExcel.Close(false, Type.Missing, Type.Missing); //закрыть не сохраняя
            ExcelApp.Quit(); // вышел из Excel
            GC.Collect(); // убрал за собой
            return players;
        }

        private int GetIntCellValue(int i, int j)
        {
            try
            {
                return Convert.ToInt32(WorkSheetExcel.Cells[i, j].Text.ToString());
            }
            catch (Exception)
            {
                return 0;
            }
        }

        private void SerializePlayer(Player player)
        {
            BinaryFormatter bin_formatter = new BinaryFormatter();
            using(FileStream fs = new FileStream("players.txt", FileMode.Create))
            {
                bin_formatter.Serialize(fs, player);
            }
        }
    }
}
