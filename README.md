# Checkout Test Assignment

Instructions to run the code:

1- Install DB
	I Assume you have MSSQL Server Developer edition or SQLExpress on your local machine or on shared server.
	
	1.1- Please configure the connection string in the settings to your SQL server instance.
	1.2- Open Powershell command prompt from VS developer tools. 
	1.3- Make sure you have instsalled .NET Core CLI tools. If not installed, then run command:  dotnet tool install --global dotnet-ef
	1.4- To create DB, change directory to CheckoutApi and run command: dotnet ef database update 

2- API Endpoints

	There are 3 endpoints in this solution
	2.1-Token API, Which is needed for authorization. I am using Auth0 as authentication/authorization provider. To retrieve the token, use the following credentials in token request.
		username: Farhana.Jabbar 
		password: Password123!

	Payment APIs are authorized and need Bearer Token in the request header which we have retrieved from the token endpoint:
	2.1- Post Payment API, this will make the transaction and return the payment response;
	2.3- Get Payment API, this will get transaction details using the payment reference.


3- Tests:

	3.1- Unit Tests will run from Test Explorer. 

		Blackbox testing/Acceptance Testing
	3.2- TransactionRepositoryTests: To run the repository Tests, we need to provide the connection string in TransactionRepositoryTests Constructor line#20, to the DB where we have run the migrations in step 1.4.

	3.3- PaymentAcceptanceTests, required running CheckoutAPI instance in IISExpress. To do this, from Visual Studio Menu, choose Debug=>Start Without Debuging.
		and then from from Test explorer, Run all TransactionRepositoryTests.
