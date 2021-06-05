using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ConsumerPoints.Models
{
    public class Transaction
    {
        [ForeignKey("PayerPoints")]
        public string Payer { get; set; }

        public int Points { get; set; }

        [Key]
        public DateTime Timestamp { get; set; }

        
        public int UnspentPoints { get; set; }
    }
}
