// using UnityEngine;
// using UnityEngine.UI;
// using System.Collections;

// public class CountdownTimer : MonoBehaviour
// {
//     public static int countdownTime = 60f;
//     // private int currentTime;

//     public TMP_Text timerText;

//     void Start()
//     {
//         currentTime = countdownTime;
//         StartCoroutine(StartCountdown());
//     }

//     IEnumerator StartCountdown()
//     {
//         while (currentTime > 0)
//         {
//             timerText.text = "Time: " + currentTime.ToString("0");
//             yield return new WaitForSeconds(1f);
//             currentTime--;
//         }

//         // timerText.text = "Time: 0";
//         // TimerEnded();  // Call TimerEnded when time is up
//     }

//     // This is the TimerEnded method that is getting duplicated
//     // void TimerEnded()
//     // {
//     //     Debug.Log("Timer has ended!");
//     //     // Add logic here for when the timer ends
//     // }
// }
