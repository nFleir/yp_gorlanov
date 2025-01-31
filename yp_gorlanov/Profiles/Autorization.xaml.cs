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

namespace yp_gorlanov.Profiles
{
    /// <summary>
    /// Логика взаимодействия для Autorization.xaml
    /// </summary>
    public partial class Autorization : Page
    {
        private DB.Entities db = new DB.Entities();

        public int CurrentUserID;

        public Autorization()
        {
            InitializeComponent();
        }

        private void Enter_Btn_Click(object sender, RoutedEventArgs e)
        {
            string login = Login_Box.Text;
            string password = Password_Box.Password;
            bool user_found = false;

            var findTeacher = db.teachers.FirstOrDefault(u => u.login == login);
            if (findTeacher != null)
            {
                user_found = true;
                if (findTeacher.password == password)
                {

                    MessageBox.Show("Вход выполнен", "Авторизация", MessageBoxButton.OK);
                    CurrentUserID = findTeacher.teacher_id;

                    AutorizFrame.Navigate(new TeachersPage(CurrentUserID));


                }
                else
                {
                    MessageBox.Show($"Пароли не совпадают", "Авторизация", MessageBoxButton.OK);
                    Password_Box.Clear();
                    return;
                }
            }
            


            var findStudent = db.students.FirstOrDefault(u => u.login == login);
            if (findStudent != null)
            {
                user_found = true;
                if (findStudent.password == password)
                {

                    MessageBox.Show("Вход выполнен", "Авторизация", MessageBoxButton.OK);
                    CurrentUserID = findStudent.student_id;

                    AutorizFrame.Navigate(new Testing(CurrentUserID));

                }
                else
                {
                    MessageBox.Show($"Пароли не совпадают", "Авторизация", MessageBoxButton.OK);
                    Password_Box.Clear();
                    return;
                }
            }

            if (user_found == false)
            {
                MessageBox.Show($"Пользователь {login} не найден", "Авторизация", MessageBoxButton.OK);
                Login_Box.Clear();
                Password_Box.Clear();
            }

        }

        
    }
}
