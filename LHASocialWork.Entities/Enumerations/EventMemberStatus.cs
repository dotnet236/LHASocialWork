namespace LHASocialWork.Entities.Enumerations
{
    public enum EventMemberStatus
    {
        /// <summary>
        /// Invited by existing user
        /// </summary>
        Invited,
        /// <summary>
        /// Requested To Join
        /// </summary>
        Requested,
        /// <summary>
        /// Joined the event
        /// </summary>
        Confirmed,
        /// <summary>
        /// Decline event invitation
        /// </summary>
        Declined,
        /// <summary>
        /// Attendance: Attended event
        /// </summary>
        Attended,
        /// <summary>
        /// Has not done anything with the respective event
        /// </summary>
        Null,
        /// <summary>
        /// User canceled there request.
        /// </summary>
        RequestCanceled,
        /// <summary>
        /// Removed a confirmed member
        /// </summary>
        Remove,
        /// <summary>
        /// Request was denied.
        /// </summary>
        Denied
    }
}