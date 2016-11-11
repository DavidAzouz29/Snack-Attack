# Food Fight 2016
2nd Year Student Major Assessment.

## Getting Started:
- In Editor, (for now) you must start the game from the "L!_splash" scene to test the game loop.
- When Preparing a build - Make sure all Game Manager in levels are turned off after testing. Only the "L!_splash" scene should have a Game Manager turned on.
- Please read the "Programming Notes.txt" file for further reading.

## Controls:
| Character | Action 	|
| --- 		| --- 		|
|Movement| Joystick Xbox|
| --- 		| --- 		|
|LS 		|Movement|
|A	 		|Jump|
|X 			|Light Attack|
|Y 			|Heavy Attack|
|B 			|Block|
|Select/ Start 		|Scoreboard/ Pause|

|Movement| Keyboard|
| --- 		| --- 		|
|W/S 	|Forward/ Back|
|A/D 	|Left/ Right|
|LMB 	|Light Attack|
|RMB 	|Heavy Attack|
|B 	|Block|
|Space 	|Jump|
|Tab 		|Scoreboard/ Pause|


## Coding Convention:
### Variable Prefixes: 
|Variable type|Variable name|
| --- 		| --- 		|
|Function names|void GetPos()|
|float|fSpeed |
|vector 2, 3, 4's|v3Position|
|matrices 2, 3, 4's|m4WorldPos |
|arguments|void SetPos(Vector3 a_v3Pos) |

## Known Issues:
- If you use the joystick to confim a level, the level gets prepared for load twice.
- If two players share the same name, they share the same score.
- https://docs.google.com/spreadsheets/d/1XGaPM5nOZfb4ZymFyNCoNhbWRWzbf047i1xz23MyZR4/edit?usp=sharing

## Tutorials:
- Camera Zoom and U.I. concept from Tanks!: https://bitbucket.org/richardfine/scriptableobjectdemo 
- U.I. Scoreboard courtesy of http://quill18.com/unity_tutorials/Scoreboard%20Tutorial.zip 
- Unity 3d Tutorial: Making a Scoreboard #1  https://youtu.be/gjFsrVWQaQw
