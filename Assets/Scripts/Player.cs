using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    [SerializeField]
    private float movementSpeed;

    private float movementIncrementX = 1;
    private float movementIncrementY = 1;
    private MazeRendering mazeRenderer;

    private int gridPositionRow;
    private int gridPositionCol;

    public float MovementIncrementX
    {
        set { movementIncrementX = value; }
    }

    public float MovementIncrementY
    {
        set { movementIncrementY = value; }
    }

    public MazeRendering MazeRenderer
    {
        set { mazeRenderer = value; }
    }

    public int GridPositionRow
    {
        set { gridPositionRow = value; }
    }

    public int GridPositionCol
    {
        set { gridPositionCol = value; }
    }

    // Use this for initialization
    void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 positionBeforeInput = transform.position;
        int tempRow = gridPositionRow;
        int tempCol = gridPositionCol;

        // Movement input
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            tempRow--;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            tempRow++;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            tempCol--;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            tempCol++;
        }

        // Attempt to move the player
        Vector3 targetPostion = mazeRenderer.CalculatePlayerGridPosition(tempRow, tempCol, transform.position);
        transform.position = Vector3.Lerp(transform.position, targetPostion, movementSpeed * Time.deltaTime);

        // If the move was successful
        if (transform.position != positionBeforeInput)
        {
            gridPositionRow = tempRow;
            gridPositionCol = tempCol;
        }
    }
}
