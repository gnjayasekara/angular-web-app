# Purchase Bill Management System

A simple full-stack Purchase Bill application built with Angular and ASP.NET Core.

The application allows users to authenticate through an external POS authentication API, access a protected Purchase Bill page, load available locations, add purchase items, calculate totals, and save purchase bills to a SQL Server database.

---

## Tech Stack

### Frontend
- Angular
- TypeScript
- Tailwind CSS
- Angular Reactive Forms
- Angular Router
- Angular HttpClient

### Backend
- ASP.NET Core Web API
- C#
- Entity Framework Core
- JWT Authentication
- SQL Server

### Database
- Microsoft SQL Server Express

---

## Main Features

- User login through an external authentication API
- JWT-based authentication using an HttpOnly cookie
- Protected Angular routes
- Automatic unauthorized-session redirection
- Location synchronization and storage
- Item autocomplete
- Purchase Bill calculations
- Add multiple items to a Purchase Bill
- Purchase Bill persistence using Entity Framework Core
- Logout and session handling

---

## Prerequisites

Install the following before running the project:

-   Git
-   .NET 10 SDK
-   Node.js and npm
-   Angular CLI
-   SQL Server or SQL Server Express
-   SQL Server Management Studio (optional)
-   Postman (optional, for API testing)

You can verify the main tools with:

``` powershell
git --version
dotnet --version
node --version
npm --version
ng version
```

## 1. Clone the Repository

``` powershell
git clone https://github.com/gnjayasekara/angular-web-app.git
cd <YOUR_REPOSITORY_FOLDER>(angular-web-app)
```

## 2. Configure the Backend

Navigate to the ASP.NET Core project:

``` powershell
cd backend\PurchaseBill.Api
```

Restore the NuGet packages:

``` powershell
dotnet restore
```

## 3. Configure SQL Server

The development setup uses SQL Server Express with Windows
Authentication.

Example connection string:

``` json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=PurchaseBillDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

If your SQL Server instance has a different name, update the `Server`
value in the backend configuration.

For example:

``` text
.\SQLEXPRESS
```

or:

``` text
YOUR-PC-NAME\SQLEXPRESS
```

The application database is:

``` text
PurchaseBillDb
```

## 4. Database Setup --- Recommended: EF Core Migrations

**EF Core migrations are the recommended way to set up the database.**

The migration files are included with the backend project, so a
developer cloning the repository should use them to create the required
database schema.

If the Entity Framework CLI tool is not installed:

``` powershell
dotnet tool install --global dotnet-ef
```

Verify it:

``` powershell
dotnet ef --version
```

Then, from the `backend\PurchaseBill.Api` directory, run:

``` powershell
dotnet ef database update
```

This applies the migrations to the configured SQL Server database.

The application also runs pending migrations during startup using:

``` csharp
await dbContext.Database.MigrateAsync();
```

Therefore, after the connection string is configured correctly, starting
the backend can also apply pending migrations automatically.

The resulting database contains the application's main tables,
including:

-   `Location_Details`
-   `Purchase_Bills`
-   `Purchase_Bill_Items`
-   `__EFMigrationsHistory`

### Important Database Setup Note

Use **EF Core migrations as the primary database setup method**.

Do not manually create the tables using the SQL setup script and then
run the existing EF Core migrations against the same database unless the
migration history has also been handled correctly. Otherwise, EF Core
may attempt to create tables that already exist.

If EF Core database setup cannot be used in a particular environment, a
`PurchaseBillDb_Setup.sql` file is provided in the repository as a
manual fallback.

In that case, open the SQL file in SQL Server Management Studio, connect
to the intended SQL Server instance, and execute the script.

**Use either the EF Core migration approach or the manual SQL schema
approach for initial setup; do not use both independently on the same
fresh database.**

## 5. Configure the JWT Signing Key

The JWT signing key is intentionally **not committed to GitHub**.

For local development, configure it using .NET User Secrets.

From:

``` text
backend\PurchaseBill.Api
```

initialize User Secrets if necessary:

``` powershell
dotnet user-secrets init
```

Generate a random development signing key in PowerShell:

``` powershell
$keyBytes = New-Object byte[] 32
[System.Security.Cryptography.RandomNumberGenerator]::Fill($keyBytes)
$key = [Convert]::ToBase64String($keyBytes)
dotnet user-secrets set "Jwt:Key" $key
```

Verify that the setting exists:

``` powershell
dotnet user-secrets list
```



The non-secret JWT settings can remain in application configuration:

``` json
"Jwt": {
  "Issuer": "PurchaseBill.Api",
  "Audience": "PurchaseBill.Client",
  "ExpiryMinutes": 60
}
```

## 6. Run the Backend

From the backend project directory:

``` powershell
dotnet run
```

In the current development configuration, the API runs at approximately:

``` text
http://localhost:5035
```

Check the terminal output for the exact URL and port on your machine.

## 7. Backend Authentication Flow

The authentication flow is:

```text
Client
  -> POST /api/auth/login
  -> ASP.NET Core backend
  -> External Login API validates credentials
  -> User locations are synchronized with SQL Server
  -> Backend generates an application JWT
  -> JWT is stored in an HttpOnly `access_token` cookie
  -> Browser automatically sends the cookie with backend requests
  -> ASP.NET Core extracts and validates the JWT from the cookie
  -> Protected endpoints allow access only for authenticated users
