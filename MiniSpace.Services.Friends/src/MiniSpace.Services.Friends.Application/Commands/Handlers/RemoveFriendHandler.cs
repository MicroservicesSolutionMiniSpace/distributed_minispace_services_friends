using Convey.CQRS.Commands;
using MiniSpace.Services.Friends.Application.Events;
using MiniSpace.Services.Friends.Application.Events.External;
using MiniSpace.Services.Friends.Application.Exceptions;
using MiniSpace.Services.Friends.Application.Services;
using MiniSpace.Services.Friends.Core.Repositories;

namespace MiniSpace.Services.Friends.Application.Commands.Handlers
{
    public class RemoveFriendHandler : ICommandHandler<RemoveFriend>
    {
        private readonly IFriendRepository _friendRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly IEventMapper _eventMapper;
        private readonly IAppContext _appContext; 

        public RemoveFriendHandler(IFriendRepository friendRepository, IMessageBroker messageBroker, IEventMapper eventMapper, IAppContext appContext)
        {
            _friendRepository = friendRepository;
            _messageBroker = messageBroker;
            _eventMapper = eventMapper;
            _appContext = appContext; 
        }

        public async Task HandleAsync(RemoveFriend command, CancellationToken cancellationToken = default)
        {
            var identity = _appContext.Identity;
            // if (!identity.IsAuthenticated)
            // {
            //     throw new UnauthorizedFriendActionException(command.RequesterId, identity.Id);
            // }
             Console.WriteLine($"Handling RemoveFriend for RequesterId: {command.RequesterId} and FriendId: {command.FriendId}. Authenticated: {identity.IsAuthenticated}");
    

            var exists = await _friendRepository.IsFriendAsync(command.RequesterId, command.FriendId);
            if (!exists)
            {
                throw new FriendshipNotFoundException(command.RequesterId, command.FriendId);
            }

            // Remove the friendship in both directions
            await _friendRepository.RemoveFriendAsync(command.RequesterId, command.FriendId);
            await _friendRepository.RemoveFriendAsync(command.FriendId, command.RequesterId);

            // Publish an event indicating the friend has been removed
            var eventToPublish = new PendingFriendDeclined(command.RequesterId, command.FriendId);
            await _messageBroker.PublishAsync(eventToPublish);

            // Publish a reciprocal event for the inverse relationship
            var reciprocalEventToPublish = new PendingFriendDeclined(command.FriendId, command.RequesterId);
            await _messageBroker.PublishAsync(reciprocalEventToPublish);
        }
    }
}
