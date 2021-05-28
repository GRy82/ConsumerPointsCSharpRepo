using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsumerPoints.Data;
using ConsumerPoints.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace ConsumerPoints.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class PointsController : ControllerBase
    {

        private readonly List<Transaction> transactions = new List<Transaction>() {

            new Transaction { Payer="CVS Pharmacy", Points=200, Timestamp=new DateTime(2021, 01, 15) },
            new Transaction { Payer="CVS Pharmacy", Points=400, Timestamp=new DateTime(2020, 12, 25) } 
        };

        public PointsController()
        {
            
        }

        [HttpPost]
        public IActionResult AddTransaction([FromBody] Transaction transaction)
        {


            return Ok(transaction);
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(transactions);
        }
    }
}
