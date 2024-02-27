using System;
using System.Collections.Generic;
using System.Data;
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
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        Entities entities = new Entities();
        int rang = 0;
        
        public Registration()
        {
            InitializeComponent();
            foreach (var g in entities.Group) { comboGroup.Items.Add(g); }
        }
        public Registration(Entities ent)
        {
            InitializeComponent();
            entities = ent;
            foreach (var g in entities.Group) { comboGroup.Items.Add(g); }
        }

        public bool Check(string login, string pass)
        {
            if (login.Length <= 5)
            {
                textBoxLogin.ToolTip = "Некорректный логин!";
                textBoxLogin.Background = Brushes.DarkRed;
                return false;
            }
            else if (pass.Length <= 5)
            {
                passBox.ToolTip = "Некорректный пароль!";
                passBox.Background = Brushes.DarkRed;
                return false;
            }
            return true;
        }

        private void Button_Reg_Click(object sender, RoutedEventArgs e)
        {
            string login = textBoxLogin.Text.Trim();
            string pass = passBox.Password.Trim();


            if (Check(login, pass))
            {
                try
                {
                    Users user = new Users();
                    if (textBoxLogin.Text == "" || textBoxLogin.Text == "" || textBoxName.Text == "" || textBoxFamilia.Text == "") 
                    { MessageBox.Show("Не все поля были заполнены!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); ; return; }
                   
                    if (comboGroup.SelectedIndex != -1)
                    { 
                        user = new Users();
                    }
                    else
                    {
                        user = new Users();
                    }
                    
                    entities.Users.Add(user);
                    entities.SaveChanges();

                    MessageBox.Show("Все хорошо!");
                    Autorization authWindow = new Autorization();
                    authWindow.Show();
                    Hide();
                }
                catch (Exception)
                {
                    MessageBox.Show("Не все поля были заполнены!");
                }

            }
        }

        private void Button_Window_Auth_Click(object sender, RoutedEventArgs e)
        {
            Autorization authWindow = new Autorization();
            authWindow.Show();
            Hide();
        }

        private void rb_prepods_Checked(object sender, RoutedEventArgs e)
        {
            if (rb_students.IsChecked == false & rb_prepods.IsChecked == false) { MessageBox.Show("Выберите ранг нового пользователя!"); return; }
            else
            {
                if (rb_students.IsChecked == true) { rang = 0; rb_prepods.IsChecked = false; PanelGroup.Visibility = Visibility.Visible; }
                else if (rb_prepods.IsChecked == true) { rang = 1; rb_students.IsChecked = false; PanelGroup.Visibility = Visibility.Hidden; }
            }
        }

        private void rb_students_Checked(object sender, RoutedEventArgs e)
        {
            if (rb_students.IsChecked == false & rb_prepods.IsChecked == false) { MessageBox.Show("Выберите ранг нового пользователя!"); return; }
            else
            {
                if (rb_students.IsChecked == true) { rang = 0; rb_prepods.IsChecked = false; PanelGroup.Visibility = Visibility.Visible; }
                else if (rb_prepods.IsChecked == true) { rang = 1; rb_students.IsChecked = false; PanelGroup.Visibility = Visibility.Hidden; }
            }
        }
    }
}
