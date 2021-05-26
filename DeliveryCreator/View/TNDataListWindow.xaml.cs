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
    /// Interaction logic for TNDataListWindow.xaml
    /// </summary>
    public partial class TNDataListWindow : Window
    {
        private DatabaseUnit db;        
        public TNDataListWindow(DatabaseUnit db)
        {
            this.db = db;            
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = new TNDataListViewModel(db);
        }

        private void btn_delete_Click(object sender, RoutedEventArgs e)
        {
            var obj = (regions_table.SelectedItem as TNData);
            if (obj == null)
                return;

            if (!MessagesProvider.Confirm($"Действительно удалить запись \"{obj.sender_full_name}\"?"))
                return;

            (DataContext as TNDataListViewModel).Remove(obj);
            (DataContext as TNDataListViewModel).Update();
        }

        private void btn_edit_Click(object sender, RoutedEventArgs e)
        {
            var id = (regions_table.SelectedItem as TNData)?.id ?? -1;
            if (id < 0)
                return;

            var w = new TNDataAddWindow(db, id);
            w.ShowDialog();
            (DataContext as TNDataListViewModel).Update();
        }

        private void tb_search_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            var w = new TNDataAddWindow(db);
            w.ShowDialog();
            (DataContext as TNDataListViewModel).Update();
        }        
    }
}
