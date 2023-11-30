using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InteractSystem : MonoBehaviour
{    
    [SerializeField] GameObject CutScene1;
    void Update()
    {
     HandleInteraction();
    }
    void HandleInteraction()
    {
      CutScene1.SetActive(true);
    }
    
}
