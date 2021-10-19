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
    /// Логика взаимодействия для AdminPanel.xaml
    /// </summary>
    public partial class AdminPanel : Page
    {
        public AdminPanel()
        {
            InitializeComponent();
            dgUsers.ItemsSource = BaseConnect.BaseModel.auth.ToList();
        }

        private void Del_Click(object sender, RoutedEventArgs e)
        {

            auth SelectedUS = (auth)dgUsers.SelectedItem;
            BaseConnect.BaseModel.auth.Remove(SelectedUS);
            BaseConnect.BaseModel.SaveChanges();
            MessageBox.Show("Чел в ремуве");
            TimeSpan.FromSeconds(3);
            dgUsers.ItemsSource = BaseConnect.BaseModel.auth.ToList();
        }

        private void Backbtn_Click(object sender, RoutedEventArgs e)
        {
            LoadCl.MFrame.GoBack();
        }

        private void UpdateBTN_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
