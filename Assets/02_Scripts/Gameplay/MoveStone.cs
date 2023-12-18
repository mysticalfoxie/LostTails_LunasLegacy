using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStone : MonoBehaviour
{
    public Rigidbody2D Rigidbody;

    void Start()
    {
        Rigidbody2D Rigidbody = GetComponent<Rigidbody2D>();
        Rigidbody.constraints = RigidbodyConstraints2D.FreezePositionX;
    }
    private void OnCollisionStay2D(Collision2D collision)
    {

        if (collision.gameObject.name == "Player")
        {
            if (InteractInput())
            {
                Rigidbody2D Rigidbody = GetComponent<Rigidbody2D>();
                Rigidbody.constraints = RigidbodyConstraints2D.None;
                Rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
            else
            {
                Rigidbody2D Rigidbody = GetComponent<Rigidbody2D>();
                Rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
            }

        }

    }
    bool InteractInput()
    {
        return Input.GetKey(KeyCode.E);
    }
}
