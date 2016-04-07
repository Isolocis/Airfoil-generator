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

namespace AirfoilGeneratorGUI
{
    /// <summary>
    /// Interaction logic for Shell.xaml
    /// </summary>
    public partial class Shell
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="Shell"/> class.
        /// </summary>
        public Shell()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// The region names of the <see cref="Shell"/>
        /// </summary>
        public class RegionNames
        {
            /// <summary>
            /// The name of the main region.
            /// </summary>
            public const string MainRegion = "MainRegion";
        }
    }
}
