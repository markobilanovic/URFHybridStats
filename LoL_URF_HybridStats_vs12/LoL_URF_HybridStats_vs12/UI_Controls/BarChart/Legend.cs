namespace LoL_URF_HybridStats_vs12.Charts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Media;

    public class Legend
    {
        public Color LegendColor;
        public string LegendText;

        public Legend()
        {
        }

        public Legend(Color legendColor, string legendText)
        {
            LegendColor = legendColor;
            LegendText = legendText;
        }
    }
}
