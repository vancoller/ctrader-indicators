using System;
using cAlgo.API;
using cAlgo.API.Internals;
using cAlgo.API.Indicators;
using cAlgo.Indicators;

namespace cAlgo
{
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class VC_GMMA : Indicator
    {
        [Output("Short EMA1", LineColor = "#FF0000FF")]
        public IndicatorDataSeries ShortEma1 { get; set; }

        [Output("Short EMA2", LineColor = "#FF0000FF")]
        public IndicatorDataSeries ShortEma2 { get; set; }

        [Output("Short EMA3", LineColor = "#FF0000FF")]
        public IndicatorDataSeries ShortEma3 { get; set; }

        [Output("Short EMA4", LineColor = "#FF0000FF")]
        public IndicatorDataSeries ShortEma4 { get; set; }

        [Output("Short EMA5", LineColor = "#FF0000FF")]
        public IndicatorDataSeries ShortEma5 { get; set; }

        [Output("Short EMA6", LineColor = "#FF0000FF")]
        public IndicatorDataSeries ShortEma6 { get; set; }


        [Output("Long EMA1", LineColor = "#FFFF0000")]
        public IndicatorDataSeries LongEma1 { get; set; }

        [Output("Long EMA2", LineColor = "#FFFF0000")]
        public IndicatorDataSeries LongEma2 { get; set; }

        [Output("Long EMA3", LineColor = "#FFFF0000")]
        public IndicatorDataSeries LongEma3 { get; set; }

        [Output("Long EMA4", LineColor = "#FFFF0000")]
        public IndicatorDataSeries LongEma4 { get; set; }

        [Output("Long EMA5", LineColor = "#FFFF0000")]
        public IndicatorDataSeries LongEma5 { get; set; }

        [Output("Long EMA6", LineColor = "#FFFF0000")]
        public IndicatorDataSeries LongEma6 { get; set; }


        private ExponentialMovingAverage m_shortEma1;
        private ExponentialMovingAverage m_shortEma2;
        private ExponentialMovingAverage m_shortEma3;
        private ExponentialMovingAverage m_shortEma4;
        private ExponentialMovingAverage m_shortEma5;
        private ExponentialMovingAverage m_shortEma6;

        private ExponentialMovingAverage m_longEma1;
        private ExponentialMovingAverage m_longEma2;
        private ExponentialMovingAverage m_longEma3;
        private ExponentialMovingAverage m_longEma4;
        private ExponentialMovingAverage m_longEma5;
        private ExponentialMovingAverage m_longEma6;

        protected override void Initialize()
        {
            m_shortEma1 = Indicators.ExponentialMovingAverage(Bars.ClosePrices, 3);
            m_shortEma2 = Indicators.ExponentialMovingAverage(Bars.ClosePrices, 5);
            m_shortEma3 = Indicators.ExponentialMovingAverage(Bars.ClosePrices, 8);
            m_shortEma4 = Indicators.ExponentialMovingAverage(Bars.ClosePrices, 10);
            m_shortEma5 = Indicators.ExponentialMovingAverage(Bars.ClosePrices, 12);
            m_shortEma6 = Indicators.ExponentialMovingAverage(Bars.ClosePrices, 15);

            m_longEma1 = Indicators.ExponentialMovingAverage(Bars.ClosePrices, 30);
            m_longEma2 = Indicators.ExponentialMovingAverage(Bars.ClosePrices, 35);
            m_longEma3 = Indicators.ExponentialMovingAverage(Bars.ClosePrices, 40);
            m_longEma4 = Indicators.ExponentialMovingAverage(Bars.ClosePrices, 45);
            m_longEma5 = Indicators.ExponentialMovingAverage(Bars.ClosePrices, 50);
            m_longEma6 = Indicators.ExponentialMovingAverage(Bars.ClosePrices, 60);
        }

        public override void Calculate(int index)
        {
            ShortEma1[index] = m_shortEma1.Result[index];
            ShortEma2[index] = m_shortEma2.Result[index];
            ShortEma3[index] = m_shortEma3.Result[index];
            ShortEma4[index] = m_shortEma4.Result[index];
            ShortEma5[index] = m_shortEma5.Result[index];
            ShortEma6[index] = m_shortEma6.Result[index];

            LongEma1[index] = m_longEma1.Result[index];
            LongEma2[index] = m_longEma2.Result[index];
            LongEma3[index] = m_longEma3.Result[index];
            LongEma4[index] = m_longEma4.Result[index];
            LongEma5[index] = m_longEma5.Result[index];
            LongEma6[index] = m_longEma6.Result[index];
        }
    }
}

