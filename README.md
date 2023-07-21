# MovieCrew  - Core API
![Build](https://github.com/MaximeMohandi/BillB0ard-API/actions/workflows/workflow.yml/badge.svg?event=push) 
[![Quality Gate Status](https://sonarqube.maximemohandi.fr/api/project_badges/measure?project=MovieCrew&metric=alert_status&token=sqb_1119b980d7d26cc2a4e8215ac276e98729e2e55c)](https://sonarqube.maximemohandi.fr/dashboard?id=MovieCrew) 
[![Maintainability Rating](https://sonarqube.maximemohandi.fr/api/project_badges/measure?project=MovieCrew&metric=sqale_rating&token=sqb_1119b980d7d26cc2a4e8215ac276e98729e2e55c)](https://sonarqube.maximemohandi.fr/dashboard?id=MovieCrew) 
[![Lines of Code](https://sonarqube.maximemohandi.fr/api/project_badges/measure?project=MovieCrew&metric=ncloc&token=sqb_1119b980d7d26cc2a4e8215ac276e98729e2e55c)](https://sonarqube.maximemohandi.fr/dashboard?id=MovieCrew) 
[![Coverage](https://sonarqube.maximemohandi.fr/api/project_badges/measure?project=MovieCrew&metric=coverage&token=sqb_1119b980d7d26cc2a4e8215ac276e98729e2e55c)](https://sonarqube.maximemohandi.fr/dashboard?id=MovieCrew)


Welcome to the Movie Rating Ecosystem - MovieCrew! This ecosystem consists of various applications and components built 
around the core API, allowing friends in the same group to rate and keep track of the movies they watch together. 
This README will provide an overview of the different components and how they work together to enhance 
the movie-watching experience for your group.

## Introduction

MovieCrew is designed to create a collaborative and fun movie-watching experience for friends. The ecosystem consists of the following components:

1.  **Core API (C#/.NET)**: The heart of the ecosystem, managing user accounts, movie details, and ratings.

2.  **Web Application**: A web-based front-end application that allows users to interact with the core API and 
access the platform's features through an intuitive and user-friendly interface.

3.  **Discord Bot Integration**: A bot that can be added to the popular messaging platforms to provide 
quick access to movie details, group ratings, and movie recommendations.

## Getting Started
### Prerequisites
Before starting the Movie Rating Platform - GroupWatch, ensure you have the following prerequisites:
- .NET Core SDK (at least version 3.1) for building and running the application locally.
- Docker installed on your machine if you want to run the application using Docker.
- MySQL server running with a database set up for the application (see [database](docs/database.md)).

### Running the Application
#### Run Locally
1. Make sure your MySQL server is running, and you have created a database for the application. 
2. Configure the database connection in the [appsettings.json](MovieCrew.API/appsettings.json)
file of your .NET Core application. 
3. Open a terminal at the root of the project and run the following commands:
```bash
cd MovieCrew.API
dotnet run
```
The application should now be running at http://localhost:5000 in your web browser.

#### Run with Docker
TO DO 

## API Documentation
For detailed information on the available API endpoints and their usage, refer 
to the API documentation.

## Origin Story
It all begun with me and my friend watching movies on Discord to kill time during quarantine. 
At this time we were rating movies on a message that we updated each time. 
Now it has become a real institution.
