using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LoL_URF_HybridStats_vs12.UI_Controls
{
    /// <summary>
    /// Interaction logic for ChampCard.xaml
    /// </summary>
    public partial class ChampCard : UserControl
    {
        public long id;
        private bool selected;

        public ChampCard(ChampionStats championInfo)
        {
            InitializeComponent();
            lblChampName.Content = championInfo.Name;
            id = championInfo.Id;
            imgChampImg.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "\\" + championInfo.ImgCardLocation));
            selected = false;
        }

        private void ChampionCardUserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!selected)
            {
                ZoomInCard();
            }
        }

        private void ChampionCardUserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!selected)
            {
                ZoomOutCard();
            }
        }

        private void ZoomInCard()
        {
            Storyboard sb = this.FindResource("MouseOver") as Storyboard;
            Storyboard.SetTarget(sb, this.LayoutRoot);
            sb.Begin();
        }

        private void ZoomOutCard()
        {
            Storyboard sb = this.FindResource("MouseLeave") as Storyboard;
            Storyboard.SetTarget(sb, this.LayoutRoot);
            sb.Begin();
        }

        public void SelectCard()
        {
            selected = true;
            ZoomInCard();
        }

        public void UnselectCard()
        {
            selected = false;
            ZoomOutCard();
        }
    }
}
