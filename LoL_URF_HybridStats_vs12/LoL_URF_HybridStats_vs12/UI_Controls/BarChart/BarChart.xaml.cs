using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.Windows.Media.Effects;
using System.Text.RegularExpressions;

namespace LoL_URF_HybridStats_vs12.Charts
{
    /// <summary>
    /// Interaction logic for BarChart.xaml
    /// </summary>
    public partial class BarChart : UserControl
    {
        private string xAxisText;
    
        private double horizontalGridLineThickness;
        
        private double defaultXYalueFontSize = 14;
        private double defaultYValueFontSize = 14;
        double? maxData;
        
        double left = 5;
        double top  = 0;

        double topMargin = 55;
        double leftMargin = 40;
        double bottomMargin = 55;
        double rightMargin = 40;
        double spaceBetweenBars = 12;

        private DataRow barRow;  // to hold current row 

        Brush prevBrush;

        Brush legendTextColor ;

        TextBlock txtXAxis;
        //TextBlock txtTopTitle;
        TextBlock bubbleText;

        Color gridLineColor = Colors.Gray;

        List<Legend> legends = new List<Legend>();

        public event EventHandler<BarEventArgs> BarClickHandler;

        public bool ShowValueOnBar { get; set; }
        public bool SmartAxisLabel { get; set; }

        public List<Legend> Legends
        {
            get { return legends; }
        }

        public enum ChartType
        {
            VerticalBar,
            HorizontalBar
        }

        
        public ChartType Type { get; set; }  // V1.1

        public double BarWidth { get; set; }
        public string Title { get; set; }
        public string ToolTipText  { get; set; }
        public string XAxisField { get; set; }
        public bool EnableZooming { get; set; }

        public string QueryParam { get; set; }
        
        public Color GridLineColor { get; set; }
        
        public string XAxisText
        {
            get { return xAxisText; }
            set 
            { 
                xAxisText = value;
                txtXAxis.Text = value;
            }
        }

        public void CleanCanvas()
        {
            chartArea.Children.Clear();
        }

        public List<string> ValueField  { get; set; }
        
        public Brush TextColor
        {
            get
            {
                return bubbleText.Foreground;
            }
            set
            {
                bubbleText.Foreground = value;
                //       txtTopTitle.Foreground = value;
                txtXAxis.Foreground = value;
            }
        }

        public double GridLineHorizontalThickness { get; set; }
        
        public bool ShowHorizontalGridLine { get; set; }
        
        public Brush BackGroundColor
        {
            get { return this.Background; }
            set
            {
                chartArea.Background = value;
                chartArea.Background = new SolidColorBrush { Color = Colors.Transparent };
                this.Background = value;
            }
        }

        public Brush LegendTextColor { get; set; }
        
             
        public DataSet DataSource { get; set; }
        
        public BarChart()
        {
            InitializeComponent();

            // Set the default chart type to VerticalBar

            this.Type = ChartType.VerticalBar;

            ValueField = new List<string>();
            InitChartControls();
            BarWidth = 60;
            horizontalGridLineThickness = 0.3;

            legendTextColor = new SolidColorBrush(Parser.GetDarkerColor(Colors.Black, 10));


            GradientStopCollection gsc = new GradientStopCollection(2);
            gsc.Add(new GradientStop(Colors.White, 1));
            gsc.Add(new GradientStop(Colors.Gray, 0));

            chartArea.Background = new SolidColorBrush { Color = Colors.Transparent };
            //    chartTitle.Background = new LinearGradientBrush(gsc, 90);
            //     chartLegendArea.Background = new LinearGradientBrush(gsc, 90);
            chartLayout.Background = new SolidColorBrush { Color = Colors.Transparent };
            
        }

        /// <summary>
        /// Get max value data element.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        double? GetMax(DataTable dt)
        {
            double? max = 0;
            double? tmp = 0;


            foreach (string valField in ValueField)
            {
                foreach (DataRow r in dt.Rows)
                {
                    if (!r.Table.Columns.Contains(valField))
                        continue;
                    if (r[valField] != DBNull.Value)
                    {
                        tmp = Convert.ToDouble(r[valField]);
                        
                        if (tmp > max)
                            max = tmp;
                    }
                }
            }

            max = (max != null) ? max : 0;

            return max;
        }

