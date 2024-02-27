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
    /// Логика взаимодействия для AddGroup.xaml
    /// </summary>
    public partial class AddGroup : Window
    {
        Entities entities;
        Users current_user;

        List<Users> prepods;
        public AddGroup(Users user, Entities ent)
        {
            InitializeComponent();
            current_user = user;
            entities = ent;
            foreach (var group in entities.Group) { GroupList.Items.Add(group); }

            //foreach (var kyrator in entities.Users) { comboBoxKyrator.Items.Add(kyrator); }

            prepods = (from u in entities.Users where u.Rang == 1 select u).ToList();
            foreach (var prepod in prepods) { comboBoxKyrator.Items.Add(prepod); }
        }

        private void GroupList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected_group = GroupList.SelectedItem as Group; 
            if (selected_group != null)
            {
                textBoxAddGroup.Text = selected_group.Title_;
                Users kurator = (from u in entities.Users where u.Id == selected_group.Kyrator select u).FirstOrDefault();
                comboBoxKyrator.SelectedIndex = prepods.IndexOf(kurator); //Find(kurator); //int.Parse(selected_group.Kyrator.ToString());
            }
        }

        private void save_button_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxAddGroup.Text == "" || comboBoxKyrator.SelectedIndex == -1)
            {
                MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (!(GroupList.SelectedItem is Group new_group))
                {
                    new_group = new Group();
                    entities.Group.Add(new_group);
                    GroupList.Items.Add(new_group);
                }
                new_group.Title_ = textBoxAddGroup.Text;
                new_group.Kyrator = (comboBoxKyrator.SelectedItem as Users).Id;
            }
            entities.SaveChanges();
            GroupList.Items.Refresh();
            MessageBox.Show("Данные успешно сохранены", "Добавление данных", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void delete_button_Click(object sender, RoutedEventArgs e)
        {
            var rezult = MessageBox.Show("Вы действительно хотите удалить запись?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question);
            var delete = GroupList.SelectedItem as Group;

            if (rezult == MessageBoxResult.No) { return; }

            if (GroupList.SelectedItem != null)
            {
                entities.Group.Remove(delete);
                entities.SaveChanges();
                clear_button_Click(sender, e);

                GroupList.Items.Remove(delete);
            }
            else MessageBox.Show("Нет объектов для удаления!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void clear_button_Click(object sender, RoutedEventArgs e)
        {
            textBoxAddGroup.Text = "";
            comboBoxKyrator.SelectedIndex = -1;
            GroupList.SelectedItem = null;
        }

        private void ToMenu(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow(current_user, entities);
            this.Close();
        }
    }
}
