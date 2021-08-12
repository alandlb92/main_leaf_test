using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    private LoadingUI _loadingUI;
    private GameObject _mainCanvas;
    
    public void LoadScene(string sceneName)
    {
        StartCoroutine(RunLoadScene(sceneName));
    }

    private void Awake()
    {
        _mainCanvas = transform.parent.gameObject;
        _loadingUI = _mainCanvas.GetComponentInChildren<LoadingUI>();
    }

    private IEnumerator RunLoadScene(string sceneName)
    {
        DontDestroyOnLoad(_mainCanvas);
        _loadingUI.Open();
        yield return SceneManager.LoadSceneAsync(sceneName);
        _loadingUI.Close();
        Destroy(_mainCanvas);
    }
}
