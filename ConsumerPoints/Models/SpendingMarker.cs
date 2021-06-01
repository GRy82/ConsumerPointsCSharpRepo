using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConsumerPoints.Models
{
    public class SpendingMarker
    {
        [Key]
        public int Id { get; set; }
        public DateTime LastSpentDate { get; set; }
        public bool LastWasPartiallySpent { get; set; }
        public int Remainder { get; set; }
    }
}
