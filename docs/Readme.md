# Shape Generator

For the purpose of displaying my capabilites as a developer. This document aims to explain the project, how to run it
and the process I used.
I created this project using React and Typescript paired with a .NET backend.

## Project Overview

The Shape Generator is a simple web application that allows the user to input a shape and see the shape rendered as an
SVG on the screen.

Example Usage:

Input: "Draw a circle with a radius of 50"
Output: Complete shape data with coordinates, dimensions, and center point

## 🛠️ Technology Stack

- Language: C#, Typescript/Javascript
- Testing: xUnit and Moq - Backend, React and Vitest - Frontend
- Architecture: Clean Architecture with Domain-Driven Design
- Documentation: Swagger and Markdown(You are here)
- CI/CD: GitHub Actions(planned)
- Backend: ASP.NET Core 9.0
- Frontend: React with Typescript

## Quick Start

Prerequisites

- .NET SDK 9.0+
- Node.js 18+ and npm
- Git

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd LynkzShapeGenerator/backend
   ```

### API Installation & Setup

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Run the tests** (TDD approach - tests first!)
   ```bash
   dotnet test
   ```

4. **Start the API**
   ```bash
   cd src/LynkzShapeGenerator.API
   dotnet run
   ```

5. **Access the API**
    - API Base URL: `http://localhost:5010`
    - Swagger Documentation: `http://localhost:5010/swagger`

### Running the Frontend

6. **Install the frontend dependencies**
    ```bash
   cd frontend
   npm install
    ```
7. **Run the dev server**
    ```
    npm run start
   ```

8. **Access the frontend**
    - Frontend Base URL: `http://localhost:3000`

## Project Structure

```aiignore
ShapeApp/
├── docs/
├── frontend/
├── src/
│   ├── LynkzShapeGenerator.Core/          # Domain models and services
│   │   ├── Models/                        # Shape, Point, ParseResult entities
│   │   ├── Services/                      # Business logic services
│   │   └── Interfaces/                    # Service contracts
│   └── LynkzShapeGenerator.API/           # Web API layer
│       ├── Controllers/                   # API endpoints
│       ├── DTOs/                         # Data transfer objects
│       └── Program.cs                    # Application configuration
└── tests/
    ├── LynkzShapeGenerator.Core.Tests/   # Unit tests for core logic
    └── LynkzShapeGenerator.API.Tests/    # Integration tests for API
```

## Testing

I used a TDD approach to build this project, which is not generally what I do.
However, I found it to be a good way to track my progress, and get more experience with in test driven development.
I have done testing in the past, but I have never done it in a strict TDD way.
I ran into some issues with the frontend testing environment, as a lot has changed since I last did tests in React.
I am glad I tackled thee project this way because it allowed me to learn more about the current testing environment and
TDD in general.

- I planned to do Integration tests, but I ran out of time, so I made a document to process
  manually. [here](QATests.md)

### Running the tests

* Run the tests for the backend:
    ```bash
  dotnet test
    ```
* Run the tests for the frontend:
    ```bash
    npm run test
    ```
  or
    ```bash
    npm run test:ui
    ```
    ^ for a UI test runner that shows the tests running in the browser.

  
* Note: Not all tests are currently passing. 
However, it gives an indication about what needs doing in the future. (I opted to leave it as is for now, as I wanted to get the project up and running)

## CI/CD

I would like to add CI/CD in the future, but I have not had the time to do it yet.
Using GitHub Actions would be where I would start. I have in my personal projects used it before, to deploy to an Azure app service. 
I have also considered using Azure DevOps, though I have not tried it yet.

## Deployment

I would like to deploy this project to Azure, but I have not had the time to do it yet.
[Deployment Plan](Deployment.md)

