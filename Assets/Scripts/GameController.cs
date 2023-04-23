using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] bool isGameOver = false;
    [SerializeField] public int TURN_DURATION_IN_SECONDS = 10;
    private int remainingTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        TURN_DURATION_IN_SECONDS = 5;
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

                Debug.Log($"You have {TURN_DURATION_IN_SECONDS} seconds to play.");

                yield return new WaitForSecondsRealtime(TURN_DURATION_IN_SECONDS);
                playerController.EndTurn();

                yield return null;
            }
        }

    }

    IEnumerator HandleTurn()
    {
        remainingTime = TURN_DURATION_IN_SECONDS;
        StartCoroutine(Countdown());
        yield return new WaitForSecondsRealtime(TURN_DURATION_IN_SECONDS);
    }

    private IEnumerator Countdown()
    {
        int duration = remainingTime;
        float normalizedTime = 0;
        while (normalizedTime <= 1f)
        {
            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }
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
