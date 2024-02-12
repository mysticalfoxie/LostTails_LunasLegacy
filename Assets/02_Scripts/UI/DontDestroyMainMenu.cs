using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyMainMenu : MonoBehaviour
{
    private static DontDestroyMainMenu singleton;

    void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
        } else if(singleton != null)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

}
