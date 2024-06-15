using System;
using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo.Indicators
{
    [Indicator(IsOverlay = true, AccessRights = AccessRights.None, AutoRescale = false)]
    public class VC_Supertrend : Indicator
    {
        [Parameter(DefaultValue = "Daily")]
        public TimeFrame timeFrame { get; set; }

        [Parameter(DefaultValue = 10)]
        public int Period { get; set; }

        [Parameter(DefaultValue = 3.0)]
        public double Multiplier { get; set; }

        [Output("UpTrend", LineColor = "Green", PlotType = PlotType.Points, Thickness = 3)]
        public IndicatorDataSeries UpTrend { get; set; }

        [Output("DownTrend", LineColor = "Red", PlotType = PlotType.Points, Thickness = 3)]
        public IndicatorDataSeries DownTrend { get; set; }

        private IndicatorDataSeries _upBuffer;
        private IndicatorDataSeries _downBuffer;
        private AverageTrueRange _averageTrueRange;
        private int[] _trend;
        private bool _changeofTrend;
        private Bars customBars;
        private double median, atr;

        protected override void Initialize()
        {
            customBars = MarketData.GetBars(timeFrame);
            _trend = new int[1];
            _upBuffer = CreateDataSeries();
            _downBuffer = CreateDataSeries();
            _averageTrueRange = Indicators.AverageTrueRange(customBars, Period, MovingAverageType.Simple);
        }


        public override void Calculate(int index)
        {
            UpTrend[index] = double.NaN;
            DownTrend[index] = double.NaN;

            int customIndex = customBars.OpenTimes.GetIndexByTime(Bars.OpenTimes[index]);
            median = (customBars.HighPrices[customIndex] + customBars.LowPrices[customIndex]) / 2;
            atr = _averageTrueRange.Result[customIndex];

            _upBuffer[index] = median + Multiplier * atr;
            _downBuffer[index] = median - Multiplier * atr;


            if (index < 1)
            {
                _trend[index] = 1;
                return;
            }

            Array.Resize(ref _trend, _trend.Length + 1);

            if (customBars.ClosePrices[customIndex] > _upBuffer[index - 1])
            {
                _trend[index] = 1;
                if (_trend[index - 1] == -1)
                    _changeofTrend = true;
            }
            else if (customBars.ClosePrices[customIndex] < _downBuffer[index - 1])
            {
                _trend[index] = -1;
                if (_trend[index - 1] == -1)
                    _changeofTrend = true;
            }
            else if (_trend[index - 1] == 1)
            {
                _trend[index] = 1;
                _changeofTrend = false;
            }
            else if (_trend[index - 1] == -1)
            {
                _trend[index] = -1;
                _changeofTrend = false;
            }

            if (_trend[index] < 0 && _trend[index - 1] > 0)
                _upBuffer[index] = median + (Multiplier * atr);
            else if (_trend[index] < 0 && _upBuffer[index] > _upBuffer[index - 1])
                _upBuffer[index] = _upBuffer[index - 1];

            if (_trend[index] > 0 && _trend[index - 1] < 0)
                _downBuffer[index] = median - (Multiplier * atr);
            else if (_trend[index] > 0 && _downBuffer[index] < _downBuffer[index - 1])
                _downBuffer[index] = _downBuffer[index - 1];

            if (_trend[index] == 1)
            {
                UpTrend[index] = _downBuffer[index];
                if (_changeofTrend)
                {
                    UpTrend[index - 1] = DownTrend[index - 1];
                    _changeofTrend = false;
                }
            }
            else if (_trend[index] == -1)
            {
                DownTrend[index] = _upBuffer[index];
                if (_changeofTrend)
                {
                    DownTrend[index - 1] = UpTrend[index - 1];
                    _changeofTrend = false;
                }
            }
        }
    }
}
