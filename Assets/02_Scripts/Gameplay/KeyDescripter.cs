using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDescripter : MonoBehaviour
{
    [SerializeField] GameObject[] interactions;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("InteractionPoint"))
        {
            for (int i = 0; i < interactions.Length; i++)
            {
                if (other.transform.childCount > 0)
                {
                    var child = other.transform.GetChild(0).gameObject;
                    if (child == interactions[i])
                    {
                        child.SetActive(true);
                    }
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("InteractionPoint"))
        {
            for (int i = 0; i < interactions.Length; i++)
            {
                if (other.transform.childCount > 0)
                {
                    var child = other.transform.GetChild(0).gameObject;
                    if (child == interactions[i])
                    {
                        child.SetActive(false);
                    }
                }
            }
        }
    }
}
