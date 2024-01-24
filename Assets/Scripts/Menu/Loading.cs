using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    public GameObject loadingScreen;
    public Image loadingBarFill;
    public GameObject mainMenu;

    public void LoadLevelBtn(int sceneId)
    {
        StartCoroutine(LoadLevelASync(sceneId));
        mainMenu.SetActive(false);
    }

    IEnumerator LoadLevelASync(int sceneId)
    {
        UnityEngine.AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneId);
        loadingScreen.SetActive(true);
        while (!loadOperation.isDone)
        {
            float progressValue = Mathf.Clamp01(loadOperation.progress / 0.999f);
            loadingBarFill.fillAmount = progressValue;
            yield return null;
        }
    }
}
