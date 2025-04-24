# 🌐 Integration API Sample

A .NET 8-based API that integrates multiple external services 
(e.g., Weather, Flights, and Public Holidays) 
and provides a unified, secure, and resilient endpoint for aggregated data.

---

## Tech Stack

- **.NET 8** — Modern cross-platform framework
- **C#**
- **ASP.NET Core Web API** — RESTful service
- **JWT Authentication** — Token-based security
- **Swagger (Swashbuckle)** — API documentation & testing
- **Polly** — Transient-fault-handling (resilience)
- **HttpClientFactory** — Typed clients with retry logic
- **xUnit / Moq** — Unit testing & mocking

---

## Architectural Patterns

- **Clean Architecture** (by feature, layered):  
  - `API` (Controllers, Startup, Auth)
  - `Application` (DTOs, Interfaces, Services)
  - `Infrastructure` (Implementations, Policies)
  - `Domain` (Models)
  
- **SOLID Principles**
- **Dependency Injection**
- **Extension Methods** for DI and Polly Resilience Policies
- **Typed HTTP Clients** for external APIs
- **Retry + Fallback Policies** using Polly
- **DTO Mapping** (manual or Automapper-ready)

---

## ⚙️ Getting Started

### 1. Clone the Repository

```
bash
git clone https://github.com/your-user/integration-api-sample.git
cd integration-api-sample
```

### 2. Build - Run

### 3. Visit Swagger for api endpoint documentation
https://localhost:7269/swagger

### Authentication
Use the following endpoint to generate a JWT:

POST /api/Authorization/token
Request:
```
{
  "username": "test",
  "password": "superstrongsecretkeywithatleast32chars"
}
```

Get the response token value
```
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5..."
}
```

then use the value on swagger authorization

### Usage
```
GET /api/Aggregation

?City=Athens&CountryCode=gr&Date=2025-04-24&Passengers=2
```
--

##  What Can Be Improved
- Add automated integration tests 
- Add rate limiting and circuit breaker to external API calls
- Centralized logging infrastructure with Serilog or similar
- Replace InMemory with actual Database for persistence
- Improve error model with ProblemDetails compliance

What Can Be Added
- Caching (e.g., MemoryCache or Redis)
- Metrics and Health Checks
- Background Jobs
- Email / SMS notifications
- Frontend UI to consume the aggregated data
- OpenAPI 3.0 enhancements for better docs
- Localization support