        /// <summary>
        /// Reset the value field.
        /// </summary>
        public void Reset()
        {
            ValueField.Clear();
        }

        /// <summary>
        /// Creates the chart based on the datasource and the charttype.
        /// </summary>
        public void Generate()
        {
            legends.Clear();
            chartArea.Children.Clear();
            
            // Setup chart elements.
            AddChartControlsToChart();

            // Setup chart area.
            SetUpChartArea();

            // Will be made more generic in the next versions.
            DataTable dt = (DataSource as DataSet).Tables[0];

            if (null != dt)
            {
                // if no data found draw empty chart.
                if (dt.Rows.Count == 0)
                {
                    DrawEmptyChart();
                    return;
                }
                
                // Hide the nodate found text.
            //    txtNoData.Visibility = Visibility.Hidden;

                try
                {
                    // Get the max y-value.  This is used to calculate the scale and y-axis.
                    maxData = GetMax(dt);

                    // Get the total bar count.
                    int barCount = dt.Rows.Count;

                    // If more than 1 value field, then this is a group chart.
                    bool isSeries = ValueField.Count > 1;

                    // no legends added yet.
                    bool legendAdded = false;  // no legends yet added.

                    // For each row in the datasource
                    int i = 1;
                    foreach (DataRow row in dt.Rows)
                    {
                        // Set the barwidth.  This is required to adjust the size based on available no. of 
                        // bars.
                        SetBarWidth(barCount);

                        // Draw axis label based on charttype.
                        if (Type == ChartType.VerticalBar)
                            DrawXAxisLabel(row, i);
                        else if (Type == ChartType.HorizontalBar)
                            DrawYAxisLabel(row);


                        // For each row the current series is initialized to 0 to indicate start of series.
                        int currentSeries = 0;

                        // For each value in the datarow, draw the bar.
                        foreach (string valField in ValueField)
                        {
                            if (null == valField)
                                continue;

                            if (!row.Table.Columns.Contains(valField))
                                continue;

                            // Draw bar for each value.
                            switch (Type)
                            {
                                case ChartType.VerticalBar:
                                    DrawVerticalBar(isSeries, legendAdded, row, ref currentSeries, valField, i);
                                    break;
                                case ChartType.HorizontalBar:
                                    DrawHorizontalBar(isSeries, legendAdded, row, ref currentSeries, valField);
                                    break;
                                default:
                                    DrawVerticalBar(isSeries, legendAdded, row, ref currentSeries, valField, i);
                                    break;
                            }
                        }
                        legendAdded = true;
                        i++;
                        // Set up location for next bar in series.

                        if (Type == ChartType.VerticalBar)
                            left = left + spaceBetweenBars;
                        else if (Type == ChartType.HorizontalBar)
                            top = top + spaceBetweenBars + BarWidth + 10;
                    }

                    if (Type == ChartType.VerticalBar)
                    {
                        if ((left + BarWidth) > chartArea.Width)
                            chartArea.Width = left + BarWidth + 20;
                    }
                    else if (Type == ChartType.HorizontalBar)
                    {
                        if ((top + BarWidth) > chartArea.Height)
                            chartArea.Height = top + BarWidth + 20;

                    }

                    // Adjust chart element location after final chart rendering.
                    AdjustChartElements();

                    DrawXAxis();  // Draw x-axis
                    DrawYAxis();  // Draw y-axis
                    DrawLegend(); // Draw the legends
                }
                catch { }
            }
        }


