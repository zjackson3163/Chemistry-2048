using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Timers;
using UnityEngine;
using TMPro;    

public class GameManager : MonoBehaviour
{
    public TileBoard board;
    public CanvasGroup gameOver;
    public CanvasGroup gameWon;
    public TextMeshProUGUI scoreText;
    private int score;

    private void Start()
    {
        NewGame();
    }

    public void NewGame()
    {
        gameOver.alpha = 0f;
        gameOver.interactable = false;
        gameWon.alpha = 0f;
        gameWon.interactable = false;

        setScore(0);
        board.ClearBoard();
        board.CreateTile();
        board.CreateTile();
        board.enabled = true;
    }

    public void GameOver()
    {
        gameOver.interactable = true;
        board.enabled = false;  
        StartCoroutine(Fade(gameOver, 1f, 1f));
    }

    public void GameWon()
    {
        gameWon.interactable = true;
        board.enabled = false;
        StartCoroutine(Fade(gameWon, 1f, 1f));
    }

    private IEnumerator Fade(CanvasGroup canvasGroup, float to, float delay)
    {
        yield return new WaitForSeconds(delay);

        float elapsed = 0f;
        float duration = 0.5f;
        float from = canvasGroup.alpha;

        while (elapsed < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = to;
    }

    public void IncreaseScore(int points)
    {
        setScore(points + score);
    }

    private void setScore(int score)
    {
        this.score = score;
        scoreText.text = score.ToString();
    }


}
