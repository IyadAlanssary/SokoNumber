# SokoNumber

This project was made for the intelligent search algorithms course in my 4th year of studying Software Engineering in Damascus University.

## Game

Move the numbers along the grid and put them in their place in Sokonumber. All the numbers move in the same direction, so plan your path wisely.

The puzzles were taken from "https://www.puzzleplayground.com/soko_number"

Try it now [here](https://github.com/IyadAlanssary/SokoNumber/releases/download/v1.0.0/SokoNumber-v1.0.0.zip)

## Sneak peek

<img src="https://github.com/IyadAlanssary/SokoNumber/blob/master/Screenshots/Screenshot%202024-06-24%20021411.png" alt="<Main Menu>" width="500"/>

<img src="https://github.com/IyadAlanssary/SokoNumber/blob/master/Screenshots/Screenshot%202024-06-24%20021520.png" alt="<Level 5>" width="500"/>

<img src="https://github.com/IyadAlanssary/SokoNumber/blob/master/Screenshots/Screenshot%202024-06-24%20021558.png" alt="<Level 5 solved>" width="500"/>

<img src="https://github.com/IyadAlanssary/SokoNumber/blob/master/Screenshots/Screenshot%202024-06-24%20021611.png" alt="<Level 24>" width="500"/>

## Algorithms

You can play the puzzles yourself or you can let your computer do the hard work using one of these graph traversal algorithms:

- Breadth First Search (BFS)
- Depth First Search (DFS)
- Uniform Cost Search (UCS)
- A\*

Note: The performance of these algorithms depends on the level's size and complexity.

## Map Structure

The map is represented as a 2D string array.

- `#`: a block
- `Number`: Cells that can move and should reach their destinations.
- `Destination`: Fixed cells and are represented in the array as letters e.g. "a".
- `+/-`: Cells that increase/decrease the value of the numbers that step on them.

### Game details

The concept of the `+/-` cells is first introduced in level 7.

Starting from level 20 the numbers that move generate a block behind them so you can't go back to that position.
