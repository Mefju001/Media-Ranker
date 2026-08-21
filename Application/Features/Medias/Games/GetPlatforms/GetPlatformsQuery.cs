using Application.Features.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Medias.Games.GetPlatforms
{
    public record GetPlatformsQuery : IQuery<List<string>>;
}
