using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void CheckSdkReady();
  
    bool isSdkReady;
    [SerializeField]
    private GameObject gameLoader;

    private void Start()
    {

    }
    private void Update()
    {
#if !UNITY_EDITOR
if (!isSdkReady) 
        CheckSdkReady();
#else
isSdkReady = true;
YandexSDK.dataIsLoaded = true;
#endif
        if (isSdkReady)
        {
            gameLoader.SetActive(true);
        }
        if(isSdkReady && YandexSDK.dataIsLoaded)
        {
            SceneManager.LoadScene(1);
        }
    }
    //из jslib
    public void ToggleSdkReady()
    {
        Debug.Log("ToggleSdkReady");
        isSdkReady = true;
    }
    
}


