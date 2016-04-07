using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace AirfoilGeneratorGUI.Views.Export
{
    /// <summary>
    /// Interaction logic for ExportView.xaml
    /// </summary>
    public partial class ExportView
    {
        public ExportView()
        {
            this.InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).DataContext == null)
                return;

            var viewModel = ((Button)sender).DataContext as ExportViewModel;
            var dialog = new SaveFileDialog
            {
                FileName = viewModel?.OutputPath,
                DefaultExt = ".dat",
                Filter = "Data files|*.dat"
            };

            if (dialog.ShowDialog() == true)
            {
                if(viewModel != null)
                    viewModel.OutputPath = dialog.FileName;
            }
        }
    }
}
