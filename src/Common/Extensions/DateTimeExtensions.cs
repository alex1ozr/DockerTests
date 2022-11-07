namespace DockerTestsSample.Common.Extensions;

public static class DateTimeExtensions
{
    public static DateTime? SetKindUtc(this DateTime? dateTime)
    {
        if (dateTime.HasValue)
        {
            return dateTime.Value.SetKindUtc();
        }
        else
        {
            return null;
        }
    }

    public static DateTime SetKindUtc(this DateTime dateTime)
    {
        if (dateTime.Kind == DateTimeKind.Utc)
        {
            return dateTime;
        }

        return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
    }
}