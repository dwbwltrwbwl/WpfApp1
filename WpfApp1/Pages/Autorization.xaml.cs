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
using WpfApp1.ApplicationData;

namespace WpfApp1.Pages
{
    public partial class Autorization : Page
    {
        public Autorization()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                var userObj = ApplicationData.AppConnect.model01.Authors.FirstOrDefault(x => x.Login == TBLogin.Text && x.Password == PBPassword.Password);
                if (userObj == null)
                {
                    MessageBox.Show("Такого пользователя нет", "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show("Здравствуйте", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                    AppConnect.AuthorID = userObj.AuthorID;
                    NavigationService.Navigate(new DataOutput());
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show("Ошибка" + ex.Message.ToString(), "Критическая ошибка приложения", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ButtonRegistr_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Registration());
        }
    }
}
