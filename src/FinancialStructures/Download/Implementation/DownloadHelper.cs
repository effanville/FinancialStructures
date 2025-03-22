using System;
using System.Linq;

namespace Effanville.FinancialStructures.Download.Implementation;

/// <summary>
/// Helper methods for downloading financial data.
/// </summary>
public static class DownloadHelper
{
    public static readonly string PenceName = "GBX";
    public static readonly string PoundsName = "GBP";

    public static decimal? ParseDataIntoNumber(string data, int startIndex, int offset, int searchLength, bool includeComma)
    {
        if (startIndex == -1)
        {
            return null;
        }
        int length = Math.Min(searchLength, data.Length - startIndex - offset);
        string shortenedDataString = data.Substring(startIndex + offset, length);
        char[] digits = shortenedDataString.SkipWhile(c => !char.IsDigit(c)).TakeWhile(c => IsNumericValue(c, includeComma)).ToArray();

        string str = new string(digits);
        if (string.IsNullOrEmpty(str))
        {
            return null;
        }

        return decimal.Parse(str);
    }

    private static bool IsNumericValue(char c, bool includeComma)
    {
        if (char.IsDigit(c) || c == '.' || (includeComma && c == ','))
        {
            return true;
        }

        return false;
    }
}
