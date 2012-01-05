using System;
using System.Windows;
using Hashzone.ViewModels;

namespace Hashzone.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // Poor man's dependency injection
            DataContext = new MainWindowViewModel("Drag 'n' Drop file into the hash zone.", true);
        }

        private void FileDropEventHandler(object sender, DragEventArgs e)
        {
            MainWindowViewModel vm = DataContext as MainWindowViewModel;
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                vm.Status = "The drop does not associate with any file types.";
                return;
            }

            vm.HandleFileDrop((string[])e.Data.GetData(DataFormats.FileDrop));
        }
    }
}
