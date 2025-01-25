using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
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
using yp_gorlanov.DB;

namespace yp_gorlanov
{
    /// <summary>
    /// Логика взаимодействия для TeachersPage.xaml
    /// </summary>
    public partial class TeachersPage : Page
    {
        int CurrentUserID;

        private Entities db = new Entities();

        public TeachersPage(int currentID)
        {
            InitializeComponent();
            CurrentUserID = currentID;

            var userr = db.users.FirstOrDefault(u => u.user_id == CurrentUserID);

            profile_box.Text = userr.surname + " " + userr.name + " " + userr.patronymic;

            
        }

        private void CreateTest_Btn_Click(object sender, RoutedEventArgs e)
        {
            string TypeOfTest = TypeOfTest_TextBox.Text;

            var test = new DB.test
            {
                test_name = TypeOfTest,
            };

            db.test.Add(test);
            db.SaveChanges();
            SP_TypeOfTest.Visibility = Visibility.Collapsed;
            SP_Test.Visibility = Visibility.Visible;
        }

        private void LoadDataGridData()
        {
            StudentsInfo_DG.ItemsSource = db.users.ToList();
        }

        private void CreateTestPage_Btn_Click(object sender, RoutedEventArgs e)
        {
            SP_TypeOfTest.Visibility = Visibility.Visible;
        }

        private void StudentsInfoPage_Btn_Click(object sender, RoutedEventArgs e)
        {
            LoadDataGridData();
            SP_StudentsInfo.Visibility = Visibility.Visible;
            
        }

        private void UpdateSIDG_Btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (StudentsInfo_DG.SelectedItems.Count > 0)
                {
                    for (int i = 0; i < StudentsInfo_DG.SelectedItems.Count; i++)
                    {
                        DB.users user = StudentsInfo_DG.SelectedItems[i] as DB.users;
                        if (user != null)
                        {
                            db.users.AddOrUpdate(user);

                        }
                    }
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            

        }

        private void DeleteSIDG_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (StudentsInfo_DG.SelectedItems.Count>0)
            {
                for (int i = 0; i < StudentsInfo_DG.SelectedItems.Count; i++)
                {
                    DB.users user = StudentsInfo_DG.SelectedItems[i] as DB.users;
                    if (user != null)
                    {
                        db.users.Remove(user);
                    }
                }
                db.SaveChanges();
            }

        }
    }
}
