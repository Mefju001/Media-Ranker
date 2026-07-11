using Domain.Enums;

namespace Domain.Extensions
{
    public static class EMovieStatusExtensions
    {
        public static EMovieStatus ToEnum(this string strEnumValue)
        {
            Enum.TryParse<EMovieStatus>(strEnumValue, true, out var parsedEnum);
            return parsedEnum;
        }
    }
}
