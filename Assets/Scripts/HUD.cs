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

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        playerMovesText.text = gameController.GetPlayerMoves().ToString();
        minMovesText.text = gameController.GetMinMoves().ToString();
	}
}
