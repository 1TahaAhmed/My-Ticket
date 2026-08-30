namespace TicketBooking.Domain.Enums
{
    public enum SupportTicketPriority
    {
        Low = 1,
        Medium = 2,
        High = 3,
        Urgent = 4
    }

    public enum SupportTicketStatus
    {
        Open = 1,
        InProgress = 2,
        PendingUser = 3,
        Resolved = 4,
        Closed = 5
    }

    public enum DisputeStatus
    {
        Open = 1,
        UnderReview = 2,
        Resolved = 3,
        Rejected = 4
    }

    public enum DisputeResolution
    {
        RefundBuyer = 1,     
        PayoutSeller = 2,    
        SplitAmount = 3,     
        Rejected = 4         
    }
}