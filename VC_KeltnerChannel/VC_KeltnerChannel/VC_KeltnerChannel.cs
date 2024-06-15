using cAlgo.API;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;
using System;

namespace cAlgo
{
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None, AutoRescale = false)]
    public class VC_KeltnerChannel : Indicator
    {
        [Parameter("MA Period", DefaultValue = 200)]
        public int MaPeriod { get; set; }

        [Parameter("MA Type", DefaultValue = "Simple")]
        public MovingAverageType MaType { get; set; }

        [Parameter("ATR Period", DefaultValue = 200)]
        public int AtrPeriod { get; set; }

        [Parameter("ATR MA Type", DefaultValue = "Simple")]
        public MovingAverageType AtrMaType { get; set; }

        [Parameter("Band Distance", DefaultValue = 5)]
        public double BandDistance { get; set; }

        [Parameter("Shift", DefaultValue = 0)]
        public int Shift { get; set; }

        [Output("Upper Band 1", LineColor = "#FFFFFF01", LineStyle = LineStyle.Lines, Thickness = 1)]
        public IndicatorDataSeries UpperBand1 { get; set; }

        [Output("Upper Band 2", LineColor = "#FFFF9E00", LineStyle = LineStyle.Lines, Thickness = 1)]
        public IndicatorDataSeries UpperBand2 { get; set; }

        [Output("Upper Band 3", LineColor = "#FFFF4E00", LineStyle = LineStyle.Lines, Thickness = 1)]
        public IndicatorDataSeries UpperBand3 { get; set; }

        [Output("Upper Band 4", LineColor = "#FFFF0000", LineStyle = LineStyle.Lines, Thickness = 2)]
        public IndicatorDataSeries UpperBand4 { get; set; }



        [Output("Lower Band 1", LineColor = "#FFFFFF01", LineStyle = LineStyle.Lines, Thickness = 1)]
        public IndicatorDataSeries LowerBand1 { get; set; }

        [Output("Lower Band 2", LineColor = "#FFFF9E00", LineStyle = LineStyle.Lines, Thickness = 1)]
        public IndicatorDataSeries LowerBand2 { get; set; }

        [Output("Lower Band 3", LineColor = "#FFFF4E00", LineStyle = LineStyle.Lines, Thickness = 1)]
        public IndicatorDataSeries LowerBand3 { get; set; }

        [Output("Lower Band 4", LineColor = "#FFFF0000", LineStyle = LineStyle.Lines, Thickness = 2)]
        public IndicatorDataSeries LowerBand4 { get; set; }


        [Output("Middle Band", LineColor = "Green", Thickness = 2)]
        public IndicatorDataSeries MiddleBand { get; set; }

        private MovingAverage _ma;
        private AverageTrueRange _atr;

        protected override void Initialize()
        {
            _ma = Indicators.MovingAverage(Bars.ClosePrices, MaPeriod, MaType);
            _atr = Indicators.AverageTrueRange(AtrPeriod, AtrMaType);
        }

        public override void Calculate(int index)
        {
            int shiftedIndex = index - Shift;
            if (shiftedIndex < 0)
                return;

            var atrVal = _atr.Result[shiftedIndex];
            var maVal = _ma.Result[shiftedIndex];
            MiddleBand[index] = maVal;

            var distance = BandDistance * 1;
            UpperBand1[index] = maVal + distance * atrVal;
            LowerBand1[index] = maVal - distance * atrVal;

            distance = BandDistance * 2;
            UpperBand2[index] = maVal + distance * atrVal;
            LowerBand2[index] = maVal - distance * atrVal;

            distance = BandDistance * 3;
            UpperBand3[index] = maVal + distance * atrVal;
            LowerBand3[index] = maVal - distance * atrVal;

            distance = BandDistance * 4;
            UpperBand4[index] = maVal + distance * atrVal;
            LowerBand4[index] = maVal - distance * atrVal;
        }
    }
}
