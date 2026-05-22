# Weekly Mini Project 3 - Asset Tracking System

A C# console application for tracking company assets across multiple offices.

## Features

- Add, view, search and remove assets
- Supports Computers and Mobile Phones
- Tracks purchase date and calculates asset age
- Highlights assets nearing end-of-life (yellow = 6 months, red = 3 months)
- Converts prices to local currency based on office location
- Saves and loads assets from a JSON file

## Offices & Currencies

| Office  | Currency |
|---------|----------|
| Sweden  | SEK      |
| USA     | USD      |
| Turkey  | TRY      |

## How to Run

1. Clone the repository
2. Open in Visual Studio or run with `dotnet run`
3. Assets are automatically saved to `Data/assets.json`
