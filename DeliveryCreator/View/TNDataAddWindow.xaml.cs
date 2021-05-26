using DeliveryCreator.Data.Repository.Abstract;
using DeliveryCreator.View.ViewModel;
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
using System.Windows.Shapes;

namespace DeliveryCreator.View
{
    /// <summary>
    /// Interaction logic for TNDataAddWindow.xaml
    /// </summary>
    public partial class TNDataAddWindow : Window
    {
        private DatabaseUnit db;
        private int entityId;
        public TNDataAddWindow(DatabaseUnit db, int entityId = -1)
        {
            this.db = db;
            this.entityId = entityId;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = new TNDataAddViewModel(db, entityId);
        }

        private void btn_ok_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as TNDataAddViewModel).Save();
            Close();
        }

        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }        
    }
}
