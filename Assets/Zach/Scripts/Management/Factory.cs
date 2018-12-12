using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("Dependencies")]
    ThirdPersonController controller;
    ThirdPersonCamera cam;
    public GameObject factoryPanel;
    [HideInInspector] public bool inspectingFactory;

    Color yellow = Color.yellow;

    void Start()
    {
        FindObjects();
    }

    public void OpenFactoryUI()
    {
        factoryPanel.SetActive(true);
        inspectingFactory = true;

        DisableControlls();
    }

    public void CloseFactoryUI()
    {
        factoryPanel.SetActive(false);
        inspectingFactory = false;

        ResetControlls();
        FactoryUI.instance.ResetUI();
    }

    void DisableControlls()
    {
        controller.enabled = false;
        cam.UnlockCursor();
        cam.camData.mouseSensitivity = 0f;
    }

    void ResetControlls()
    {
        controller.enabled = true;
        cam.LockCursor();
        cam.camData.mouseSensitivity = 10f;
    }

    void FindObjects()
    {
        controller = FindObjectOfType<ThirdPersonController>();
        cam = FindObjectOfType<ThirdPersonCamera>();
        factoryPanel = GameObject.FindGameObjectWithTag("Factory Panel");
    }
}
