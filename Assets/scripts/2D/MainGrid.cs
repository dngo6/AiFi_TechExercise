using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainGrid : MonoBehaviour
{
    /*
	 * This will indicate a "broadcast message" towards the tiles in the system.
	 * STANDBY indicates that the tiles do not update their status yet
	 * UPDATE will broadcast a message to the tiles to change their states accordingly.
	*/
    public enum SystemStatus { STANDBY, UPDATE };
    public SystemStatus status = SystemStatus.STANDBY;

    public const int TILE_SIZE = 16; //by pixel [UNUSED]
    public int GRID_SIZE = 32; //by tile

    public GameObject tilePrefab;
    public float genPerSec = 2;
    
    private GameObject[,] tile;
    private int generation = 0;
    private float timer;
    public Text genCount;

    // Use this for initialization
    void Start()
    {
        tile = new GameObject[GRID_SIZE, GRID_SIZE];
        generateGrid();
        Debug.Log("CONTROLS: Press \"Space\" to increment generations. Use the mouse scroll to to zoom in/out with the camera.");
    }

    // Update is called once per frame
    void Update()
    {
        if (status == SystemStatus.UPDATE)
        {
            BroadcastMessage("takeSnapshot");
            status = SystemStatus.STANDBY;
        }

        //For testing purposes, each generation will increment per button click
        if (Input.GetKey("space") && status == SystemStatus.STANDBY)
        {
            status = SystemStatus.UPDATE;
            BroadcastMessage("updateState");
            generation++;
            genCount.text = "Generation: " + generation;
        }
        
    }

    void generateGrid()
    {
        int id = 0;
        for (int i = 0; i < GRID_SIZE; i++)
        {
            for (int j = 0; j < GRID_SIZE; j++)
            {
                tile[i, j] = Instantiate(tilePrefab, new Vector3(j*tilePrefab.transform.localScale.x, -(i*tilePrefab.transform.localScale.y), 0), Quaternion.identity, gameObject.transform);
                tile[i, j].GetComponent<LifeBehavior>().setID(id);
                tile[i, j].GetComponent<LifeBehavior>().setState(Random.Range(0,2));
                id++;
            }
        }
        //now assign neighbors to each tile
        assignNeighbors();
    }

    void assignNeighbors()
    {
        for (int i = 0; i < GRID_SIZE; i++)
        {
            for (int j = 0; j < GRID_SIZE; j++)
            {
                /*
                 * For Simplicity's sake, we will wrap the world around edge to edge as calculated by the modulus
                 */
                tile[i, j].GetComponent<LifeBehavior>().neighbors = new GameObject[8];
                if (i != 0)
                {
                    tile[i, j].GetComponent<LifeBehavior>().neighbors[1] = tile[(i - 1) % GRID_SIZE, j % GRID_SIZE]; //top
                    if (j != 0)
                        tile[i, j].GetComponent<LifeBehavior>().neighbors[0] = tile[(i - 1) % GRID_SIZE, (j - 1) % GRID_SIZE]; //top left
                
                    if (j != GRID_SIZE - 1)
                        tile[i, j].GetComponent<LifeBehavior>().neighbors[2] = tile[(i - 1) % GRID_SIZE, (j + 1) % GRID_SIZE]; //top right
                }

                if (j != 0)
                    tile[i, j].GetComponent<LifeBehavior>().neighbors[3] = tile[i % GRID_SIZE, (j - 1) % GRID_SIZE]; //left
                if (j != GRID_SIZE - 1)
                    tile[i, j].GetComponent<LifeBehavior>().neighbors[4] = tile[i % GRID_SIZE, (j + 1) % GRID_SIZE]; //right

                if (i != GRID_SIZE - 1)
                {
                    tile[i, j].GetComponent<LifeBehavior>().neighbors[6] = tile[(i + 1) % GRID_SIZE, j % GRID_SIZE]; //bottom
                    if (j != 0)
                        tile[i, j].GetComponent<LifeBehavior>().neighbors[5] = tile[(i + 1) % GRID_SIZE, (j - 1) % GRID_SIZE]; //bottom left
                    if (j != GRID_SIZE - 1 )
                        tile[i, j].GetComponent<LifeBehavior>().neighbors[7] = tile[(i + 1) % GRID_SIZE, (j + 1) % GRID_SIZE]; //bottom right
                }
            }
        }
    }
}
