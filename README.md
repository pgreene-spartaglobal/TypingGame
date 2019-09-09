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

##### TODO:
* File system - the word generator should select words from a text/csv file
* Allow the user to select which file they wish to generate words from
* Main menu - New game, quit
* On game over allow the player to restart or quit
* Highscores - when the game is over allow the player to store highscores
* Sound - play sounds when the player scores points and when the player loses a life
* Effects - create effects such as explosions when the player clears a word
