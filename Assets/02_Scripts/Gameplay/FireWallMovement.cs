using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWallMovement : MonoBehaviour
{
    [SerializeField] float walkSpeed = 12.5f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveToLeft();
    }
    void MoveToLeft()
    {
        transform.position += transform.right * -walkSpeed * Time.deltaTime;
    }
    
}
