using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFillScript : MonoBehaviour
{
    [SerializeField] GameObject Water;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("WaterButton"))
        {
        Water.SetActive(true);
        }
    }
}
