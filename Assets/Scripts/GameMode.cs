using UnityEngine;
using System.Collections;

public class GameMode : MonoBehaviour {

    public enum Mode { ENDLESS }

    [SerializeField]
    private Mode currentMode;

    [Header("Endless Mode Settings")]
    [SerializeField]
    private bool randomMazeSize;
    [SerializeField]
    private int mazeSize;

    public bool RandomizedMazeSize
    {
        set { randomMazeSize = value; }
    }

    public int MazeSize
    {
        set { mazeSize = value; }
    }

    // Return the size of the next maze that should be generated
    public int GetNextMazeSize()
    {
        if (randomMazeSize)
        {
            int randomSize = Random.Range(9, 29);

            // Change the size if the generated number was even
            while (randomSize % 2 == 0)
            {
                randomSize = Random.Range(9, 29);
            }

            return randomSize;
        }
        else
        {
            return mazeSize;
        }
    }
}
