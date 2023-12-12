using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWallMovement : MonoBehaviour
{
    [SerializeField] float walkSpeed = 12.5f;
    public GameObject stopPoint;

    private bool canMove;
    // Start is called before the first frame update
    void Start()
    {
        canMove = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveToLeft();
    }
    void MoveToLeft()
    {
        if (canMove == true)
        {
          transform.position += transform.right * -walkSpeed * Time.deltaTime;
        }
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("StopPoint"))
        {
            canMove = false;  
        }
    }
}
