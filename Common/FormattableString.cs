using System;
using System.Globalization;

namespace Common
{
    public static class FormattableString
    {

        public static string Invariant(IFormattable formattable) => formattable?.ToString(null, CultureInfo.InvariantCulture);

    }
}
