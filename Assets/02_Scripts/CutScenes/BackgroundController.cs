using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundController : MonoBehaviour
{
    public Image background1;
   

    public void SetImage(Sprite sprite)
    {
            background1.sprite = sprite;
    }
 

}

