using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class HUD : MonoBehaviour {

    [SerializeField]
    private Text playerMovesText;
    [SerializeField]
    private Text minMovesText;
    [SerializeField]
    private Text timeText;
    [SerializeField]
    private Text mazesSolvedText;

    [Header("Script References")]
    [SerializeField]
    private GameController gameController;
    [SerializeField]
    private GameMode gameMode;

    [Header("Pause Menu Settings")]
    [SerializeField]
    private GameObject pausePanel;
    private bool paused = false;

    [Header("Maze Options Settings")]
    [SerializeField]
    private Button generateMazeButton;
    [SerializeField]
    private Toggle randomizedMazeSizeToggle;
    [SerializeField]
    private GameObject setSizePanel;
    [SerializeField]
    private InputField sizeInput;

    private bool sizeInputValid = false;

    // Use this for initialization
    void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        // Listen for pause input
        if (Input.GetButtonDown("Pause"))
        {
            ToggleGamePauseState();
        }

        // Determine whether or not the generate maze button should be enabled
        generateMazeButton.interactable = gameController.ReadyForNewMaze;

        // Update HUD fields
        playerMovesText.text = gameController.GetPlayerMoves().ToString();
        minMovesText.text = (gameController.GetMinMoves() < 0) ? "..." : gameController.GetMinMoves().ToString();
        mazesSolvedText.text = gameController.TotalMazesSolved.ToString();

        float time = gameController.TimeSinceGameStart;
        if (time == 0)
        {
            timeText.text = "Building Maze";
        }
        else
        {
            timeText.text = string.Format("{0:00}:{1:00}", Mathf.Floor(time / 60), Mathf.Floor(time % 60));
        }
	}

    // Toggles the pause state of the game
    public void ToggleGamePauseState()
    {
        paused = !paused;
        pausePanel.SetActive(paused);

        gameController.PauseStateChanged(paused);
    }

    // Toggle the visibilty of the set size panel
    public void ToggleSetSizePanel()
    {
        setSizePanel.SetActive(!randomizedMazeSizeToggle.isOn);
        sizeInput.text = null;
        sizeInputValid = false;
    }

    // Check if the inserted value is valid
    public void InputValueChanged()
    {
        int desiredSize;
        bool conversionSuccess = int.TryParse(sizeInput.text, out desiredSize);

        if (conversionSuccess)
        {
            if (desiredSize >= 9 && desiredSize <= 99 && (desiredSize % 2 != 0))
            {
                sizeInput.textComponent.color = Color.black;
                sizeInputValid = true;
            }
            else
            {
                sizeInput.textComponent.color = Color.red;
                sizeInputValid = false;
            }
        }
        else
        {
            sizeInput.textComponent.color = Color.red;
            sizeInputValid = false;
        }
    }

    // Informs the GameController to generate a new maze based on the updated values from the options panel
    public void GenerateNewMaze()
    {
        if (randomizedMazeSizeToggle.isOn)
        {
            gameMode.RandomizedMazeSize = true;
        }
        else
        {
            if (!sizeInputValid)
            {
                return;
            }

            gameMode.RandomizedMazeSize = false;
            gameMode.MazeSize = int.Parse(sizeInput.text);
        }

        paused = false;
        pausePanel.SetActive(false);
        gameController.PauseStateChanged(false);
        gameController.GenerateNewMaze();
    }

    // Loads the main menu
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
