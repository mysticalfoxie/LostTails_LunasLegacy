using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioAbspielen : MonoBehaviour
{
    [SerializeField] private AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            audio.Play();
        }
    }
}
