# Personal Blog API

A RESTful API for managing personal blog portfolio built with ASP.NET Core 9.0.

## Features

- JWT Authentication
- CRUD operations for Projects, Skills, Blog Posts, Education, Experience, and Contact Messages
- Soft Delete implementation
- Global Exception Handling
- Swagger/OpenAPI documentation

## Prerequisites

- .NET 9.0 SDK
- SQL Server (LocalDB or SQL Server Express)
- Visual Studio 2022 or VS Code

## Configuration

### JWT Secret Key Setup

**⚠️ IMPORTANT:** The JWT SecretKey is required for the application to run. It must be at least 32 characters long for security.

The application supports multiple methods to configure the SecretKey (in priority order):

1. **Environment Variable** (Recommended for Production)
2. **User Secrets** (Recommended for Development)
3. **appsettings.Development.json** (Fallback for local development)

#### Method 1: Environment Variable (Production)

Set the `JWT_SECRET_KEY` environment variable:

**Windows (PowerShell):**
```powershell
$env:JWT_SECRET_KEY="your-minimum-32-character-secret-key-here"
```

**Windows (Command Prompt):**
```cmd
set JWT_SECRET_KEY=your-minimum-32-character-secret-key-here
```

**Linux/macOS:**
```bash
export JWT_SECRET_KEY="your-minimum-32-character-secret-key-here"
```

**For Production Deployment:**
- Azure App Service: Add in Configuration > Application Settings
- Docker: Use `-e JWT_SECRET_KEY="your-key"` or environment file
- IIS: Set in System Environment Variables or web.config

#### Method 2: User Secrets (Development - Recommended)

User Secrets are stored outside of your project tree, so they won't be committed to source control.

**Initialize User Secrets:**
```bash
cd Backend/PersonalBlog.API
dotnet user-secrets init
```

**Set the SecretKey:**
```bash
dotnet user-secrets set "JwtSettings:SecretKey" "your-minimum-32-character-secret-key-here"
```

**Verify it's set:**
```bash
dotnet user-secrets list
```

**Note:** User Secrets are only available when running in Development environment.

#### Method 3: appsettings.Development.json (Fallback)

For local development only, you can add the SecretKey directly to `appsettings.Development.json`:

```json
{
  "JwtSettings": {
    "SecretKey": "your-minimum-32-character-secret-key-here",
    "Issuer": "PersonalBlogAPI",
    "Audience": "PersonalBlogClient",
    "ExpirationInMinutes": 60
  }
}
```

**⚠️ Warning:** This file should NOT be committed to source control if it contains real secrets. The current development key is safe for local testing only.

### CORS Configuration

**⚠️ IMPORTANT:** CORS (Cross-Origin Resource Sharing) policy is configured to restrict access to specific origins only.

#### Development

In Development environment, the following origins are allowed by default:
- `http://localhost:3000` (Frontend Vite dev server)
- `http://localhost:5173` (Vite default port - fallback)
- `https://localhost:7281` (Backend HTTPS - Swagger)
- `http://localhost:5098` (Backend HTTP - Swagger)

These are configured in `appsettings.Development.json`.

#### Production

**CORS origins are REQUIRED in Production.** You must configure allowed origins using one of the following methods:

**Method 1: Environment Variable (Recommended)**

Set the `CORS__AllowedOrigins` environment variable with comma-separated origins:

**Windows (PowerShell):**
```powershell
$env:CORS__AllowedOrigins="https://yourdomain.com,https://www.yourdomain.com"
```

**Windows (Command Prompt):**
```cmd
set CORS__AllowedOrigins=https://yourdomain.com,https://www.yourdomain.com
```

**Linux/macOS:**
```bash
export CORS__AllowedOrigins="https://yourdomain.com,https://www.yourdomain.com"
```

**Docker:**
```yaml
environment:
  - CORS__AllowedOrigins=https://yourdomain.com,https://www.yourdomain.com
```

**Azure App Service:**
- Go to Configuration > Application Settings
- Add: `CORS__AllowedOrigins` = `https://yourdomain.com,https://www.yourdomain.com`

**Method 2: appsettings.Production.json**

Create `appsettings.Production.json`:
```json
{
  "Cors": {
    "AllowedOrigins": [
      "https://yourdomain.com",
      "https://www.yourdomain.com"
    ]
  }
}
```

**Priority Order:**
1. Environment Variable (`CORS__AllowedOrigins`)
2. `appsettings.{Environment}.json`
3. `appsettings.json`
4. Default Development origins (Development only)

**Security Notes:**
- Never use `*` (wildcard) with `AllowCredentials = true`
- Always use HTTPS in Production
- Only include trusted domains
- The application will fail to start in Production if no origins are configured

### Database Connection

Update the connection string in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "Default": "Server=(localdb)\\mssqllocaldb;Database=PersonalBlogDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

## Running the Application

1. **Set up the JWT SecretKey** using one of the methods above
2. **Update the database connection string** if needed
3. **Run database migrations:**
   ```bash
   cd Backend/PersonalBlog.API
   dotnet ef database update
   ```
4. **Run the application:**
   ```bash
   dotnet run
   ```
   Or use Visual Studio: Press F5

5. **Access Swagger UI:**
   - Navigate to `https://localhost:7281/swagger` or `http://localhost:5098/swagger`

## API Endpoints

- **Authentication:** `/api/auth/register`, `/api/auth/login`
- **Projects:** `/api/projects`
- **Blog Posts:** `/api/blogposts`
- **Skills:** `/api/skills`
- **Education:** `/api/education`
- **Experience:** `/api/experience`
- **Contact:** `/api/contact`

## Security Best Practices

1. **Never commit secrets to source control**
2. **Use environment variables or secret management services in production**
3. **Generate strong, random secret keys (minimum 32 characters)**
4. **Use different keys for different environments (dev, staging, production)**
5. **Rotate keys periodically**

## Troubleshooting

### Error: "JWT SecretKey is required"

**Solution:** Make sure you've set the SecretKey using one of the methods above. Check:
- Environment variable is set: `echo $JWT_SECRET_KEY` (Linux/macOS) or `echo %JWT_SECRET_KEY%` (Windows)
- User Secrets are configured: `dotnet user-secrets list`
- appsettings.Development.json contains SecretKey (if using Method 3)

### Error: "JWT SecretKey must be at least 32 characters long"

**Solution:** Your SecretKey is too short. Generate a new key with at least 32 characters.

**Generate a secure key (PowerShell):**
```powershell
-join ((48..57) + (65..90) + (97..122) | Get-Random -Count 32 | ForEach-Object {[char]$_})
```

**Generate a secure key (Linux/macOS):**
```bash
openssl rand -base64 32
```

## Project Structure

```
Backend/PersonalBlog.API/
├── Controllers/          # API Controllers
├── Data/                # DbContext and configurations
├── DTOs/                # Data Transfer Objects
├── Exceptions/          # Custom exceptions
├── Middlewares/         # Global exception handler
├── Models/              # Entity models
├── Repositories/        # Data access layer
├── Services/            # Business logic layer
└── Settings/            # Configuration classes
```

## License

MIT License