```

## 8. Main API Endpoints

### Authentication

```text
POST /api/auth/login
GET  /api/auth/me
POST /api/auth/logout
```
`POST /api/auth/login` is publicly accessible because the user is not
authenticated before logging in.

`GET /api/auth/me` is protected and is used by the Angular route guard
to verify whether the current session is authenticated.

`POST /api/auth/logout` removes the authentication cookie.

### Locations

``` text
GET /api/locations
```

Returns locations available to the authenticated user.

This endpoint is protected by JWT authentication.

### Purchase Bills

``` text
GET  /api/purchase-bills
GET  /api/purchase-bills/{id}
POST /api/purchase-bills
```

These endpoints are protected by JWT authentication.

The JWT is stored in the HttpOnly `access_token` cookie.

The browser automatically includes the authentication cookie with
backend requests when credentials are enabled.

A request without a valid authentication cookie should return:

``` text
401 Unauthorized
```

## 9. Run the Angular Frontend

Open a second terminal and navigate to the Angular project:

``` powershell
cd purchase-bill-frontend
```

Install dependencies:

``` powershell
npm install
```

Start the development server:

``` powershell
ng serve
```

The frontend normally runs at:

``` text
http://localhost:4200
```

The backend CORS policy is configured to allow the Angular development
origin:

``` text
http://localhost:4200
```
The Angular HTTP client sends requests to the backend with credentials
enabled so the authentication cookie is included.

## 10. Running the Full Application

Use two terminals.

### Terminal 1 --- Backend

``` powershell
cd backend\PurchaseBill.Api
dotnet restore
dotnet ef database update
dotnet run
```

### Terminal 2 --- Frontend

``` powershell
cd purchase-bill-frontend
npm install
ng serve
```

Then open:

``` text
http://localhost:4200
```

## 11. Purchase Bill Logic

The backend validates the selected location and calculates the financial
values before saving a Purchase Bill.

``` text
Base Cost = Standard Cost × Quantity

Discount Amount = Base Cost × Discount Percentage / 100

Total Cost = Base Cost - Discount Amount

Total Selling = Standard Price × Quantity
```

For example:

``` text
Standard Cost = 100
Standard Price = 150
Quantity = 5
Discount = 20%

Base Cost = 100 × 5 = 500
Discount = 100
Total Cost = 400
Total Selling = 750
```

Calculations are performed on the backend so the API does not rely on
client-calculated financial values.

## 12. Basic Backend Verification

After starting the backend, the following checks can be performed with
the browser, Postman, or another API client:

1. Log in through `POST /api/auth/login` and confirm the response is
   successful.
2. Confirm that the backend creates the HttpOnly `access_token` cookie.
3. Call `GET /api/auth/me` while authenticated and confirm `200 OK`.
4. Call a protected endpoint without the authentication cookie and
   confirm `401 Unauthorized`.
5. Call `GET /api/locations` while authenticated and confirm locations
   are returned.
6. Create a Purchase Bill using `POST /api/purchase-bills` and confirm
   the request succeeds.
7. Retrieve the created bill using `GET /api/purchase-bills/{id}`.
8. Verify that an invalid location is rejected with `400 Bad Request`.
9. Call `POST /api/auth/logout` and confirm the authentication cookie is
   removed.
10. Verify the saved data in SQL Server if necessary.

## Security Notes

- Never commit the JWT signing key to Git.
- Never commit real usernames or passwords.
- Keep development secrets in .NET User Secrets or environment variables.
- The JWT is stored in an HttpOnly cookie rather than `localStorage`.
- Protected API endpoints require a valid authenticated session.
- Frontend route protection alone does not secure an API; backend
  endpoints are protected using ASP.NET Core authentication and
  `[Authorize]`.
- The backend CORS policy explicitly allows the Angular development
  origin and credentials.
- The local development cookie uses `Secure = false` because the
  application currently runs over HTTP.
- Production deployments should use HTTPS and set the authentication
  cookie to `Secure = true`.
- Production deployments should use environment-specific secret
  management and review CSRF protection for state-changing requests.

## Database Setup Troubleshooting

If `dotnet ef database update` fails:

1.  Confirm SQL Server is running.
2.  Confirm the SQL Server instance name in the connection string is
    correct.
3.  Confirm the configured Windows user has access to SQL Server.
4.  Run `dotnet restore`.
5.  Confirm `dotnet-ef` is installed.
6.  Retry:

``` powershell
dotnet ef database update
```

If EF Core setup still cannot be used, use the provided
`PurchaseBillDb_Setup.sql` manual setup script as the fallback database
setup method.
