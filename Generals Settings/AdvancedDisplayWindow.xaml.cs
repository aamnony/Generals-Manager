using System.Windows;

namespace Generals_Manager
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