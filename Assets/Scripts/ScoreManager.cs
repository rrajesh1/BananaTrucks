using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static int player1Score;
    public static int player2Score;

    public TMP_Text player1_ui_text;
    public TMP_Text player2_ui_text;

    private static ScoreManager instance;

    public void Start()
    {
        instance = this;
        UpdateUI();
    }

    public static void player1AddScore(int score)
    {
        player1Score += score;
        instance.UpdateUI();
    }

    public static void player2AddScore(int score)
    {
        player2Score += score;
        instance.UpdateUI();
    }

    private void UpdateUI()
    {
        player1_ui_text.text = player1Score.ToString();
        player2_ui_text.text = player2Score.ToString();
    }
}
