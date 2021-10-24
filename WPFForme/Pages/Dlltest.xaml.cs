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
    /// Логика взаимодействия для Dlltest.xaml
    /// </summary>
    public partial class Dlltest : Page
    {
        List<users> users;
        public Dlltest()
        {
            InitializeComponent();
            users = BaseConnect.BaseModel.users.ToList();
        }

        private void serch_Click(object sender, RoutedEventArgs e)
        {
            serchtxt.Text = null;

            List<string> nameserch = new List<string>();

            foreach(users u in users)
            {
                nameserch.Add(u.name);
            }

            nameserch = DLLTry.serchname(nameserch, txt.Text);
            foreach(string s in nameserch)
            {
                serchtxt.Text += s + "\n";
            }
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            LoadCl.MFrame.GoBack();
        }

        private void avgdt_Click(object sender, RoutedEventArgs e)
        {
            int i = 0;
            DateTime[] mas = new DateTime[users.Count];
            foreach (users us in users)
            {
                mas[i] = us.dr;
                i++;
            }

            MessageBox.Show("Средний возраст: " + DLLTry.drsr(mas).ToString());

        }

        private void txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            serchtxt.Text = null;
        }
    }
}


