using DeliveryCreator.Data.Model;
using DeliveryCreator.Data.Repository.Abstract;
using DeliveryCreator.Logic;
using DeliveryCreator.View;
using DeliveryCreator.View.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DeliveryCreator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private DatabaseUnit db;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            db = new DatabaseUnit();
            DataContext = new SettingsViewModel(db);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            (DataContext as SettingsViewModel).Save();
        }

        private void btnPriceList_click(object sender, RoutedEventArgs e)
        {
            var w = new PriceListWindow(db);
            w.ShowDialog();
        }

        private void btnAddPriceList_click(object sender, RoutedEventArgs e)
        {
            var w = new PriceListAddWindow(db);
            w.ShowDialog();
        }

        private void btnPatterns_click(object sender, RoutedEventArgs e)
        {
            var w = new TNDataListWindow(db);
            w.ShowDialog();
        }

        private void btnAddPattern_click(object sender, RoutedEventArgs e)
        {
            var w = new TNDataAddWindow(db);
            w.ShowDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as SettingsViewModel).Save();

            var s = (DataContext as SettingsViewModel)?.Settings;
            if (s == null)
                return;

            string folderName;
            using (var dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    return;

                folderName = dialog.SelectedPath;
            }

            var bgw = new BackgroundWorker();            
            bgw.DoWork += (object a, DoWorkEventArgs b) =>
            {
                var builder = new MainBuilder(s, folderName);
                builder.UpdateStatusEvent += B_UpdateStatusEvent;
                builder.Build();                
            };
            bgw.RunWorkerAsync();
        }
        
        private void B_UpdateStatusEvent(string message)
        {
            Dispatcher.Invoke(() => tb_status.Text = message);
        }

        private void btn_browseFile_click(object sender, RoutedEventArgs e)
        {
            var dc = (DataContext as SettingsViewModel);
            if (dc == null)
                return;

            var ofd = new OpenFileDialog();
            if (ofd.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;

            dc.RecipientsFileName = ofd.FileName;
        }
    }
}
