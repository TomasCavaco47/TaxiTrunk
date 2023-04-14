using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Image _imageToFill;
    [SerializeField] GameObject _mainMenu;
    Scene SceneLoading;
   [SerializeField] float timer;
    bool startTimer;
    float currentLoaded = 0f;
    float teste;

    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if(startTimer == true)
        {
         timer += Time.deltaTime;

            teste = Mathf.Lerp(currentLoaded, 1f, timer / 2);
            _imageToFill.fillAmount = teste;
            Debug.Log(teste);
        }
    }
    public void StartGame()
    {

        StartCoroutine(StartLoading());
        
    }
    IEnumerator StartLoading()
    {


      
          yield return null;

        _mainMenu.SetActive(false);
        string currentScene = SceneManager.GetActiveScene().name;
        var sceneLoading = SceneManager.LoadSceneAsync("PlayTest", LoadSceneMode.Additive);
        sceneLoading.allowSceneActivation = false;
        while (!sceneLoading.isDone)
        {
            if(currentLoaded<0.7f&& sceneLoading.progress != 0.9f)
            {
                currentLoaded += Time.deltaTime/20f;
                _imageToFill.fillAmount = currentLoaded;

            }
            if(sceneLoading.progress==0.9f)
            {
                startTimer = true;

            }
            if (teste==1)
            {
                sceneLoading.allowSceneActivation = true;

            }
            yield return null;
        }



        Debug.Log(SceneManager.sceneCount);
        Debug.Log(sceneLoading.isDone);
        SceneManager.UnloadSceneAsync(currentScene);

    }
    public void OpenSettings()
    {
        
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
