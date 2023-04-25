using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] bool isGameOver = false;
    [SerializeField] public int TURN_DURATION_IN_SECONDS = 10;
    [SerializeField] TextMeshProUGUI timer;
    private float remainingTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        TURN_DURATION_IN_SECONDS = 10;
        remainingTime = TURN_DURATION_IN_SECONDS;

        RevivePlayers();
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        while (!isGameOver)
        {
            foreach (GameObject player in players)
            {
                if (player == null)
                    yield return null;

                PlayerController playerController = player.GetComponent<PlayerController>();
                if (playerController == null)
                    yield return null;

                playerController.StartTurn();
                yield return TurnClock(playerController, player.name);

                playerController.EndTurn();

                yield return null;
            }
        }

    }

    IEnumerator TurnClock(PlayerController playerController, string playerName)
    {
        remainingTime = TURN_DURATION_IN_SECONDS;
        float clockResolution = 0.1f;

        Debug.Log($"You have {remainingTime} seconds to play.");
        UpdateTimer(remainingTime, playerName);

        while (remainingTime >= 0)
        {
            if (!playerController.IsPlayerTurn())
                yield break;

            remainingTime -= clockResolution;

            UpdateTimer(remainingTime, playerName);
            Debug.Log($"Remeaning time: {remainingTime} seconds");

            yield return new WaitForSecondsRealtime(clockResolution);
        }

        Debug.Log("Timeout!");
    }

    void UpdateTimer(float currentTime, string playerName)
    {
        int secondsLeft = (int)currentTime;
        //timer.text = string.Format("00", secondsLeft);
        timer.SetText(string.Format("{0} - {1}", secondsLeft.ToString(), playerName));
    }


    void RevivePlayers()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players)
        {
            PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerController == null)
                return;

            playerController.RevivePlayer();
        }
    }

    void TerminateTurn()
    {
        remainingTime = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