        #region VerticalBar
        /// <summary>
        /// Draws a bar
        /// </summary>
        /// <param name="isSeries">Whether current bar is in a series or group.</param>
        /// <param name="legendAdded">Indicates whether to add legend.</param>
        /// <param name="row">The current bar row.</param>
        /// <param name="currentSeries">The current series.  Used to group series and color code bars.</param>
        /// <param name="valField">Value is fetched from the datasource from this field.</param>
        private void DrawVerticalBar(bool isSeries, bool legendAdded, DataRow row, ref int currentSeries, string valField, int i)
        {
            double val = 0.0;

            if (row[valField] == DBNull.Value)
                val = 0;
            else
                val = Convert.ToDouble(row[valField]);

            // Calculate bar value.
            double? calValue = (((float)val * 100 / maxData)) * 
                    (chartArea.Height - bottomMargin - topMargin) / 100;

            Rectangle rect = new Rectangle();
            
            // Setup bar attributes.
            SetVerticalBarAttributes(calValue, rect);

            // Color the bar.
            Color stroke = Helper.GetDarkColorByIndex(currentSeries);
          /*  if (i == 0)
                rect.Fill = new SolidColorBrush { Color = Colors.Red };
            else
                rect.Fill = new SolidColorBrush(stroke);
            */
            rect.Fill = new SolidColorBrush { Color = Colors.Black };

            // Setup bar events.
            SetBarEvents(rect);

            // Add the legend if not added.
            if (isSeries && !legendAdded)
            {
                legends.Add(new Legend(stroke, ValueField[currentSeries]));
            }

            // Calculate bar top and left position.
            top = (chartArea.Height - bottomMargin) - rect.Height;
            Canvas.SetTop(rect, top);
            Canvas.SetLeft(rect, left + leftMargin);

            // Add bar to chart area.
            chartArea.Children.Add(rect);

            // Display value on bar if set to true.
            if (ShowValueOnBar)
            {
                DisplayYValueOnVerticalBar(val, rect);
            }

            // Create Bar object and assign to the rect.
            rect.Tag = new Bar(val, row, valField);

            // Calculate the new left postion for subsequent bars.
            if (isSeries)
                left = left + rect.Width;
            else
                left = left + BarWidth + spaceBetweenBars;

            // Increment the series
            currentSeries++;  
        }

        /// <summary>
        /// Setup bar events.
        /// </summary>
        /// <param name="rect"></param>
        private void SetBarEvents(Rectangle rect)
        {
            rect.MouseLeftButtonUp += new MouseButtonEventHandler(Bar_MouseLeftButtonUp);
            rect.MouseEnter += new MouseEventHandler(Bar_MouseEnter);
            rect.MouseLeave += new MouseEventHandler(Bar_MouseLeave);
        }

        /// <summary>
        /// Setup bar attributes.
        /// </summary>
        /// <param name="currentSeries"></param>
        /// <param name="calValue"></param>
        /// <param name="rect"></param>
        /// <returns></returns>
        private void SetVerticalBarAttributes(double? calValue, Rectangle rect)
        {
            rect.Width = BarWidth;
            if (calValue < 1)
                rect.Height = 1;
            else
                rect.Height = calValue.Value;

            rect.HorizontalAlignment = HorizontalAlignment.Left;
            rect.VerticalAlignment = VerticalAlignment.Center;
            rect.StrokeThickness = 1;
           
        }

        /// <summary>
        /// Display y-value on bar.
        /// </summary>
        /// <param name="val"></param>
        /// <param name="rect"></param>
        private void DisplayYValueOnVerticalBar(double val, Rectangle rect)
        {
            TextBlock yValue = new TextBlock();
            yValue.Text = val.ToString();
            yValue.Width = 80;
            yValue.SetValue(TextBlock.FontWeightProperty, FontWeights.Bold);
            yValue.Foreground = TextColor;
            yValue.HorizontalAlignment = HorizontalAlignment.Center;
            yValue.TextAlignment = TextAlignment.Center;
            yValue.FontSize = defaultYValueFontSize;

            yValue.MouseEnter += new MouseEventHandler(yValue_MouseEnter);
            yValue.MouseLeave += new MouseEventHandler(yValue_MouseLeave);
            chartArea.Children.Add(yValue);
            Canvas.SetTop(yValue, top - 15);
            Canvas.SetLeft(yValue, left + (rect.Width / 2));
        }

        private void SetBarWidth(int barCount)
        {
            BarWidth = (chartArea.Width - (spaceBetweenBars * ValueField.Count * barCount) -
                (leftMargin * 3)) / (barCount * ValueField.Count);

            // check min bar width
            if (BarWidth > 20 || BarWidth < 20)
                BarWidth = 20;
        }

