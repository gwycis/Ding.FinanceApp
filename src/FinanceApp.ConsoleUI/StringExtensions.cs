using System;

namespace FinanceApp.ConsoleUI
{
    public static class StringExtensions
    {
        public static string Pad(this string value, int desiredLength)
        {
            if (desiredLength <= 0) throw new ArgumentOutOfRangeException(nameof(desiredLength));
            if (string.IsNullOrWhiteSpace(value) || value.Length > desiredLength)
                return value;

            var distanceToPad = desiredLength - value.Length;
            var adjustment = distanceToPad % 2 == 0 ? 0 : 1;
            var padSize = Convert.ToInt32(Math.Round(distanceToPad / 2d, MidpointRounding.ToZero));
            return value.PadLeft(value.Length + adjustment + padSize).PadRight(value.Length + adjustment + padSize * 2);
        }
    }
}
