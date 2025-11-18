using Hotel.Entities.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Hotel.Entities.ValueObjects
{
    /*a Value Object represents a concept in your business domain.
    For a booking system, a date interval(Check-In → Check-Out) is a real business concept.*/
    public class DateRange
    {
        [Required]
        public DateTime From { get; init; }

        [Required]
        public DateTime To { get; init; }

        public DateRange(DateTime from, DateTime to)
        {
            if (to <= from)
                throw new ArgumentException("Check-out date must be after check-in.");

            From = from;
            To = to;
        }

        public int TotalNights => (To - From).Days;
    }
}
