# E-Commerce Microservices Lab
## Overview

This repository is a learning and experimental lab for building an e-commerce system using microservices architecture.
It demonstrates modern distributed system patterns including CQRS, Event Sourcing, MediatR, Kafka messaging, observability, and scalable API design.

The goal is to start simple, iteratively add features, refactor, and evolve toward a full-featured distributed e-commerce platform.

## Architecture

The system is designed with a modular microservices approach, following Clean Architecture principles:

## 1. Services

Each domain area is a separate service, e.g.:

* Products Service – manages product catalog.
* Inventory Service - manages how many units of each product are available.
* Orders Service – handles order creation and processing.
* Payments Service – processes payments and transactions.

## Each service contains:

* API Layer – Minimal APIs exposing REST endpoints.
* Application Layer – Commands, queries, and business logic handlers.
* Domain Layer – Core entities and domain rules.
* Infrastructure Layer – Repository implementations, messaging, caching.

## 2. Patterns & Concepts

* CQRS (Command Query Responsibility Segregation) – separates commands (writes) from queries (reads).
* MediatR / Mediator Pattern – decouples request handling from business logic.
* Event-driven architecture – services communicate via Kafka events, allowing eventual consistency.
* Dependency Injection & Modularity – services are loosely coupled; endpoints and handlers are registered dynamically.
* Observability & Monitoring – structured for Prometheus/Grafana metrics and logging.

## 3. Infrastructure & Tools

* In-memory repositories (for quick prototyping).
* Redis / Kafka integration (for caching and messaging).
* .NET 10 / C# 12 with Minimal APIs.
* Unit and integration testing support.

### Key Features

* Modular microservices architecture.
* Automatic endpoint registration and DI for services.
* Layered design: API, Application, Domain, Infrastructure.
* Command & Query handlers using MediatR.
* Event-driven communication for distributed consistency.
* Ready to extend to load balancing, caching, and observability.


> Test endpoints via REST Client (.http files) or Postman.

## Roadmap

* Implement full CRUD for all services.
* Introduce Redis for caching read models.
* Integrate Kafka for asynchronous communication.
* Implement observability (logging, metrics, tracing).
* Deploy with Docker and simulate load-balanced services.

## Contribution

This project is a learning lab for .NET microservices.
Contributions are welcome — fork, experiment, and propose improvements.