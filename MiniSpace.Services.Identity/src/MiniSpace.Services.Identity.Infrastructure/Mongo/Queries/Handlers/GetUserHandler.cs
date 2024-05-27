using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using MiniSpace.Services.Identity.Application.DTO;
using MiniSpace.Services.Identity.Application.Queries;
using MiniSpace.Services.Identity.Infrastructure.Mongo.Documents;

namespace MiniSpace.Services.Identity.Infrastructure.Mongo.Queries.Handlers
{
    [ExcludeFromCodeCoverage]
    internal sealed  class GetUserHandler : IQueryHandler<GetUser, UserDto>
    {
        private readonly IMongoRepository<UserDocument, Guid> _userRepository;

        public GetUserHandler(IMongoRepository<UserDocument, Guid> userRepository)
        {
            _userRepository = userRepository;
        }
        
        public async Task<UserDto> HandleAsync(GetUser query)
        {
            var user = await _userRepository.GetAsync(query.UserId);

            return user?.AsDto();
        }
    }
}