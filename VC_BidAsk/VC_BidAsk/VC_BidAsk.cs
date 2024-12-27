using cAlgo.API;
using System;
using System.Linq;

namespace cAlgo
{
    [Indicator(AccessRights = AccessRights.None, IsOverlay = true)]
    public class BidAskOHLCIndicator : Indicator
    {
        private double bidOpen, bidHigh, bidLow, bidClose;
        private double askOpen, askHigh, askLow, askClose;
        private int currentBarStartTime;

        bool haveRun = false;

        protected override void Initialize()
        {
        }

        public override void Calculate(int index)
        { 
            Print("Calculate");
            if (!haveRun)
            {
                var ticks = MarketData.GetTicks();
                //var ticksLoaded = ticks.LoadMoreHistory();
                ////ticksLoaded += ticks.LoadMoreHistory();

                //if (ticksLoaded > 0) {
                //    Print($"Loaded {ticksLoaded} ticks");
                //}
                Print($"First tick time: {ticks.First().Time}");
                Print($"Last tick time: {ticks.Last().Time}");

                int minIndex = 0;
                Tick? prevTick = null;
                foreach(var tick in ticks)
                {
                    OnTick(tick.Bid, tick.Ask, minIndex);
                    if (prevTick.HasValue)
                    {
                        if (tick.Time.Minute != prevTick.Value.Time.Minute)
                        {
                            minIndex++;
                        }
                    }
                }

                haveRun = true;
            }
        }

        private void OnTick(double currentBid, double currentAsk, int index)
        {
            // Logic to update OHLC values for Bid/Ask
            if (index != currentBarStartTime)
            {
                // This is a new bar; finalize previous bar data
                bidClose = bidOpen;
                askClose = askOpen;

                // Store or process the completed bar values here //include time
                var color = bidClose > askClose ? Color.Red : Color.Green;
                DrawBar($"bid_bar_{index}", index, bidHigh, bidLow, color);
                DrawBar($"ask_bar_{index}", index, askHigh, askLow, color);

                Print("Bid {4} OHLC: {0}, {1}, {2}, {3}", bidOpen, bidHigh, bidLow, bidClose, index);
                Print("Ask {4} OHLC: {0}, {1}, {2}, {3}", askOpen, askHigh, askLow, askClose, index);

                // Reset for new bar
                currentBarStartTime = index;
                bidOpen = currentBid;
                askOpen = currentAsk;
                bidHigh = bidLow = currentBid;
                askHigh = askLow = currentAsk;
            }

            // Update High and Low
            if (currentBid > bidHigh) bidHigh = currentBid;
            if (currentBid < bidLow) bidLow = currentBid;
            if (currentAsk > askHigh) askHigh = currentAsk;
            if (currentAsk < askLow) askLow = currentAsk;

            // Continuously update close values
            bidClose = currentBid;
            askClose = currentAsk;
        }

        private void DrawBar(string name, int barIndex, double top, double bottom, Color color)
        {
            var trendLine = Chart.DrawTrendLine(name, barIndex, top, barIndex, bottom, color, 6, LineStyle.Solid);
            if (trendLine != null)
            {
                Print("trendLine");
            }
        }
    }
}
