using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Interactor : MonoBehaviour
{

    [SerializeField] GameObject Dialogue;
    [SerializeField] public GameObject PressE;

    void Update()
    {
        
        
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        
        if (collision.gameObject.name=="Player")
        {
            PressE.SetActive(true);
            if (InteractInput())
            {
                Dialogue.SetActive(true);
                PressE.SetActive(false);
            }
            
        }
        
    }
    
    bool InteractInput()
    {
        return Input.GetKeyDown(KeyCode.E);
    }
   
}
