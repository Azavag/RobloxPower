using UnityEngine;

public class Loader : MonoBehaviour
{
    [SerializeField] GameObject canvases, sdk, controllers, player;
    bool isObjectsLoaded;

    private void Awake()
    {
        sdk.gameObject.SetActive(false);
        player.gameObject.SetActive(false);
        canvases.SetActive(false);
        controllers.SetActive(false);
    }
    private void Start()
    {
        Application.targetFrameRate = -1;
    }

    private void Update()
    {
        if(YandexSDK.dataIsLoaded)
        {
            Loading();          
        }
    }
    public void Loading()
    {
        Debug.Log("LoadObjects");
        sdk.gameObject.SetActive(true);
        controllers.SetActive(true);
        player.gameObject.SetActive(true);
        canvases.SetActive(true);
        gameObject.GetComponent<Loader>().enabled = false;
    }

}
