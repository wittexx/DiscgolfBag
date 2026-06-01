# DiscGolf Bag

A web app for disc golfers to manage their disc collections and share bags with friends.

## Features

- Create a profile and manage your disc golf bag
- Upload discs with photos, flight numbers, and details
- Add friends and view their bags
- 45 disc limit per user

## Tech Stack

- **Backend:** ASP.NET Core 8 Web API (C#) with Minimal APIs
- **Frontend:** SvelteKit (TypeScript)
- **Database:** SQLite (local) via Entity Framework Core
- **Auth:** ASP.NET Identity + JWT

## Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) (8.0+)
- [Node.js](https://nodejs.org/) (18+)
- [EF Core CLI](https://learn.microsoft.com/en-us/ef/core/cli/dotnet) — `dotnet tool install --global dotnet-ef`

## Getting Started

### Backend

```bash
# Navigate to the API project
cd src/DiscGolfBag.Api

# Run migrations
dotnet ef database update

# Start the API
dotnet run
```

The API will run on `http://localhost:5000` by default.

### Frontend

```bash
# Navigate to the frontend
cd frontend

# Install dependencies
npm install

# Start the dev server
npm run dev
```

The frontend will run on `http://localhost:5173`.

## Project Structure

```
DiscGolfBag/
├── DiscGolfBag.sln
├── src/
│   └── DiscGolfBag.Api/       # ASP.NET Core Web API
│       ├── Features/          # Vertical slices (Auth, Discs, Friends, Profiles)
│       ├── Common/            # Shared models, DbContext, extensions
│       └── Program.cs
├── frontend/                  # SvelteKit app
├── .github/workflows/         # CI pipeline
└── README.md
```
