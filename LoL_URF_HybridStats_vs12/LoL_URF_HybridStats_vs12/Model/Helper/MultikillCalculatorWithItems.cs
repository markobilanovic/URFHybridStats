using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RiotSharp.MatchEndpoint;

namespace LoL_URF_HybridStats_vs12.Model.Helper
{
    public class MultikillCalculatorWithItems
    {
        //item, count
        private Dictionary<long, int> items;
        private int multikill;
        private double lastKillTimestamp;
        private long lastItemPurchased;
        private long lastItemPurchasedTimestamp;
        private List<long> lastItemsDestroyedOrSold;

        public MultikillCalculatorWithItems()
        {
            lastKillTimestamp = 0;
            items = new Dictionary<long,int>();
            lastItemsDestroyedOrSold = new List<long>();
        }

        //returns list of items if pentakill
        public Dictionary<long, int> AddKill(double timestamp)
        {
            if (lastKillTimestamp == 0)
            {
                
                multikill++;
            }
            else
            {
                if (multikill < 4)
                {
                    if (timestamp - lastKillTimestamp < 10000)
                        multikill++;
                    else
                        multikill = 1;
                }
                else
                {
                    if (timestamp - lastKillTimestamp < 30000)      //pentakill
                    {
                        multikill = 0;
                        lastKillTimestamp = 0;
                        return items;
                    }
                }
            }
            lastKillTimestamp = timestamp;
            return null;
        }

        public void ItemPurchased(Event e)
        {
            if (!items.ContainsKey(e.ItemId))
                items.Add(e.ItemId, 1);
            else
                items[e.ItemId]++;

            lastItemPurchased = e.ItemId;
            lastItemPurchasedTimestamp = e.Timestamp.Milliseconds;
        }

        public void ItemDestroyedOrSold(Event e)
        {
            if (e.ItemId == 2052)
                return;

            if (items[e.ItemId] == 1)
                items.Remove(e.ItemId);
            else
                items[e.ItemId]--;

            if (e.Timestamp.Milliseconds == lastItemPurchasedTimestamp)
                lastItemsDestroyedOrSold.Add(e.ItemId);
            else
                lastItemsDestroyedOrSold.Clear();
        }

        public void ItemUndo(Event e)
        {
            if (e.ItemBefore != 0)
            {
                ItemDestroyedOrSold(e.ItemBefore);
                foreach(long item in lastItemsDestroyedOrSold)
                {
                    ItemPurchased(item);
                }
            }
            else
            {
                ItemPurchased(e.ItemAfter);
            }
        }

        public void ItemPurchased(long itemId)
        {
            if (!items.ContainsKey(itemId))
                items.Add(itemId, 1);
            else
                items[itemId]++;
        }

        public void ItemDestroyedOrSold(long itemId)
        {
            if (itemId == 2052)
                return;

            if (items[itemId] == 1)
                items.Remove(itemId);
            else
                items[itemId]--;

        }

    }
}
