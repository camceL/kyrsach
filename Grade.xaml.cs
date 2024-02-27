using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using System.Windows.Shapes;

namespace StutentsKyrs
{
    /// <summary>
    /// Логика взаимодействия для Grade.xaml
    /// </summary>
    public partial class Grade : Window
    {
        Entities entities;
        Users current_user;

        int min_disc;
        int max_disc;

        int min_group;
        int max_group;

        string min_value;
        string max_value;

        DateTime start_date;
        DateTime end_date;

        string[] values = new string[] { "3", "4", "5", "По болезни", "Военкомат", "По заявлению", "По распоряжению" };

        public Grade(Users user , Entities ent )
        {
            InitializeComponent();
            current_user = user;
            entities = ent;
            comboGroup.Items.Add("Все");
            comboGroup.SelectedIndex = 0;

            comboDiscipline.Items.Add("Все");
            comboDiscipline.SelectedIndex = 0;

            comboValue.Items.Add("Все");
            comboValue.SelectedIndex = 0;

            foreach (var g in entities.Group) { comboGroup.Items.Add(g); }
            foreach (var d in entities.Distiplines) { comboDiscipline.Items.Add(d); }

            
            foreach (var v in values) { comboValue.Items.Add(v); }
        }

        private void comboDiscipline_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboDiscipline.SelectedIndex > 0)
            {
                min_disc = (comboDiscipline.SelectedItem as Distiplines).Id;
                max_disc = min_disc;
            }
            else
            {
                min_disc = 0;
                max_disc = comboDiscipline.Items.Count - 1;
            }
        }

        private void comboGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (comboGroup.SelectedIndex > 0)
            {
                min_group = (comboGroup.SelectedItem as Group).Id;
                max_group = min_group;
            }
            else
            {
                min_group = 0;
                max_group = comboGroup.Items.Count - 1;
            }
        }

        private void Date_in_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Date_in.SelectedDate.HasValue)
            {
                start_date = Date_in.SelectedDate.Value;
            }
        }

        private void Date_out_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Date_in.SelectedDate.HasValue)
            {
                end_date = Date_out.SelectedDate.Value;
            }
        }

        private void comboValue_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (comboValue.SelectedIndex > 0)
            {
                min_value = comboValue.SelectedItem.ToString();
                max_value = min_value;
            }
            else
            {
                int flag;
                if (int.TryParse(comboValue.SelectedItem.ToString(), out flag) == true)
                {
                    min_value = "3";
                    max_value = "5";
                }
                else
                {
                    min_value = "0";
                    max_value = min_value = values.Count().ToString();
                }
            }
        }

        private void AddToGrid(object sender, RoutedEventArgs e)
        {
            dGrid.Items.Clear();
            int value;

            var all_evalution = (from en in entities.Evaluation where en.Distipline >= min_disc && en.Distipline 
                                 <= max_disc && en.Group >= min_group && en.Group <= max_group && en.Data >= start_date 
                                 && en.Data <= end_date select en).ToList();

            foreach (var item in all_evalution)
            {
                string str_value = string.Empty;

                if (int.TryParse(item.Value, out value) == true)
                {
                    str_value = "да";
                }
                else
                {
                    str_value = "нет";
                }

                var elem = new
                {

                    FIO = (from s in entities.Users where s.Id == item.Student select s).FirstOrDefault().ToString(),
                    Value = str_value,
                    Other = item.Value,
                    Date = item.Data.Value.ToShortDateString(),
                };
                dGrid.Items.Add(elem);
            }
        }

        private void Return_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow(current_user, entities);
            mainWindow.Show();
            this.Close();
        }

        private void ExportGrid(object sender, RoutedEventArgs e)
        {
            dGrid.SelectAllCells();
            dGrid.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
            ApplicationCommands.Copy.Execute(null, dGrid);
            String resultat = (string)Clipboard.GetData(DataFormats.CommaSeparatedValue);
            String result = (string)Clipboard.GetData(DataFormats.Text);
            dGrid.UnselectAllCells();

            StreamWriter file = new StreamWriter(@"stat.xls", true, Encoding.GetEncoding(1251));
            file.WriteLine(result.Replace(',', ' '));
            file.Close();

            MessageBox.Show("Экспорт данных в файл stat.xls прошёл успешно");
            Process.Start(System.IO.Path.GetFullPath("stat.xls"));
        }
    }
}
