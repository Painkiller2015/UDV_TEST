namespace UDV_TEST.Services
{
    public static class Date_Service
    {
        public static string TicksToDateTimeString(long? timeTicks)
        {
            if (timeTicks is null)
                return "";

            return new DateTime((long)timeTicks).ToString("dd.MM.yy HH:mm");
        }
        public static string DateTimeToString(DateTime dateTime)
        {
            return dateTime.ToString("dd.MM.yy HH:mm");
        }
    }
}
