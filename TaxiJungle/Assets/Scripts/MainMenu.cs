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
    float timer;
    bool startTimer;
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

        }
    }
    public void StartGame()
    {

        StartCoroutine(StartLoading());
        
    }
    IEnumerator StartLoading()
    {


        float currentLoaded=0f;
          yield return null;

        _mainMenu.SetActive(false);
        string currentScene = SceneManager.GetActiveScene().name;
        var sceneLoading = SceneManager.LoadSceneAsync("PlayTest", LoadSceneMode.Additive);
        sceneLoading.allowSceneActivation = false;
        while (!sceneLoading.isDone)
        {
            if(currentLoaded<0.7f)
            {
            currentLoaded += Time.deltaTime/30f;
                _imageToFill.fillAmount = currentLoaded;

            }


            if (sceneLoading.progress >= 0.9f)
            {
                startTimer = true;
                _imageToFill.fillAmount = Mathf.Lerp(currentLoaded, 1f, timer/2);
                yield return new WaitForSeconds(2f);
                sceneLoading.allowSceneActivation = true;

            }

            yield return null;
        }
       
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
