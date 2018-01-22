using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Class Description - LifeBehavior
 * 
 * 
 * Race Conditions:
 * 1. Must take to account the other tiles that may or may not change their state before others. It may cause the system to run incorrectly.
 * SOLUTION #1: Each tile takes a "snapshot" of their surrounding tiles before updating their state during standby.
 * 		ISSUES:
 * 			1. I still need to be careful about the race condition for this solution too. Need to make sure each snapshot is consistent with the other.
 * 			2. Considering a small system, this may not be an issue however as we scale up, we will notice a small slowdown during the snapshot phase.
 * 		DESIGN:
 * 			1. Each tile will have an array of size 8 to indicate the surrounding tiles. 
 * 			2. ex. int grid_snapshot = {0,0,0,1,1,0,1,1};
 * 				|0|0|0|
 * 				|1|x|1|
 * 				|0|1|1|
 * 			3. updateState()
 * 				Just update the tile state according to the grid. Then during standby, take another snapshot.
 * SOLUTION #2: Tiles do not update their current state until all tiles broadcast that they are ready.
 * 		ISSUES: 
 * 			1. May be a bit more complicated in design.
 */

public class LifeBehavior3d : MonoBehaviour
{
    public enum Status { DEAD, ALIVE };
    public Status state = Status.DEAD; //At default, all life objects are dead unless specified.
    public int TILE_ID = 0; //Default value is 0, however ID's will be assigned as MainGrid generates it.
    public GameObject[] neighbors;
    public int[] grid_snapshot;
    // Use this for initialization

    void Start()
    {
        grid_snapshot = new int[8];

        takeSnapshot();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == Status.ALIVE)
            gameObject.GetComponent<MeshRenderer>().enabled = true;
        else
            gameObject.GetComponent<MeshRenderer>().enabled = false;

    }

    void takeSnapshot()
    {
        //turn the neighbors' state values to grid_snapshot
        int i = 0;
        foreach (GameObject tile in neighbors)
        {
            if (tile != null)
            {
                switch (tile.GetComponent<LifeBehavior3d>().getState())
                {
                    case Status.DEAD:
                        grid_snapshot[i] = 0;
                        break;
                    case Status.ALIVE:
                        grid_snapshot[i] = 1;
                        break;
                }
            }
            i++;
        }
    }

    void updateState()
    {
        /* Scenarios
         * #1 Any live cell with fewer than two live neighbours dies. | SUM < 2
         * #2 Any live cell with two or three live neighbours lives. | SUM == 2 OR SUM == 3
         * #3 Any live cell with more than three live neighbours dies. | SUM > 3
         * #4  Any dead cell with exactly three live neighbours becomes a live cell. | SUM == 3
         * */

        int SUM = 0;
        foreach (int num in grid_snapshot)
            SUM += num;

        //No case for SUM == 2 because that is a neutral state.
        if (SUM < 2 || SUM > 3)
            state = Status.DEAD;
        else if (SUM == 3) //Tile is always alive in this case
            state = Status.ALIVE;
    }

    public Status getState()
    {
        return state;
    }

    public void setState(int s)
    {
        switch (s)
        {
            case 0:
                state = Status.DEAD;
                break;
            case 1:
                state = Status.ALIVE;
                break;
        }
    }

    public int getID()
    {
        return TILE_ID;
    }

    public void setID(int new_id)
    {
        TILE_ID = new_id;
    }
}
