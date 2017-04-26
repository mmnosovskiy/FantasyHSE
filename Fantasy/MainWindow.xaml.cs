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
using System.IO;

namespace Fantasy
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static bool NotReady;
        static bool CapExists;
        static List<Player> list;
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
            CapExists = players.Contains(CapitainTextBox.Text);
        }
        public Team CreateTeam()
        {
            Team team = new Team();
            Goalkeeper keeper = (Goalkeeper)list.Find(pl => pl.Surname == KeeperTextBox.Text);
            Defender[] def = new Defender[DefensePanel.Children.Count];
            MidFielder[] mid = new MidFielder[MidFieldPanel.Children.Count];
            Forward[] attack = new Forward[AttackPanel.Children.Count];
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

            team.Keeper = keeper;
            team.Defense = def;
            team.MidField = mid;
            team.Attack = attack;
            team.Capitain = list.Find(pl => pl.Surname == CapitainTextBox.Text);
            return team;

        }
        public MainWindow()
        {
            InitializeComponent();
            list = new List<Player>
            {
                new Goalkeeper() { Surname = "g", Stat = new Statistics() { Goals = 1 } },
                new Defender() { Surname = "d", Stat = new Statistics() { Assists = 2 } },
                new MidFielder() { Surname = "m", Stat = new Statistics() { Goals = 3 } },
                new Forward() { Surname = "f", Stat = new Statistics() { CleanSheet = 1 } }
            };
        }

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
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            Flag();
            if (!NotReady && CapExists)
            {
                if (MessageBox.Show("Вы уверены?", "Внимание!", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    Team team = CreateTeam();
                    string str = string.Empty;
                    foreach (Player player in team.GetTeamArr())
                    {
                        str += player.ToString() + "\n";
                    }
                    str += "Общее количество очков: " + team.GetScore();
                    MessageBox.Show(str);
                }
            }
            else if (!CapExists)
            {
                MessageBox.Show("Команде нужен капитан, который проходит в состав!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                NotReady = false;
            }
            else if (NotReady)
            {
                MessageBox.Show("Состав не собран!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                NotReady = false;
            }
        }

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
        }
    }
}
