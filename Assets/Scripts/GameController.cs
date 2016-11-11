using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

    [SerializeField]
    private Player player;
    [SerializeField]
    private GameObject maze;

    private MazeRendering mazeRenderer;
    private bool playerInitilaized = false;

    void Awake()
    {
        mazeRenderer = maze.GetComponent<MazeRendering>();
    }

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	    if (mazeRenderer.IsMazeGenerated && !playerInitilaized)
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

            playerInitilaized = true;
        }
	}
}
