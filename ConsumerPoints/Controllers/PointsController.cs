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
            _transactionStorage.AddTransaction(transaction);

            return Ok(transaction);
        }

        //public IActionResult GetTransactions()
        //{
        //    var thing = _transactionStorage.GetTransactions();

        //    return Ok(thing);
        //}

        [HttpGet]
        public IActionResult GetPayerBalances()
        {
            List<PayerPoints> payerBalances = _transactionStorage.GetPayerBalances();

            return Ok(payerBalances);
        }


        [HttpPut]
        public IActionResult SpendPoints(int withdrawal)
        {
            var expendituresByPayer = _transactionStorage.SpendPoints(withdrawal);

            return Ok(expendituresByPayer);
        }
    }
}
