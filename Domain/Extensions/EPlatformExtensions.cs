using Domain.Enums;

namespace Domain.Extensions
{
    public static class EPlatformExtensions
    {
        public static List<EPlatform> ToEnum(this List<string> strEnumValues)
        {
            var enumPlatforms = new List<EPlatform>();

            foreach (string strEnumValue in strEnumValues)
            {
                if (Enum.TryParse<EPlatform>(strEnumValue, true, out var parsedEnum))
                {
                    enumPlatforms.Add(parsedEnum);
                }
            }

            return enumPlatforms;
        }
        public static EPlatform ToEnum(this string strEnumValue)
        {
            Enum.TryParse<EPlatform>(strEnumValue, true, out var parsedEnum);
            return parsedEnum;
        }
    }
}
