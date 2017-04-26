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
        private Range RangeExcel;
        public List<Player> InitializeList()
        {
            var ExcelApp = new Application();
            //Книга.
            WorkBookExcel = ExcelApp.Workbooks.Open(@"C:\Users\StasDon\Source\Repos\FantasyHSE\Fantasy\bin\Debug\Fantasy.xlsx");
            //Таблица.
            // WorkSheetExcel = ExcelApp.ActiveSheet as Microsoft.Office.Interop.Excel.Worksheet;
            //    RangeExcel = null;
            WorkSheetExcel = (Worksheet)WorkBookExcel.Sheets[1];

            var lastCell = WorkSheetExcel.Cells.SpecialCells(XlCellType.xlCellTypeLastCell);
            List<Player> players = new List<Player>();
            string[,] list = new string[lastCell.Row, lastCell.Column];

            for (int i = 1; i < lastCell.Column; i++)
            {
                string surname = WorkSheetExcel.Cells[i + 1, 1].Text.ToString();
                string club = WorkSheetExcel.Cells[i + 1, 3].Text.ToString();
                double price = Convert.ToDouble(WorkSheetExcel.Cells[i + 1, 4].Text.ToString());
                switch (WorkSheetExcel.Cells[i + 1, 2].Text.ToString())
                {
                    case "ВР": Goalkeeper gk = new Goalkeeper();
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
                string test = WorkSheetExcel.Cells[i + 1, 5].Text.ToString();
                st.Goals = Convert.ToInt32(WorkSheetExcel.Cells[i + 1, 5].Text.ToString());
                st.Assists = Convert.ToInt32(WorkSheetExcel.Cells[i + 1, 6].Text.ToString());
                st.PenaltyMiss = Convert.ToInt32(WorkSheetExcel.Cells[i + 1, 7].Text.ToString());
                st.YellowCard = Convert.ToInt32(WorkSheetExcel.Cells[i + 1, 8].Text.ToString());
                st.RedCard = Convert.ToInt32(WorkSheetExcel.Cells[i + 1, 9].Text.ToString());
                st.OwnGoal = Convert.ToInt32(WorkSheetExcel.Cells[i + 1, 10].Text.ToString());
                st.GoalsConc = Convert.ToInt32(WorkSheetExcel.Cells[i + 1, 11].Text.ToString());
                st.CleanSheet = Convert.ToInt32(WorkSheetExcel.Cells[i + 1, 12].Text.ToString());
                st.PenaltySave = Convert.ToInt32(WorkSheetExcel.Cells[i + 1, 13].Text.ToString());
                players[i - 1].Stat = st;
                SerializePlayer(players[i - 1]);
            }

            WorkBookExcel.Close(false, Type.Missing, Type.Missing); //закрыть не сохраняя
            ExcelApp.Quit(); // вышел из Excel
            GC.Collect(); // убрал за собой
            return players;
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
