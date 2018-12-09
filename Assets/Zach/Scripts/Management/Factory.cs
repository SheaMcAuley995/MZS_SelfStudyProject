using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour
{
    #region singleton
    public static Factory instance;

    private void Awake()
    {

        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }
    #endregion

    ThirdPersonController controller;
    ThirdPersonCamera cam;
    [HideInInspector] public bool inspectingFactory;

    public void OpenFactoryUI()
    {

    }

    public void CloseFactoryUI()
    {

    }
}
