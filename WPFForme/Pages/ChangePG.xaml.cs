using System;
using System.Collections.Generic;
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

namespace WPFForme.Pages
{
    /// <summary>
    /// Логика взаимодействия для ChangePG.xaml
    /// </summary>
    public partial class ChangePG : Page
    {
        public string LogStr, PassStr;
        bool flag = false;
        int puf;
        List<usersimage> userImg;
        List<usersimage> userImgBuf;
        public ChangePG(auth auths)
        {
            InitializeComponent();

            try
            {
                Logtxt.Text = auths.login;
                pass.Text = auths.password;
                LogStr = auths.login;
                PassStr = auths.password;
                Nametxt.Text = auths.users.name;
                drtxt.SelectedDate = auths.users.dr;
                List<users_to_traits> LUTT = BaseConnect.BaseModel.users_to_traits.Where(x => x.id_user == auths.id).ToList();
                string[] traits2 = new string[3];
                List<traits> traits1 = BaseConnect.BaseModel.traits.ToList();
                int i = 0;
                foreach (traits tr in traits1)
                {
                    traits2[i] = tr.trait;
                    i++;
                }
                d1.Content = traits2[0];
                d2.Content = traits2[1];
                d3.Content = traits2[2];

                foreach (users_to_traits tr in LUTT)
                {
                    if (d1.Content == tr.traits.trait)
                    {
                        d1.IsChecked = true;
                    }
                    if (d2.Content == tr.traits.trait)
                    {
                        d2.IsChecked = true;
                    }
                    if (d3.Content == tr.traits.trait)
                    {
                        d3.IsChecked = true;
                    }
                }
                genderLt.ItemsSource = BaseConnect.BaseModel.genders.ToList();
                genderLt.SelectedValuePath = "id";
                genderLt.DisplayMemberPath = "gender";
                genderLt.SelectedIndex = auths.users.genders.id - 1;
                flag = false;
                userImg = BaseConnect.BaseModel.usersimage.Where(x => x.id_user == auths.users.id && x.avatar == false).ToList();
                userImgBuf = BaseConnect.BaseModel.usersimage.Where(x => x.id_user == auths.users.id && x.avatar == true).ToList();
                foreach (usersimage ui in userImgBuf)
                {
                    userImg.Insert(0, ui);
                }
                puf = auths.id;

            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка загрузки: " + e.Message);

            }
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            if (flag == false)
                LoadCl.MFrame.Navigate(new UsersListPG());
            else
            {
                MessageBoxResult otv = MessageBox.Show("Вы хотите сохранить изменения?", "Вы внесли изменения!", MessageBoxButton.YesNoCancel, MessageBoxImage.Information);

                if (otv == MessageBoxResult.Yes)
                {
                    save();
                    LoadCl.MFrame.GoBack();
                }
                if (otv == MessageBoxResult.No)
                {
                    LoadCl.MFrame.GoBack();
                }
                if (otv == MessageBoxResult.Cancel)
                {

                }

            }
        }

        private void Logtxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            flag = true;
        }



        private void d1_Checked(object sender, RoutedEventArgs e)
        {
            flag = true;
        }


        private void drtxt_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            flag = true;
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {

            save();

        }




