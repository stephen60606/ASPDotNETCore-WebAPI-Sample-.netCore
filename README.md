# dotnet_api_basic_structure

This basic api structure designed for saving time on developing every new project and could fulfill decouled and APO design.

                      Client
                        ⬇
                NetCore MiddleWare
                        ⬇
                      WebAPI
                        ⬇
                   FeatureModule
                        ⬇
                     Database


  Here is the structure design pattern. When receive a request, the first layer dealling with the request is Middleware which can fulfill the AOP design here,
  then calling the method in WebAPI depending on the route. In the FeatureModule, the interface can offer different services for different environments, and this 
  is where we deal with business logic.

------------------------------------------------------------------------------------------------------------------------------------------------------------------------
Generating model and dbContext in Entities:

for inite, add this code in "Entities/Context/DemoDbContext.cs":
<img width="791" alt="Screenshot 2023-08-03 at 1 42 03 AM" src="https://github.com/stephen60606/dotnet_api_basic_structure/assets/131985111/693ca806-7e0d-4ea0-a400-9a1c15923c52">

then follow these steps:

dotnet tool install --global dotnet-ef

dotnet add package Pomelo.EntityFrameworkCore.MySql

dotnet add package Microsoft.EntityFrameworkCore.Design

//MySql
dotnet ef dbcontext scaffold "server=localhost;Port=3306;Database=Demo_db; User=___;Password=___;" "Pomelo.EntityFrameworkCore.MySql"  --output-dir Models --context-dir Contexts --context DemoDbContext -f

//MSSql
 dotnet ef dbcontext scaffold "Data Source=___;Initial Catalog=___; Persist Security Info=True;User ID=___;Password=___" Microsoft.EntityFrameworkCore.SqlServer --output-dir Models --context-dir Contexts --context DemoDbContext -f


------------------------------------------------------------------------------------------------------------------------------------------------------------------------
Here is my DB Schema of mysql for this basic project:


<img width="613" alt="Screenshot 2023-08-05 at 2 33 49 AM" src="https://github.com/stephen60606/dotnet_api_basic_structure/assets/131985111/892f67b4-92b4-4bb7-a3a0-cd0a07b87a9e">


------------------------------------------------------------------------------------------------------------------------------------------------------------------------








 
