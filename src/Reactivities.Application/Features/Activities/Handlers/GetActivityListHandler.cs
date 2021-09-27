using MediatR;
using Microsoft.EntityFrameworkCore;
using Reactivities.Application.Features.Activities.Queries;
using Reactivities.Domain.Entities;
using Reactivities.Infrastructure.Data;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Reactivities.Application.Features.Activities.Handlers
{
    public class GetActivityListHandler : IRequestHandler<GetActivityListQuery, List<Activity>>
    {
        private readonly ApplicationDbContext _context;

        public GetActivityListHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Activity>> Handle(GetActivityListQuery request, CancellationToken cancellationToken)
        {
            return await _context.Activities.ToListAsync(cancellationToken);
        }
    }
}
