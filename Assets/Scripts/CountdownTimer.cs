using System.Collections;
using TMPro;
using UnityEngine;

public class CountdownTimer : MonoBehaviour
{
    public int timer = 10;  
    public TMP_Text timer_text;  
    public GameObject player1_win_text;
    public GameObject player2_win_text;
    public GameObject tie_text;
    public AudioSource winSound;
    public AudioSource tieSound;

    private void Start()
    {
        StartCoroutine(CountdownCoroutine());
    }

    IEnumerator CountdownCoroutine()
    {
        while (timer > 0)
        {
            // Waiting for 1 second
            UpdateUI();
            yield return new WaitForSeconds(1f);

            // Decrease the timer by 1
            timer--;
        }

        // Timer is 0 when countdown ends then UI update
        timer = 0;
        UpdateUI();

        int player1Score = ScoreManager.getPlayer1Score();
        int player2Score = ScoreManager.getPlayer2Score();

        if (player1Score > player2Score)
        {
            Instantiate(player1_win_text, new Vector3(0, 0, 0), Quaternion.identity);
            winSound.Play();
            Debug.Log("Player 1 won!");
            
        }
        else if (player1Score < player2Score)
        {
            Instantiate(player2_win_text, new Vector3(0, 0, 0), Quaternion.identity);
            winSound.Play();
            Debug.Log("Player 2 won!");
        }
        else
        {
            Instantiate(tie_text, new Vector3(0, 0, 0), Quaternion.identity);
            tieSound.Play();
            Debug.Log("Player 1 and Player 2 tied!");
        }

        Debug.Log("Timer has finished!");
    }

    // Updating the UI text
    private void UpdateUI()
    {
        timer_text.text = timer.ToString();
    }
}
