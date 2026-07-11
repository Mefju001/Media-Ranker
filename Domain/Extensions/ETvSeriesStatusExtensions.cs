using Domain.Enums;

namespace Domain.Extensions
{
    public static class ETvSeriesStatusExtensions
    {
        public static ETvSeriesStatus ToEnum(this string strEnumValue)
        {
            var enumValue = Enum.TryParse<ETvSeriesStatus>(strEnumValue, true, out var result) ? result : throw new ArgumentException($"Invalid enum value: {strEnumValue}");
            return enumValue;
        }
    }
}
