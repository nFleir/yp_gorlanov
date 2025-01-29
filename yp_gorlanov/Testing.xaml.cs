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
using yp_gorlanov.DB;

namespace yp_gorlanov
{
    /// <summary>
    /// Логика взаимодействия для testing.xaml
    /// </summary>
    public partial class Testing : Page
    {
        private Entities db = new Entities();

        int CurrentUserID; //текущий пользователь

        int CurrentTestID; //текущий тест

        int CurrentQuestionID; //текущий вопрос

        double RightAnswer; //подсчет правильных ответов

        int StudentMark;

        List<DB.questions> CurrentQuestions;


        public Testing(int CurrentID)
        {
            InitializeComponent();
            CurrentUserID = CurrentID;

            var userr = db.students.FirstOrDefault(u => u.student_id == CurrentUserID);

            LoadTests();

            
        }

        public void LoadTests()
        {
            var tests = db.test.ToList();
            TypeOfTest_Box.ItemsSource = tests;
            TypeOfTest_Box.DisplayMemberPath = "test_name";
            TypeOfTest_Box.SelectedValuePath = "test_id";
        }

        private void StartTest_Btn_Click(object sender, RoutedEventArgs e)
        {
            var selected_answer = GetSelectedAnswer();

            if (selected_answer != null && (bool)selected_answer.Tag)
            {
                RightAnswer++;
            }

            CurrentTestID = (int)TypeOfTest_Box.SelectedValue;
            SP_Start.Visibility = Visibility.Collapsed;
            SP_Test.Visibility = Visibility.Visible;

            var test = db.test.FirstOrDefault(u => u.test_id == CurrentTestID);

            if (test != null)
            {
                CurrentQuestions = test.questions.ToList();
                if (CurrentQuestions.Count > 0)
                {
                    CurrentQuestionID = 0;
                    ShowQuestion(CurrentQuestions[CurrentQuestionID]);
                }
            }
        }

        private void NextQuestion_Btn_Click(object sender, RoutedEventArgs e)
        {
            

            var selected_answer = GetSelectedAnswer();

            if (selected_answer != null && (bool)selected_answer.Tag) 
            {
                RightAnswer++;
            }


            Answer1_Btn.IsChecked = false;
            Answer2_Btn.IsChecked = false;
            Answer3_Btn.IsChecked = false;
            Answer4_Btn.IsChecked = false;

            StudentMark = GetStudentMark(RightAnswer, (double)CurrentQuestions.Count);

            if (CurrentQuestions != null && CurrentQuestionID < CurrentQuestions.Count - 1)
            {
                CurrentQuestionID++;
                ShowQuestion(CurrentQuestions[CurrentQuestionID]);
            }
            else
            {
                
                MessageBoxResult result = MessageBox.Show("Вопросы кончились, завершить тест?", "Тест", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    SP_Test.Visibility = Visibility.Collapsed;
                    MessageBox.Show($"Оценка - {StudentMark}");
                    RecordMarkToDB(StudentMark);
                }
            }

        }



        private void StopTest_Btn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы хотите завершить тест досрочно?", "Тест", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                SP_Test.Visibility = Visibility.Collapsed;
                StudentMark = GetStudentMark(RightAnswer, (double)CurrentQuestions.Count);
                MessageBox.Show($"Оценка - {StudentMark}");
                RecordMarkToDB(StudentMark);
            }
            
        }

        private void ShowQuestion(DB.questions question)
        {
            Question1_Box.Text = question.question_text;
            var answers = question.answers.ToList();
            if (answers.Count > 0)
            {
                Answer1_Btn.Content = answers[0].answer_text;
                Answer2_Btn.Content = answers[1].answer_text;
                Answer3_Btn.Content = answers[2].answer_text;
                Answer4_Btn.Content = answers[3].answer_text;

                Answer1_Btn.Tag = answers[0].is_correct;
                Answer2_Btn.Tag = answers[1].is_correct;
                Answer3_Btn.Tag = answers[2].is_correct;
                Answer4_Btn.Tag = answers[3].is_correct;
            }

            //if (Answer1_Btn.IsChecked == true && answers[0].is_correct == true) StudentMark++;
            //else if (Answer2_Btn.IsChecked == true && answers[1].is_correct == true) StudentMark++;
            //else if (Answer3_Btn.IsChecked == true && answers[2].is_correct == true) StudentMark++;
            //else if (Answer4_Btn.IsChecked == true && answers[3].is_correct == true) StudentMark++;
        }

        private void Answer1_Btn_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;

            if (button?.Tag is bool isCorrect && isCorrect) RightAnswer++;

        }


        private RadioButton GetSelectedAnswer()
        {
            if (Answer1_Btn.IsChecked == true) return Answer1_Btn;
            if (Answer2_Btn.IsChecked == true) return Answer2_Btn;
            if (Answer3_Btn.IsChecked == true) return Answer3_Btn;
            if (Answer4_Btn.IsChecked == true) return Answer4_Btn;
            return null;
        }

        private int GetStudentMark(double QuestionCorrect, double QuestionCount)
        {
            double percent = QuestionCorrect / QuestionCount;

            if (percent > 0 && percent < 0.5)
            {
                return 2;
            }
            if (percent >= 0.5 &&  percent < 0.7)
            {
                return 3;
            }
            if (percent >= 0.7 && percent < 0.9)
            {
                return 4;
            }
            else
            {
                return 5;
            }
        }

        private void RecordMarkToDB(int mark)
        {
            var student_mark = new DB.marks
            {
                test_id = CurrentTestID,
                student_id = CurrentUserID,
                mark = mark
            };

            db.marks.Add(student_mark);
            db.SaveChanges();

        }
    }
}
