using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFillScript : MonoBehaviour
{
    [SerializeField] GameObject Water;
    [SerializeField] GameObject Baumstamm;
    [SerializeField] GameObject Baumstamm2;
    [SerializeField] GameObject Baumstamm3;
    [SerializeField] GameObject TransformPos;
    [SerializeField] GameObject TransformPos2;
    [SerializeField] GameObject TransformPos3;
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
            Baumstamm.transform.position = TransformPos.transform.position;
            Baumstamm2.transform.position = TransformPos2.transform.position;
            Baumstamm3.transform.position = TransformPos3.transform.position;
        }
    }
}
