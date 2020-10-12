# Flare.Battleship.Api
This is a Flare Coding Exercise with the following tasks to accomplish:
## The task
The task is to implement a Battleship state-tracker for a single player that must support the
following logic:
* Create a board
* Add a battleship to the board
* Take an “attack” at a given position, and report back whether the attack resulted in a
hit or a miss
* Return whether the player has lost the game yet (i.e. all battleships are sunk)
## Background
This exercise is based on the classic game “Battleship”.
* Two players
* Each player has a 10x10 board
* During setup, players can place an arbitrary number of “battleships” on their board.
The ships are 1-by-n sized, must fit entirely on the board, and must be aligned either
vertically or horizontally.
* During play, players take a turn “attacking” a single position on the opponent’s board,
and the opponent must respond by either reporting a “hit” on one of their battleships
(if one occupies that position) or a “miss”
* A battleship is sunk if it has been hit on all the squares it occupies
* A player wins if all of their opponent’s battleships have been sunk.
		
## Building it locally
Flare.Battleship.Api is built against ASP.NET Core 3.1
It will run with F5 in Visual Studio or the usual dotnet commands from the root folder where the SLN file is located:
* dotnet restore
* dotnet build
* dotnet run --project .\src\Flare.Battleship.Api\Flare.Battleship.Api.csproj

And to run the tests:
* dotnet test

## Swagger
Swagger endpoint gives information about api endpoints and their models.

## TODO
Repository code for the persistance layers was not implemented as per instructions.
Methods should be made into asynchronous once Repository code is complete.
Logging is only made to console sink.