        /// <summary>
        /// Draw the x-axis label.
        /// </summary>
        /// <param name="row">The bar data row.</param>
        private void DrawXAxisLabel(DataRow row, int i)
        {
            // Setup XAxis label
            TextBlock markText = new TextBlock();
            markText.Text = row[XAxisField].ToString();
            markText.Width = 80;
            markText.HorizontalAlignment = HorizontalAlignment.Stretch;

            if (i == 0)
                markText.Foreground = new SolidColorBrush { Color = Colors.Red };
            else
                markText.Foreground = TextColor;
          
            markText.TextAlignment = TextAlignment.Center;
            markText.FontSize = 15;

            markText.MouseEnter += new MouseEventHandler(XText_MouseEnter);
            markText.MouseLeave += new MouseEventHandler(XText_MouseLeave);

            if (SmartAxisLabel)
            {
                Transform st = new SkewTransform(0, 20);
                markText.RenderTransform = st;
            }

            chartArea.Children.Add(markText);
            Canvas.SetTop(markText, this.Height - bottomMargin);  // adjust y location
            Canvas.SetLeft(markText, left + leftMargin / 2);
        }

        #endregion

        #region Common
        /// <summary>
        /// Prepares the chart for rendering.  Sets up control width and location.
        /// </summary>
        private void AdjustChartElements()
        {
            //    txtTopTitle.Width = this.Width;
            //    txtTopTitle.FontSize = 14;
            //    txtTopTitle.Text = Title;
            //    txtTopTitle.TextAlignment = TextAlignment.Center;
            
            if (Type == ChartType.VerticalBar)
            {
                Canvas.SetTop(txtXAxis, chartArea.Height - (bottomMargin/2));
                Canvas.SetLeft(txtXAxis, leftMargin);
            }
            else if (Type == ChartType.HorizontalBar)
            {
                Canvas.SetTop(txtXAxis, top + 20);
                Canvas.SetLeft(txtXAxis, leftMargin);
            }
        }

        /// <summary>
        /// Sets up the chart area with default values
        /// </summary>
        private void SetUpChartArea()
        {
            if (!EnableZooming)
            {
                //       zoomSlider.Visibility = Visibility.Hidden;
            }

            if (this.Height.ToString() == "NaN")
                this.Height = 450;

            chartArea.Height = this.Height;

            if (this.Width.ToString() == "NaN")
                this.Width = 800;

            chartArea.Width = this.Width;
        }

        /// <summary>
        /// Draws an empty chart.
        /// </summary>
        private void DrawEmptyChart()
        {
            //     txtNoData.Visibility = Visibility.Visible;
            //      Canvas.SetTop(txtNoData, chartArea.Height / 2);
            //      Canvas.SetLeft(txtNoData, chartArea.Width / 2);
        }

        void yValue_MouseLeave(object sender, MouseEventArgs e)
        {
            TextBlock tb = (sender as TextBlock);
            tb.FontSize = defaultXYalueFontSize;
        }

        void yValue_MouseEnter(object sender, MouseEventArgs e)
        {
            TextBlock tb = (sender as TextBlock);
            tb.FontSize = 15;
        }

        void XText_MouseLeave(object sender, MouseEventArgs e)
        {
            TextBlock tb = (sender as TextBlock);
            tb.FontSize = defaultXYalueFontSize;
        }

        void XText_MouseEnter(object sender, MouseEventArgs e)
        {
            TextBlock tb = (sender as TextBlock);
            tb.FontSize = 15;
        }

        
        /// <summary>
        /// Initialize chart controls.
        /// </summary>
        private void InitChartControls()
        {
            //txtTopTitle = new TextBlock();
            txtXAxis = new TextBlock();
            bubbleText = new TextBlock();

            bubbleText.FontSize = 9;
            
            Transform tf = new ScaleTransform(1.5, 1.5, 12, 24);
            bubbleText.RenderTransform = tf;

         
        }

        /// <summary>
        /// Add chart controls to chart.  This creates a basic layout for the chart.
        /// </summary>
        private void AddChartControlsToChart()
        {
            chartArea.Children.Add(txtXAxis);
            chartArea.Children.Add(bubbleText);

            //chartTitle.Children.Add(txtTopTitle);
        }


