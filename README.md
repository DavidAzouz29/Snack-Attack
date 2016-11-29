# Food Fight 2016
2nd Year Student Major Assessment. [LINK](https://github.com/DavidAzouz29/Food-Fight-2016-Prototype-Unity)

## Getting Started:
- In Editor, (for now) you must start the game from the "L!_splash" scene to test the game loop.
- When Preparing a build - Make sure all Game Manager in levels are turned off after testing. Only the "L!_splash" scene should have a Game Manager turned on.
- This game is intended to be played with a joystick and other people. However please read the "Known Issues" section below for bugs.
- Please read the "Programming Notes.txt" file for further reading.

## Controls:
|Movement	| Keyboard		|Movement	| Joystick Xbox|               
| --- 		| --- 			| --- 		| --- 		|               
|W/S 		|Forward/ Back	|LS 		|Movement|                  
|A/D 		|Left/ Right	|A	 		|Jump|                      
|LMB 		|Light Attack	|X 			|Light Attack|              
|RMB 		|Heavy Attack	|Y 			|Heavy Attack|              
|B 			|Block			|B 			|Block|                     
|Space 		|Jump			|Select/ Start 		|Scoreboard/ Pause| 
|Tab 		|Scoreboard/ Pause|                                        

## Coding Convention:
### Variable Prefixes: 
|Variable type	|Variable name|
| --- 			| --- 		|
|Function names	|void GetPos()|
|int			|iHealth |
|float			|fSpeed |
|member variables|m_fSpeed |
|enum			|E_PLAYER_STATE |
|vector 2, 3, 4's|v3Position|
|matrices 2, 3, 4's|m4WorldPos |
|arguments		|void SetPos(Vector3 a_v3Pos) |
|component 		|Image c_playerImage |
|reference to a script|PlayerController r_PlayerCon |

### Code Examples:
Getters and Setters will be inlined where possible.

## Known Issues:
- Sometimes you lose control of your character (they keep walking straight) - hold "B" (either Keyboard or Joystick) to combat this.
- https://docs.google.com/spreadsheets/d/1XGaPM5nOZfb4ZymFyNCoNhbWRWzbf047i1xz23MyZR4/edit?usp=sharing

## Tutorials:
- Camera Zoom and U.I. concept from Tanks!: https://bitbucket.org/richardfine/scriptableobjectdemo 
- U.I. Scoreboard courtesy of http://quill18.com/unity_tutorials/Scoreboard%20Tutorial.zip 
- Unity 3d Tutorial: Making a Scoreboard #1  https://youtu.be/gjFsrVWQaQw
