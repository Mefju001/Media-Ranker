using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Recommendation.GetForUser
{
    public interface IRecommendationEngine
    {
        Task<List<Media>> GetMediasAsync(UserProfileData profile, UserPreferencesData prefs, CancellationToken cancellation);
    }
}
