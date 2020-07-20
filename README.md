# Game Prototype

This prototype tests the use of procedural animations and mesh generation in Unity.

The player character's legs are generated using inverse kinematics and procedural mesh generation. After a leg's positioning has been calculated, a flat mesh is generated in a way similar to Unity's Line Renderer component, which is then extruded.

![Inverse Kinematics](https://github.com/jonathancary1/game-prototype/blob/master/Images/Leg.gif)

For the player character's walk animation, each time a new step is performed, the resulting position of the non-pivot foot is calculated using constraints to guarantee natural movement. For example, while striding forwards or backwards, the non-pivot foot's target position is guaranteed to not cross the pivot foot with respect to the resulting player character's forward vector.

![Gameplay](https://github.com/jonathancary1/game-prototype/blob/master/Images/Game.gif)
