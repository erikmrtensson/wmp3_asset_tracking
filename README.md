# Weekly Mini Project - Entity Framework

A C# console application for tracking company assets across multiple offices, built with Entity Framework Core and SQL Server.

## Assignment Completion Status - Level 4

- ✅ Level 1 — Basic Asset Registration
- ✅ Level 2 — Full CRUD, sorting, status highlighting
- ✅ Level 3 — Office management, live currency conversion (ECB rates), reporting
- ✅ Level 4 — Advanced search, filtering, and export (TXT/CSV/JSON)
- ❌ Level 5 — Not attempted (enterprise features: roles, auth, employee assignment)

## Features
- Full CRUD: add, view, update, search, filter, and remove assets
- Supports Computers and Mobile Phones
- Tracks purchase date and calculates asset age
- Highlights assets nearing end-of-life (yellow = 6 months, red = 3 months remaining)
- Converts prices to local currency based on office location, using live daily rates from the European Central Bank (with fallback rates if offline)
- Reporting: asset count per office, total value per office, assets nearing expiration, top 5 most expensive assets
- Search by brand, model, office, or purchase year
- Filter by expired assets, computers only, or mobile phones only
- Export asset list and reports to TXT, CSV, or JSON
- Data persisted via Entity Framework Core to SQL Server (LocalDB)

## Offices & Currencies

| Office  | Currency |
|---------|----------|
| Sweden  | SEK      |
| USA     | USD      |
| Turkey  | TRY      |

## Tech Stack
- .NET 10, C#
- Entity Framework Core with SQL Server LocalDB
- LINQ for filtering, sorting, and reporting

## How to Run
1. Clone the repository
2. Open the solution in Visual Studio
3. In the Package Manager Console, run `Update-Database` to create the local database and apply migrations
4. Run the app (Ctrl+F5) — sample data seeds automatically on first run
