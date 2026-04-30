using System.Collections.Generic;

namespace Effanville.FinancialStructures.Stocks.Download
{
    public static class DictionaryExtensions
    {
        public static bool TryGetDoubleValue(this Dictionary<string, string> map, string key, out double value,
            double defaultValue = double.NaN)
        {
            if (map.TryGetValue(key, out string val))
            {
                if (double.TryParse(val, out double eps))
                {
                    value = eps;
                    return true;
                }
            }

            value = defaultValue;
            return false;
        }
    }
}