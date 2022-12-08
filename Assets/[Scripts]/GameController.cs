using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject onScreenControls;
    public GameObject miniMap;
    // Start is called before the first frame update
    void Start()
    {
        onScreenControls = GameObject.Find("OnScreenControls");
        Debug.Log(Application.platform != RuntimePlatform.WindowsEditor);
        onScreenControls.SetActive(Application.platform != RuntimePlatform.WindowsEditor);

        miniMap = GameObject.Find("Minimap");


        FindObjectOfType<SoundManager>().PlayMusic();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            miniMap.SetActive(!miniMap.activeInHierarchy);
        }
    }

}
