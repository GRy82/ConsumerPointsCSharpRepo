using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ConsumerPoints.Data;
using ConsumerPoints.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ConsumerPoints.Interfaces;


namespace ConsumerPoints.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class PointsController : ControllerBase
    {
        ITransactionStorage _transactionStorage;

        public PointsController(ITransactionStorage transactionStorage)
        {
            _transactionStorage = transactionStorage;
        }

        [HttpPost]
        public IActionResult AddTransaction([FromBody] Transaction transaction)
        {
            try
            {
                _transactionStorage.AddTransaction(transaction);
                return Ok(transaction);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        public IActionResult GetPayerBalances()
        {
            var payerBalances = _transactionStorage.GetPayerBalances();

            return Ok(payerBalances);
        }

        
        [HttpPut]
        public IActionResult SpendPoints([FromBody] Expenditure spendRequest)
        {
            List<PayerPoints> expendituresByPayer;

            try
            {
                expendituresByPayer = _transactionStorage.SpendPoints(spendRequest.Points);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
            
            return Ok(expendituresByPayer);
        }
    }
}
