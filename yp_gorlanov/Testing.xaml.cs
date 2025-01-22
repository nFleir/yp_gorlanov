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

        int CurrentUserID;

        private Entities db = new Entities();

        public Testing(int CurrentID)
        {
            InitializeComponent();
            CurrentUserID = CurrentID;

            var userr = db.users.FirstOrDefault(u => u.user_id == CurrentUserID);


        }

        
    }
}
