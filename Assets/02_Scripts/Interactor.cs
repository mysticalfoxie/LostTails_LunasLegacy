using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interactor : MonoBehaviour
{
    public Transform detectionPoint;
    private const float _detectionRadius = 0.5f;
    public LayerMask detectionLayer;

    [SerializeField] GameObject Dialogue;

    void Update()
    {
      if(DetectObject())
      {
        if (InteractInput())
        {
        PlayerMovement moveScript= GetComponent<PlayerMovement>();
          Dialogue.SetActive(true);
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
