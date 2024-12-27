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

        [Parameter("MA Type", DefaultValue = "Exponential")]
        public MovingAverageType MaType { get; set; }

        [Parameter("ATR Period", DefaultValue = 200)]
        public int AtrPeriod { get; set; }

        [Parameter("ATR MA Type", DefaultValue = "Exponential")]
        public MovingAverageType AtrMaType { get; set; }

        [Parameter("Band Distance", DefaultValue = 3)]
        public double BandDistance { get; set; }

        [Parameter("Shift", DefaultValue = 0)]
        public int Shift { get; set; }

        [Parameter("Show Text", DefaultValue = false)]
        public bool ShowText { get; set; }

        [Parameter("Volume Calculated", DefaultValue = 1000)]
        public int VolumeCalculated { get; set; }

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

            var pipValue = Symbol.PipValue * VolumeCalculated;

            // Calculate and display bands
            double[][] bands = new double[4][];
            bands[0] = new double[2] { maVal + BandDistance * atrVal, maVal - BandDistance * atrVal };
            bands[1] = new double[2] { maVal + 2 * BandDistance * atrVal, maVal - 2 * BandDistance * atrVal };
            bands[2] = new double[2] { maVal + 3 * BandDistance * atrVal, maVal - 3 * BandDistance * atrVal };
            bands[3] = new double[2] { maVal + 4 * BandDistance * atrVal, maVal - 4 * BandDistance * atrVal };

            UpperBand1[index] = bands[0][0];
            LowerBand1[index] = bands[0][1];
            UpperBand2[index] = bands[1][0];
            LowerBand2[index] = bands[1][1];
            UpperBand3[index] = bands[2][0];
            LowerBand3[index] = bands[2][1];
            UpperBand4[index] = bands[3][0];
            LowerBand4[index] = bands[3][1];

            //1
            if (ShowText)
            {
                double bandDistance = Math.Abs(MiddleBand[index] - LowerBand1[index]);
                double priceDiff = bandDistance / Symbol.PipSize * pipValue;
                var lane1 = priceDiff;
                var lane2 = priceDiff * 2;
                var lane3 = priceDiff * 3;
                var lane4 = priceDiff * 4;

            
                Chart.DrawText($"txt_u_1", $"{priceDiff:C}", index, UpperBand1[index], Color.White);
                Chart.DrawText($"txt_u_5", $"{priceDiff:C}", index, LowerBand1[index], Color.White);
    
                priceDiff = lane1 + lane2;
                Chart.DrawText($"txt_u_2", $"{lane2:C} | {priceDiff:C}", index, UpperBand2[index], Color.White);
                Chart.DrawText($"txt_u_6", $"{lane2:C} | {priceDiff:C}", index, LowerBand2[index], Color.White);
    
                priceDiff = lane1 + lane2 + lane3;
                Chart.DrawText($"txt_u_3", $"{lane3:C} | {priceDiff:C}", index, UpperBand3[index], Color.White);
                Chart.DrawText($"txt_u_7", $"{lane3:C} | {priceDiff:C}", index, LowerBand3[index], Color.White);
    
                priceDiff = lane1 + lane2 + lane3 + lane4;
                Chart.DrawText($"txt_u_4", $"{lane4:C} | {priceDiff:C}", index, UpperBand4[index], Color.White);
                Chart.DrawText($"txt_u_8", $"{lane4:C} | {priceDiff:C}", index, LowerBand4[index], Color.White);
            }
        }
    }
}