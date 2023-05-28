using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public OVROverlay overlay_Background;
    public OVROverlay overlay_LoadingText;

    public static SceneLoader instance;

    private void Awake()
    {
        if(instance !=null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(ShowOverlayAndLoad(sceneName));
    }    

    IEnumerator ShowOverlayAndLoad(string sceneName)
    {

        //Enables the Loading text and background
        overlay_Background.enabled = true;
        overlay_LoadingText.enabled = true;

        //Puts loading text overlay 3 meters forward
        GameObject centerEyeAnchor = GameObject.Find("CenterEyeAnchor");
        overlay_LoadingText.gameObject.transform.position = centerEyeAnchor.transform.position + new Vector3(0,0,3f);

        //Waiting a few seconds to prevent popping into the new scene
        yield return new WaitForSeconds(3f);

        //Loads Scene and Waits Until Complete
        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }


        //Disables the loading overlays once again, once completed
        overlay_Background.enabled = false;
        overlay_LoadingText.enabled = false;

        yield return null;
    }
}
