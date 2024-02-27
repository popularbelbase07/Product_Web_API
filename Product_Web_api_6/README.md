# Product_Web_api_6

 ## Advanced Web Apis with ASP.NET CORE in .NET 6

 ## Installation Packages
		1. Install Microsoft.EntityFrameworkCore  => using nuget Package manager.			
		2. dotnet add package Microsoft.EntityFrameworkCore.InMemory

 1. # This Project basically use inmemory database and do some very crucial operation like CRUD

	1. Pagination and filtering 
	2. Nuget
	3. Enforcing HTTPS
	4. Java Script request
	5. Tokens
	6. Testing => using XUnit test for controllers

 2. # Advance data retrieval 
	
	* Pagination 
	* filtering 
	* Sorting
	* Searching
	* Advance Searching
	
 3. # Advance data  retrieval 
		* Versioning options 
			* => HTTP Header(X-API-Version: 1.0 = Information may also be the part of the Accept HTTP Header.) 
			* => URL(/v1.0/products = Seperates API versions, but gives up the one URI principle) => REST ARCHICTURE
			* => QueryString(/products?api-version=1.0 = Might mix with other URL parameters.)
		1. Nuget Package : Microsoft.AspNetCore.Mvc.Versioning
		2. Fixing the swagger API Documentation use Nuget package
			* => Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer
4. Securing APIS
1. Enforcing HTTPS
	
	


