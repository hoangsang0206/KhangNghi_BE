namespace KhangNghi_BE.Utils
{
    public static class DateTimeUtils
    {
        public static DateTime ConvertUnixTimeToDateTime(this long unixTime)
        {
            return DateTimeOffset.FromUnixTimeSeconds(unixTime).DateTime;
        }
    }
}
