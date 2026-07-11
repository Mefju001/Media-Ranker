using Domain.Enums;

namespace Domain.Extensions
{
    public static class EDistributionTypeExtensions
    {
        public static EDistributionType ToDistributionType(this string distributionType)
        {
            var EDistritionType = Enum.TryParse<EDistributionType>(distributionType, true, out var result);
            return result;
        }
    }
}
