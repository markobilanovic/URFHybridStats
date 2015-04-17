namespace LoL_URF_HybridStats_vs12.Charts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class BarEventArgs : EventArgs
    {
        Bar bar = null;

        public Bar BarObject
        {
            get { return bar; }
            set { bar = value; }
        }

        public BarEventArgs()
        {}

        public BarEventArgs(Bar b)
        {
            this.bar = b;
        }
    }

}
