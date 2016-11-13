using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GameMode))]
public class GameController : MonoBehaviour {

    [SerializeField]
    private Player player;
    [SerializeField]
    private GameObject maze;

    // Reference to the GameMode script
    private GameMode gameMode;
    // Reference to the AudioSource
    private AudioSource audioSrc;

    private MazeGeneration2 mazeGenerator;
    private MazeRendering mazeRenderer;
    private bool destinationReached = false;
    private bool gameStarted = false;
    private float timeSinceGameStart = 0;
    private int totalMazesSolved = 0;
    private bool readyForNewMaze = false;

    public float TimeSinceGameStart
    {
        get { return timeSinceGameStart; }
    }

    public int TotalMazesSolved
    {
        get { return totalMazesSolved; }
    }

    public bool ReadyForNewMaze
    {
        get { return readyForNewMaze; }
    }

    void Awake()
    {
        gameMode = GetComponent<GameMode>();
        audioSrc = GetComponent<AudioSource>();
        mazeGenerator = maze.GetComponent<MazeGeneration2>();
        mazeRenderer = maze.GetComponent<MazeRendering>();
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
            player.MazeRenderer = mazeRenderer;
            player.gameObject.SetActive(true);

            player.Initialized = true;
            player.CanMove = true;
            gameStarted = true;
            readyForNewMaze = true;
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

        audioSrc.Play();
        destinationReached = true;
        totalMazesSolved++;
        player.CanMove = false;
        GenerateNewMaze();
    }

    // Initiates the creation of a new maze
    public void GenerateNewMaze()
    {
        readyForNewMaze = false;
        int newMazeSize = gameMode.GetNextMazeSize();

        mazeRenderer.Reset();
        player.Reset();
        mazeGenerator.GenerateNewMaze(newMazeSize, newMazeSize);

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

    public void PauseStateChanged (bool paused)
    {
        player.CanMove = !paused;
    }
}
