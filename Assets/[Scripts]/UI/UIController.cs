using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public GameObject miniMap;


    // Start is called before the first frame update
    void Start()
    {
        miniMap = GameObject.Find("Minimap");
    }

    public void OnRestartButtonPressed()
    {
        SceneManager.LoadScene("Main");
    }

    public void OnYButtonClick()
    {
        if (miniMap)
        {
            miniMap.SetActive(!miniMap.activeInHierarchy);
        }
    }

}
