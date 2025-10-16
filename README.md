# AccountingOnline - Backend API

## Overview
Backend API za ERP sistem računovodstva razvijen prema Clean Architecture principima koristeći .NET 8, Entity Framework Core, i direktnu integraciju sa SQL Server stored procedurama.

## Architecture

### Clean Architecture Layers
- **Domain Layer** - Osnovni entiteti, value objects, enums
- **Application Layer** - Business logika, CQRS, DTOs, validacija
- **Infrastructure Layer** - Data access, external services, stored procedures
- **Presentation Layer (API)** - Controllers, middleware, API konfiguracija

### Key Technologies
- .NET 8.0
- ASP.NET Core Web API
- Entity Framework Core 8.0
- SQL Server 2019+
- MediatR (CQRS)
- AutoMapper
- FluentValidation
- Serilog
- Swagger/OpenAPI
- JWT Authentication

## Project Structure

```
AccountingOnline/
├── src/
│   ├── AccountingOnline.API/           # Presentation Layer
│   ├── AccountingOnline.Application/    # Application Layer  
│   ├── AccountingOnline.Domain/        # Domain Layer
│   └── AccountingOnline.Infrastructure/ # Infrastructure Layer
├── tests/
│   ├── AccountingOnline.UnitTests/
│   └── AccountingOnline.IntegrationTests/
├── docs/
└── scripts/
```

## Getting Started

### Prerequisites
- .NET 8.0 SDK
- SQL Server 2019+ (LocalDB for development)
- Visual Studio 2022 or VS Code
- Git

### Setup

1. **Clone the repository**
```bash
git clone https://github.com/sasonaldekant/AccountingOnline.git
cd AccountingOnline
```

2. **Restore packages**
```bash
dotnet restore
```

3. **Update connection string**
Edit `src/AccountingOnline.API/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=Genecom2024Dragicevic;Trusted_Connection=true;MultipleActiveResultSets=true;"
  }
}
```

4. **Run migrations** (kada budu kreirani)
```bash
dotnet ef database update --project src/AccountingOnline.Infrastructure --startup-project src/AccountingOnline.API
```

5. **Start the API**
```bash
dotnet run --project src/AccountingOnline.API
```

6. **Open Swagger UI**
Navigate to `https://localhost:7000` (ili port koji je konfigurisan)

## API Endpoints

### Health
- `GET /api/health` - Health check

### Partners
- `GET /api/partners` - Svi partneri
- `GET /api/partners/{id}` - Partner po ID
- `GET /api/partners/combo/{status}` - Combo za dropdown
- `POST /api/partners` - Kreiranje novog partnera
- `PUT /api/partners/{id}` - Ažuriranje partnera
- `DELETE /api/partners/{id}` - Brisanje partnera

### Documents
- `GET /api/documents` - Svi dokumenti
- `GET /api/documents/{id}` - Dokument po ID
- `POST /api/documents` - Kreiranje novog dokumenta
- `PUT /api/documents/{id}` - Ažuriranje dokumenta

### Sifarnici
- `GET /api/sifarnici/organizacione-jedinice` - Organizacione jedinice
- `GET /api/sifarnici/artikli` - Artikli
- `GET /api/sifarnici/mesta` - Mesta

## Development Guidelines

### Code Style
- Koristiti C# naming conventions
- Async/await pattern za sve I/O operacije
- Dependency injection za sve servise
- SOLID principi

### Error Handling
- Centralizovano error handling kroz middleware
- Strukturirane error response-e
- Logging svih grešaka

### Testing
- Unit testovi za business logiku
- Integration testovi za API endpoints
- Test coverage minimum 80%

### API Response Format
```json
{
  "success": true,
  "data": { ... },
  "message": "Operation completed successfully",
  "errors": []
}
```

## Database Integration

### Entity Framework
- Code-first approach
- Fluent API konfiguracija
- Migration management

### Stored Procedures
- Wrapper services za SP pozive
- Raw SQL execution through EF Core
- Performance optimizacija za complex queries

## Deployment

### Docker
```bash
docker build -t accountingonline-api .
docker run -p 8080:80 accountingonline-api
```

### IIS
1. Publish aplikaciju
2. Kopiraj u IIS folder
3. Konfiguriši Application Pool (.NET 8)
4. Set connection strings

## Contributing

1. Create feature branch iz `develop`
2. Napravi izmene
3. Pokreni testove
4. Create pull request
5. Code review
6. Merge u `develop`

## Milestones

### Milestone 0 - Setup ✅
- [x] Kreirana Clean Architecture struktura
- [x] Konfigurisani basic servisi
- [x] Health endpoint
- [x] Swagger dokumentacija

### Milestone 1 - Šifarnici (Planiran)
- [ ] Partner CRUD operations
- [ ] Stored procedure integration
- [ ] Osnovni error handling
- [ ] Unit testovi

### Milestone 2 - Documents (Planiran)
- [ ] Document CRUD operations
- [ ] Master/Detail relationship
- [ ] Business validation
- [ ] Integration testovi

### Milestone 3 - Performance & Security (Planiran)
- [ ] Caching strategije
- [ ] JWT authentication
- [ ] Rate limiting
- [ ] Performance optimization

## Support

Za pitanja i podršku, kontaktiraj development team.

## License

MIT License - vidi LICENSE fajl za detalje.