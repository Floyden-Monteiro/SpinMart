# RetailShop E-Commerce API

## Description

A modern e-commerce backend built with .NET Core 8.0, implementing clean architecture principles. This API provides robust endpoints for product management, order processing, and customer handling, using PostgreSQL for data persistence.

## Project Structure

```
RetailShop/
├── API/                  # Web API & Controllers
│   ├── Controllers/      # API endpoints
│   ├── DTOs/            # Data transfer objects
│   └── Helpers/         # Utility classes
├── Core/                # Domain Layer
│   ├── Entities/        # Domain models
│   ├── Interfaces/      # Abstractions
│   └── Specifications/  # Query specifications
└── Infrastructure/      # Data Access Layer
    ├── Data/            # Database context
    └── Services/        # Implementations
```

## Technologies Used

- .NET Core 8.0
- Entity Framework Core
- PostgreSQL
- AutoMapper
- Swagger/OpenAPI

## Prerequisites

- .NET Core SDK 8.0
- PostgreSQL
- Visual Studio Code or Visual Studio 2022

## Setup and Installation

1. Clone the repository

```powershell
git clone https://github.com/yourusername/RetailShop.git
cd RetailShop
```

2. Update database connection string in `API/appsettings.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=retailshop;Username=your_username;Password=your_password"
  }
}
```

3. Apply database migrations

```powershell
dotnet ef database update -p Infrastructure -s API
```

4. Run the application

```powershell
cd API
dotnet run
```

## Features

- ✅ Product Management (CRUD)
- ✅ Brand Management
- ✅ Order Processing
- ✅ Customer Management
- ✅ Pagination Support
- ✅ Swagger Documentation

## API Endpoints

- Products: `/api/products`
- Brands: `/api/brands`
- Orders: `/api/orders`
- Customers: `/api/customers`

## Design Patterns

- Repository Pattern
- Specification Pattern
- Unit of Work
- DTO Pattern

## Future Enhancements

- [ ] Angular Frontend Implementation
- [ ] Authentication/Authorization
- [ ] Payment Integration
- [ ] Order Processing System
- [ ] Cloud Deployment

## Contributing

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details

## Contact

Your Name - [@yourusername](https://github.com/yourusername)

Project Link: [https://github.com/yourusername/RetailShop](https://github.com/yourusername/RetailShop)
