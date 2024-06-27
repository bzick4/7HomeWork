using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PauseScript : MonoBehaviour
{
    private bool isPauseGame;

    public void PausedGame()
    {
        if (isPauseGame)
        {
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 0;
        }

        isPauseGame = !isPauseGame;
    }
}
