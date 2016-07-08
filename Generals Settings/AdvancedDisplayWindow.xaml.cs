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

namespace Generals_Settings
{
    /// <summary>
    /// Interaction logic for AdvancedDisplayWindow.xaml
    /// </summary>
    public partial class AdvancedDisplayWindow : Window
    {
        public AdvancedDisplayWindow()
        {
            InitializeComponent();
        }

        public string Version { get; internal set; }

        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
