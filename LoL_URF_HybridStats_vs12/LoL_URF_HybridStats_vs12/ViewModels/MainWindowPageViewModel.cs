using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using LoL_URF_HybridStats_vs12.Helper;
using LoL_URF_HybridStats_vs12.UI_Controls;
using WpfCharts;

namespace LoL_URF_HybridStats_vs12.ViewModels
{
    public class MainWindowPageViewModel : ViewModelBase
    {
        private UserControl _selectedControl;
        private BarChartControl _selectedChart;
        private List<string> _regions;
        private int _selectedRegion;
        private List<string> _graphCriterium;
        private string _selectedCriterium;
        private ObservableCollection<ChartLine> _spiderLeftLines;
        private ObservableCollection<ChartLine> _spiderRightLines;
        private string[] _spiderLeftAxes;
        private string[] _spiderRightAxes;

        private List<ChampionsProcessor> _processors;
        private ChampionsProcessor _selectedProcessor;
        private ChampionStats _selectedChampion;

        public ChampionStats SelectedChampion
        {
            get
            {
                return _selectedChampion;
            }
            set
            {
                _selectedChampion = value;
                UpdateSpiderGraph();
                UpdateBarChart();
                OnPropertyChanged("SelectedChampion");
            }
        }

        public int SelectedRegion
        {
            get
            {
                return _selectedRegion;
            }
            set
            {
                _selectedRegion = value;
                SelectedProcessor = _processors[value];
                if (SelectedChampion != null)
                    SelectedChampion = SelectedProcessor.Champs[(int)SelectedChampion.Id];
                OnPropertyChanged("SelectedRegion");
            }
        }
        

        public MainWindowPageViewModel()
        {
            InitProcessors();
            _spiderLeftLines = new ObservableCollection<ChartLine>();
            _spiderRightLines = new ObservableCollection<ChartLine>();

            InitRegions();
            InitAxes();
            
            
            InitCriteriums();
            SelectedChampion = SelectedProcessor.Champs[266];
           

            //aatrox
           
            SelectedRegion = 0;
       
        }

        private double GetValueFromZeroToHundred(double oldMin, double oldMax, double oldValue)
        {
            double OldRange = oldMax - oldMin;

            double NewMax = 100;
            double NewMin = 0;
            double NewRange = NewMax - NewMin;
            double NewValue;

            if (OldRange == 0)
                NewValue = NewMin;
            else
            {
                NewRange = (NewMax - NewMin);
                NewValue = (((oldValue - oldMin) * NewRange) / OldRange) + NewMin;
            }
            return NewValue;
        }

