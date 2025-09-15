# Order Management System API

A lightweight, modular, and scalable Order Management System API for logistics, built with .NET 8.

Designed to demonstrate senior engineering practices and clean architecture.

## Features

### Products
- **CRUD Endpoints**: Create, Read, Update, Delete products.
- **Fields**: `Id`, `Name`, `Price`, `StockQuantity`.
- **Validation**: Ensures non-negative stock and price > 0 using FluentValidation.

### Orders
- **Create Order Endpoint**: Submit orders with multiple product items (`ProductId`, `Quantity`).
- **Order Processing**:
  - Validates stock availability.
  - Deducts stock on successful order.
  - Returns an `OrderSummary` (total amount, items, timestamp).

### Reports
- **GET /reports/daily-summary**: Returns today's total orders and revenue.
- **GET /reports/low-stock**: Returns products with `StockQuantity < 5`.

## Technical Stack

- **.NET 8**
- **Controller-based API** (can be adapted to Minimal APIs)
- **Entity Framework Core** (In-Memory provider)
- **Dependency Injection** throughout all layers
- **FluentValidation** for model validation
- **Async/await** for all data operations
- **Global Error Handling** via middleware
- **Proper HTTP Status Codes** for all responses

## Solution Structure

- **OMS.Domain**: Domain entities and business rules.
- **OMS.Application**: Application services, business logic, and abstractions.
- **OMS.Infrastructure**: EF Core DbContext, repositories, and persistence logic.
- **OMS.Api**: API controllers, DTOs, mapping (Automapper), and validation.
- **OMS.Application.Tests**: Unit tests for application services.

## Getting Started

1. **Clone the repository**

git clone https://github.com/tolmivan/OMS.git 

cd OMS
   
2. **Run the API**

dotnet run --project OMS.Api

4. **API Documentation**
- Swagger UI available at `/swagger` in development mode.

## Example Endpoints

- `GET /products`
- `POST /products`
- `PUT /products/{id}`
- `DELETE /products/{id}`
- `POST /orders`
- `GET /reports/daily-summary`
- `GET /reports/low-stock`

## Testing

- Unit tests are located in `OMS.Application.Tests`.
