using System.Windows;
using System.Windows.Media;

namespace StutentsKyrs
{
    /// <summary>
    /// Логика взаимодействия для Autorization.xaml
    /// </summary>
    public partial class Autorization : Window
    {
        Entities entities = new Entities();
        public Autorization()
        {
            InitializeComponent();
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string login = textBoxLogin.Text.Trim();
            string pass = passBox.Password.Trim();

            if (login.Length < 5)
            {
                textBoxLogin.ToolTip = "Некорректный логин!";
                textBoxLogin.Background = Brushes.DarkRed;
            }
            else if (pass.Length < 5)
            {
                passBox.ToolTip = "Некорректный пароль!";
                passBox.Background = Brushes.DarkRed;
            }
            else
            {
                textBoxLogin.ToolTip = "";
                textBoxLogin.Background = Brushes.Transparent;
                passBox.ToolTip = "";
                passBox.Background = Brushes.Transparent;

                Users authUser = null;
                Entities entities = new Entities();
                foreach (var u in entities.Users)
                {
                    if (u.Login == login && u.Password == pass)
                    {
                        authUser = u;
                    }
                }

                if (authUser != null)
                {
                    MessageBox.Show("Все хорошо!");
                    new MainWindow(authUser, entities).Show();
                    this.Close();
                }
                else
                    MessageBox.Show("Ошибка", "Вы ввели что-то не корректно", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Registration regist = new Registration(entities);
            regist.Show();
            Hide();
        }
    }
}