        /// <summary>
        /// Draw chart legends.
        /// </summary>
        private void DrawLegend()
        {
            if (legends == null || legends.Count == 0)
            {
                //         chartLegendArea.Visibility = Visibility.Hidden;
                return;
            }

            //     chartLegendArea.Visibility = Visibility.Visible;

            double x1 = 5;
            double y1 = 5;
            double legendWidth = 20;
            TextBlock tb;

            // Draw all legends
            foreach (Legend legend in legends)
            {
                Line legendShape = new Line();

                // Calculate the legend width.
                Size size = new Size(0, 0);
                for (int i = 0; i < legends.Count; i++)
                {
                    tb = new TextBlock();
                    tb.Text = legends[i].LegendText;
                    tb.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
                    size = tb.DesiredSize;
                    if (legendWidth < size.Width)
                        legendWidth = size.Width;
                }


                legendShape.Stroke = new SolidColorBrush(legend.LegendColor);
                legendShape.StrokeDashCap = PenLineCap.Round;
                legendShape.StrokeThickness = 2;

                legendShape.StrokeStartLineCap = PenLineCap.Round;
                legendShape.StrokeEndLineCap = PenLineCap.Triangle;

                legendShape.X1 = x1;
                legendShape.Y1 = y1;
                legendShape.X2 = x1 + legendWidth;
                legendShape.Y2 = y1;

                //         chartLegendArea.Children.Add(legendShape);

                TextBlock txtLegend = new TextBlock();
                txtLegend.Text = legend.LegendText;
                txtLegend.Foreground = legendTextColor;

                //        chartLegendArea.Children.Add(txtLegend);
                
                Canvas.SetTop(txtLegend, y1 - size.Height /2);
                Canvas.SetLeft(txtLegend, legendShape.X1 + legendShape.X2 + 5);

                y1 += 15;
            }
        }

        private void DrawLegend1()
        {
            if (legends == null || legends.Count == 0)
                return;

            // Initialize legend location.
            double legendX1 = leftMargin + txtXAxis.Text.Length + 100;
            double legendWidth = 20;

            // Draw all legends
            foreach (Legend legend in legends)
            {
                Line legendShape = new Line();

                legendShape.Stroke = new SolidColorBrush(legend.LegendColor);
                legendShape.StrokeDashCap = PenLineCap.Round;
                legendShape.StrokeThickness = 8;

                legendShape.StrokeStartLineCap = PenLineCap.Round;
                legendShape.StrokeEndLineCap = PenLineCap.Triangle;


                legendShape.X1 = legendX1;
                legendShape.Y1 = this.Height - 10;
                legendShape.X2 = legendX1 + legendWidth;
                legendShape.Y2 = this.Height - 10;

                chartArea.Children.Add(legendShape);
                

                TextBlock txtLegend = new TextBlock();
                txtLegend.Text = legend.LegendText;
                txtLegend.Foreground = legendTextColor;

                chartArea.Children.Add(txtLegend);
                
                Canvas.SetTop(txtLegend, this.Height - 20);
                Canvas.SetLeft(txtLegend, legendShape.X2 + 2);

                legendX1 += legendWidth + 30 + txtLegend.Text.Length;
            }
        }
        #endregion

        /// <summary>
        /// Draws xAxis.  For vertical bar chart it's as simple as drawing a line
        /// beginning from top to bottom.  For horizontal bar some calculations are
        /// involved as we need to position the marker correctly along with display
        /// value.
        /// </summary>
        private void DrawXAxis()
        {
            // Draw axis
            if (Type == ChartType.VerticalBar)
            {
                Line xaxis = new Line();
                xaxis.X1 = leftMargin;
                xaxis.Y1 = this.Height - bottomMargin;
                xaxis.X2 = this.chartArea.Width;
                xaxis.Y2 = this.Height - bottomMargin;

                xaxis.Stroke = new SolidColorBrush(Colors.Black);
                chartArea.Children.Add(xaxis);
            }
            else if (Type == ChartType.HorizontalBar)
            {
                Line xaxis = new Line();
                xaxis.X1 = leftMargin;
                xaxis.Y1 = top;
                xaxis.X2 = this.chartArea.Width;
                xaxis.Y2 = top;

                xaxis.Stroke = new SolidColorBrush(Colors.Black);
                chartArea.Children.Add(xaxis);


                // Set the scale factor for y-axis marker.
                double scaleFactor = 10;

                // this value is used to increment the y-axis marker value.
                double xMarkerValue = Math.Ceiling(maxData.Value / scaleFactor);

                // This value is used to increment the y-axis marker location.
                double scale = 5;  // default value 5.

                // get the scale based on the current max y value and other chart element area adjustments.
                scale = (((float)xMarkerValue * 100 / maxData.Value)) *
                    (chartArea.Width - leftMargin - rightMargin) / 100;

                double x1 = chartArea.Width - rightMargin;

                double xAxisValue = 0;

                x1 = leftMargin;
                for (double i = 0; i <= scaleFactor; i++)
                {
                    // Add x-axis marker line chart.
                    Line marker = AddMarkerLineToChart(x1);

                    // Add the y-marker to the chart.
                    AddMarkerTextToChart(x1 - scale, xAxisValue);

                    // Adjust the top location for next marker.
                    x1 += scale;

                    // Increment the y-marker value.
                    xAxisValue += xMarkerValue;
                }
            }
        }

