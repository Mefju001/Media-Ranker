using Application.Common.Interfaces;
using Application.Features.Common.Interfaces;
using Domain.Aggregate;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Features.Liked.Add
{
    internal class AddHandler : IRequestHandler<AddCommand, bool>
    {
        private readonly ILogger<AddHandler> logger;
        private readonly IMediaRepository<Media> mediaRepository;
        private readonly IUserDetailsRepository userDetailsRepository;
        private readonly IAppDbContext _context;

        public AddHandler(ILogger<AddHandler> logger, IMediaRepository<Media> mediaRepository, IUserDetailsRepository userDetailsRepository, IAppDbContext context)
        {
            this.logger = logger;
            this.mediaRepository = mediaRepository;
            this.userDetailsRepository = userDetailsRepository;
            this._context = context;
        }

        public async Task<bool> Handle(AddCommand request, CancellationToken cancellationToken)
        {
            var user = await userDetailsRepository.GetByIdAsync(request.UserId, cancellationToken);
            if (user is null)
            {
                logger.LogWarning("User not found. UserId: {UserId}", request.UserId);
                throw new NotFoundException("User not found.");
            }
            var media = await mediaRepository.ExistById(request.MediaId, cancellationToken);
            if (media is false)
            {
                logger.LogWarning("Media not found. MediaId: {MediaId}", request.MediaId);
                throw new NotFoundException("Media not found.");
            }
            user.SetRatingVote(request.MediaId, ERatingVote.Liked);
            var debugInteractionsCount = user.UserInteractions.Count;
            var isTracked = _context.Context.ChangeTracker.Entries<UserDetails>().Any(e => e.Entity.Id == user.Id);

            logger.LogInformation("Liczba interakcji w pamięci: {Count}, Czy EF śledzi użytkownika: {IsTracked}",
                debugInteractionsCount, isTracked);
            return true;
        }
    }
}
