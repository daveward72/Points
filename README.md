Fetch Rewards Coding Exercise - David Ward


I completed this assignment using C# on .NET 6.0.

.NET 6.0 is in a preview release. In order to run it you must first download and install the .NET SDK:
https://dotnet.microsoft.com/download/dotnet/6.0

It can be installed for Windows, macOS or Linux.

To run the web service, first clone the repository from github.

Then you will need to navigate locally to \Fetch\Points\Points

Run this on the command line:
dotnet run

This will host the service on localhost.
https://localhost:5001/api is the endpoint.

You can also navigate to swagger for testing the service:
https://localhost:5001/swagger/index.html

The swagger page is helpful for showing the routes and sample JSON values.

The supported requests are:

GET /api/Balances
POST /api/Spends
POST /api/Transactions
