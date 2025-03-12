# RetailShop E-Commerce API

## Description

A modern, feature-rich e-commerce backend built with .NET Core 8.0, implementing clean architecture principles. This API provides comprehensive endpoints for product management, order processing, payment handling, and customer management, using PostgreSQL for data persistence.

## Key Features

### Product Management

- ✅ CRUD operations for products
- ✅ Product categorization and filtering
- ✅ Brand management
- ✅ Price and inventory tracking

### Order System

- ✅ Order creation and management
- ✅ Order status tracking
- ✅ Multiple payment methods
- ✅ Order history

### Payment Processing

- ✅ Secure payment handling
- ✅ Multiple payment methods (Credit Card, PayPal, etc.)
- ✅ Payment status tracking
- ✅ Transaction history

### Customer Management

- ✅ Customer profiles
- ✅ Order history
- ✅ Address management

### Inventory Management

- ✅ Stock level tracking
- ✅ Low stock alerts
- ✅ Stock updates
- ✅ Stock availability checks

## Project Structure

```
RetailShop/
├── API/                  # Web API Layer
│   ├── Controllers/      # API endpoints
│   ├── DTOs/            # Data transfer objects
│   ├── Helpers/         # Utility classes
│   └── Middleware/      # Custom middleware
├── Core/                # Domain Layer
│   ├── Entities/        # Domain models
│   ├── Interfaces/      # Core abstractions
│   ├── Services/        # Domain services
│   └── Specifications/  # Query specifications
└── Infrastructure/      # Data Layer
    ├── Data/           # Database context
    ├── Repositories/   # Data access
    └── Services/       # External services
```

## Technologies Used

- .NET Core 8.0
- Entity Framework Core 8.0
- PostgreSQL
- AutoMapper
- Swagger/OpenAPI

## Prerequisites

- .NET Core SDK 8.0
- PostgreSQL
- Visual Studio Code or Visual Studio 2022

## Setup Instructions

1. Clone the repository

```powershell
git clone https://github.com/Floyden-Monteiro/RetailShop.git
cd RetailShop
```

2. Update connection string in `API/appsettings.json`

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

## API Endpoints

### Products

- `GET /api/products` - Get all products
- `GET /api/products/{id}` - Get product by ID
- `POST /api/products` - Create product
- `PUT /api/products/{id}` - Update product
- `DELETE /api/products/{id}` - Delete product

### Orders

- `GET /api/orders` - Get all orders
- `GET /api/orders/{id}` - Get order by ID
- `POST /api/orders` - Create order
- `PUT /api/orders/{id}/status` - Update order status

### Payments

- `GET /api/payments` - Get all payments
- `GET /api/payments/{id}` - Get payment by ID
- `POST /api/payments` - Process payment
- `GET /api/payments/order/{orderId}` - Get order payment

### Customers

- `GET /api/customers` - Get all customers
- `GET /api/customers/{id}` - Get customer details
- `POST /api/customers` - Create customer
- `PUT /api/customers/{id}` - Update customer

### Inventory

- `GET /api/inventory/low-stock` - Get products with low stock (below threshold)
- `PUT /api/inventory/update-stock/{id}` - Update product stock quantity
- `GET /api/inventory/check-stock/{id}` - Check if product is in stock for requested quantity

Example Requests:

**Check Low Stock Products**

```http
GET /api/inventory/low-stock?threshold=5
```

**Update Stock**

```http
PUT /api/inventory/update-stock/1
Content-Type: application/json

{
    "quantity": 100
}
```

**Check Stock Availability**

```http
GET /api/inventory/check-stock/1?quantity=5
```

## Design Patterns & Principles

- Clean Architecture
- Repository Pattern
- Specification Pattern
- SOLID Principles
- DRY (Don't Repeat Yourself)

## Future Enhancements

- [ ] Angular Frontend Implementation
- [ ] Authentication/Authorization
- [ ] Payment Integration
- [ ] Order Processing System
- [ ] Cloud Deployment

## Contributing

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/NewFeature`)
3. Commit changes (`git commit -m 'Add NewFeature'`)
4. Push to branch (`git push origin feature/NewFeature`)
5. Open a Pull Request

Project Link: [https://github.com/Floyden-Monteiro/RetailShop](https://github.com/Floyden-Monteiro/RetailShop)
