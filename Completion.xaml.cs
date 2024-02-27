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
using System.Windows.Shapes;

namespace StutentsKyrs
{
    /// <summary>
    /// Логика взаимодействия для Completion.xaml
    /// </summary>
    public partial class Completion : Window
    {
        Users current_user;
        Entities entities;
        int[] values = new int[] { 3, 4, 5 };
        string[] prich = new string[] { "По болезни"};
        public Completion(Users user , Entities ent )
        {
            InitializeComponent();
            current_user = user;
            entities = ent;
            foreach (var g in entities.Group) { ComboGroup.Items.Add(g); }
            foreach (var d in entities.Distiplines) { Discipline.Items.Add(d); }

            foreach (var v in values) { ComboValue.Items.Add(v);}
            foreach (var p in prich) { Prichina.Items.Add(p);}
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (StudentEval.IsChecked == false)
            {
                PanelValue.Visibility = Visibility.Visible;
                PanelPrichina.Visibility = Visibility.Hidden;
            }
            else
            {
                PanelPrichina.Visibility = Visibility.Visible;
                PanelValue.Visibility = Visibility.Hidden;
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected_group = ComboGroup.SelectedItem as Group;
            if (selected_group != null)
            {
                var students = from s in entities.Users where s.Group == selected_group.Id select s;
                students_of_group.Items.Clear();
                foreach (var student in students) { students_of_group.Items.Add(student); } 
            }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected_student = students_of_group.SelectedItem as Users;
            if (selected_student != null)
            {
                Panel.Visibility = Visibility.Visible;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Date.Text != string.Empty || Discipline.SelectedItem != null)
            {
                var date = Date.SelectedDate;
                var discipline = (Discipline.SelectedItem as Distiplines).Id;
                string value = string.Empty;
                if (StudentEval.IsChecked == false)
                {
                    if (ComboValue.SelectedIndex != -1)
                    {
                        value = ComboValue.SelectedItem.ToString();
                    }
                }
                else
                {
                    if (Prichina.SelectedIndex != -1)
                    {

                        value = Prichina.SelectedItem.ToString();

                    }
                }
                var student = (students_of_group.SelectedItem as Users).Id;

                var evaluation = (from ev in entities.Evaluation where ev.Data == date & ev.Distipline == discipline & ev.Student == student select ev).FirstOrDefault();
                if (evaluation == null)
                {
                    evaluation = new Evaluation(); // Evalution(int val, int dis, int stud, DateTime date)
                    entities.Evaluation.Add(evaluation);
                }
                evaluation.Data = date;
                evaluation.Distipline = discipline;
                evaluation.Student = student;
                evaluation.Group = (ComboGroup.SelectedItem as Group).Id;
                evaluation.Value = value;
                entities.SaveChanges();
            }
        }

        private void menu_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow(current_user, entities);
            mainWindow.Show();
            Hide();
        }

        private void table_button_Click(object sender, RoutedEventArgs e)
        {
            Grade gr = new Grade(current_user, entities);
            gr.Show();
            Hide();
        }
    }
  }

