using Microsoft.Win32;
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

namespace StutentsKyrs
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Entities entities;
        Users current_user;
        String[] rangs = new string[] { "Пользователь", "Куратор"};
        public MainWindow(Users user, Entities ent)
        {
            InitializeComponent();
            current_user = user;
            entities = ent;
            HelloMessage.Text = $"Добро пожаловать, {current_user.Name}";
            LabelUser.Content = $"Вход выполнен: {rangs[current_user.Rang]} {current_user.Name} {current_user.Familia}";

            if (current_user.Photo != null)
            {
                image.Source = new BitmapImage(new Uri(current_user.Photo, UriKind.RelativeOrAbsolute));
            }

            if (current_user.Rang == 0)
            {
                adminPanel.Visibility = Visibility.Hidden;
            }
            else
            {
                adminPanel.Visibility = Visibility.Visible;
            }
            
        }

        

        private void ChangeImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            if (dlg.ShowDialog() == true && !string.IsNullOrWhiteSpace(dlg.FileName))
                current_user.Photo = dlg.FileName;
            
            entities.SaveChanges();
            MessageBox.Show("Фото изменено");
            image.Source = new BitmapImage(new Uri(current_user.Photo, UriKind.RelativeOrAbsolute));
        }

        public void OnClickAddUs(object sender, RoutedEventArgs e)
        {
            Completion compl = new Completion(current_user, entities);
            compl.Show();
            Hide();
        }

        public void OnClickAddGroup(object sender, RoutedEventArgs e)
        {
            AddGroup AGroup = new AddGroup(current_user, entities);
            AGroup.Show();
            Hide();
        }

        public void OnClickAddDist(object sender, RoutedEventArgs e)
        {
            AddDiscipline Dist = new AddDiscipline(current_user, entities);
            Dist.Show();
            Hide();
        }

        public void OnClickAddInf(object sender, RoutedEventArgs e)
        {
            Grade grad = new Grade(current_user, entities);
            grad.Show();
            Hide();
        }

    }
}
