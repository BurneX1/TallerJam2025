using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public Animator sceneTransition;
    public static GameManager manager;
    public static string nextScene;

    //public GameData gameData;

    public static event Action StateChanged = delegate { };
    public static GameState gameState;

    public enum GameState
    {
        Gameplay,
        OnMenu

    };
    private void Awake()
    {
        manager = this;
        GameManager.StateChanged += CheckState;
    }

    #region GameState&InputConfigIssues


    public static void SetState(GameState state)
    {
        if (gameState != state)
        {
            gameState = state;
            Debug.Log("GameManager: set state to " + state);
            StateChanged.Invoke();
        }
    }

    public void CheckState()
    {
        Debug.Log("ChangeState gamemanager checker");
        if (gameState != GameState.Gameplay)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    #endregion

    #region SceneManagment

    public void Change(string sceneName)
    {
        StartCoroutine(TransitionLoadScene(sceneName));
    }

    public void Escape()
    {
        StartCoroutine(ExitTransitionLoadScene());
    }
    IEnumerator TransitionLoadScene(string sceneName)
    {
        if (sceneTransition) sceneTransition.SetTrigger("Start");

        yield return new WaitForSecondsRealtime(0);
        Debug.Log(sceneName);
        SceneManager.LoadScene(sceneName);
    }
    IEnumerator ExitTransitionLoadScene()
    {
        if (sceneTransition) sceneTransition.SetTrigger("Start");


        yield return new WaitForSecondsRealtime(0);

        Application.Quit();
    }

    #endregion

    

}
