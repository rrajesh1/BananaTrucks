using System.Collections;
using TMPro;
using UnityEngine;

public class CountdownTimer : MonoBehaviour
{
    public int timer = 60;  // Countdown starting time in seconds
    public TMP_Text timer_text;  // Reference to the TextMeshProUGUI text component

    private void Start()
    {
        // Start the countdown coroutine
        StartCoroutine(CountdownCoroutine());
    }

    // Coroutine to handle the countdown logic
    IEnumerator CountdownCoroutine()
    {
        while (timer > 0)
        {
            // Update the UI each second
            UpdateUI();

            // Wait for 1 second
            yield return new WaitForSeconds(1f);

            // Decrease the timer by 1
            timer--;
        }

        // Ensure the timer hits 0 and updates the UI when the countdown is finished
        timer = 0;
        UpdateUI();

        int player1Score = ScoreManager.getPlayer1Score();
        int player2Score = ScoreManager.getPlayer2Score();

        if (player1Score > player2Score)
        {
            Debug.Log("Player 1 won!");
        }
        else if (player1Score < player2Score)
        {
            Debug.Log("Player 2 won!");
        }
        else
        {
            Debug.Log("Player 1 and Player 2 tied!");
        }

        Debug.Log("Timer has finished!");
        // Optional: Trigger any event when the timer ends
    }

    // Method to update the UI text
    private void UpdateUI()
    {
        timer_text.text = timer.ToString();
    }
}
