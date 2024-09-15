using System;
using System.Collections;
using System.Collections.Generic;
using SceneStructure;
using Services;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneLoaderService : IService,IMonoHelper
{
    public event Action<float> OnProgressChanged;
    public event Action OnCompleted;
    
    readonly string sceneLoaderName = "SceneLoader";
    private string currentSceneName;
    private string nextSceneName;
    
    public void LoadNewScene(string nextSceneName)
    {
        currentSceneName = SceneManager.GetActiveScene().name;
        this.nextSceneName = nextSceneName;

        StartCoroutine(IE_LoadNewScene());
    }

    IEnumerator IE_LoadNewScene()
    {
        var loadingScene = SceneManager.LoadSceneAsync(sceneLoaderName,LoadSceneMode.Additive);
        while (!loadingScene.isDone)
        {
            yield return null;
        }

        var previousScene = SceneManager.UnloadSceneAsync(currentSceneName);
        while (!previousScene.isDone)
        {
            yield return null;
        }
        
        var nextScene = SceneManager.LoadSceneAsync(nextSceneName,LoadSceneMode.Additive);
        while (!nextScene.isDone)
        {
            yield return null;
        }

        loadingScene = SceneManager.UnloadSceneAsync(sceneLoaderName);
        while (!loadingScene.isDone)
        {
            yield return null;
        }
        OnCompleted?.Invoke();
    }
}
