using Application.Features.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Watching.Add
{
    public record AddCommand(Guid userId,Guid mediaId):ICommand<bool>;
}
