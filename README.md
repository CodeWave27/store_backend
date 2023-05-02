# store_backend

This is the backend for the NetSysinformation store, it's made with C#(Entity Framework) and Postgres SQL.

It has an interactive documentation with Swagger UI, it also uses authentication and role authorization for the differente endpoints requests to protect the resources.

To start with the project the main thing you have to do, it's install all the project dependencies with NuGet, after that, you must change the Postgres SQL connection string,
it is in the `app.settings.json` file and it has the following name: **dbConnection**, you just have to change that according to your local or cloud database, with the respective server, user id, password and database.

In this case, I'm using Entity Framework, so I don't need to create the Database manually, Entity Framework will make after call the enpoint `http:localhost:'portNumber'/api/db/CreateDb`.

If you want to see the interactive documentation with Swagger, you must open the following endpoint: http:localhost: `http:localhost:portNumber/api/db/CreateDb` and you will see all the endpoints along the application.