        /// <summary>
        /// Draws YAxis.  For horizantal chart it's as simple as drawing a line
        /// beginning from top to bottom.  For vertical bar some calculations are
        /// involved as we need to position the marker correctly along with display
        /// value.
        /// </summary>
        private void DrawYAxis()
        {
            if (Type == ChartType.VerticalBar)
            {
                Line yaxis = new Line();
                yaxis.X1 = leftMargin;
                yaxis.Y1 = 0;
                yaxis.X2 = leftMargin;
                yaxis.Y2 = this.Height - bottomMargin;
                yaxis.Stroke = new SolidColorBrush(Colors.Black);
                chartArea.Children.Add(yaxis);

                // Set the scale factor for y-axis marker.
                double scaleFactor = 10;

                // this value is used to increment the y-axis marker value.
                double yMarkerValue = Math.Ceiling(maxData.Value / scaleFactor);

                // This value is used to increment the y-axis marker location.
                double scale = 5;  // default value 5.

                // get the scale based on the current max y value and other chart element area adjustments.
                scale = (((float)yMarkerValue * 100 / maxData.Value)) *
                    (chartArea.Height - bottomMargin - topMargin) / 100;

                double y1 = this.Height - bottomMargin;

                double yAxisValue = 0;

                for (int i = 0; i <= scaleFactor; i++)
                {
                    // Add y-axis marker line chart.
                    Line marker = AddMarkerLineToChart(y1);

                    // Draw horizontal grid based on marker location.
                    DrawHorizontalGrid(marker.X1, y1);

                    // Add the y-marker to the chart.
                    AddMarkerTextToChart(y1, yAxisValue);

                    // Adjust the top location for next marker.
                    y1 -= scale;

                    // Increment the y-marker value.
                    yAxisValue += yMarkerValue;
                }
            }
            else if (Type == ChartType.HorizontalBar)
            {
                Line yaxis = new Line();
                yaxis.X1 = leftMargin;
                yaxis.Y1 = 0;
                yaxis.X2 = leftMargin;
                yaxis.Y2 = top;
                yaxis.Stroke = new SolidColorBrush(Colors.Black);
                chartArea.Children.Add(yaxis);
            }
        }

        /// <summary>
        /// Add the marker line to chart.
        /// </summary>
        /// <param name="top">The top location where the marker is to be placed.</param>
        /// <returns>The marker line.  This is used for drawing the horizontal grid line.</returns>
        private Line AddMarkerLineToChart(double location)
        {
            Line marker = new Line();
            
            if (Type == ChartType.VerticalBar)
            {
                marker.X1 = leftMargin - 4;
                marker.Y1 = location;
                marker.X2 = marker.X1 + 4;
                marker.Y2 = location;
                marker.Stroke = new SolidColorBrush(Colors.Red);
            }
            else if (Type == ChartType.HorizontalBar)
            {
                marker.X1 = location;
                marker.Y1 = top-4;
                marker.X2 = location;
                marker.Y2 = top+4;
                marker.Stroke = new SolidColorBrush(Colors.Red);
            }

            chartArea.Children.Add(marker);
            return marker;
        }

