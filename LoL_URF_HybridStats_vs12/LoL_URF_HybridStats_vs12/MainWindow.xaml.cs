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
using LoL_URF_HybridStats_vs12.Model;
using LoL_URF_HybridStats_vs12.UI_Controls;
using RiotSharp;
using RiotSharp.ChampionEndpoint;
using RiotSharp.MatchEndpoint;
using RiotSharp.StaticDataEndpoint;

namespace LoL_URF_HybridStats_vs12
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string apiKey = "";

        private ChampionsProcessor processor;

        public MainWindow()
        {
            InitializeComponent();

      /*      long x = 9223372036854775807;
            long y = 100000;
            long res = x / y;*/

            PageSwitcher.pageSwitcher = this;
            PageSwitcher.SwitchToMainPage();
            /*
            ChampionsProcessor processor = new ChampionsProcessor();
            processor.UpdateChampionsList();
            SaveData(processor);
            PopulateCarouselControl(processor);*/

            
            
        }

        public void Navigate(UserControl nextPage)
        {
            this.Content = nextPage;
        }  

        /*
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                SelectPrevious();
            }
            else if (e.Key == Key.Right)
            {
                SelectNext();
            }
        }
        */
      


    }
}
