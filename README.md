# ConsumerPointsCSharpRepo
ConsumerPoints is a backend web service handling the transactions of a consumer rewards program. In this program, points are earned with purchases, and these points then have the potential to be used to save money on future purchases. This web service is hypothetically facing the accounting team of the company facilitating the rewards program. They are concerned with which companies a given customer's points are attributed to, and which past transaction those points are associated with.  

### Fundamental Premises
- Points from the oldest transactions are spent first.
- No payer's balance is allowed to go into the negative for a given customer.

## Operation Procedures
This web service is operated by interacting with a database. During the development of this web service, it was tested and implemented using a locally-hosted database, using Microsoft SQL Server. So it is recommended that this be the method for storing persistent data when using this web service. 

To interact with the web service with actions such as adding transactions, and spending points, the endpoints of this web service must be accessed.  This can be done with a separate front end application that exposes the functionality of the web service. This of course requires a software developer. For those not of the skillset, or those looking to use the service for short-term duration, using an endpoint testing software will be more simplistic, and is much quicker to get up-and-running. I recommend using the software, Postman, for these purposes.  The directions that follow are written with the assumption that you are using Postman, or a comparable software tool.  

### The ConsumerPoints endpoint
In Postman, in the text input field, you will provide the url for the endpoint.  If accessing a locally-hosted database, this url will start with https://localhost:[port-number]

[port-number] will be substituted with the respective port you are using. 

Regardless of how you access the database, the endpoint will read:  /api/points

Altogether, given a locally-hosted database, https://localhost:[port-number]/api/points is what should occupy the text field. 

### Get Payer Balances
To get a readout or list of payers and each corresponding balance, you will use the 'GET' request in postman. Select from the drop-down menu to the left of the text field. Press send, and expect to see a response body. The body may look something like this:

{"DANNON":1000,"MILLER COORS":5300,"UNILEVER":0}

### Add Transaction
To add a transaction, you must use a 'POST' request. To be successful, you will need to provide a textual body of your own in the large text box. See example below:

{
  "Payer": "DANNON",
  "Points": 300,
  "Timestamp": "2021-04-03T10:00:00Z"
}

Note the format. Note where quotations are, and where they are not. Note the format representing the time of the transaction. Not where the commas are. 

You can expect a response of the same format if the transaction is added successfully, and you will receive an error message if the opposite is true.

### Spend Points
To spend points, and create a deduction from the payer balances in the database, you will use a 'PUT' request. Enter the number of points being spent in the same text body as before. See example below:

{
  "Points": 5000
}

If successful, you will receive a response that specifies what payer/company points were deducted from, and how many from each. Otherwise, an error message will specify what went wrong. 