        /// <summary>
        /// Add marker text to chart on yaxis.
        /// </summary>
        /// <param name="top">The top location.</param>
        /// <param name="markerTextValue">The marker text value.</param>
        private void AddMarkerTextToChart(double location, double markerTextValue)
        {
            TextBlock markText = new TextBlock();
            markText.Text = markerTextValue.ToString();
            markText.Width = 30;
            markText.FontSize = 14;
            markText.Foreground = TextColor;
            markText.HorizontalAlignment = HorizontalAlignment.Right;
            markText.TextAlignment = TextAlignment.Right;
            if (Type == ChartType.VerticalBar)
            {
                Canvas.SetTop(markText, location - 10);        // adjust y location
                Canvas.SetLeft(markText, leftMargin - 40);    
            }
            else if (Type == ChartType.HorizontalBar)
            {
                Canvas.SetTop(markText, top + 4);        // adjust y location
                Canvas.SetLeft(markText, location);
            }
            chartArea.Children.Add(markText);

            
        }

        /// <summary>
        /// Draw horizontal Grid, if ShowHorizontalGridLine property is set.
        /// </summary>
        /// <param name="x1">starting left postion</param>
        /// <param name="y1">starting top postion</param>
        private void DrawHorizontalGrid(double x1, double y1)
        {
            if (!ShowHorizontalGridLine)
                return;

            Line gridLine = new Line();
            gridLine.X1 = x1;
            gridLine.Y1 = y1;
            gridLine.X2 = chartArea.Width;
            gridLine.Y2 = y1;

            gridLine.StrokeThickness = horizontalGridLineThickness;

            gridLine.Stroke = new SolidColorBrush(GridLineColor);
            
            chartArea.Children.Add(gridLine);

        }

        void Bar_MouseLeave(object sender, MouseEventArgs e)
        {
            Rectangle rect = (sender as Rectangle);
            rect.Fill = prevBrush;
            prevBrush = null;
            
        }

        void Bar_MouseEnter(object sender, MouseEventArgs e)
        {
            Rectangle rect = (sender as Rectangle);
            prevBrush = rect.Fill;

            rect.Fill = new SolidColorBrush(Colors.Gray);

            Bar b = rect.Tag as Bar;
            ToolTip tip = new ToolTip();

            barRow = b.BarRow;
            tip.Content = MatchToolTipTemplate(b.ValueField); 
            
            rect.ToolTip = tip;

        }

        /// <summary>
        /// Match tooltip template and replace fields.  Need help in improving this.
        /// Searches for a tokein in {} and replaces with the actual value from DataSource.
        /// Supports only single token replacement.  Need to add support for multiple token replacement.
        /// </summary>
        /// <returns></returns>

        public string MatchToolTipTemplate(string valueField)
        {
            //string matchExpression = @"{\w+}";
            //string matchExpression = @"{field}";
            //MatchEvaluator matchField = MatchEvaluatorField;
            
            return (ToolTipText.Replace("{field}", GetResolvedTemplateValue(valueField)));

            //return Regex.Replace(ToolTipText, matchExpression, matchField);
        }

        private string GetResolvedTemplateValue(string valueField)
        {
            string newText = "" ;
            try
            {
                newText = barRow[valueField].ToString();
            }
            catch { }
            return newText;
        }


        [Obsolete]
        private string MatchEvaluatorField(Match m)
        {
            string newText = m.Value.Replace('{',' ');
            newText = newText.Replace('}', ' ');
            try
            {
                newText = barRow[newText.Trim()].ToString();
            }
            catch { }
            return newText;
        }


        void Bar_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Rectangle rect = (sender as Rectangle);
            Bar b = rect.Tag as Bar;
            
            if (BarClickHandler != null)
                BarClickHandler(this, new BarEventArgs(b));

        }

