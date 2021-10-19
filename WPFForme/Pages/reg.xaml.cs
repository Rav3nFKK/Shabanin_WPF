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
    /// Логика взаимодействия для reg.xaml
    /// </summary>
    public partial class reg : Page
    {
        public reg()
        {
            InitializeComponent();
            list1.ItemsSource = BaseConnect.BaseModel.genders.ToList();
            list1.SelectedValuePath = "id";
            list1.DisplayMemberPath = "gender";

            string[] traits2 = new string[3];
            List<traits> traits1 = BaseConnect.BaseModel.traits.ToList();
            int i = 0;
            foreach (traits tr in traits1)
            {
                traits2[i] = tr.trait;
                i++;
            }
            //dobr.Content = traits2[0];
            //nezh.Content = traits2[1];
            //lask.Content = traits2[2];

        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            LoadCl.MFrame.GoBack();
        }

        private void Zapbut_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                auth LogPass = new auth() { login = Logtxt.Text, password = Passtxt.Password, role = 2 };
                BaseConnect.BaseModel.auth.Add(LogPass);
                BaseConnect.BaseModel.SaveChanges();


                users NewUs = new users();
                NewUs.name = Nametxt.Text;
                NewUs.id = LogPass.id;
                NewUs.gender = (int)list1.SelectedValue;
                NewUs.dr = (DateTime)Datedr.SelectedDate;

                BaseConnect.BaseModel.users.Add(NewUs);


                if (dobr.IsChecked == true)
                {
                    users_to_traits UTT = new users_to_traits();
                    UTT.id_user = NewUs.id;
                    UTT.id_trait = 1;
                    BaseConnect.BaseModel.users_to_traits.Add(UTT);
                }
                if (nezh.IsChecked == true)
                {
                    users_to_traits UTT = new users_to_traits();
                    UTT.id_user = NewUs.id;
                    UTT.id_trait = 2;
                    BaseConnect.BaseModel.users_to_traits.Add(UTT);

                }
                if (lask.IsChecked == true)
                {
                    users_to_traits UTT = new users_to_traits();
                    UTT.id_user = NewUs.id;
                    UTT.id_trait = 3;
                    BaseConnect.BaseModel.users_to_traits.Add(UTT);

                }
                BaseConnect.BaseModel.SaveChanges();

                MessageBox.Show("Пользователь " + Logtxt.Text + ", успешно зарегистрирован");
            }
            catch
            {
                MessageBox.Show("Произошла ошибка");
            }
        }
    }
}
