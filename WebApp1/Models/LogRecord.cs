using System;
using System.Collections.Generic;

namespace WebApp1.Models
{
    public partial class LogRecord
    {
        public int Id { get; set; }

        public DateTime? Timestamp { get; set; }

        public int? ReservationId { get; set; }

        public int? RoomId { get; set; }

        public virtual Reservation? Reservation { get; set; }

        public virtual Room? Room { get; set; }
    }
}