        #region Horizontal Bar
        private void DrawHorizontalBar(bool isSeries, bool legendAdded, DataRow row, ref int currentSeries, string valField)
        {
            double val = 0.0;

            if (row[valField] == DBNull.Value)
                val = 0;
            else
                val = Convert.ToDouble(row[valField]);

            // Calculate bar value.
            double? calValue = (((float)val * 100 / maxData)) *
                    (chartArea.Width - rightMargin- leftMargin) / 100;

            Rectangle rect = new Rectangle();

            // Setup bar attributes.
            SetHorizontalBarAttributes(calValue, rect);

            // Color the bar.
            Color stroke = Helper.GetDarkColorByIndex(currentSeries);
            rect.Fill = new SolidColorBrush(stroke);

            // Setup bar events.
            SetBarEvents(rect);

            // Add the legend if not added.
            if (isSeries && !legendAdded)
            {
                legends.Add(new Legend(stroke, ValueField[currentSeries]));
            }

            Canvas.SetTop(rect, top);
            Canvas.SetLeft(rect, 0 + leftMargin);

            // Add bar to chart area.
            chartArea.Children.Add(rect);

            // Display value on bar if set to true.
            if (ShowValueOnBar)
            {
                DisplayYValueOnHorizontalBar(val, rect);
            }

            // Create Bar object and assign to the rect.
            rect.Tag = new Bar(val, row, valField);

            // Calculate the new top  postion for subsequent bars.
            if (isSeries)
                top = top + rect.Height;
            
            // Increment the series
            currentSeries++;
        }

        /// <summary>
        /// Draw the y-axis label.
        /// </summary>
        /// <param name="row">The bar data row.</param>
        private void DrawYAxisLabel(DataRow row)
        {
            TextBlock markText = new TextBlock();
            markText.Text = row[XAxisField].ToString();
            markText.Width = 80;
            markText.HorizontalAlignment = HorizontalAlignment.Stretch;
            markText.Foreground = TextColor;
            markText.TextAlignment = TextAlignment.Center;
            markText.FontSize = 15;

            markText.MouseEnter += new MouseEventHandler(XText_MouseEnter);
            markText.MouseLeave += new MouseEventHandler(XText_MouseLeave);

            if (SmartAxisLabel)
            {
                Transform st = new SkewTransform(0, 20);
                markText.RenderTransform = st;
            }

            chartArea.Children.Add(markText);
            Canvas.SetTop(markText, (top + (BarWidth * ValueField.Count)/2));  // adjust y location
            Canvas.SetLeft(markText, left-leftMargin+10);
        }


        /// <summary>
        /// Setup bar attributes.
        /// </summary>
        /// <param name="currentSeries"></param>
        /// <param name="calValue"></param>
        /// <param name="rect"></param>
        /// <returns></returns>
        private void SetHorizontalBarAttributes(double? calValue, Rectangle rect)
        {
            rect.HorizontalAlignment = HorizontalAlignment.Left;
            rect.VerticalAlignment = VerticalAlignment.Center;
            rect.StrokeThickness = 1;

            rect.Height= BarWidth;
            if (calValue < 1)
                rect.Width = 1;
            else
                rect.Width= calValue.Value;

            rect.HorizontalAlignment = HorizontalAlignment.Left;
            rect.VerticalAlignment = VerticalAlignment.Center;
            rect.StrokeThickness = 1;

        }

        /// <summary>
        /// Display y-value on bar.
        /// </summary>
        /// <param name="val"></param>
        /// <param name="rect"></param>
        private void DisplayYValueOnHorizontalBar(double val, Rectangle rect)
        {
            TextBlock yValue = new TextBlock();
            yValue.Text = val.ToString();
            yValue.Width = 80;
            yValue.Foreground = TextColor;
            yValue.HorizontalAlignment = HorizontalAlignment.Center;
            yValue.TextAlignment = TextAlignment.Center;
            yValue.FontSize = defaultYValueFontSize;

            yValue.MouseEnter += new MouseEventHandler(yValue_MouseEnter);
            yValue.MouseLeave += new MouseEventHandler(yValue_MouseLeave);
            chartArea.Children.Add(yValue);
            Canvas.SetTop(yValue, top + (rect.Height/2/2));
            Canvas.SetLeft(yValue, left + (rect.Width + 5));
        }

        #endregion

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scrollviewer = sender as ScrollViewer;
            if (e.Delta > 0)
            {
                scrollviewer.LineLeft();
                scrollviewer.LineLeft();
            }
            else
            {
                scrollviewer.LineRight();
                scrollviewer.LineRight();
            }
            e.Handled = true;
        }

    }
}
