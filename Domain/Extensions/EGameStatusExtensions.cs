using Domain.Enums;

namespace Domain.Extensions
{
    public static class EGameStatusExtensions
    {
        public static EGameStatus ToEnum(this string strEnumValue)
        {
            Enum.TryParse<EGameStatus>(strEnumValue, true, out var parsedEnum);
            return parsedEnum;
        }
    }
}
