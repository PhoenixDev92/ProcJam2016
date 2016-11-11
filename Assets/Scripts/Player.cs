using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    [SerializeField]
    private float movementSpeed;

    private bool initialized = false;

    private float movementIncrementX = 1;
    private float movementIncrementY = 1;
    private MazeRendering mazeRenderer;

    private int gridPositionRow;
    private int gridPositionCol;

    private Vector3 prevTargetPosition = Vector3.zero;

    // Counts the player's moves
    private int timesMoved = 0;

    private float moveCoolDown = 0.2f;
    private float elapsedTime = 0;

    public bool Initialized
    {
        get { return initialized; }
        set
        {
            initialized = value;
        }
    }

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

    public int TimesMoved
    {
        get { return timesMoved; }
    }

    // Use this for initialization
    void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        if (!initialized)
        {
            return;
        }

        elapsedTime += Time.deltaTime;

        int tempRow = gridPositionRow;
        int tempCol = gridPositionCol;

        // Movement input
        if (Input.GetButton("Up"))
        {
            if (elapsedTime >= moveCoolDown)
            {
                tempRow--;
                elapsedTime = 0;
            }
        }
        else if (Input.GetButton("Down"))
        {
            if (elapsedTime >= moveCoolDown)
            {
                tempRow++;
                elapsedTime = 0;
            }
        }
        else if (Input.GetButton("Left"))
        {
            if (elapsedTime >= moveCoolDown)
            {
                tempCol--;
                elapsedTime = 0;
            }              
        }
        else if (Input.GetButton("Right"))
        {
            if (elapsedTime >= moveCoolDown)
            {
                tempCol++;
                elapsedTime = 0;
            }  
        }

        // Attempt to move the player
        Vector3 currentPosition = transform.position;
        Vector3 targetPostion = mazeRenderer.CalculatePlayerGridPosition(tempRow, tempCol, gridPositionRow, gridPositionCol, currentPosition);

        // If the move was successful
        if (targetPostion != currentPosition && mazeRenderer.VerifyCell(tempRow, tempCol))
        {
            // If the grid position changed
            if (gridPositionRow != tempRow || gridPositionCol != tempCol)
            {
                timesMoved++;
            }

            gridPositionRow = tempRow;
            gridPositionCol = tempCol;
        }

        prevTargetPosition = targetPostion;

        // Move the player
        transform.position = Vector3.Lerp(transform.position, targetPostion, movementSpeed * Time.deltaTime);
    }

    // Resets the player for a new level
    public void Reset()
    {
        initialized = false;
        prevTargetPosition = Vector3.zero;
        timesMoved = 0;

        this.gameObject.SetActive(false);
    }
}
