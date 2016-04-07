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
using Microsoft.Win32;

namespace AirfoilGeneratorGUI.Views.Export
{
    /// <summary>
    /// Interaction logic for ExportView.xaml
    /// </summary>
    public partial class ExportView : UserControl
    {
        public ExportView()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).DataContext == null)
                return;

            var viewModel = ((Button)sender).DataContext as ExportViewModel;
            var dialog = new SaveFileDialog
            {
                FileName = viewModel.OutputPath,
                DefaultExt = ".dat",
                Filter = "Data files|*.dat"
            };

            if (dialog.ShowDialog() == true)
            {
                viewModel.OutputPath = dialog.FileName;
            }
        }
    }
}
