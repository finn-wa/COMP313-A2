# COMP313-A2 — LABYRINTH
*Finn Welsford-Ackroyd and Emily Fung*

## Basic outline of game:  
* The player's goal is to reach the top of a maze-like tower
* The player must avoid patrolling enemies
* The enemies are robots with a cone of light representing their field of vision
* If the player enters the cone of light, the bots damage the player
* They will also stop patrolling and chase the player
* If the player sneaks behind an enemy they can press a button on its back to disable it

## Instructions
The scene file path is Assets/Scene/Labyrinth. There's also an exported build in the root of the repository.

## Architecture
Labyrinth uses a basic architecture. There is a very minimal GameState script in the root of the scene which handles the restarting of the game. Enemies store their own AI scripts and use Unity Events to track the location of the player. The player's health is tracked with a basic Health script in the Player object.

## Libraries and Assets
The [Panda BT](https://assetstore.unity.com/packages/tools/ai/panda-bt-free-33057) library was used to implement Behaviour Tree AI for the enemies.
Unity Standard Assets were used for the FPSController and the ProceduralSkybox.

## Behaviour Trees
The most technically interesting part of the prototype is the Behaviour Tree AI. Panda BT provides a simple scripting interface for handling the tree structure, which is then linked to C# scripts for task implementation. The enemies switch between three trees: Patrol, Attack, and Search. While this is simple enough AI to be implemented in a Finite State Machine, the Panda BT scripting interface allowed for a clear separation of decision-making logic and utility methods which made for a quick development process.
