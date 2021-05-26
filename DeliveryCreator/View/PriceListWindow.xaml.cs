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
    /// Interaction logic for PriceListWindow.xaml
    /// </summary>
    public partial class PriceListWindow : Window
    {
        private DatabaseUnit db;
        public PriceListWindow(DatabaseUnit db)
        {
            this.db = db;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = new PriceListViewModel(db);
        }

        private void tb_searchRegion_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btn_editRegion_Click(object sender, RoutedEventArgs e)
        {
            var id = (regions_table.SelectedItem as PriceList)?.id ?? -1;
            if (id < 0)
                return;

            var w = new PriceListAddWindow(db, id);
            w.ShowDialog();
            (DataContext as PriceListViewModel).Update();
        }

        private void btn_deleteRegion_Click(object sender, RoutedEventArgs e)
        {
            var obj = (regions_table.SelectedItem as PriceList);
            if (obj == null)
                return;

            if (!MessagesProvider.Confirm($"Действительно удалить запись \"{obj.region_name}\"?"))
                return;

            (DataContext as PriceListViewModel).Remove(obj);
            (DataContext as PriceListViewModel).Update();
        }

        private void btn_addRegion_Click(object sender, RoutedEventArgs e)
        {
            var w = new PriceListAddWindow(db);
            w.ShowDialog();
            (DataContext as PriceListViewModel).Update();
        }

    }
}