        private int ChampionPositionInList(List<ChampionStats> list, ChampionStats champ)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Id == champ.Id)
                {
                    return i;
                }
            }
            return 0;
        }

        private List<int> GetPositions(int i, int maxNum)
        {
            List<int> retval = new List<int>();
            if (i <= 5)
            {
                for (int p = 0; p < 11; p++)
                    retval.Add(p);
            }
            else if (i >= maxNum - 5)
            {
                for (int p = maxNum - 10; p <= maxNum; p++)
                    retval.Add(p);
            }
            else
            {
                for (int p = i - 5; p <= i + 5; p++)
                    retval.Add(p);
            }

            return retval;
        }

        private void UpdateBarChart()
        {
            List<ChampionStats> sortedChamps;
            List<int> indexList;
            List<object> sortedValuesList = new List<object>();
            List<string> sortedNames = new List<string>();
            string tooltipText = null;
            if (SelectedCriterium.Equals("Popularity"))
            {
                sortedChamps = SelectedProcessor.GetSortedListByNoOfPlayedMatch();
                indexList = GetPositions(ChampionPositionInList(sortedChamps, SelectedChampion), SelectedProcessor.Champs.Count - 1);
                tooltipText = "Played in {field}% of matches";
                foreach (int i in indexList)
                {
                    sortedValuesList.Add(Math.Round(((double)sortedChamps[i].NoOfPlayedGames / SelectedProcessor.noOfMatches) * 100, 2));
                    sortedNames.Add(sortedChamps[i].Name);
                }
            }
            else if (SelectedCriterium.Equals("Kills PG"))
            {
                sortedChamps = SelectedProcessor.GetSortedListByNoOfKills();
                indexList = GetPositions(ChampionPositionInList(sortedChamps, SelectedChampion), SelectedProcessor.Champs.Count - 1);
                tooltipText = "Kills PG: {field}";
                foreach (int i in indexList)
                {
                    sortedValuesList.Add(Math.Round((double)sortedChamps[i].NoOfKills / sortedChamps[i].NoOfPlayedGames, 1));
                    sortedNames.Add(sortedChamps[i].Name);
                }
            }
            else if (SelectedCriterium.Equals("Deaths PG"))
            {
                sortedChamps = SelectedProcessor.GetSortedListByNoOfDeaths();
                indexList = GetPositions(ChampionPositionInList(sortedChamps, SelectedChampion), SelectedProcessor.Champs.Count - 1);
                tooltipText = "Deaths PG: {field}";
                foreach (int i in indexList)
                {
                    sortedValuesList.Add(Math.Round((double)sortedChamps[i].NoOfDeaths / sortedChamps[i].NoOfPlayedGames, 1));
                    sortedNames.Add(sortedChamps[i].Name);
                }
            }
            else if (SelectedCriterium.Equals("Assist PG"))
            {
                sortedChamps = SelectedProcessor.GetSortedListByNoOfAssists();
                indexList = GetPositions(ChampionPositionInList(sortedChamps, SelectedChampion), SelectedProcessor.Champs.Count - 1);
                tooltipText = "Assists PG: {field}";
                foreach (int i in indexList)
                {
                    sortedValuesList.Add(Math.Round((double)sortedChamps[i].NoOfAssists / sortedChamps[i].NoOfPlayedGames, 1));
                    sortedNames.Add(sortedChamps[i].Name);
                }
            }
            else if (SelectedCriterium.Equals("Solo kills PG"))
            {
                sortedChamps = SelectedProcessor.GetSortedListByNoOfSoloKills();
                indexList = GetPositions(ChampionPositionInList(sortedChamps, SelectedChampion), SelectedProcessor.Champs.Count - 1);
                tooltipText = "Solo kills PG: {field}";
                foreach (int i in indexList)
                {
                    sortedValuesList.Add(Math.Round((double)sortedChamps[i].NoOfSoloKills / sortedChamps[i].NoOfPlayedGames, 1));
                    sortedNames.Add(sortedChamps[i].Name);
                }
            }
            else if (SelectedCriterium.Equals("Minions PG"))
            {
                sortedChamps = SelectedProcessor.GetSortedListByNoOfMinions();
                indexList = GetPositions(ChampionPositionInList(sortedChamps, SelectedChampion), SelectedProcessor.Champs.Count - 1);
                tooltipText = "Minions PG: {field}";
                foreach (int i in indexList)
                {
                    sortedValuesList.Add(Math.Round((double)sortedChamps[i].NoOfMinions / sortedChamps[i].NoOfPlayedGames, 1));
                    sortedNames.Add(sortedChamps[i].Name);
                }
            }
            else if (SelectedCriterium.Equals("Gold PG"))
            {
                sortedChamps = SelectedProcessor.GetSortedListByGoldEarned();
                indexList = GetPositions(ChampionPositionInList(sortedChamps, SelectedChampion), SelectedProcessor.Champs.Count - 1);
                tooltipText = "Gold PG: {field}";
                foreach (int i in indexList)
                {
                    sortedValuesList.Add(Math.Round((double)sortedChamps[i].GoldEarned / sortedChamps[i].NoOfPlayedGames, 0));
                    sortedNames.Add(sortedChamps[i].Name);
                }
            }
            else if (SelectedCriterium.Equals("Damage dealt"))
            {
                sortedChamps = SelectedProcessor.GetSortedListByDmgDealt();
                indexList = GetPositions(ChampionPositionInList(sortedChamps, SelectedChampion), SelectedProcessor.Champs.Count - 1);
                tooltipText = "Damage dealt PG: {field}";
                foreach (int i in indexList)
                {
                    sortedValuesList.Add(Math.Round((double)sortedChamps[i].DamageDealt / sortedChamps[i].NoOfPlayedGames, 0));
                    sortedNames.Add(sortedChamps[i].Name);
                }
            }
            else if (SelectedCriterium.Equals("Damage taken"))
            {
                sortedChamps = SelectedProcessor.GetSortedListByDamageTaken();
                indexList = GetPositions(ChampionPositionInList(sortedChamps, SelectedChampion), SelectedProcessor.Champs.Count - 1);
                tooltipText = "Damage taken PG: {field}";
                foreach (int i in indexList)
                {
                    sortedValuesList.Add(Math.Round((double)sortedChamps[i].DamageTaken / sortedChamps[i].NoOfPlayedGames, 0));
                    sortedNames.Add(sortedChamps[i].Name);
                }
            }
            else if (SelectedCriterium.Equals("MVP"))
            {
                sortedChamps = SelectedProcessor.GetSortedListByNoOfMVP();
                indexList = GetPositions(ChampionPositionInList(sortedChamps, SelectedChampion), SelectedProcessor.Champs.Count - 1);
                tooltipText = "MVP in {field}% of games";
                foreach (int i in indexList)
                {
                    sortedValuesList.Add(Math.Round((double)sortedChamps[i].MvpCount / sortedChamps[i].NoOfPlayedGames, 2));
                    sortedNames.Add(sortedChamps[i].Name);
                }
            }
            else if (SelectedCriterium.Equals("Banned"))
            {
                sortedChamps = SelectedProcessor.GetSortedListByBans();
                indexList = GetPositions(ChampionPositionInList(sortedChamps, SelectedChampion), SelectedProcessor.Champs.Count - 1);
                tooltipText = "Banned in {field}% of matches";
                foreach (int i in indexList)
                {
                    sortedValuesList.Add(Math.Round(((double)sortedChamps[i].BannedCount / SelectedProcessor.noOfMatches) * 100, 1));
                    sortedNames.Add(sortedChamps[i].Name);
                }
            }
            else if (SelectedCriterium.Equals("Level 30"))
            {
                sortedChamps = SelectedProcessor.GetSortedListByLVL30();
                indexList = GetPositions(ChampionPositionInList(sortedChamps, SelectedChampion), SelectedProcessor.Champs.Count - 1);
                tooltipText = "Level 30 in {field}% of games";
                foreach (int i in indexList)
                {
                    sortedValuesList.Add(Math.Round((double)sortedChamps[i].NoOfLvl30 / sortedChamps[i].NoOfPlayedGames, 2));
                    sortedNames.Add(sortedChamps[i].Name);
                }
            }

            SelectedChart = new BarChartControl(sortedValuesList, sortedNames, tooltipText, 0);
        }

        private void UpdateSpiderGraph()
        {
            SpiderLeftLines.Clear();
            SpiderRightLines.Clear();

            //average line
            double popularityAvg = 0;
            double killsAvg = 0;
            double deathAvg = 0;
            double assistAvg = 0;
            double soloKillsAvg = 0;
            double minionsAvg = 0;
            double goldAvg = 0;
            double dmgDealtAvg = 0;
            double dmgTakenAvg = 0;
            double mvpAvg = 0;
            double bannedAvg = 0;
            double lvl30Avg = 0;
            foreach (ChampionStats champ in SelectedProcessor.Champs.Values)
            {
                popularityAvg += champ.NoOfPlayedGames ;
                killsAvg += champ.NoOfKills / champ.NoOfPlayedGames;
                deathAvg += champ.NoOfDeaths / champ.NoOfPlayedGames;
                assistAvg += champ.NoOfAssists / champ.NoOfPlayedGames;
                soloKillsAvg += champ.NoOfSoloKills / champ.NoOfPlayedGames;
                minionsAvg += champ.NoOfMinions / champ.NoOfPlayedGames;
                goldAvg += champ.GoldEarned / champ.NoOfPlayedGames;
                dmgDealtAvg += champ.DamageDealt / champ.NoOfPlayedGames;
                dmgTakenAvg += champ.DamageTaken / champ.NoOfPlayedGames;
                mvpAvg += champ.MvpCount / champ.NoOfPlayedGames;
                bannedAvg += champ.BannedCount;
                lvl30Avg += champ.NoOfLvl30 / champ.NoOfPlayedGames;
            }
            int noOfChamps = SelectedProcessor.Champs.Count;

            popularityAvg /= noOfChamps;
            killsAvg /= noOfChamps;
            deathAvg /= noOfChamps;
            assistAvg /= noOfChamps;
            soloKillsAvg /= noOfChamps;
            minionsAvg /= noOfChamps;
            goldAvg /= noOfChamps;
            dmgDealtAvg /= noOfChamps;
            dmgTakenAvg /= noOfChamps;
            mvpAvg /= noOfChamps;
            bannedAvg /= noOfChamps;
            lvl30Avg /= noOfChamps;


            //champion line
            List<ChampionStats> list = SelectedProcessor.GetSortedListByNoOfPlayedMatch();
            double popularityValue = GetValueFromZeroToHundred(list[list.Count - 1].NoOfPlayedGames, 
                                                               list[0].NoOfPlayedGames, 
                                                               SelectedChampion.NoOfPlayedGames);
            popularityAvg = GetValueFromZeroToHundred(list[list.Count - 1].NoOfPlayedGames, 
                                                      list[0].NoOfPlayedGames, 
                                                      popularityAvg);
            
            list = SelectedProcessor.GetSortedListByNoOfKills();
            double killsValue = GetValueFromZeroToHundred(list[list.Count - 1].NoOfKills / list[list.Count - 1].NoOfPlayedGames,
                                                          list[0].NoOfKills / list[0].NoOfPlayedGames,
                                                          SelectedChampion.NoOfKills / SelectedChampion.NoOfPlayedGames);
            killsAvg = GetValueFromZeroToHundred(list[list.Count - 1].NoOfKills / list[list.Count - 1].NoOfPlayedGames,
                                                 list[0].NoOfKills / list[0].NoOfPlayedGames,
                                                 killsAvg);

            list = SelectedProcessor.GetSortedListByNoOfDeaths();
            double deathValue = GetValueFromZeroToHundred(list[list.Count - 1].NoOfDeaths / list[list.Count - 1].NoOfPlayedGames,
                                                          list[0].NoOfDeaths / list[0].NoOfPlayedGames,
                                                          SelectedChampion.NoOfDeaths / SelectedChampion.NoOfPlayedGames);
            deathAvg = GetValueFromZeroToHundred(list[list.Count - 1].NoOfDeaths / list[list.Count - 1].NoOfPlayedGames,
                                                 list[0].NoOfDeaths / list[0].NoOfPlayedGames,
                                                 deathAvg);

            list = SelectedProcessor.GetSortedListByNoOfAssists();
            double assistValue = GetValueFromZeroToHundred(list[list.Count - 1].NoOfAssists / list[list.Count - 1].NoOfPlayedGames,
                                                          list[0].NoOfAssists / list[0].NoOfPlayedGames,
                                                          SelectedChampion.NoOfAssists / SelectedChampion.NoOfPlayedGames);
            assistAvg = GetValueFromZeroToHundred(list[list.Count - 1].NoOfAssists / list[list.Count - 1].NoOfPlayedGames,
                                                 list[0].NoOfAssists / list[0].NoOfPlayedGames,
                                                 assistAvg);


            list = SelectedProcessor.GetSortedListByNoOfSoloKills();
            double soloKillsValue = GetValueFromZeroToHundred(list[list.Count - 1].NoOfSoloKills / list[list.Count - 1].NoOfPlayedGames,
                                                          list[0].NoOfSoloKills / list[0].NoOfPlayedGames,
                                                          SelectedChampion.NoOfSoloKills / SelectedChampion.NoOfPlayedGames);
            soloKillsAvg = GetValueFromZeroToHundred(list[list.Count - 1].NoOfSoloKills / list[list.Count - 1].NoOfPlayedGames,
                                                 list[0].NoOfSoloKills / list[0].NoOfPlayedGames,
                                                 soloKillsAvg);

            list = SelectedProcessor.GetSortedListByNoOfMinions();
            double minionsValue = GetValueFromZeroToHundred(list[list.Count - 1].NoOfMinions / list[list.Count - 1].NoOfPlayedGames,
                                                          list[0].NoOfMinions / list[0].NoOfPlayedGames,
                                                          SelectedChampion.NoOfMinions / SelectedChampion.NoOfPlayedGames);
            minionsAvg = GetValueFromZeroToHundred(list[list.Count - 1].NoOfMinions / list[list.Count - 1].NoOfPlayedGames,
                                                 list[0].NoOfMinions / list[0].NoOfPlayedGames,
                                                 minionsAvg);


            list = SelectedProcessor.GetSortedListByGoldEarned();
            double goldValue = GetValueFromZeroToHundred(list[list.Count - 1].GoldEarned / list[list.Count - 1].NoOfPlayedGames,
                                                          list[0].GoldEarned / list[0].NoOfPlayedGames,
                                                          SelectedChampion.GoldEarned / SelectedChampion.NoOfPlayedGames);
            goldAvg = GetValueFromZeroToHundred(list[list.Count - 1].GoldEarned / list[list.Count - 1].NoOfPlayedGames,
                                                 list[0].GoldEarned / list[0].NoOfPlayedGames,
                                                 goldAvg);



            list = SelectedProcessor.GetSortedListByDmgDealt();
            double dmgDealtValue = GetValueFromZeroToHundred(list[list.Count - 1].DamageDealt / list[list.Count - 1].NoOfPlayedGames,
                                                          list[0].DamageDealt / list[0].NoOfPlayedGames,
                                                          SelectedChampion.DamageDealt / SelectedChampion.NoOfPlayedGames);
            dmgDealtAvg = GetValueFromZeroToHundred(list[list.Count - 1].DamageDealt / list[list.Count - 1].NoOfPlayedGames,
                                                 list[0].DamageDealt / list[0].NoOfPlayedGames,
                                                 dmgDealtAvg);


            list = SelectedProcessor.GetSortedListByDamageTaken();
            double dmgTakenValue = GetValueFromZeroToHundred(list[list.Count - 1].DamageTaken / list[list.Count - 1].NoOfPlayedGames,
                                                          list[0].DamageTaken / list[0].NoOfPlayedGames,
                                                          SelectedChampion.DamageTaken / SelectedChampion.NoOfPlayedGames);
            dmgTakenAvg = GetValueFromZeroToHundred(list[list.Count - 1].DamageTaken / list[list.Count - 1].NoOfPlayedGames,
                                                 list[0].DamageTaken / list[0].NoOfPlayedGames,
                                                 dmgTakenAvg);

            list = SelectedProcessor.GetSortedListByNoOfMVP();
            double mvpValue = GetValueFromZeroToHundred(list[list.Count - 1].MvpCount / list[list.Count - 1].NoOfPlayedGames,
                                                          list[0].MvpCount / list[0].NoOfPlayedGames,
                                                          SelectedChampion.MvpCount / SelectedChampion.NoOfPlayedGames);
            mvpAvg = GetValueFromZeroToHundred(list[list.Count - 1].MvpCount / list[list.Count - 1].NoOfPlayedGames,
                                                 list[0].MvpCount / list[0].NoOfPlayedGames,
                                                 mvpAvg);

            list = SelectedProcessor.GetSortedListByBans();
            double bannedValue = GetValueFromZeroToHundred(list[list.Count - 1].BannedCount ,
                                                          list[0].BannedCount ,
                                                          SelectedChampion.BannedCount );
            bannedAvg = GetValueFromZeroToHundred(list[list.Count - 1].BannedCount ,
                                                 list[0].BannedCount ,
                                                 bannedAvg);

            list = SelectedProcessor.GetSortedListByLVL30();
            double lvl30Value = GetValueFromZeroToHundred(list[list.Count - 1].NoOfLvl30 / list[list.Count - 1].NoOfPlayedGames,
                                                          list[0].NoOfLvl30 / list[0].NoOfPlayedGames,
                                                          SelectedChampion.NoOfLvl30 / SelectedChampion.NoOfPlayedGames);
            lvl30Avg = GetValueFromZeroToHundred(list[list.Count - 1].NoOfLvl30 / list[list.Count - 1].NoOfPlayedGames,
                                                 list[0].NoOfLvl30 / list[0].NoOfPlayedGames,
                                                 lvl30Avg);
            


            SpiderLeftLines.Add(new ChartLine
            {
                LineColor = Colors.Gray,
                FillColor = Color.FromArgb(0, 128, 128, 128),
                LineThickness = 2,
                PointDataSource = new List<double>() { popularityAvg, killsAvg, deathAvg, assistAvg, soloKillsAvg, minionsAvg },
            });
            SpiderRightLines.Add(new ChartLine
            {
                LineColor = Colors.Gray,
                FillColor = Color.FromArgb(0, 128, 128, 128),
                LineThickness = 2,
                PointDataSource = new List<double>() { goldAvg, dmgDealtAvg, dmgTakenAvg, mvpAvg, bannedAvg, lvl30Avg },
            });

            SpiderLeftLines.Add(new ChartLine
            {
                LineColor = Colors.Green,
                FillColor = Color.FromArgb(0, 0, 255, 0),
                LineThickness = 2,
                PointDataSource = new List<double>() { popularityValue, killsValue, deathValue, assistValue, soloKillsValue, minionsValue },
            });
            SpiderRightLines.Add(new ChartLine
            {
                LineColor = Colors.Green,
                FillColor = Color.FromArgb(0, 0, 255, 0),
                LineThickness = 2,
                PointDataSource = new List<double>() { goldValue, dmgDealtValue, dmgTakenValue, mvpValue, bannedValue, lvl30Value },
            });
            //SelectedProcessor.
            //_processors[SelectedRegion]

        }

        private void InitProcessors()
        {
            _processors = new List<ChampionsProcessor>();
            _processors.Add(Serializator.LoadData("all.dat"));
            _processors.Add(Serializator.LoadData("br.dat"));
            _processors.Add(Serializator.LoadData("eune.dat"));
            _processors.Add(Serializator.LoadData("euw.dat"));
            _processors.Add(Serializator.LoadData("kr.dat"));
            _processors.Add(Serializator.LoadData("lan.dat"));
            _processors.Add(Serializator.LoadData("las.dat"));
            _processors.Add(Serializator.LoadData("na.dat"));
            _processors.Add(Serializator.LoadData("oce.dat"));
            _processors.Add(Serializator.LoadData("ru.dat"));
            _processors.Add(Serializator.LoadData("tr.dat"));

            SelectedProcessor = _processors[0];
        }

        private void InitAxes()
        {
            _spiderLeftAxes = new[] { "Popularity", "Kills PG", "Deaths PG", "Assist PG", "Solo kills PG", "Minions PG" };
            _spiderRightAxes = new[] { "Gold PG", "Damage dealt", "Damage taken", "MVP", "Banned", "Level 30" };
        }

        private void InitRegions()
        {
            _regions = new List<string>();
            _regions.Add("ALL");
            _regions.Add("BR");
            _regions.Add("EUNE");
            _regions.Add("EUW");
            _regions.Add("KR");
            _regions.Add("LAN");
            _regions.Add("LAS");
            _regions.Add("NA");
            _regions.Add("OCE");
            _regions.Add("RU");
            _regions.Add("TR");
        }

        private void InitCriteriums()
        {
            _graphCriterium = new List<string>();
            _graphCriterium.AddRange(_spiderLeftAxes);
            _graphCriterium.AddRange(_spiderRightAxes);
            SelectedCriterium = _graphCriterium[0];
        }

        public ObservableCollection<ChartLine> SpiderLeftLines
        {
            get
            {
                return _spiderLeftLines;
            }
            set
            {
                _spiderLeftLines = value;
                OnPropertyChanged("SpiderLeftLines");
            }
        }

        public ObservableCollection<ChartLine> SpiderRightLines
        {
            get
            {
                return _spiderRightLines;
            }
            set
            {
                _spiderRightLines = value;
                OnPropertyChanged("SpiderRightLines");
            }
        }

        public string[] SpiderLeftAxes
        {
            get
            {
                return _spiderLeftAxes;
            }
            set
            {
                _spiderLeftAxes = value;
                OnPropertyChanged("SpiderLeftAxes");
            }
        }

        public string[] SpiderRightAxes
        {
            get
            {
                return _spiderRightAxes;
            }
            set
            {
                _spiderRightAxes = value;
                OnPropertyChanged("SpiderRightAxes");
            }
        }

        public BarChartControl SelectedChart
        {
            get
            {
                return _selectedChart;
            }
            set
            {
                _selectedChart = value;
                OnPropertyChanged("SelectedChart");
            }
        }

        public UserControl SelectedControl
        {
            get
            {
                return _selectedControl;
            }
            set
            {
                _selectedControl = value;
                OnPropertyChanged("SelectedControl");
            }
        }

        public List<ChampionsProcessor> Processors
        {
            get
            {
                return _processors;
            }
            set
            {
                _processors = value;
                OnPropertyChanged("Processors");
            }
        }

        public ChampionsProcessor SelectedProcessor
        {
            get
            {
                return _selectedProcessor;
            }
            set
            {
                _selectedProcessor = value;
                OnPropertyChanged("SelectedProcessor");
            }
        }

        public List<string> Regions
        {
            get
            {
                return _regions;
            }
            set
            {
                _regions = value;
                OnPropertyChanged("SelectedControl");
            }
        }

        public List<string> GraphCriterium
        {
            get
            {
                return _graphCriterium;
            }
            set
            {
                _graphCriterium = value;
                OnPropertyChanged("GraphCriterium");
            }
        }

        public string SelectedCriterium
        {
            get
            {
                return _selectedCriterium;
            }
            set
            {
                _selectedCriterium = value;
                if(SelectedChampion != null)
                    UpdateBarChart();
                OnPropertyChanged("SelectedCriterium");
            }
        }


    }
}
