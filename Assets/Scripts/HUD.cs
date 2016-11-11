using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUD : MonoBehaviour {

    [SerializeField]
    private GameController gameController;
    [SerializeField]
    private Text playerMovesText;
    [SerializeField]
    private Text minMovesText;
    [SerializeField]
    private Text timeText;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        playerMovesText.text = gameController.GetPlayerMoves().ToString();
        minMovesText.text = gameController.GetMinMoves().ToString();

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
}
