using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int numRoundsToWin = 5;
    public float startDelay = 3f;
    public float endDelay = 3f;
    public CameraController cameraController;
    public Text messageText;
    public GameObject tankPrefab;
    public TankManager[] tanks;

    private int roundNum = 0;
    private WaitForSeconds startWait;
    private WaitForSeconds endWait;
    private TankManager roundWinner;
    private TankManager gameWinner;

    private void Start()
    {
        startWait = new WaitForSeconds(startDelay);
        endWait = new WaitForSeconds(endDelay);

        SpawnAllTanks();
        SetCameraTargets();

        StartCoroutine(GameLoop());
    }

    private void SpawnAllTanks()
    {
        for (int i = 0; i < tanks.Length; i++)
        {
            tanks[i].instance = Instantiate(tankPrefab,tanks[i].spawnPoint.position,tanks[i].spawnPoint.rotation) as GameObject;
            tanks[i].playerNum = i + 1;
            tanks[i].Setup();
        }
    }

    private void SetCameraTargets()
    {
        Transform[] targets = new Transform[tanks.Length];
        for (int i = 0; i < targets.Length; i++)
        {
            targets[i] = tanks[i].instance.transform;
        }
        cameraController.targets = targets;
    }

    //游戏循环协程
    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(RoundStarting());
        yield return StartCoroutine(RoundPlaying());
        yield return StartCoroutine(RoundEnding());
        if (gameWinner != null)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            StartCoroutine(GameLoop());
        }
    }

    //回合准备协程
    private IEnumerator RoundStarting()
    {
        ResetAllTanks();
        DisableTankControl();
        cameraController.SetStartPositionAndSize();
        roundNum++;
        messageText.text = "ROUND " + roundNum;
        yield return startWait;
    }

    //回合开始协程
    private IEnumerator RoundPlaying()
    {
        EnableTankControl();
        messageText.text = string.Empty;
        while (!OneTankLeft())
        {
            yield return null;
        }
    }

    //回合结束协程
    private IEnumerator RoundEnding()
    {
        DisableTankControl();
        roundWinner = null;
        roundWinner = GetRoundWinner();
        if (roundWinner != null)
            roundWinner.wins++;
        gameWinner = GetGameWinner();
        string message = EndMessage();
        messageText.text = message;
        yield return endWait;
    }

    private bool OneTankLeft()
    {
        int numTanksLeft = 0;
        for (int i = 0; i < tanks.Length; i++)
        {
            if (tanks[i].instance.activeSelf)
                numTanksLeft++;
        }
        return numTanksLeft <= 1;
    }

    private TankManager GetRoundWinner()
    {
        for (int i = 0; i < tanks.Length; i++)
        {
            if (tanks[i].instance.activeSelf)
                return tanks[i];
        }
        return null;
    }

    private TankManager GetGameWinner()
    {
        for (int i = 0; i < tanks.Length; i++)
        {
            if (tanks[i].wins == numRoundsToWin)
                return tanks[i];
        }
        return null;
    }

    private string EndMessage()
    {
        string message = "DRAW!";
        if (roundWinner != null)
            message = roundWinner.coloredPlayerText + "WINS THE ROUND!";
        if (gameWinner != null)
            message = gameWinner.coloredPlayerText + " WINS THE GAME!";
        message += "\n\n\n\n";
        for (int i = 0; i < tanks.Length; i++)
        {
            message += tanks[i].coloredPlayerText + ": " + tanks[i].wins + " WINS\n";
        }
        return message;
    }

    private void ResetAllTanks()
    {
        for (int i = 0; i < tanks.Length; i++)
        {
            tanks[i].Reset();
        }
    }

    private void EnableTankControl()
    {
        for (int i = 0; i < tanks.Length; i++)
        {
            tanks[i].EnableControl();
        }
    }


    private void DisableTankControl()
    {
        for (int i = 0; i < tanks.Length; i++)
        {
            tanks[i].DisableControl();
        }
    }
}
