using System.Globalization;

namespace Helper
{
    static public class NumericExtensions
    {
        /// <summary>
        /// Usage: 
        /// 
        /// var Numerator = 100;
        /// var Denominator = 0;
        ///
        /// var SampleResult1 = NumericExtensions.SafeDivision(Numerator, Denominator);
        ///
        /// var SampleResult2 = Numerator.SafeDivision(Denominator);
        /// 
        /// </summary>
        /// <param name="Numerator"></param>
        /// <param name="Denominator"></param>
        /// <returns></returns>
        static public decimal SafeDivision(this decimal Numerator, decimal Denominator)
        {
            return (Denominator == 0) ? 0 : Numerator / Denominator;
        }

        static public decimal? CustomParseDecimal(string incomingValue)
        {
            decimal val;
            if (!decimal.TryParse(incomingValue.Replace(",", "").Replace(".", ""), NumberStyles.Number, CultureInfo.InvariantCulture, out val))
                return null;
            return val / 100;
        }
    }
}
