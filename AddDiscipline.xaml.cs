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
    /// Логика взаимодействия для AddDiscipline.xaml
    /// </summary>
    public partial class AddDiscipline : Window
    {
        Entities entities;
        Users current_user;
        List<Users> prepods;
        public AddDiscipline(Users user, Entities ent)
        {
            InitializeComponent();
            current_user = user;
            DistiplineList.Items.Clear();
            comboBoxPrepod.Items.Clear();

            entities = ent;

            

            foreach (var entity in entities.Distiplines) { DistiplineList.Items.Add(entity); }

            prepods = (from u in entities.Users where u.Rang == 1 select u).ToList();
            foreach (var prepod in prepods) { comboBoxPrepod.Items.Add(prepod); }
            
        }

        private void DistiplineList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected_distipline = DistiplineList.SelectedItem as Distiplines;
            if (selected_distipline != null)
            {
                textBoxAddDistipline.Text = selected_distipline.Title_;
                Users kurator = (from u in entities.Users where u.Id == selected_distipline.Teatcher select u).FirstOrDefault();
                comboBoxPrepod.SelectedIndex = prepods.IndexOf(kurator);
            }

        }

        private void save_button_Click(object sender, RoutedEventArgs e)
        {
            if(textBoxAddDistipline.Text == "" || comboBoxPrepod.SelectedIndex == -1)
            {
                MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if(!(DistiplineList.SelectedItem is Distiplines new_distiplines))
                {
                    new_distiplines = new Distiplines();
                    entities.Distiplines.Add(new_distiplines);
                    DistiplineList.Items.Add(new_distiplines);
                }
                new_distiplines.Title_ = textBoxAddDistipline.Text;
                new_distiplines.Teatcher = (comboBoxPrepod.SelectedItem as Users).Id;
            }
            entities.SaveChanges();
            DistiplineList.Items.Refresh();
            MessageBox.Show("Данные успешно сохранены", "Добавление данных", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void delete_button_Click(object sender, RoutedEventArgs e)
        {
            var rezult = MessageBox.Show("Вы действительно хотите удалить запись?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question);
            var delete = DistiplineList.SelectedItem as Distiplines;

            if (rezult == MessageBoxResult.No) { return; }

            if (DistiplineList.SelectedItem != null)
            {
                entities.Distiplines.Remove(delete);
                try
                {
                    entities.SaveChanges();
                }
                catch (Exception u) 
                {
                    MessageBox.Show("Проверьте таблицу с выводом!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                clear_button_Click(sender, e);

                DistiplineList.Items.Remove(delete);
            }
            else MessageBox.Show("Нет объектов для удаления!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void clear_button_Click(object sender, RoutedEventArgs e)
        {
            textBoxAddDistipline.Text = "";
            comboBoxPrepod.SelectedIndex = -1;
            DistiplineList.SelectedItem = null;
        }

        private void ToMenu(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow(current_user, entities);
            mainWindow.Show();
            this.Close();


        }
    }
}
