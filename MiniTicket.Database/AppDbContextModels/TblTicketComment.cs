using System;
using System.Collections.Generic;

namespace MiniTicket.Database.AppDbContextModels;

public partial class TblTicketComment
{
    public int TicketCommentId { get; set; }

    public int TicketId { get; set; }

    public int UserId { get; set; }

    public string Message { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }
}
