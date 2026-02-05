# 🎫 High-Load Ticket Booking System

A robust REST API for ticket booking capable of handling high concurrency scenarios without data inconsistency. 
Built with **.NET 8**, **PostgreSQL**, and **Docker**.

![Build Status](https://img.shields.io/badge/Build-Passing-brightgreen)
![Coverage](https://img.shields.io/badge/Tests-100%25-success)
![Tech](https://img.shields.io/badge/Tech-High%20Load-orange)

## 🚀 The Problem: "The Sold-Out Concert"
When 50 users try to buy the **last ticket** simultaneously, a standard API might sell the same ticket 50 times (Race Condition).
This project solves this using **Optimistic Concurrency Control** (PostgreSQL `xmin` system column).

## 🛠️ Tech Stack
* **Core:** ASP.NET Core Web API (.NET 8)
* **Database:** PostgreSQL (EF Core)
* **Infrastructure:** Docker & Docker Compose
* **Testing:** xUnit, FluentAssertions, JMeter (for Load Testing)

## ⭐️ Key Features
* **Race Condition Protection:** Implemented via `xmin` concurrency token.
* **Clean Architecture:** Usage of DTOs (`BookingRequest`) to separate API contract from Database Entities.
* **Containerized:** Runs fully in Docker with one command.
* **Unit Tested:** Core logic is covered with xUnit tests ensuring business rules stability.

## 🏗️ Architecture
1.  **API Layer:** Accepts JSON requests.
2.  **Logic Layer:** Checks availability.
3.  **Data Layer:** Tries to commit transaction. If data version (`xmin`) changed during processing, `DbUpdateConcurrencyException` is thrown.
4.  **Response:** The user receives `409 Conflict` instead of bad data.

## 🧪 Load Testing Results (JMeter)
Running **50 concurrent threads** trying to book **5 tickets**:
* ✅ **5 Success (200 OK)** - Tickets booked correctly.
* 🛡️ **45 Conflicts (409 Conflict)** - "Sorry, ticket already taken".
* ❌ **0 Overbookings** - Database integrity preserved.

## 🔧 How to Run

1. **Clone the repo**
   ```bash
   git clone [https://github.com/VlaDmaDee/HighLoad-Ticket-Booking-System.git](https://github.com/VlaDmaDee/HighLoad-Ticket-Booking-System.git)
2. Start Infrastructure (DB + App)
   ```bash
   docker-compose up -d
4. Open Swagger UI
  Visit: http://localhost:5000/swagger (or check your Docker port mapping).
5. Run Tests
  ```bash
  dotnet test
