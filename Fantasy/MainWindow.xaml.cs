using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FantasyLib;
using Microsoft.Win32;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Fantasy
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Проверка состава на пустые строки
        /// </summary>
        static bool NotReady;
        /// <summary>
        /// Проверка на наличие капитана в команде
        /// </summary>
        static bool CapExists;
        /// <summary>
        /// Проверка на существование игроков в списке
        /// </summary>
        static bool NotPlayersExist;
        /// <summary>
        /// Список игроков
        /// </summary>
        static List<Player> list;
        /// <summary>
        /// Метод для проверки условий перед рассчетом очков
        /// </summary>
        public void Flag()
        {
            string[] players = new string[9];
            int i = 0;
            if (KeeperTextBox.Text == string.Empty) NotReady = true;
            players[i++] = KeeperTextBox.Text;
            foreach (TextBox item in DefensePanel.Children)
            {
                if (item.Text == string.Empty) NotReady = true;
                players[i++] = item.Text;
            }
            foreach (TextBox item in MidFieldPanel.Children)
            {
                if (item.Text == string.Empty) NotReady = true;
                players[i++] = item.Text;
            }
            foreach (TextBox item in AttackPanel.Children)
            {
                if (item.Text == string.Empty) NotReady = true;
                players[i++] = item.Text;
            }
            //foreach (string item in players)
            //{
            //    if (players.Count(t => item == t) > 1) NotReady = true;
            //}
            CapExists = players.Contains(CapitainTextBox.Text);
            foreach (string item in players)
            {
                if (list.Find(pl => pl.Surname == item) == null) NotPlayersExist = true;
            }
        }
        /// <summary>
        /// Метод для создания объекта команды по текстовым полям
        /// </summary>
        /// <returns>team</returns>
        public Team CreateTeam()
        {
            Team team = new Team();
            Goalkeeper keeper = (Goalkeeper)list.Find(pl => pl.Surname == KeeperTextBox.Text);
            Defender[] def = new Defender[DefensePanel.Children.Count];
            MidFielder[] mid = new MidFielder[MidFieldPanel.Children.Count];
            Forward[] attack = new Forward[AttackPanel.Children.Count];
            Player[] sub = new Player[4];
            
            int i = 0;
            foreach (TextBox item in DefensePanel.Children)
            {
                def[i++] = (Defender)list.Find(pl => pl.Surname == item.Text);
            }
            i = 0;
            foreach (TextBox item in MidFieldPanel.Children)
            {
                mid[i++] = (MidFielder)list.Find(pl => pl.Surname == item.Text);
            }
            i = 0;
            foreach (TextBox item in AttackPanel.Children)
            {
                attack[i++] = (Forward)list.Find(pl => pl.Surname == item.Text);
            }
            
            sub[0] = list.Find(pl => pl.Surname == Sub1.Text);
            sub[1] = list.Find(pl => pl.Surname == Sub2.Text);
            sub[2] = list.Find(pl => pl.Surname == Sub3.Text);
            sub[3] = list.Find(pl => pl.Surname == Sub4.Text);

            team.Keeper = keeper;
            team.Defense = def;
            team.MidField = mid;
            team.Attack = attack;
            team.Substitutions = sub;
            team.Capitain = list.Find(pl => pl.Surname == CapitainTextBox.Text);
            return team;

        }

        //public static List<Player> Init()
        //{
        //    ExcelParser ex = new ExcelParser();
        //    return ex.InitializeList();
        //}
        public static async void Loading(ProgressBar pb, Button sub, Button subFromFile)
        {
            list = await InitAsync();
            pb.Visibility = Visibility.Hidden;
            pb.IsEnabled = false;
            sub.IsEnabled = true;
            subFromFile.IsEnabled = true;
        }
        public static Task<List<Player>> InitAsync()
        {         
            return Task.Run(() =>
            {
                ExcelParser ex = new ExcelParser();
                return ex.InitializeList();
            });
        }
        public MainWindow()
        {
            InitializeComponent();    
        }
        /// <summary>
        /// Обработчик события смены схемы команды
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SchemeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selectedItem = (sender as ComboBox).SelectedItem as ComboBoxItem;
            string scheme = selectedItem.Content.ToString();
            DefensePanel.Children.Clear();
            MidFieldPanel.Children.Clear();
            AttackPanel.Children.Clear();
            TextBox[] defense = new TextBox[Convert.ToInt32(scheme.Substring(0, 1))];
            TextBox[] mid = new TextBox[Convert.ToInt32(scheme.Substring(2, 1))];
            TextBox[] attack = new TextBox[Convert.ToInt32(scheme.Substring(4, 1))];
            for (int i = 0; i < defense.Length; i++)
            {
                defense[i] = new TextBox()
                {
                    Width = 125,
                    FontSize = 20,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(5),
                    ToolTip = "Введите фамилию защитника"
                };
                DefensePanel.Children.Add(defense[i]);
            }
            for (int i = 0; i < mid.Length; i++)
            {
                mid[i] = new TextBox()
                {
                    Width = 125,
                    FontSize = 20,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(5),
                    ToolTip = "Введите фамилию полузащитника"
                };
                MidFieldPanel.Children.Add(mid[i]);
            }
            for (int i = 0; i < attack.Length; i++)
            {
                attack[i] = new TextBox()
                {
                    Width = 125,
                    FontSize = 20,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(5),
                    ToolTip = "Введите фамилию нападающего"
                };
                AttackPanel.Children.Add(attack[i]);
            }
            KeeperTextBox.Text = string.Empty;
            CapitainTextBox.Text = string.Empty;
            Sub1.Text = string.Empty;
            Sub2.Text = string.Empty;
            Sub3.Text = string.Empty;
            Sub4.Text = string.Empty;
        }
        /// <summary>
        /// Обрабротчик нажатия кнопки Submit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            Flag();
            if (!NotReady && CapExists && !NotPlayersExist)
            {
                if (MessageBox.Show("Вы уверены?", "Внимание!", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    try
                    {
                        Team team = CreateTeam();
                        string str = string.Empty;
                        foreach (Player player in team.GetTeamArr())
                        {
                            if (team.Capitain == player)
                            {
                                str += player + " - капитан (x2)\n";
                            }
                            else
                            {
                                str += player + "\n";
                            }
                        }
                        str += "Запасные: ";
                        foreach (Player player in team.Substitutions)
                        {
                            str += player + ", ";
                        }
                        str = str.Remove(str.Length - 2);
                        str += "\n";
                        str += "Стоимость команды: " + team.Price + " млн.\n";
                        str += "Общее количество очков: " + team.GetScore();
                        MessageBox.Show(str, "Результаты команды");
                    }
                    catch
                    {
                        MessageBox.Show("Не все игроки на своих позициях!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else if (NotPlayersExist)
            {
                MessageBox.Show("В команде есть несуществующие игроки!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                NotReady = false;
                CapExists = false;
                NotPlayersExist = false;
            }
            else if (!CapExists)
            {
                MessageBox.Show("Команде нужен капитан, который проходит в состав!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                NotReady = false;
                CapExists = false;
                NotPlayersExist = false;
            }
            else if (NotReady)
            {
                MessageBox.Show("Некорректный состав!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                NotReady = false;
                CapExists = false;
                NotPlayersExist = false;
            }
        }
        /// <summary>
        /// Инициализация текстовых полей из текстового файла
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddFromFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog()
            {
                Filter = "Текстовый документ|*.txt"
            };
            var result = openDialog.ShowDialog();
            if (result == false)
            {
                return;
            }
            string fileName = System.IO.Path.GetFullPath(openDialog.FileName);
            try
            {
                Team team = Team.CreateTeamFromFile(fileName, list);

                string scheme = "s" + team.Defense.Length + "_" + team.MidField.Length + "_" + team.Attack.Length;
                SchemeComboBox.SelectedItem = SchemeComboBox.FindName(scheme) as ComboBoxItem;
                KeeperTextBox.Text = team.Keeper.Surname;
                for (int i = 0; i < team.Defense.Length; i++)
                {
                    (DefensePanel.Children[i] as TextBox).Text = team.Defense[i].Surname;
                }
                for (int i = 0; i < team.MidField.Length; i++)
                {
                    (MidFieldPanel.Children[i] as TextBox).Text = team.MidField[i].Surname;
                }
                for (int i = 0; i < team.Attack.Length; i++)
                {
                    (AttackPanel.Children[i] as TextBox).Text = team.Attack[i].Surname;
                }
                CapitainTextBox.Text = team.Capitain.Surname;
                for (int i = 1; i < team.Substitutions.Length + 1; i++)
                {
                    (SubPanel.Children[i] as TextBox).Text = team.Substitutions[i - 1].Surname;
                }
            }
            catch
            {
                MessageBox.Show("Некорректный состав!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Loading(ProgBar, SubmitButton, AddFromFileButton);
        }
    }
}
