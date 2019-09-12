# Typing Spartan

This is my first project created as part of my training during Sparta Academy.

## Game Overview
**Words are falling!**

Type as quickly as you can to clear the words before they reach the bottom of the screen. 

The longer you can survive the better your score!

### Day 1 Summary
Day 1 was focused on implementing the most important gameplay elements
#### Main classes
* Main Window - Handles the main game functionality and dispatch timer including score, lives, adding/removing from canvas and game over
* Word Manager - Contains and manages the list of 'Words'
* Word - The class that is created when the game creates a new word to fall down the screen, holds data about the word
* Word Generator - Used to generate a random word string that will populate the Word value 

#### Major Implementations
* Words spawn at the top of the screen before falling 
* User can type words to clear them 
* Game difficulty increases each time the player scores points
* Score - increases when the player clears a word
* Lives - decreases when a word reaches the bottom of the screen
* Game over - when the player loses all of their lives

#### Day 1 Game Image
![alt text](https://i.imgur.com/2ofW1zl.png "Day 1 Typing Spartan")

### Day 2 Summary
Day 2 focused on learning how to implement and improve the existing UI as well as further improvements to the gameplay

#### Major Implementations
* The game reads random words from a text file rather than the words being hard coded
* Main Menu - The main menu appears in a seperate window 
* Difficulty levels - The user can select from easy, normal and hard. This affects the speed at which words fall.
* Game over - User control dialog that appears when the player loses the game. This new dialog allows the player to restart and select another difficulty 

### Day 3 Summary
Added a new window for the user to submit highscores by writing to a file. This file is then read to display all the previously achieved scores 

![alt text](https://i.imgur.com/2k2yG5a.png "Day 3 Submit Highscore")
![alt text](https://i.imgur.com/63WOAWE.png "Day 3 Highscores")

##### TODO:
* ~File system - the word generator should select words from a text/csv file~
* ~Difficulty levels with different speeds~
* Allow the user to select which file they wish to generate words from
* ~Main menu - New game, quit~
* ~On game over allow the player to restart or quit - use a new user control~
* ~Highscores - when the game is over allow the player to store highscores~
* Sound - play sounds when the player scores points and when the player loses a life
* Effects - create effects such as explosions when the player clears a word

## Game Implementation
The game consists of four main classes: MainWindow, WordManager, Word and Word Generator. 

### Word class
When the game starts words begin to fall from the screen. 

Each of these falling words is encapsulated within a Word object. 

This object contains data that has its string value, its index and TextBlock. The TextBlock being the UIElement that is seen by the player. The reason for creating a variable to store the index is so that the program can know how many letters the player has typed of the word.

### Word Manager
By creating an object to store all of this data about the word, the data can be stored and passed around easily with the WordManager being responsible for managing all the words through a list.

### Word Generator
The word generator is responsible for storing all possible words and then generating a random word. The word manager then calls  the word generator to generate a new word to add the list of words.

### MainWindow
The main window manages the game logic such as score, lives, game over as well as the UI elements on the screen

