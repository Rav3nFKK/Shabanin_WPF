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
using drSr;

namespace WPFForme.Pages
{
    /// <summary>
    /// Логика взаимодействия для UsersListPG.xaml
    /// </summary>
    public partial class UsersListPG : Page
    {
        List<users> users;
        List<users> lt;
        public UsersListPG()
        {
            InitializeComponent();
            lbUsersList.ItemsSource = BaseConnect.BaseModel.users.ToList();
            lt = users;

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
            LoadCl.MFrame.Navigate(new Login());
        }
        private void Filter(object sender, RoutedEventArgs e)
        {

            //фильтр по строкам
            try
            {
                int start = Convert.ToInt32(tbStart.Text) - 1;
                int finish = Convert.ToInt32(tbFinish.Text);
                lt = users.Skip(start).Take(finish - start).ToList();

            }
            catch
            {
                //null
            }
            //фильтр по полу
            try
            {
                if (Gen.SelectedIndex != -1)
                    lt = lt.Where(x => x.gender == Convert.ToInt32(Gen.SelectedValue)).ToList();

            }
            catch
            {
                //null
            }

            //фильтр по имени
            try
            {
                if (txtNameFilter.Text != "")
                {
                    lt = lt.Where(x => x.name.Contains(txtNameFilter.Text)).ToList();
                }
            }
            catch
            {
                //null
            }

            lbUsersList.ItemsSource = lt;
        }
        private void btnRset_Click(object sender, RoutedEventArgs e)
        {
            tbStart.Text = null;
            tbFinish.Text = null;
            Gen.SelectedItem = null;
            txtNameFilter.Text = null;

            List<users> lt = users.ToList();
            lbUsersList.ItemsSource = lt;

        }
        private void tbStart_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(Char.IsDigit(e.Text, 0));
        }
        int currpg = 1;
        private void GoPage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                TextBlock tb = (TextBlock)sender;
                int countList = users.Count;
                int countzap = Convert.ToInt32(txtPageCount.Text);
                int countpage = countList / countzap;

                switch (tb.Uid)
                {
                    case "prev":
                        currpg--;
                        break;
                    case "1":
                        if (currpg < 3) currpg = 1;
                        else if (currpg > countpage) currpg = countpage - 4;
                        else currpg -= 2;
                        break;
                    case "2":
                        if (currpg < 3) currpg = 2;
                        else if (currpg > countpage) currpg = countpage - 3;
                        else currpg -= 1;
                        break;
                    case "3":
                        if (currpg < 3) currpg = 3;
                        else if (currpg > countpage) currpg = countpage - 2;
                        break;
                    case "4":
                        if (currpg < 3) currpg = 4;
                        else if (currpg > countpage) currpg = countpage - 1;
                        else currpg++;
                        break;
                    case "5":
                        if (currpg < 3) currpg = 5;
                        else if (currpg > countpage) currpg = countpage;
                        else currpg += 2;
                        break;
                    case "next":
                        currpg++;
                        break;
                    default:
                        currpg = 1;
                        break;
                }

                //отрисовка
                if (currpg < 3)
                {
                    txt1.Text = " 1 ";
                    txt2.Text = " 2 ";
                    txt3.Text = " 3 ";
                    txt4.Text = " 4 ";
                    txt5.Text = " 5 ";
                }
                else if (currpg > countpage - 2)
                {
                    txt1.Text = " " + (countpage - 4).ToString() + " ";
                    txt2.Text = " " + (countpage - 3).ToString() + " ";
                    txt3.Text = " " + (countpage - 2).ToString() + " ";
                    txt4.Text = " " + (countpage - 1).ToString() + " ";
                    txt5.Text = " " + (countpage).ToString() + " ";

                }
                else
                {
                    txt1.Text = " " + (currpg - 2).ToString() + " ";
                    txt2.Text = " " + (currpg - 1).ToString() + " ";
                    txt3.Text = " " + (currpg).ToString() + " ";
                    txt4.Text = " " + (currpg + 1).ToString() + " ";
                    txt5.Text = " " + (currpg + 2).ToString() + " ";

                }
                txtCurentPage.Text = "Текущая страница: " + (currpg).ToString();

                if (currpg < 1) currpg = 1;
                if (currpg >= countpage) currpg = countpage;

                lt = users.Skip(currpg * countzap - countzap).Take(countzap).ToList();
                lbUsersList.ItemsSource = lt;
            }
            catch
            {
                //null
            }
        }
        private void txtPageCount_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (txtPageCount.Text == "")
                {
                    lt = users.ToList();
                }
                else
                    lt = users.Take(Convert.ToInt32(txtPageCount.Text)).ToList();

                lbUsersList.ItemsSource = lt;
            }
            catch
            {
                //null
            }
        }
        private void DLlL_Click(object sender, RoutedEventArgs e)
        {
            LoadCl.MFrame.Navigate(new Dlltest());
           
        }
    }
}
