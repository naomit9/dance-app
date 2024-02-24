# Danceforce Now
This is an application to keep track of the records of dancers and dance groups, as well as the showcase events they have performed in.

## Description
I created this dance project because I love dancing and used to be a part of a Toronto-based dance group and I also performed a few showcases. The application features the use of Code-First Migrations to create our database, and WebAPI and LINQ to perform CRUD operations.

## Features
* a 1-M relationship between Dancers and Dance Groups (One dance groups can have many dancer but a dancer belongs to one group)
* a M-M relationship between Dance Groups and Showcases (Many groups perform in a showcase and a showcase has many groups participating)
* ASP.NET MVC Architecture Pattern
* Entityframework code first migrations to store information in the SQL Server local database
* Language integrated query (LINQ) used for CRUD
* Search bar for dancer's names

## Future Considerations
* Due to time constraint, the image upload was half way finished. I would like to get the images up and running for each dancer bio and maybe for the group and showcase photos as well.
* Authentication
* It would be cool to implement a ticket system so users can buy tickets to the showcase events

[Youtube Demo](https://youtu.be/0RuH-v_-WbI)
