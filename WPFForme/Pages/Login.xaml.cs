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
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               auth CurrentUsers = BaseConnect.BaseModel.auth.FirstOrDefault(x => x.login == Logtxt.Text && x.password == Passtxt.Password);
                if (CurrentUsers != null)
                {
                    switch(CurrentUsers.role)
                    {
                        case 1:
                            MessageBox.Show("Вы авторизовались под учетной записью администратора", "Успешная авторизация");
                            LoadCl.MFrame.Navigate(new UsersListPG());
                            break;

                        case 2:
                        default:
                            MessageBox.Show("Вы вошли как " + CurrentUsers.login, "Успешная авторизация");
                            LoadCl.MFrame.Navigate(new Informs(CurrentUsers));
                            break;
                            
                    }

                
                }
                else
                    MessageBox.Show("Ошибка авторизации!");
            }
            catch
            {
                MessageBox.Show("Произошла неизвестная ошибка...");
            }

        }

        private void RegBtn_Click(object sender, RoutedEventArgs e)
        {
            LoadCl.MFrame.Navigate(new reg());
        }

        private void RegBtn_Click_1(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
            
        }
    }
}
