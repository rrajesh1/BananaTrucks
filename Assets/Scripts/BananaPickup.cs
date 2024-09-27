//using UnityEngine;

//public class BananaPickup : MonoBehaviour
//{
//    GameObject Banana;



//    public void OnCollisionEnter2D(Collision2D collision)
//    {
//        GameObject pickupObject = collision.gameObject;
//        // TODO change this to collect multiple bananas
//        if (collision.gameObject.CompareTag("Banana") && Banana == null)
//        {
//            collision.transform.SetParent(transform);
//            collision.transform.localPosition = new Vector3(0.5f, 0, -1f);
//            Banana = collision.gameObject;
//            // TODO update player score         
//        }

//    }

//    //public void OnTriggerEnter2D(Collider2D other)
//    //{
//    //    GameObject pickupObject = other.gameObject;
//    //    // TODO change this to collect multiple bananas
//    //    if (other.CompareTag("Banana") && Banana == null)
//    //    {
//    //        other.transform.SetParent(transform);
//    //        other.transform.localPosition = new Vector3(0.5f, 0, -1f);
//    //        Banana = other.gameObject;
//    //        // TODO update player score         
//    //    } 

//    //}


//}

//using UnityEngine;

//public class BananaPickup : MonoBehaviour
//{
//    GameObject Banana;

//    public void OnCollisionEnter2D(Collision2D collision)
//    {
//        if (collision.gameObject.CompareTag("Banana"))
//        {
//            // Collect the banana
//            Banana = collision.gameObject;
//            Destroy(Banana); // Make the banana disappear

//            // Update score for the player
//            Player player = GetComponent<Player>();
//            if (player.playerNumber == 1)
//            {
//                ScoreManager.player1AddScore(1);
//            }
//            else if (player.playerNumber == 2)
//            {
//                ScoreManager.player2AddScore(1);
//            }

//            // Spawn a new banana
//            player.SpawnNewBanana();
//        }
//    }
//}
