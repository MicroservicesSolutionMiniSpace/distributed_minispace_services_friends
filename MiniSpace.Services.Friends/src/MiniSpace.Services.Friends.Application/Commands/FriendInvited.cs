using System.Windows.Input;
using Convey.CQRS.Events;

namespace MiniSpace.Services.Friends.Application.Events
{
    public class FriendInvited : ICommand
    {
        public Guid InviterId { get; }
        public Guid InviteeId { get; }

        public FriendInvited(Guid inviterId, Guid inviteeId)
        {
            InviterId = inviterId;
            InviteeId = inviteeId;
        }
    }
}
