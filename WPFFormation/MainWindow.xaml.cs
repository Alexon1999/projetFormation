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


namespace WPFFormation
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        formationEntities gst;

        private void btnInscription_Click(object sender, RoutedEventArgs e)
        {
            FrmInscription fentre = new FrmInscription(gst);
            fentre.Show();
        }

        private void btnPresence_Click(object sender, RoutedEventArgs e)
        {
            FrmPresence fentre = new FrmPresence(gst);
            fentre.Show();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            gst = new formationEntities();
        }
    }
}
