using WebApplication1.Models;

namespace WebApplication1.Strategy
{
    public interface IMediaValidationStrategy
    {
        void Validate(Media entity);
    }
}
