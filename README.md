# travel-invoice-reclaim-platform

A full-stack travel expense invoice submission and VAT reclaim system where hotels submit invoices via a React dashboard, a .NET API validates and processes reclaim eligibility, SQL Server stores transactional data (invoices, payments, reclaim status), and MongoDB stores audit trails and validation event logs.

## Backend

- Migration

```bash
dotnet ef database update [MigrationName] --project src/backend/TravelReclaim.Infrastructure/ --startup-project src/backend/TravelReclaim.Api
```
