using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyCeiling : MonoBehaviour
{
    [SerializeField] GameObject Ceiling;
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
        if (other.gameObject.CompareTag("CeilingButton"))
        {
            Ceiling.SetActive(false);
        }
    }
}
