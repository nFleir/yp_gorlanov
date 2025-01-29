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

        int CurrentTestID;



        private Entities db = new Entities();

        public TeachersPage(int currentID)
        {
            InitializeComponent();
            CurrentUserID = currentID;

            var userr = db.teachers.FirstOrDefault(u => u.teacher_id == CurrentUserID);

            profile_box.Text = userr.surname + " " + userr.name + " " + userr.patronymic;

            LoadStudents();
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

            CurrentTestID = test.test_id;

            SP_TypeOfTest.Visibility = Visibility.Collapsed;
            SP_Test.Visibility = Visibility.Visible;
        }

        private void LoadDataGridData(int StudentId)
        {
            var student = db.students.Where(u => u.student_id == CurrentUserID).ToList();
            StudentsInfo_DG.ItemsSource = student;
        }

        private void LoadMarksDataGridData()
        {
            var marks = db.marks.ToList();
            MarksInfo_DG.ItemsSource = marks;
        }

        private void CreateTestPage_Btn_Click(object sender, RoutedEventArgs e)
        {
            SP_StudentsInfo.Visibility = Visibility.Collapsed;
            SP_Test.Visibility = Visibility.Collapsed;
            SP_Reports.Visibility = Visibility.Collapsed;
            SP_TypeOfTest.Visibility = Visibility.Visible;
        }

        private void LoadStudents()
        {
            var students = db.students.ToList();
            ChooseStudent_CBox.ItemsSource = students;
            ChooseStudent_CBox.DisplayMemberPath = "surname";
            ChooseStudent_CBox.SelectedValuePath = "student_id";
        }

        private void StudentsInfoPage_Btn_Click(object sender, RoutedEventArgs e)
        {
            SP_Test.Visibility = Visibility.Collapsed;
            SP_Reports.Visibility = Visibility.Collapsed;
            SP_TypeOfTest.Visibility = Visibility.Collapsed;
            SP_StudentsInfo.Visibility = Visibility.Visible;
            

        }

        private void ChooseStudent_CBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ChooseStudent_CBox.SelectedValue != null && int.TryParse(ChooseStudent_CBox.SelectedValue.ToString(), out int selectedId))
            {
                LoadDataGridData(selectedId);
            }
            else
            {
                StudentsInfo_DG.ItemsSource = null; // Очистка при отсутствии выбора
            }
        }

        private void UpdateSIDG_Btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (StudentsInfo_DG.SelectedItems.Count > 0)
                {
                    for (int i = 0; i < StudentsInfo_DG.SelectedItems.Count; i++)
                    {
                        DB.students student = StudentsInfo_DG.SelectedItems[i] as DB.students;
                        if (student != null)
                        {
                            db.students.AddOrUpdate(student);

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
                    DB.students student = StudentsInfo_DG.SelectedItems[i] as DB.students;
                    if (student != null)
                    {
                        db.students.Remove(student);
                    }
                }
                db.SaveChanges();
            }

        }

        private void AllTests_Btn_Click(object sender, RoutedEventArgs e)
        {
            SP_StudentsInfo.Visibility = Visibility.Collapsed;
            SP_Test.Visibility = Visibility.Collapsed;
            SP_TypeOfTest.Visibility = Visibility.Collapsed;
        }

        private void Reports_Btn_Click(object sender, RoutedEventArgs e)
        {
            SP_StudentsInfo.Visibility = Visibility.Collapsed;
            SP_Test.Visibility = Visibility.Collapsed;
            SP_TypeOfTest.Visibility = Visibility.Collapsed;
            SP_Reports.Visibility = Visibility.Visible;
            LoadMarksDataGridData();
        }

        private void AddOneMoreQuestion_Btn_Click(object sender, RoutedEventArgs e)
        {
            string question_text = Question1_Box.Text;
            string answer1 = Answer1_Box.Text;
            string answer2 = Answer2_Box.Text;
            string answer3 = Answer3_Box.Text;
            string answer4 = Answer4_Box.Text;
            string right_answer = RightAnswer_Box.Text;


            var question = new DB.questions
            {
                question_text = question_text,
                test_id = CurrentTestID
            };

            db.questions.Add(question);
            db.SaveChanges();

            var answers = new List<DB.answers>
            {
                new DB.answers { question_id = question.question_id, answer_text = answer1, is_correct = (answer1 == right_answer) },
                new DB.answers { question_id = question.question_id, answer_text = answer2, is_correct = (answer2 == right_answer) },
                new DB.answers { question_id = question.question_id, answer_text = answer3, is_correct = (answer3 == right_answer) },
                new DB.answers { question_id = question.question_id, answer_text = answer4, is_correct = (answer4 == right_answer) }
            };

            db.answers.AddRange(answers);
            db.SaveChanges();

            Question1_Box.Clear();

            Answer1_Box.Clear();
            Answer2_Box.Clear();
            Answer3_Box.Clear();
            Answer4_Box.Clear();
            RightAnswer_Box.Clear();

        }

        private void SaveQuestions_Btn_Click(object sender, RoutedEventArgs e)
        {
            string question_text = Question1_Box.Text;
            string answer1 = Answer1_Box.Text;
            string answer2 = Answer2_Box.Text;
            string answer3 = Answer3_Box.Text;
            string answer4 = Answer4_Box.Text;
            string right_answer = RightAnswer_Box.Text;

            

            var question = new DB.questions
            {
                question_text = question_text,
                test_id = CurrentTestID
            };

            db.questions.Add(question);
            db.SaveChanges();

            var answers = new List<DB.answers>
            {
                new DB.answers { question_id = question.question_id, answer_text = answer1, is_correct = (answer1 == right_answer) },
                new DB.answers { question_id = question.question_id, answer_text = answer2, is_correct = (answer2 == right_answer) },
                new DB.answers { question_id = question.question_id, answer_text = answer3, is_correct = (answer3 == right_answer) },
                new DB.answers { question_id = question.question_id, answer_text = answer4, is_correct = (answer4 == right_answer) }
            };

            db.answers.AddRange(answers);
            db.SaveChanges();

            MessageBox.Show("Тест успешно сохранен", "Создание теста", MessageBoxButton.OK);
            SP_Test.Visibility = Visibility.Collapsed;
        }

        private void UpdateRDG_Btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MarksInfo_DG.SelectedItems.Count > 0)
                {
                    for (int i = 0; i < MarksInfo_DG.SelectedItems.Count; i++)
                    {
                        DB.marks mark = MarksInfo_DG.SelectedItems[i] as DB.marks;
                        if (mark != null)
                        {
                            db.marks.AddOrUpdate(mark);

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

        private void DeleteRDG_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (MarksInfo_DG.SelectedItems.Count > 0)
            {
                for (int i = 0; i < MarksInfo_DG.SelectedItems.Count; i++)
                {
                    DB.marks mark = MarksInfo_DG.SelectedItems[i] as DB.marks;
                    if (mark != null)
                    {
                        db.marks.Remove(mark);
                    }
                }
                db.SaveChanges();
            }
        }
    }
}
