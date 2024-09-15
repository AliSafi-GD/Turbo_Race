using System;
using System.Collections;
using System.Collections.Generic;
using Services;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<T>();
                    singletonObject.name = typeof(T).ToString() + " (Singleton)";
                    DontDestroyOnLoad(singletonObject);
                }
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }
}



public class SceneLoaderService : MonoBehaviour, IService
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
