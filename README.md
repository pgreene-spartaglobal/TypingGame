# TypingGame

## Overview
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

##### TODO:
* ~File system - the word generator should select words from a text/csv file~
* ~Difficulty levels with different speeds~
* Allow the user to select which file they wish to generate words from
* ~Main menu - New game, quit~
* ~On game over allow the player to restart or quit - use a new user control~
* Highscores - when the game is over allow the player to store highscores
* Sound - play sounds when the player scores points and when the player loses a life
* Effects - create effects such as explosions when the player clears a word
