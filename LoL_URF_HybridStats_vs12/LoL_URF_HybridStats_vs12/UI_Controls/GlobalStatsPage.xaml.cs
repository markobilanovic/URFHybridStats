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
using LoL_URF_HybridStats_vs12.Helper;

namespace LoL_URF_HybridStats_vs12.UI_Controls
{
    /// <summary>
    /// Interaction logic for GlobalStatsPage.xaml
    /// </summary>
    public partial class GlobalStatsPage : UserControl
    {
        public GlobalStatsPage()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            PageSwitcher.SwitchToMainPage();
        }

        private void borderGalio_MouseEnter(object sender, MouseEventArgs e)
        {
            imgGalio.Visibility = Visibility.Visible;
            pathGalio.Visibility = Visibility.Hidden;
            msgGalio.Visibility = Visibility.Visible;
        }

        private void borderGalio_MouseLeave(object sender, MouseEventArgs e)
        {
            imgGalio.Visibility = Visibility.Hidden;
            pathGalio.Visibility = Visibility.Visible;
            msgGalio.Visibility = Visibility.Hidden;
        }

        private void evePath_MouseEnter(object sender, MouseEventArgs e)
        {
            imgEve.Visibility = Visibility.Visible;
            pathEve.Visibility = Visibility.Hidden;
            msgEve.Visibility = Visibility.Visible;
        }

        private void evePath_MouseLeave(object sender, MouseEventArgs e)
        {
            imgEve.Visibility = Visibility.Hidden;
            pathEve.Visibility = Visibility.Visible;
            msgEve.Visibility = Visibility.Hidden;
        }

        private void pathEzrael_MouseEnter(object sender, MouseEventArgs e)
        {
            imgEz.Visibility = Visibility.Visible;
            pathEzrael.Visibility = Visibility.Hidden;
            msgEz.Visibility = Visibility.Visible;
        }

        private void imgEz_MouseLeave(object sender, MouseEventArgs e)
        {
            imgEz.Visibility = Visibility.Hidden;
            pathEzrael.Visibility = Visibility.Visible;
            msgEz.Visibility = Visibility.Hidden;
        }

        private void pathKartus_MouseEnter(object sender, MouseEventArgs e)
        {
            imgKhartus.Visibility = Visibility.Visible;
            pathKartus.Visibility = Visibility.Hidden;
            msgKartus.Visibility = Visibility.Visible;
        }

        private void imgKhartus_MouseLeave(object sender, MouseEventArgs e)
        {
            imgKhartus.Visibility = Visibility.Hidden;
            pathKartus.Visibility = Visibility.Visible;
            msgKartus.Visibility = Visibility.Hidden;
        }

        private void pathShaco2_MouseEnter(object sender, MouseEventArgs e)
        {
            imgShaco1.Visibility = Visibility.Visible;
            imgShaco2.Visibility = Visibility.Hidden;
        }

        private void pathShaco1_MouseEnter(object sender, MouseEventArgs e)
        {
            imgShaco1.Visibility = Visibility.Hidden;
            imgShaco2.Visibility = Visibility.Visible;
        }

        private void imgMundo_MouseLeave(object sender, MouseEventArgs e)
        {
            imgMundo.Visibility = Visibility.Hidden;
            pathMundo.Visibility = Visibility.Visible;
            msgMundo.Visibility = Visibility.Hidden;
        }

        private void pathMundo_MouseEnter(object sender, MouseEventArgs e)
        {
            imgMundo.Visibility = Visibility.Visible;
            pathMundo.Visibility = Visibility.Hidden;
            msgMundo.Visibility = Visibility.Visible;
        }


        private void pathSona_MouseEnter(object sender, MouseEventArgs e)
        {
            imgSona.Visibility = Visibility.Visible;
            pathSona.Visibility = Visibility.Hidden;
            msgSona.Visibility = Visibility.Visible;
        }

        private void imgSona_MouseLeave(object sender, MouseEventArgs e)
        {
            imgSona.Visibility = Visibility.Hidden;
            pathSona.Visibility = Visibility.Visible;
            msgSona.Visibility = Visibility.Hidden;
        }








    }
}
