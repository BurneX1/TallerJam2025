using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public KeyCode escapeCode;
    public string[] backCanvHist;
    public string latestCanv;
    public CanvasObj[] canvas;
    public GameObject panelCanv;
    public CanvasObj backDefaultCanv;

    public static CanvasManager instance;


    private void Awake()
    {
        OnUICheck();
        //inptStm.GamePlay.Escape.performed += _ => ReturnCanv();
        //inptStm.GamePlay.Escape.performed += _ => Debug.Log("GamePlay");
        //inptStm.UI.Escape.performed += _ => ReturnCanv();
        //inptStm.UI.Escape.performed += _ => Debug.Log("UI");
    }
    //public PopPanelMethods popAlrt;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        OnUICheck();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(escapeCode))
        {
            ReturnCanv();
            Debug.Log(GameManager.gameState);
        }
    }

    public void ReturnCanv()
    {

        if (backCanvHist.Length <= 0)
        {
            SetActiveCanvas(backDefaultCanv.name, true);
            return;
        }

        if (backCanvHist.Length <= 1)
        {
            SetActiveCanvas(latestCanv, false);
            backCanvHist = backCanvHist.Where((source, index) => index != backCanvHist.Length - 1).ToArray();
        }
        else
        {
            ChangeToCanvas(latestCanv, backCanvHist[backCanvHist.Length - 2]);
            backCanvHist = backCanvHist.Where((source, index) => index != backCanvHist.Length - 1 && index != backCanvHist.Length - 2).ToArray();
        }

    }

    public void ActiveCanv(string canvName)
    {
        SetActiveCanvas(canvName, true);
    }
    public void DesactiveCanv(string canvName)
    {
        SetActiveCanvas(canvName, false);
    }
    public void ChangeToCanvas(string previuousCanv, string beforeCanv = (null))
    {
        if (previuousCanv != null)
        {
            SetActiveCanvas(previuousCanv, false);
        }

        if (beforeCanv != null)
        {
            SetActiveCanvas(beforeCanv, true);
        }

    }

    private void SetActiveCanvas(string canvasName, bool active)
    {

        CanvasObj s = Array.Find(canvas, canvasobj => canvasobj.name == canvasName);
        if (s == null)
        {
            Debug.Log("El canvas " + canvasName + " no se ha encontrado");
            return;
        }

        Debug.Log(canvasName);
        if (panelCanv)
        {
            panelCanv.GetComponent<Canvas>().sortingOrder = s.canvObj.GetComponent<Canvas>().sortingOrder - 1;
        }
        if (s.canvObj.activeSelf == active && latestCanv == canvasName)
        {
            latestCanv = canvasName;
            Debug.Log(canvasName);
            return;
        }
        s.canvObj.SetActive(active);
        if (active == true)
        {

            latestCanv = canvasName;
            string[] bootsAdd = { canvasName };
            if (backCanvHist.Length == 0)
            {
                backCanvHist = bootsAdd;
            }
            else
            {
                backCanvHist = backCanvHist.Concat(bootsAdd).ToArray();
            }


        }
        OnUICheck();
    }

    public void OnUICheck()
    {

        int actCanv = 0;
        for (int i = 0; i < canvas.Length; i++)
        {
            if (canvas[i].canvObj.activeSelf == true)
            {
                actCanv++;
                GameManager.SetState(GameManager.GameState.OnMenu);
                if (panelCanv)
                {
                    panelCanv.SetActive(true);
                }

                
                return;
            }
        }

        Debug.Log(actCanv);

        if (actCanv <= 0)
        {

            GameManager.SetState(GameManager.GameState.Gameplay);
            if (panelCanv)
            {
                panelCanv.GetComponent<Canvas>().sortingOrder = 1;
                panelCanv.SetActive(false);
            }
            
        }


    }
    public void ClearHist()
    {
        string[] refresh = { };
        backCanvHist = refresh;
    }
}

[System.Serializable]
public class CanvasObj
{
    public string name;
    public GameObject canvObj;
}

