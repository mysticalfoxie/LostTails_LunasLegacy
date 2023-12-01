using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Interactor : MonoBehaviour
{
    public Transform detectionPoint;
    private const float _detectionRadius = 0.5f;
    public LayerMask detectionLayer;

    [SerializeField] GameObject Dialogue;
    [SerializeField] public GameObject PressE;

    void Update()
    {
        HandleInteraction();
    }
    void HandleInteraction()
    {
        if (DetectObject())
        {
            PressE.SetActive(true);
            if (InteractInput())
            {
                PlayerMovement moveScript = GetComponent<PlayerMovement>();
                Dialogue.SetActive(true);
            }
        }
        if (!DetectObject())
        {
            PressE.SetActive(false);
            if (InteractInput())
            {
                PlayerMovement moveScript = GetComponent<PlayerMovement>();
                Dialogue.SetActive(false);
            }
        }
    }
    bool InteractInput()
    {
        return Input.GetKeyDown(KeyCode.E);
        
    }
    bool DetectObject()
    {
        return Physics2D.OverlapCircle(detectionPoint.position, _detectionRadius, detectionLayer);
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(detectionPoint.position, _detectionRadius);
    }
}
