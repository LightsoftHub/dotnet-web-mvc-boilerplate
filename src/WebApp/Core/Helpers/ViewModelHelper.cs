namespace CleanArch.eCode.WebApp.Core.Helpers
{
    public static class ViewModelHelper
    {
        public static string ToFNumber(this int value)
            => $"{value:#,##0}";

        public static string ToFNumber(this long value)
            => $"{value:#,##0}";

        public static string ToFNumber(this double value)
            => $"{value:#,##0}";

        public static string ToFNumber(this decimal value)
            => $"{value:#,##0}";

        public static string ToFDate(this DateTime value)
            => value > new DateTime(1970, 01, 01) ? value.ToString("dd/MM/yyyy") : "";
    }
}