# BillB0ard-API
![Build](https://github.com/MaximeMohandi/BillB0ard-API/actions/workflows/workflow.yml/badge.svg?event=push) [![Quality Gate Status](https://sonarqube.maximemohandi.fr/api/project_badges/measure?project=billboard-api&metric=alert_status&token=785dc26bb42c7062fc471780f8c29947d5508036)](https://sonarqube.maximemohandi.fr/dashboard?id=billboard-api) [![Coverage](https://sonarqube.maximemohandi.fr/api/project_badges/measure?project=billboard-api&metric=coverage&token=785dc26bb42c7062fc471780f8c29947d5508036)](https://sonarqube.maximemohandi.fr/dashboard?id=billboard-api) [![Security Rating](https://sonarqube.maximemohandi.fr/api/project_badges/measure?project=billboard-api&metric=security_rating&token=785dc26bb42c7062fc471780f8c29947d5508036)](https://sonarqube.maximemohandi.fr/dashboard?id=billboard-api)
## Description
It all begun with me and my friend watching movies on Discord to kill time during quarantine. At this time we were rating movies on a message that we updated each time. Now it has become a real institution. I made a [discord bot](https://github.com/MaximeMohandi/MSQBot-discord) that let us manage all the movies we have seen and we want to see and rate them. 

Now in addition to this bot I want to make a graphical interface for me and my friend. Thus this API, I didn't want to duplicate each feature on the bot and the web interface so I made a service that let all the current application use the same feature and that let me build new app around it later if i want to.

## Project notes
For now this project is strictly for my personnal usage so I didn't planned for it to be installed or used by anyone else but I'm thinking of making it public when I'll figure how to configure the database creation or a way to separate users by servers (it is not designed for that at the moment).

This project is also a training project so I will change it often and I will make a lot of refacto

## Roadmap
- Migrate all current features
- Integration of TVDB (to replace my google scraping)
- Transforme authentication to open the usage to others
