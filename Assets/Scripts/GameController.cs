using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

    [SerializeField]
    private Player player;
    [SerializeField]
    private GameObject maze;

    private MazeGeneration2 mazeGenerator;
    private MazeRendering mazeRenderer;
    private bool destinationReached = false;
    private bool gameStarted = false;
    private float timeSinceGameStart = 0;

    public float TimeSinceGameStart
    {
        get { return timeSinceGameStart; }
    }

    void Awake()
    {
        mazeGenerator = maze.GetComponent<MazeGeneration2>();
        mazeRenderer = maze.GetComponent<MazeRendering>();
    }

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	    if (mazeRenderer.IsMazeGenerated && !player.Initialized)
        {
            // Move the player's start position forward on the z axis
            Vector3 playerStartPosition = mazeRenderer.StartingPosition;
            playerStartPosition.z = -1;

            player.transform.position = playerStartPosition;
            player.transform.localScale = mazeRenderer.ObjectCalculatedScale;
            player.GridPositionRow = mazeRenderer.StartingGridRow;
            player.GridPositionCol = mazeRenderer.StartingGridCol;
            player.MovementIncrementX = mazeRenderer.ColIncrement;
            player.MovementIncrementY = mazeRenderer.RowIncrement;
            player.MazeRenderer = mazeRenderer;
            player.gameObject.SetActive(true);

            player.Initialized = true;
            gameStarted = true;
        }


        if (gameStarted)
        {
            timeSinceGameStart += Time.deltaTime;
        }
    }

    // Used to indicate that the player has reached the destination
    public void PlayerReachedDestination()
    {
        if (destinationReached)
        {
            return;
        }

        destinationReached = true;

        Debug.Log("Level Complete");
        mazeRenderer.Reset();
        player.Reset();
        mazeGenerator.GenerateNewMaze();

        destinationReached = false;
    }

    // Returns the number of times that the player has moved
    public int GetPlayerMoves()
    {
        return player.TimesMoved;
    }

    // Returns the minimum amount of moves required to solve the maze
    public int GetMinMoves()
    {
        return mazeRenderer.MinMovesToDestination;
    }
}
