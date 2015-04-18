using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
using LoL_URF_HybridStats_vs12.Helper;
using LoL_URF_HybridStats_vs12.Model;
using LoL_URF_HybridStats_vs12.ViewModels;
using RiotSharp;

namespace LoL_URF_HybridStats_vs12.UI_Controls
{
    /// <summary>
    /// Interaction logic for MainWindowPage.xaml
    /// </summary>
    public partial class MainWindowPage : UserControl
    {
        private string apiKey = "";
        private string apiKey2 = "";
        

        private ChampionsProcessor processor;

        private static MainWindowPage instance;

        public static MainWindowPage Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MainWindowPage();
                }
                return instance;
            }
        }

        private MainWindowPageViewModel viewModel;
        private MainWindowPage()
        {
            InitializeComponent();
            viewModel = new MainWindowPageViewModel();
            this.DataContext = viewModel;


            processor = Serializator.LoadData("allFinal" + ".dat");
            PopulateCarouselControl(processor);

            



            

            //Serializator.SaveData(processor, "allFinal" + ".dat");

            /*
            double OldMax = 39.5;
            double OldMin = 0.91;
            double OldRange = OldMax - OldMin;
            double OldValue = 39;

            double NewMax = 100;
            double NewMin = 0;
            double NewRange = NewMax - NewMin;
            double NewValue;

            OldRange = (OldMax - OldMin);
            if (OldRange == 0)
                NewValue = NewMin;
            else
            {
                NewRange = (NewMax - NewMin);
                NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;
            }

            */

            /*
            processor = new ChampionsProcessor();
            processor.UpdateChampionsList();


            ChampionsProcessor processorBr = Serializator.LoadData("br3" + ".dat");
            ChampionsProcessor processorOce = Serializator.LoadData("oce3" + ".dat");
            ChampionsProcessor processorKr = Serializator.LoadData("kr3" + ".dat");
            ChampionsProcessor processorLan = Serializator.LoadData("lan3" + ".dat");
            ChampionsProcessor processorLas = Serializator.LoadData("las3" + ".dat");
            ChampionsProcessor processorTr = Serializator.LoadData("tr3" + ".dat");
            ChampionsProcessor processorEuw = Serializator.LoadData("euw3" + ".dat");
            ChampionsProcessor processorRu = Serializator.LoadData("ru3" + ".dat");
            ChampionsProcessor processorEune = Serializator.LoadData("eune3" + ".dat");
            ChampionsProcessor processorNa = Serializator.LoadData("na3" + ".dat");
            List<ChampionsProcessor> processors = new List<ChampionsProcessor>();
            processors.Add(processorBr);
            processors.Add(processorOce);
            processors.Add(processorKr);
            processors.Add(processorLan);
            processors.Add(processorLas);
            processors.Add(processorTr);
            processors.Add(processorEuw);
            processors.Add(processorRu);
            processors.Add(processorEune);
            processors.Add(processorNa);

            processor.AddProcessorsData(processors);
            */


            /*  List<ChampionsProcessor> processors = new List<ChampionsProcessor>();
              ChampionsProcessor processorNa = Serializator.LoadData("na" + ".dat");
              ChampionsProcessor processorEune = Serializator.LoadData("eune" + ".dat");
              processors.Add(processorNa);
              processors.Add(processorEune);
              processor.AddProcessorsData(processors);*/
          

           //  ReadMatchesFromJsonFiles();
         //    Serializator.SaveData(processor, "allData2" + ".dat");

         //   processor = Serializator.LoadData("allData2" + ".dat");
         //   PopulateCarouselControl(processor);

          /*  string s = "ru";
            ChampionsProcessor processorBr = Serializator.LoadData(s + "2.dat");
            Serializator.SaveData(processorBr, s + "3.dat");*/

        //    processorBr.TransferInfoToStats();
        //    Serializator.SaveData(processorBr, "tr2" + ".dat");

           //  viewModel.SelectedControl = new ChampInfoOnMainPage(processor);
          //   List<ChampionInfo> tmp = processor.GetSortedListByNoOfPentaKills();
        }

        private void ReadMatchesFromJsonFiles()
        {
            string region = "br";
            var api = RiotApi.GetInstance(apiKey2);
            string line;
            System.IO.StreamReader file = new System.IO.StreamReader(string.Format(@"D:\LoL_Challange\Data\COPY\{0}.txt", region));
            while ((line = file.ReadLine()) != null)
            {
                try
                {
                    long gameIdLong = Convert.ToInt64(line);
                    processor.AddGameAndCalculate(api.GetMatch(Region.br, gameIdLong, true));
                    Console.WriteLine(string.Format("Game with id {0} processed", line));
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error with id: " + line);
                    using (System.IO.StreamWriter fileError = new System.IO.StreamWriter(string.Format(@"D:\LoL_Challange\Data\COPY\{0}_errors.txt", region), true))
                    {
                        fileError.WriteLine(line);
                    }
                }
            }

            file.Close();
            return;
        }

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
            else if (e.Key == Key.Enter)
            {
                SeeSelectedChampion();
            }
            else
            {
                tbSearch_PreviewKeyDown(null, e);
            }
            e.Handled = true;   //prevents from losing focus
        }

        private void PopulateCarouselControl(ChampionsProcessor processor)
        {
            //sort
            List<KeyValuePair<int, ChampionStats>> sortedChamps = processor.Champs.ToList();
            sortedChamps.Sort((firstPair, nextPair) =>
            {
                return firstPair.Value.Name.CompareTo(nextPair.Value.Name);
            }
            );

            foreach (KeyValuePair<int, ChampionStats> kvp in sortedChamps)
            {
                ChampCard uiCard = new ChampCard(kvp.Value);
                uiCard.Width = 190;
                uiCard.Height = 360;
                carouselControl.Children.Add(uiCard);
                Canvas.SetLeft(uiCard, 340);
            }
            carouselControl.ReInitialize();
        }

        private void btnSelectPrevious_Click(object sender, RoutedEventArgs e)
        {
            SelectPrevious();
        }

        private void btnSelectNext_Click(object sender, RoutedEventArgs e)
        {
            SelectNext();
        }

        private void SelectNext()
        {
            int maxElement = carouselControl.Children.Count - 1;
            for (int i = 0; i < carouselControl.Children.Count; i++)
            {
                if (carouselControl.Children[i].Equals(carouselControl.CurrentlySelected))
                {
                    if (i != maxElement)
                        carouselControl.SelectElement((FrameworkElement)carouselControl.Children[i + 1]);
                    else
                        carouselControl.SelectElement((FrameworkElement)carouselControl.Children[0]);
                    break;
                }
            }
        }

        private void SelectPrevious()
        {
            for (int i = 0; i < carouselControl.Children.Count; i++)
            {
                if (carouselControl.Children[i].Equals(carouselControl.CurrentlySelected))
                {
                    if (i != 0)
                        carouselControl.SelectElement((FrameworkElement)carouselControl.Children[i - 1]);
                    else
                        carouselControl.SelectElement((FrameworkElement)carouselControl.Children[carouselControl.Children.Count - 1]);
                    break;
                }
            }
        }

        private void carouselControl_OnElementSelected(object sender)
        {
             ChampCard selected = carouselControl.CurrentlySelected as ChampCard;
             viewModel.SelectedChampion = viewModel.SelectedProcessor.Champs[(int)selected.id];
        }

        private void SeeSelectedChampion()
        {
            ChampCard selected = carouselControl.CurrentlySelected as ChampCard;
            System.Windows.Media.MediaPlayer player = new System.Windows.Media.MediaPlayer();
            player.Open(new Uri(Directory.GetCurrentDirectory() + "\\Sounds\\" + processor.Champs[(int)selected.id].Name + ".mp3"));
            player.Play();
        }

        private void carouselControl_OnElementShowWindow(object sender)
        {
            SeeSelectedChampion();
        }

        private void tbSearch_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void tbSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbSearch.Text.Equals("Search"))
            {
                tbSearch.Text = "";
            }
        }

        private void tbSearch_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (tbSearch.Text.Equals("Search"))
                tbSearch.Text = "";
            string tbText = tbSearch.Text;
            if (e.Key >= Key.A && e.Key <= Key.Z)
            {
                tbText += e.Key;
            }
            else if (e.Key == Key.Back && tbText.Length > 0)
                tbText = tbText.Substring(0, tbText.Length - 1);

            e.Handled = true;

            tbSearch.Text = tbText;

             List<KeyValuePair<int, ChampionStats>> sortedChamps = processor.Champs.ToList();
            sortedChamps.Sort((firstPair, nextPair) =>
            {
                return firstPair.Value.Name.CompareTo(nextPair.Value.Name);
            }
            );

            int i = 0;
            ChampCard selected = carouselControl.CurrentlySelected as ChampCard;
            foreach (KeyValuePair<int, ChampionStats> kvp in sortedChamps)
            {
                if (kvp.Value.Name.ToUpper().StartsWith(tbText))
                {
                    if (!selected.Equals((ChampCard)carouselControl.Children[i]))
                        carouselControl.SelectElement((ChampCard)carouselControl.Children[i]);
                    break;
                }
                i++;
            }
        }

        private void btnGlobalStats_Click(object sender, RoutedEventArgs e)
        {
            GlobalStatsPage globalStats = new GlobalStatsPage();
            PageSwitcher.Switch(globalStats);
        }


        


    }
}
