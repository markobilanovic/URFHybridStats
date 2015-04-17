namespace LoL_URF_HybridStats_vs12.Charts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Media;
    using System.Data;

    public class Bar
    {
        #region "Private Variables"

        private Color barColor = Colors.Red;
        private double barWidth = 5;
        private string barLabel;
        private Color colorOnMouseOver = Colors.Yellow;
        private DateTime timeStamp;


        double height;
        string id;
        double barValue;
        double top;
        double left;

        string queryParam;
        string paramValue;

        private DataRow dataRow;


        #endregion

        #region "Properties"

        public Color BarColor
        {
            get { return barColor; }
            set { barColor = value; }
        }

        public double BarWidth
        {
            get { return barWidth; }
            set { barWidth = value; }
        }

        public string BarLabel
        {
            get { return barLabel; }
            set { barLabel = value; }
        }

        public Color ColorOnMouseOver
        {
            get { return colorOnMouseOver; }
            set { colorOnMouseOver = value; }
        }

        public DateTime TimeStamp
        {
            get { return timeStamp; }
            set { timeStamp = value; }
        }

        public string QueryParam
        {
            set { this.queryParam = value; }
            get { return this.queryParam; }
        }


        public string ParamValue
        {
            set { this.paramValue = value; }
            get { return this.paramValue; }
        }


        public double BarHeight
        {
            set
            {
                this.height = value;
            }
            get
            {
                return this.height;
            }
        }

        public double Top
        {
            set
            {
                this.top = value;
            }
            get
            {
                return this.top;
            }
        }


        public double Left
        {
            set
            {
                this.left = value;
            }
            get
            {
                return this.left;
            }
        }


        public string ID
        {
            set { this.id = value; }
            get { return this.id; }
        }

        public double BarValue
        {
            set { this.barValue = value; }
            get { return this.barValue; }
        }

        public string ValueField { get; set; }

        public Bar()
        {

        }
 

        public Bar(double value, DataRow row, string valueField)
        {
            this.id = string.Empty;
            this.barValue = value;
            this.ValueField = valueField;
            this.dataRow = row;
        }

        public DataRow BarRow
        {
            get { return dataRow; }
            set { dataRow = value; }
        }

        public override string ToString()
        {
            StringBuilder s = new StringBuilder();

            try
            {
                s.Append("BaLabel  = " + this.BarLabel + Environment.NewLine);
                s.Append("BarValue = " + this.BarValue + Environment.NewLine);
                s.Append("BarWidth = " + this.BarWidth + Environment.NewLine);

                s.Append("BarRowInfo : " + Environment.NewLine);

                foreach (DataColumn c in BarRow.Table.Columns)
                {
                    s.Append("---->" + c.ColumnName + " = " + BarRow[c].ToString() + Environment.NewLine);
                }
            }
            catch { return base.ToString(); }
            
            return s.ToString();
        }



        #endregion

     
    }
}
