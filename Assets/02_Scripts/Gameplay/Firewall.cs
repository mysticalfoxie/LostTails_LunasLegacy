using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Firewall : MonoBehaviour
{
    public GameObject player; // ~unused
    //void Replay()
    //{
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); ~doesnt use fading
    //}
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag ("Player"))
        {
            //Replay();
            GameManager.RespawnPlayer(RespawnMethod.SceneReload); // uses fading
        }
    }
}
