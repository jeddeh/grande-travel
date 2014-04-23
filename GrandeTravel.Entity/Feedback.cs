using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrandeTravel.Entity
{
    public class Feedback
    {
        // Properties
        public int FeedbackId { get; set; }

        public int BookingId { get; set; }
        public virtual Booking Booking { get; set; }

        public string Message { get; set; }
    }
}
