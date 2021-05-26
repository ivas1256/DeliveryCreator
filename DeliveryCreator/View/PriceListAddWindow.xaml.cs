using DeliveryCreator.Data.Model;
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
    /// Interaction logic for PriceListAddWindow.xaml
    /// </summary>
    public partial class PriceListAddWindow : Window
    {
        private DatabaseUnit db;
        private int entityId;
        public PriceListAddWindow(DatabaseUnit db, int entityId = -1)
        {
            this.db = db;
            this.entityId = entityId;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = new PriceListAddViewModel(db, entityId);
        }

        private void btn_ok_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as PriceListAddViewModel).Save();
            Close();
        }

        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }        
    }
}
