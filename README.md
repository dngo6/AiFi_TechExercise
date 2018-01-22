# AiFi_TechExercise
Unity project files for AiFi's exercise for Conway's Game of Life.

# System Design Specification
## Conway’s Game of Life
### Updated: January 21, 2018
## Prompt
Your task is to use Unity to create an implementation of Conway’s Game of Life in both 2D and 3D. You can read up on the game on the Wikipedia page. For a simplified explanation of the goals, here are the rules for the 2D implementation, copied from Wikipedia:
- Any live cell with fewer than two live neighbours dies.
- Any live cell with two or three live neighbours lives.
- Any live cell with more than three live neighbours dies.
- Any dead cell with exactly three live neighbours becomes a live cell.

## User Definable Attributes
### Grid Size (default 32x32 tiles)
The grid will always be a square. The size of the grid can be specified by the user via the beginning of the simulation or through the presets.
The unit of measurement is in Tiles.
## Controls
The user can increment to the next generation by pressing “Space”. The reason being is just in case they want to stop at a certain generation for observation.
## System
The system in Unity consists of two classes: MainGrid and LifeBehavior
### MainGrid - Parent Object
#### Attributes
SystemState - Has two states {STANDBY, UPDATE}

SystemState is STANDBY until the end of the generation then, it becomes UPDATE for 1 frame
UPDATE will allow MainGrid to BroadcastMessage to all of the tile objects to call updateState()
private void generateGrid() - Creates the grid and instantiate TILE child objects based on initial specifications.

void Update() 
Countdown per frame until trigger the next generation
On UPDATE, BroadcastMessage(“updateState”)

If SystemState == UPDATE, then BroadcastMessage(“takeSnapshot”)
SystemState = STANDBY
##### Future Features
private void loadGrid() - Reads an input file  (see Notes) and generates the grid based on the binary info. 

float genPerSecond - indicates generations per second.
Calculated by Time.deltaTime/genPerSecond

float timeLeft - just counts down how many frames before the next UPDATE.
If timeleft == 0, SystemState = UPDATE

## LifeBehavior - Child Object
### Attributes
Status - Has two states {DEAD, ALIVE}

GameObject neighbors[8] - Array of neighboring TILE objects that this TILE can pull states from.
- Same format as grid_snapshot

grid_snapshot[0] = neighbors[0].getState();
Assignments for each element are to be handled by the MainGrid

int grid_snapshot [8]			
1. Each tile will have an array of size 8 to indicate the surrounding tiles. 
2. ex. int grid_snapshot = {0,0,0,1,1,0,1,1};

|0|0|0|

|1|x|1|

|0|1|1|

3. In the case that the tile is on the corner or the edge, the boundaries will be 0

4.  ex. int grid_snapshot = {1,1,0,1,0,0,0,0}; //bottom right corner

|1|1|0|

|1|x|0|

|0|0|0|

int TILE_ID - each tile will have an ID to use as a reference

private void takeSnapshot()
On standby, take a quick snapshot of the states of the surrounding tiles.
Should only be called once per generation.

private void  updateState()
int scenario = 1; 

1 = underpopulated = death 

2 = just right = lives

3 = overpopulated = death

4 = reproduction = lives (revives)

Loop through the grid_snapshot array to determine the scenario based on the sum of the grid values.
sum < 2, then scenario 1

sum == 2 OR sum == 3, then scenario 2

Don’t change state.
sum > 3, then scenario 3

sum == 3, then scenario 4

if DEAD, then switch to ALIVE

## Notes:
### BroadcastMessage()
Two messages to be broadcasted: STANDBY and UPDATE
#### STANDBY
Tiles will take the snapshot of the surrounding tiles for them to properly update their state when UPDATE is called. Call takeSnapshot()
#### UPDATE
Tiles will update their state accordingly. Call updateState().
After the broadcast, change SystemState to STANDBY.
### Future Features:
Some of these features were planned in the initial stages of design. However, they will be implemented later for the interest of time.
#### Time Unit (default 1 second)
Describes the lifetime of each generation. At the end of each generation, the MainGrid will BroadcastMessage to the Tiles to update their states (DEAD or ALIVE) accordingly.
1 second = 1 generation/second
0.5 second = 1 generation/0.5 second
#### End Generation (default 1000 generations) 
Just indicates the last generation to end the system.
#### Input File
Below is an example of an initial condition of the simulation. Users can specify the size of the grid as well as the initial states of each tile (0 is DEAD and 1 is ALIVE).
initial_condition.txt
8
00000100
01100000
01100000
00000110
00100000
00010000
00001000
01000000
The user can specify the initial conditions for the simulation. 
#### Tile Size (default 16x16 pixels)
This is more for aesthetics. This is just the size of the tile by pixels.
