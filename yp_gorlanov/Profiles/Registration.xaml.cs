using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Page
    {
        private static Regex LoginReg = new Regex(@"^[a-zA-Z0-9]{5,}$");
        private static Regex PassReg = new Regex(@"^[a-zA-Zа-яА-Я0-9]{8,}$");

        private DB.Entities db = new DB.Entities();
        
        public Registration()
        {
            InitializeComponent();
            LoadRoles();
            LoadGroups();
        }

        private void LoadRoles()
        {
            var roles = db.roles.ToList();
            Role_Box.ItemsSource = roles;
            Role_Box.DisplayMemberPath = "role_name";
            Role_Box.SelectedValuePath = "role_id";
        }

        private void LoadGroups()
        {
            var groups = db.groups.ToList();
            Group_Box.ItemsSource = groups;
            Group_Box.DisplayMemberPath = "group_name";
            Group_Box.SelectedValuePath = "group_id";
        }


        private void Registration_Box_Click(object sender, RoutedEventArgs e)
        {
            string login = Login_Box.Text;
            string password = Password_Box.Text;
            string name = Name_Box.Text;
            string surname = Surname_Box.Text;
            string patronymic = Patronymic_Box.Text;
            int role = (int)Role_Box.SelectedValue;
            int group = (int)Role_Box.SelectedValue;
            var selectedRole = Role_Box.SelectedItem as DB.roles;
            string student_number = StudentCard_Box.Text;

            try
            {
                if (!LoginReg.IsMatch(login))
                {
                    MessageBox.Show("Логин должен содержать не менее 5 английских символов!", "Ошибка", MessageBoxButton.OK);
                    return;
                }
                if (!PassReg.IsMatch(password))
                {
                    MessageBox.Show("Пароль должен содержать не менее 8 букв/цифр!", "Ошибка", MessageBoxButton.OK);
                    return;
                }
                if (db.teachers.Any(u => u.login == login))
                {
                    MessageBox.Show("Такой пользователь уже есть!", "Ошибка", MessageBoxButton.OK);
                    return;
                }
                if (db.students.Any(u => u.login == login))
                {
                    MessageBox.Show("Такой пользователь уже есть!", "Ошибка", MessageBoxButton.OK);
                    return;
                }


                if (Role_Box.SelectedItem == null)
                {
                    MessageBox.Show("Роль не выбрана!", "Ошибка", MessageBoxButton.OK);
                    return;
                }

                if (Group_Box.SelectedItem == null)
                {
                    MessageBox.Show("Группа не выбрана!", "Ошибка", MessageBoxButton.OK);
                    return;
                }




                if (Role_Box.SelectedItem != null)
                {
                    if (selectedRole.role_id == 1 || selectedRole.role_name == "Преподаватель")
                    {
                        var teacher = new DB.teachers
                        {
                            login = login,
                            password = password,
                            role_id = role,
                            name = name,
                            surname = surname,
                            patronymic = patronymic,

                        };
                        db.teachers.Add(teacher);
                        db.SaveChanges();
                    }
                    else if (selectedRole.role_id == 2 ||  selectedRole.role_name == "Студент")
                    {
                        var student = new DB.students
                        {
                            login = login,
                            password = password,
                            role_id = role,
                            name = name,
                            surname = surname,
                            patronymic = patronymic,
                            group_number = group,
                            student_card = student_number
                        };
                        db.students.Add(student);
                        db.SaveChanges();
                    }
                }

                MessageBox.Show("Вы зарегистрировались!", login, MessageBoxButton.OK);
                RegFrame.Navigate(new Autorization());
                //MessageBox.Show($"{login}\n{password}\n{name}\n{surname}\n{patronymic}\n{role}\n{group}\n{student_number}");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        private void Role_Box_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedRole = Role_Box.SelectedItem as DB.roles;
            if (selectedRole != null)
            {
                // Проверяем по ID или имени
                if (selectedRole.role_id == 1 || selectedRole.role_name == "Преподаватель")
                {
                    StudentCard_Box.IsEnabled = false;
                }
                else
                {
                    StudentCard_Box.IsEnabled = true;
                    //if (!int.TryParse(StudentCard_Box.Text, out student_number))
                    //{
                    //    MessageBox.Show("Студ.билет не содержит букв!", "Ошибка", MessageBoxButton.OK);
                    //    return;
                    //}
                }
            }
        }
    }
}
