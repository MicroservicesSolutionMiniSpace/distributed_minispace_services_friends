using Convey.CQRS.Events;
using Convey.MessageBrokers;

namespace MiniSpace.Services.Friends.Application.Events.External
{
    [Message("notifications")]
    public class PendingFriendAccepted : IEvent
    {
        public Guid RequesterId { get; }
        public Guid FriendId { get; }

        public PendingFriendAccepted(Guid requesterId, Guid friendId)
        {
            RequesterId = requesterId;
            FriendId = friendId;
        }
    }
}
