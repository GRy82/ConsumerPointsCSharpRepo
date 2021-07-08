# ConsumerPointsCSharpRepo
ConsumerPoints is a backend web service handling the transactions of a consumer rewards program. In this program, points are earned with purchases, and these points then have the potential to be used to save money on future purchases. This web service is hypothetically facing the accounting team of the company facilitating the rewards program. They are concerned with which companies a given customer's points are attributed to, and which past transaction those points are associated with.  

### Fundamental Premises
- Points from the oldest transactions are spent first.
- No payer's balance is allowed to go into the negative for a given customer.

## Operation Procedures
This web service is operated by interacting with a database. During the development of this web service, it was tested and implemented with a locally-hosted database, using Microsoft SQL Server within Visual Studio 2019 Community. So it is recommended that this be the method for storing persistent data when using this web service. 

### Getting Started
First, what you will need to do is create a folder where you want to store the source code of this web service. Then you can 'clone' the project into that location, and open it from there. All of this can be done from a command-line interface or "terminal" with the commands that we will go over below.

- Go to the location where you want to store the project by 'changing directory' to that location.
  example:
 > cd /documents/projects

- Create a new folder, naming it appropriately, by 'making directory'.
  example:
 > mkdir ConsumerPoints

- Change directory once more to execute commands from inside that new folder/directory. The period after cd communicates to the computer that you're searching for the ConsumerPoints folder within the folder you are currently in. 
  example:
 > cd ./ConsumerPoints

- From this repository's main page, you can copy the url, or click the 'clone' drop-down to copy it from there. This url will be used to acquire the repository.  

- Back within the terminal, making sure you're still in the correct folder, execute the following command to clone the project.
 
 > git clone https://github.com/GRy82/ConsumerPointsCSharpRepo

- Voila! You should now have the project within the folder designated, whether you named the folder 'ConsumerPoints' or something else. You will need to install Visual Studio 2019 Community if you do not already have it. But once you do, just double click the .sln file or 'solution' within the folder to open the project. 

### Setup Your Database
In order to generate a new database with tables to store data for this web service, you will need to run the command, "update-database" in the terminal or 'Developer PowerShell' of visual studio. If you do not see this component of the user interface, go to the 'view' menu and click 'terminal'. Once you run the "update-database" command, it will use files already present within the project to structure the database. You will know you're successful if you see in the terminal, 'Build Succeeded.' and 'Done.'

### Running the Web Service
Test the application to make sure it is running properly, and that everything has been setup correctly to this point. To do this, you will press the green 'play' button at the top of the window, just below the menus. It may take several seconds to initialize, but once it does, you can expect a browser window to open up(default browser can be changed to one of your choice later). The browser window should be empty, white space, except for a pair of curly brackets, "{}". This indicates that there is no data in a certain table in the database. Great! Now, close the window or press the red 'stop' button to stop the application for now. You will run it again later, prior to making any interactions with the database.  Infact, it will need to be running any time that you do. 

### The ConsumerPoints endpoint
To interact with the web service with actions such as adding transactions and spending points, the endpoints of this web service must be accessed.  This can be done with a separate front end application that exposes the functionality of the web service. This of course requires a software developer. For those not of the skillset, or those looking to use the service for short-term duration, using an endpoint testing software will be more simplistic, and is much quicker to get up-and-running. I recommend using the software, Postman, for these purposes. The directions that follow are written with the assumption that you are using Postman, or a comparable software tool. A browser version of Postman can be found on the web for free, as well as an application that can be downloaded and installed.

In Postman, in the text input field, you will provide the url for the endpoint.  If accessing a locally-hosted database, this url will start with https://localhost:[port-number]

[port-number] will be substituted with the respective port you are using. 

Regardless of how you access the database, the endpoint will read:  /api/points

Altogether, given a locally-hosted database, https://localhost:[port-number]/api/points is what should occupy the text field. 

### Get Payer Balances
To get a readout or list of payers and each corresponding balance, you will use the 'GET' request in postman. Select this from the drop-down menu to the left of the text field. Press send, and expect to see a response body. The body may look something like this:

{"DANNON":1000,"MILLER COORS":5300,"UNILEVER":0}

If there are no points present for this customer in the database, you will see empty curly brackets as a response, representing this absence("{}").

### Add Transaction
To add a transaction, you must use a 'POST' request from the drop-down menu. To be successful, you will need to provide a textual body of your own in the larger text box. Directly above this large box, you must have the following options selected: 'Body', 'raw', and 'Json'(the latter two from drop-downs) See example below for what your text body should resemble:

{
  "Payer": "DANNON",
  "Points": 300,
  "Timestamp": "2021-04-03T10:00:00Z"
}

Note the format. Note where quotations are, and where they are not. Note the format representing the time of the transaction. Note where the commas are. 

You can expect a response of the same format if the transaction is added successfully, and you will receive an error message if the opposite is true.

### Spend Points
To spend points, and create a deduction from the payer balances in the database, you will use a 'PUT' request. Enter the number of points being spent in the same text body, and same settings as before. See example below:

{
  "Points": 5000
}

If successful, you will receive a response that specifies what payer/company points were deducted from, and how many from each. Otherwise, an error message will specify what went wrong. 