        public void save()
        {
            auth User = BaseConnect.BaseModel.auth.FirstOrDefault(x => x.login == LogStr && x.password == PassStr);
            users mbUser = BaseConnect.BaseModel.users.FirstOrDefault(x => x.id == User.id);
            if (mbUser == null)
            {
                users newUser = new users { id = User.id, name = Nametxt.Text, dr = (DateTime)drtxt.SelectedDate, gender = (int)genderLt.SelectedValue };
                BaseConnect.BaseModel.users.Add(newUser);
                if (d1.IsChecked == true)
                {
                    users_to_traits UTT = new users_to_traits();
                    UTT.id_user = newUser.id;
                    UTT.id_trait = 1;
                    BaseConnect.BaseModel.users_to_traits.Add(UTT);
                }
                if (d2.IsChecked == true)
                {
                    users_to_traits UTT = new users_to_traits();
                    UTT.id_user = newUser.id;
                    UTT.id_trait = 2;
                    BaseConnect.BaseModel.users_to_traits.Add(UTT);
                }
                if (d3.IsChecked == true)
                {
                    users_to_traits UTT = new users_to_traits();
                    UTT.id_user = newUser.id;
                    UTT.id_trait = 3;
                    BaseConnect.BaseModel.users_to_traits.Add(UTT);
                }
                BaseConnect.BaseModel.SaveChanges();
            }
            else
            {
                mbUser.dr = (DateTime)drtxt.SelectedDate;
                mbUser.name = Nametxt.Text;
                User.login = Logtxt.Text;
                User.password = pass.Text;
                users_to_traits trait1 = BaseConnect.BaseModel.users_to_traits.FirstOrDefault(x => x.id_user == User.id && x.id_trait == 1);
                users_to_traits trait2 = BaseConnect.BaseModel.users_to_traits.FirstOrDefault(x => x.id_user == User.id && x.id_trait == 2);
                users_to_traits trait3 = BaseConnect.BaseModel.users_to_traits.FirstOrDefault(x => x.id_user == User.id && x.id_trait == 3);
                try
                {
                    if (genderLt.SelectedItem != null)
                    {
                        mbUser.gender = (int)genderLt.SelectedValue;
                    }
                    if (d1.IsChecked == false && trait1 != null)
                    {
                        BaseConnect.BaseModel.users_to_traits.Remove(trait1);
                    }
                    else if (d1.IsChecked == true && trait1 == null)
                    {
                        CreateTrait(User, 1);
                    }
                    if (d2.IsChecked == false && trait2 != null)
                    {
                        BaseConnect.BaseModel.users_to_traits.Remove(trait2);
                    }
                    else if (d2.IsChecked == true && trait2 == null)
                    {
                        CreateTrait(User, 2);
                    }
                    if (d3.IsChecked == false && trait3 != null)
                    {
                        BaseConnect.BaseModel.users_to_traits.Remove(trait3);
                    }
                    else if (d3.IsChecked == true && trait3 == null)
                    {
                        CreateTrait(User, 3);
                    }
                    BaseConnect.BaseModel.SaveChanges();
                    MessageBox.Show("Изменения успешно внесены");
                    flag = false;
                }
                catch (Exception a)
                {
                    MessageBox.Show(a.Message);
                }
            }

        }

        private void Image_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Image IMG = sender as System.Windows.Controls.Image;
            int ind = Convert.ToInt32(puf);
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
                    BI.StreamSource = new MemoryStream(userImg[0].image);//помещаем в источник данных двоичные данные из потока
                    BI.EndInit();//закончить инициализацию


                }
            }
            else
            {

                switch (U.gender)
                {
                    case 1:
                        BI = new BitmapImage(new Uri(@"/img/Dog.jpg", UriKind.Relative));
                        break;
                    case 2:
                        BI = new BitmapImage(new Uri(@"/img/Panda.jpg", UriKind.Relative));
                        break;
                    default:
                        BI = new BitmapImage(new Uri(@"/img/unnamed.jpg", UriKind.Relative));
                        break;
                }
            }



            IMG.Source = BI;
        }
        int x;
        
        
        private void imgChange(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            BitmapImage BI2 = new BitmapImage();

            switch (btn.Content)
            {
                case "Туда":
                    if (x < userImg.Count - 1)
                        x++;
                    else
                        x = 0;
                    if (x < userImg.Count)
                    {
                        if (userImg[x].path != null)//если присутствует путь к картинке
                        {
                            BI2 = new BitmapImage(new Uri(userImg[x].path, UriKind.Relative));
                        }
                        else//если присутствуют двоичные данные
                        {
                            BI2.BeginInit();//начать инициализацию BitmapImage (для помещения данных из какого-либо потока)
                            BI2.StreamSource = new MemoryStream(userImg[x].image);//помещаем в источник данных двоичные данные из потока
                            BI2.EndInit();//закончить инициализацию
                        }
                        Image.Source = BI2;
                    }

                    break;
                case "Сюда":
                    if (x != 0)
                        x--;
                    else
                        x = userImg.Count - 1;
                    if (x >= 0)
                    {
                        if (userImg[x].path != null)//если присутствует путь к картинке
                        {
                            BI2 = new BitmapImage(new Uri(userImg[x].path, UriKind.Relative));
                        }
                        else//если присутствуют двоичные данные
                        {
                            BI2.BeginInit();//начать инициализацию BitmapImage (для помещения данных из какого-либо потока)
                            BI2.StreamSource = new MemoryStream(userImg[x].image);//помещаем в источник данных двоичные данные из потока
                            BI2.EndInit();//закончить инициализацию
                        }
                        Image.Source = BI2;
                    }

                    break;
            }
        }



        public static void CreateTrait(auth User, int i)
        {
            users_to_traits UTT = new users_to_traits();
            UTT.id_user = User.id;
            UTT.id_trait = i;
            BaseConnect.BaseModel.users_to_traits.Add(UTT);
        }

    }
}
