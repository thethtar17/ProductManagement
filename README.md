# Product Management System

A product management web application with a Vue 3 frontend and an ASP.NET Core 8 Web API backend. Users can register and log in, manage products, browse products, add products to a cart, view their own products, and use the exchange-rate and translation UI features.

## Technology stack

- **Frontend:** Vue 3, Vue Router, Axios, Vue CLI
- **Backend:** ASP.NET Core 8 Web API
- **Database:** Microsoft SQL Server with Entity Framework Core migrations
- **Authentication:** JWT bearer tokens and BCrypt password hashing
- **API documentation:** Swagger/OpenAPI in the backend development environment

## Project structure

```text
backend/     ASP.NET Core API, EF Core data layer, models, controllers, migrations
frontend/    Vue application and client-side routes/components
```

## Prerequisites

Install the following before setting up the project:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js](https://nodejs.org/) and npm
- Microsoft SQL Server (local SQL Server or SQL Server Express)
- Optional: SQL Server Management Studio or Azure Data Studio

## Configuration

### Backend database and JWT settings

The backend reads its database connection string and JWT settings from `backend/appsettings.json` and environment-specific configuration files.

Update the connection string for your SQL Server instance:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=Management;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Key": "replace-with-a-long-random-secret",
    "ExpirationMinutes": 60
  }
}
```

For anything other than local development, do not commit a real JWT key or database password to source control. Use user secrets, environment variables, or your deployment platform's secret manager instead. The JWT key must be long enough for the configured HMAC signing algorithm.

### Frontend API URL

The frontend currently sends API requests to:

```text
http://localhost:5004
```

If the backend runs on another host or port, update the Axios `baseURL` in `frontend/src/App.vue` and any other frontend components that define an API URL.

## Setup and running the project

The backend and frontend run as separate applications. Start SQL Server first, then use two terminals so both applications can run at the same time.

### 1. Create or update the database

From the project root, open a terminal and run:

```powershell
cd backend
dotnet restore
dotnet ef database update
```

If the Entity Framework CLI is not installed, install it once:

```powershell
dotnet tool install --global dotnet-ef
```

The existing migrations create the application database schema for users, products, and cart items.

### 2. Start the backend API

Keep the first terminal open and run:

```powershell
cd backend
dotnet run
```

Wait until the API starts. The frontend is configured to use:

```text
http://localhost:5004
```

When running in the Development environment, Swagger is available at:

```text
http://localhost:5004/swagger
```

Use the **Authorize** button in Swagger and enter a token in the format `Bearer <token>` for protected endpoints.

### 3. Start the frontend

Open a second terminal and run:


```powershell
cd frontend
npm install
npm run serve
```

Open the URL printed by Vue CLI, normally:

```text
http://localhost:8080
```

Keep both terminals running while using the application. To stop either application, press `Ctrl+C` in its terminal.

### Quick start after the first setup

After dependencies and the database have already been configured, start the project with these commands in two separate terminals:

**Terminal 1 - backend**

```powershell
cd "C:\Users\user\Downloads\Product Management\backend"
dotnet run
```

**Terminal 2 - frontend**

```powershell
cd "C:\Users\user\Downloads\Product Management\frontend"
npm run serve
```

Then visit `http://localhost:8080` in a browser.

## Typical user flow

1. Open the frontend and register an account.
2. Log in to receive a JWT token.
3. Browse products at `/product`.
4. Add products at `/addproduct` or edit owned products from `/myproduct`.
5. Add products to the cart and view the cart at `/displaycart`.
6. Use `/exchange-rate` for exchange-rate information.
7. Use `/translate` for the translation interface.

Protected frontend routes require a token in browser `localStorage`. Logging out removes the token and returns the user to the login page.

## Main API routes

The backend exposes these controller routes:

### Users

- `POST /api/user/register` - Register a user
- `POST /api/user/login` - Authenticate and receive a JWT
- `GET /api/user/current-user` - Get the authenticated user's details

### Products and cart

- `GET /api/products` - List products
- `GET /api/products/{id}` - Get one product
- `GET /api/products/{id}/image` - Get a product image
- `GET /api/products/current-user-id` - Get the current user's ID
- `GET /api/products/current-user-products` - List products owned by the current user
- `POST /api/products/add` - Create a product (multipart form data)
- `PUT /api/products/{id}` - Update a product (multipart form data)
- `DELETE /api/products/{id}` - Delete a product
- `POST /api/products/add-to-cart` - Add a product to the current user's cart
- `GET /api/products/current-user-cart` - Get the current user's cart

Most product and cart operations require the JWT returned by the login endpoint.

## Development commands

### Backend

```powershell
cd backend
dotnet build
dotnet run
```

Create a new EF Core migration after changing the data model:

```powershell
dotnet ef migrations add <MigrationName>
dotnet ef database update
```

### Frontend

```powershell
cd frontend
npm run serve
npm run build
npm run lint
```

## Troubleshooting

- **Cannot connect to SQL Server:** Check that SQL Server is running and that `DefaultConnection` matches the instance and authentication method.
- **Database tables are missing:** Run `dotnet ef database update` from the `backend` directory.
- **401 Unauthorized:** Log in again, confirm the token is stored in the browser, and send it as `Authorization: Bearer <token>`.
- **Frontend cannot reach the API:** Confirm the backend is running on port `5004` or update the frontend Axios base URL.
- **Swagger is unavailable:** Run the backend with the `Development` environment enabled; Swagger is only registered in development.

## Security notes

- Replace the development JWT key before sharing or deploying the application.
- Keep database credentials and cloud-service credentials outside tracked configuration files.
- Use HTTPS and secure secret storage in production.
- Configure CORS to allow only trusted frontend origins instead of allowing every origin.
