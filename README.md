# Multi-Tenant License Management System

Microservices-based ASP.NET Core application to manage professional licenses for multiple government agencies with multi-tenancy, CQRS, background jobs, and API Gateway.

## Tech Stack

- .NET 8 (ASP.NET Core Web API + MVC)
- SQL Server (LocalDB) for tenant databases
- Entity Framework Core (multi-tenant data access)
- MediatR (CQRS)
- Hangfire (license renewal background jobs)
- Ocelot (API Gateway)
- JWT Authentication (role-based dashboards)

## Solution Structure

- `LicenseService` – License CRUD, CQRS, renewals (core business service)
- `DocumentService` – Document upload/download endpoints
- `NotificationService` – Email/SMS notification stub
- `LicenseManagement.Api` – ASP.NET MVC UI with role-based dashboards
- `LicenseManagement.Gateway` – API Gateway with Ocelot routing
- `LicenseManagement.Shared` – Shared models and contracts

## Architecture Overview

- Multi-tenant: Tenant ID is passed via `X-Tenant-Id` header and enforced through EF Core global query filters.
- CQRS: License commands/queries implemented using MediatR (`CreateLicenseCommand`, `GetLicensesQuery`, `ApproveLicenseCommand`).
- Background Jobs: Hangfire processes periodic license renewal reminders and approval notifications.
- API Gateway: All external calls go through Ocelot, which routes to LicenseService and other microservices.

> Include a simple PNG diagram exported from draw.io showing: MVC → Gateway → License/Document/Notification services → SQL Server.[web:79]

## Getting Started

### Prerequisites

- Visual Studio 2022 Community
- .NET 8 SDK
- SQL Server Express / LocalDB

### Setup

1. Clone the repository:
