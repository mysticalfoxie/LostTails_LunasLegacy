using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialInteractions : MonoBehaviour
{
    public Transform detectionPoint;
    private const float detectionRadius = 1.0f;
    public LayerMask detectionLayer;
    public GameObject TutorialText;

    void Update()
    {
      if(DetectObject())
        {
            TutorialText.SetActive(true);
        }
        else
        {
            TutorialText.SetActive(false);
        }
    }
    bool DetectObject()
    {
        return Physics2D.OverlapCircle(detectionPoint.position,detectionRadius,detectionLayer);
    }

}
