using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private ITransactionStorage _transactionStorage;

        public PointsController(ITransactionStorage transactionStorage)
        {
            _transactionStorage = transactionStorage;
        }

        [HttpPost]
        public IActionResult AddTransactions([FromBody] List<Transaction> transactions)
        {
            _transactionStorage.AddTransactions(transactions);

            return Ok(transactions);
        }


        public IActionResult GetPayerBalances()
        {
            var payerBalances = _transactionStorage.GetPayerBalances();

            return Ok(payerBalances);
        }

        [HttpPut]
        public IActionResult SpendPoints(int pointsToBeSpent)
        {
            var expendituresByPayer = _transactionStorage.SpendPoints(pointsToBeSpent);

            return Ok(expendituresByPayer);
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(LocalMemOperations.transactions);
        }
    }
}
