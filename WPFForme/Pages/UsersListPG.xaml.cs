using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
        PageChange pc = new PageChange();
        public UsersListPG()
        {
            InitializeComponent();
            lbUsersList.ItemsSource = BaseConnect.BaseModel.users.ToList();
            users = BaseConnect.BaseModel.users.ToList();
            lt = users;

            users = BaseConnect.BaseModel.users.ToList();
            List<genders> genders = BaseConnect.BaseModel.genders.ToList();
            Gen.ItemsSource = genders;
            Gen.SelectedValuePath = "id";
            Gen.DisplayMemberPath = "gender";
            pgPanel.Visibility = Visibility.Hidden;
            DataContext = pc;
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
            lt = users;
            //фильтр по строкам
            try
            {
                int start = Convert.ToInt32(tbStart.Text) - 1;
                int finish = Convert.ToInt32(tbFinish.Text);
                lt = lt.Skip(start).Take(finish - start).ToList();

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

            lt = users.ToList();
            lbUsersList.ItemsSource = lt;

        }
        private void tbStart_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(Char.IsDigit(e.Text, 0));
        }
        private void GoPage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock tb = (TextBlock)sender;//определяем, какой текстовый блок был нажат           
                                             //изменение номера страници при нажатии на кнопку
            switch (tb.Uid)
            {
                case "prev":
                    pc.CurrentPage--;
                    break;
                case "next":
                    pc.CurrentPage++;
                    break;
                default:
                    pc.CurrentPage = Convert.ToInt32(tb.Text);
                    break;
            }


            //определение списка
            lbUsersList.ItemsSource = lt.Skip(pc.CurrentPage * pc.CountPage - pc.CountPage).Take(pc.CountPage).ToList();

            txtCurrentPage.Text = "Текущая страница: " + (pc.CurrentPage).ToString();
        }
        private void txtPageCount_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                pc.CountPage = Convert.ToInt32(txtPageCount.Text);
                if(pc.CountPage >0)
                {
                    pgPanel.Visibility = Visibility.Visible;
                }
                else
                {
                    pgPanel.Visibility = Visibility.Hidden;
                    txtCurrentPage.Text = null;
                }
            }
            catch
            {
                pc.CountPage = lt.Count;
                pgPanel.Visibility = Visibility.Hidden;
                txtCurrentPage.Text = null;
            }
            pc.Countlist = users.Count;
            lbUsersList.ItemsSource = lt.Skip(0).Take(pc.CountPage).ToList();
        }
    
        private void DLlL_Click(object sender, RoutedEventArgs e)
        {
            LoadCl.MFrame.Navigate(new Dlltest());
           
        }

        private void BtmAddImage_Click(object sender, RoutedEventArgs e)
        {
            Button BTN = (Button)sender;
            int ind = Convert.ToInt32(BTN.Uid);
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = ".jpg"; // задаем расширение по умолчанию
            openFileDialog.Filter = "Изображения |*.jpg;*.png"; // задаем фильтр на форматы файлов
            var result = openFileDialog.ShowDialog();
            if (result == true)//если файл выбран
            {
                usersimage avatar = new usersimage();//создаем новый объект usersimage
                List<usersimage> U = BaseConnect.BaseModel.usersimage.Where(x => x.id_user == ind).ToList();//получаем запись о картинке для текущего пользователя
                if (U != null)
                {
                    foreach (usersimage um in U)
                    {
                        if (um.avatar == true)
                        {
                            avatar = um;
                        }
                    }
                }
                if (avatar != null)
                {
                    if (MessageBox.Show("Сменить аватар пользователя?", "Вопрос", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        
                        System.Drawing.Image UserImage = System.Drawing.Image.FromFile(openFileDialog.FileName);//создаем изображение
                        ImageConverter IC = new ImageConverter();//конвертер изображения в массив байт
                        byte[] ByteArr = (byte[])IC.ConvertTo(UserImage, typeof(byte[]));//непосредственно конвертация
                        usersimage UI = new usersimage() { id_user = ind, image = ByteArr, avatar = true };//создаем новый объект usersimage
                        avatar.avatar = false;
                        BaseConnect.BaseModel.usersimage.Add(UI);//добавляем его в модель
                        BaseConnect.BaseModel.SaveChanges();//синхронизируем с базой
                        MessageBox.Show("картинка пользователя добавлена в базу");
                    }
                    else
                    {
                        System.Drawing.Image UserImage = System.Drawing.Image.FromFile(openFileDialog.FileName);//создаем изображение
                        ImageConverter IC = new ImageConverter();//конвертер изображения в массив байт
                        byte[] ByteArr = (byte[])IC.ConvertTo(UserImage, typeof(byte[]));//непосредственно конвертация
                        usersimage UI = new usersimage() { id_user = ind, image = ByteArr, avatar = false };//создаем новый объект usersimage
                        BaseConnect.BaseModel.usersimage.Add(UI);//добавляем его в модель
                        BaseConnect.BaseModel.SaveChanges();//синхронизируем с базой
                        MessageBox.Show("картинка пользователя добавлена в базу");
                    }
                }
                else
                {
                    System.Drawing.Image UserImage = System.Drawing.Image.FromFile(openFileDialog.FileName);//создаем изображение
                    ImageConverter IC = new ImageConverter();//конвертер изображения в массив байт
                    byte[] ByteArr = (byte[])IC.ConvertTo(UserImage, typeof(byte[]));//непосредственно конвертация
                    usersimage UI = new usersimage() { id_user = ind, image = ByteArr, avatar = false };//создаем новый объект usersimage
                    BaseConnect.BaseModel.usersimage.Add(UI);//добавляем его в модель
                    BaseConnect.BaseModel.SaveChanges();//синхронизируем с базой
                    MessageBox.Show("картинка пользователя добавлена в базу");
                }
            }
            else
            {
                MessageBox.Show("операция выбора изображения отменена");
            }
            users = BaseConnect.BaseModel.users.ToList();
            lbUsersList.ItemsSource = users;
        }

        private void btnGoToGallery_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UserImage_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Image IMG = sender as System.Windows.Controls.Image;
            int ind = Convert.ToInt32(IMG.Uid);
            users U = BaseConnect.BaseModel.users.FirstOrDefault(x => x.id == ind);//запись о текущем пользователе
            usersimage UI = BaseConnect.BaseModel.usersimage.FirstOrDefault(x => x.id_user == ind && x.avatar == true);//получаем запись о картинке для текущего пользователя
            BitmapImage BI = new BitmapImage();
            if (UI != null)//если для текущего пользователя существует запись о его катринке
            {

                if (UI.path != null)//если присутствует путь к картинке
                {
                    BI = new BitmapImage(new Uri(UI.path, UriKind.Relative));
                }
                else//если присутствуют двоичные данные
                {
                    BI.BeginInit();//начать инициализацию BitmapImage (для помещения данных из какого-либо потока)
                    BI.StreamSource = new MemoryStream(UI.image);//помещаем в источник данных двоичные данные из потока
                    BI.EndInit();//закончить инициализацию
                }

            }

            else//если в базе не содержится картинки, то ставим заглушку
            {
                switch (U.gender)//в зависимости от пола пользователя устанавливаем ту или иную картинку
                {
                    case 1:
                        BI = new BitmapImage(new Uri(@"/img/male.jpg", UriKind.Relative));
                        break;
                    case 2:
                        BI = new BitmapImage(new Uri(@"/img/female.jpg", UriKind.Relative));
                        break;
                    default:
                        BI = new BitmapImage(new Uri(@"/img/other.jpg", UriKind.Relative));
                        break;
                }
            }
            IMG.Source = BI;//помещаем картинку в image
        }
    }
}
