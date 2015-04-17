using System;
using System.Collections.Generic;
using System.Data;
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
using LoL_URF_HybridStats_vs12.Charts;

namespace LoL_URF_HybridStats_vs12.UI_Controls
{
    /// <summary>
    /// Interaction logic for BarChartControl.xaml
    /// </summary>
    public partial class BarChartControl : UserControl
    {
        public BarChartControl(List<object> sortedValuesList, List<string> sortedNames, string tooltipText, int selectedIndex)
        {
            InitializeComponent();

            BarChart chart = mainChart;
            chart.Reset();
            chart.SmartAxisLabel = true;
            chart.ValueField.Add("Amount");
            chart.XAxisField = "ChampName";
           // chart.ToolTipText = "Gold earned: {field}";
            chart.ToolTipText = tooltipText;
            chart.ShowValueOnBar = true;

            DataTable table1 = new DataTable("patients");
            table1.Columns.Add("Amount");
            table1.Columns.Add("ChampName");

            for (int i = 0; i < sortedValuesList.Count; i++)
            {
                table1.Rows.Add(sortedValuesList[i], sortedNames[i]);
            }
            DataSet dataset = new DataSet();
            dataset.Tables.Add(table1);
            chart.DataSource = dataset;
            chart.Generate();





        }
    }
}
