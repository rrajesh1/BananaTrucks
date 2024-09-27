//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Net.Sockets;
//using TMPro;
//using UnityEngine;
//using UnityEngine.UI;

//public class ScoreManager : MonoBehaviour
//{
//    //public static int player1Score;
//    //public static int player2Score;


//    public static TMP_Text player1_ui_text;
//    public static TMP_Text player2_ui_text;

//    public static int player1Score = 0;
//    public static int player2Score = 0;

//    private static ScoreManager instance;

//    private void Awake()
//    {
//        instance = this;
//    }

//    void Start()
//    {
//        player1_ui_text.text = player1Score.ToString();
//        player2_ui_text.text = player2Score.ToString();
//    }
//    //public void Start()
//    //{
//    //    instance = this;
//    //    UpdateUI();
//    //}

//    public static void player1AddScore(int score)
//    {
//        player1Score += score;
//        //player1_ui_text.text = player1Score.ToString();
//        instance.UpdateUI();
//    }

//    public static void player2AddScore(int score)
//    {
//        player2Score += score;
//        //player2_ui_text.text = player2Score.ToString();
//        instance.UpdateUI();
//    }

//    private void UpdateUI()
//    {
//        player1_ui_text.text = player1Score.ToString();
//        player2_ui_text.text = player2Score.ToString();
//    }




//}

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