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

namespace WPFForme.Pages
{
    /// <summary>
    /// Логика взаимодействия для UsersListPG.xaml
    /// </summary>
    public partial class UsersListPG : Page
    {
        List<users> users;
        public UsersListPG()
        {
            InitializeComponent();
            lbUsersList.ItemsSource = BaseConnect.BaseModel.users.ToList();
           
            users = BaseConnect.BaseModel.users.ToList();
            List<genders> genders = BaseConnect.BaseModel.genders.ToList();
            Gen.ItemsSource = genders;
            Gen.SelectedValuePath = "id";
            Gen.DisplayMemberPath = "gender";
        }

        private void lbTraits_Loaded(object sender, RoutedEventArgs e)
        {

            ListBox lb = (ListBox)sender;
            int id = Convert.ToInt32(lb.Uid);
            lb.ItemsSource = BaseConnect.BaseModel.users_to_traits.Where(x => x.id_user == id).ToList();
            lb.DisplayMemberPath = "traits.trait";
        }

        private void Changebtn_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            int id = Convert.ToInt32(b.Uid);
            auth tUser = BaseConnect.BaseModel.auth.FirstOrDefault(x => x.id == id);
            LoadCl.MFrame.Navigate(new ChangePG(tUser));
        }

        private void Delbtn_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            int id = Convert.ToInt32(b.Uid);
            auth DellUs = BaseConnect.BaseModel.auth.FirstOrDefault(x => x.id == id);
            BaseConnect.BaseModel.auth.Remove(DellUs);
            BaseConnect.BaseModel.SaveChanges();
            MessageBox.Show("Пользователь удален!");
            TimeSpan.FromSeconds(3);
            lbUsersList.ItemsSource = BaseConnect.BaseModel.users.ToList();
        }

        private void createbtn_Click(object sender, RoutedEventArgs e)
        {
            LoadCl.MFrame.Navigate(new reg(1));

        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            LoadCl.MFrame.GoBack();
        }

        private void btnSort_Click(object sender, RoutedEventArgs e)
        {
            List<users> lt = users.ToList();
            if (tbStart.Text != "" && tbFinish.Text != "")
            {
                if (Convert.ToInt32(tbStart.Text) >= 0)
                {
                    if (Convert.ToInt32(tbFinish.Text) < users.Count)
                    {
                        int start = Convert.ToInt32(tbStart.Text) - 1;
                        int finish = Convert.ToInt32(tbFinish.Text);
                        lt = users.Skip(start).Take(finish - start).ToList();
                    }
                    else
                    {
                        MessageBox.Show("Строка, до которой необходимо выбрать элементы, больше максимальной!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Information);
                        int start = Convert.ToInt32(tbStart.Text) - 1;
                        int finish = Convert.ToInt32(tbFinish.Text);
                        lt = users.Skip(start).Take(finish - start).ToList();
                    }
                }
                else
                {
                    MessageBox.Show("Строка, от которой необходимо реализовать выборку - отрицательная.\nДля корректной выборки, поправьте данное значение.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Information);
                    int start = Convert.ToInt32(tbStart.Text) - 1;
                    int finish = Convert.ToInt32(tbFinish.Text);
                    lt = users.Skip(start).Take(finish - start).ToList();
                }


            }
            if (Gen.SelectedItem != null)
                lt = lt.Where(x => x.gender == Convert.ToInt32(Gen.SelectedValue)).ToList();

            lbUsersList.ItemsSource = lt;
        }

        private void btnRset_Click(object sender, RoutedEventArgs e)
        {
            tbStart.Text = null;
            tbFinish.Text = null;
            Gen.SelectedItem = null;
            List<users> lt = users.ToList();
            lbUsersList.ItemsSource = lt;

        }
    }
}
