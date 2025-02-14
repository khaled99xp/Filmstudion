# Filmstudion

## Overview
Filmstudion is a web application built using ASP.NET Core for the backend and a simple frontend using HTML, CSS, and JavaScript. The system is designed to manage film studios, films, and user authentication.

## Features
- User authentication (registration, login)
- Film management (create, update, delete, list films)
- Film studio registration and management
- Renting film copies
- Secure API using JWT authentication

## Technologies Used
### Backend (API)
- **ASP.NET Core**
- **Entity Framework Core** (Database handling)
- **JWT Authentication** (User authentication and security)
- **Mediator Pattern** (For better command handling)

### Frontend
- **HTML, CSS, JavaScript**

## Project Structure
```
Filmstudion/
│── API/                     # Backend files
│   ├── Controllers/         # API endpoints
│   ├── Data/                # Database configuration
│   ├── DTO/                 # Data Transfer Objects
│   ├── Helpers/             # Utility classes
│   ├── Interfaces/          # Interface definitions
│   ├── Mediator/            # Command handling
│   ├── Models/              # Data models
│   ├── Repositories/        # Data access layers
│   ├── Services/            # Business logic
│   ├── appsettings.json     # Configuration file
│   ├── Program.cs           # Entry point for backend
│   ├── test-vg.rest         # API test file
│── Frontend/                # Simple frontend
│   ├── index.html           # Main frontend page
│   ├── css/style.css        # Styles
│   ├── js/app.js            # Frontend logic
```

## Setup Instructions
### Prerequisites
- .NET SDK 9.0+


### Backend Setup
1. Navigate to the `API/` directory:
   ```sh
   cd API
   ```
2. Restore dependencies:
   ```sh
   dotnet restore

3. Run the API:
   ```sh
   dotnet run
   ```

### Frontend Setup
Simply open `Frontend/index.html` in a browser.

## API Testing
Use `test-vg.rest` to test the API endpoints.


## License
This project is licensed under the MIT License.

