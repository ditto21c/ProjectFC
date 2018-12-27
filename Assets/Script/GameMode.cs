using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMode : MonoBehaviour {

	// Use this for initialization
	void Start () {
        TextObj = GameObject.FindGameObjectWithTag("Text");
        text = TextObj.GetComponent<Text>();
        CircleObj = GameObject.FindGameObjectWithTag("Circle");

        BeginStep();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (GameStep == EGameStep.Begin)
        {
            if (Input.GetMouseButtonDown(0))
            {
                StartCount();
            }
        }
        else if(GameStep == EGameStep.CalcPoint)
        {
            if (Input.GetMouseButtonDown(0))
            {
                BeginStep();
            }
        }
	}

    public void ClickedCircle()
    {
        ++ClickedCircleCount;
        float PlayTime = Time.fixedTime - PlayStartTime;
        AllPlayTime += PlayTime;

        if(ClickedCircleCount == 5)
        {
            CalcPointStep();
        }
        else
        {
            StartCount();
        }
    }

    void StartCount()
    {
        GameStep = EGameStep.Count;
        Invoke("PlayCount", 1.0f);
        Count = 3;
    }
    void PlayCount()
    {
        TextObj.SetActive(true);
        text.text = Count.ToString();
        --Count;

        if(-1 == Count)
        {
            StartCheckTime();
        }
        else
        {
            Invoke("PlayCount", 1.0f);
        }
    }
    void StartCheckTime()
    {
        PlayStep();
        PlayStartTime = Time.fixedTime;

    }
    void BeginStep()
    {
        TextObj.SetActive(true);
        text.text = "Press Enter to Start";
        CircleObj.SetActive(false);
        GameStep = EGameStep.Begin;
        ClickedCircleCount = 0;
    }
    void PlayStep()
    {
        TextObj.SetActive(false);
        CircleObj.SetActive(true);
        Circle circle = CircleObj.GetComponent<Circle>();
        circle.ShowCircle(true);

        GameStep = EGameStep.Play;
    }
    void CalcPointStep()
    {
        TextObj.SetActive(true);
        CircleObj.SetActive(false);
        GameStep = EGameStep.CalcPoint;

        float PlayTimeAverage = AllPlayTime / 5.0f;
        float PreAllTime = AllTimeAverage * AllPlayCount;

        AllPlayCount += 5;
        PreAllTime += AllPlayTime;

        AllTimeAverage = PreAllTime / AllPlayCount;

        string str = "PlayCount : {0}\nTime Average : {1}sec\n\nAllPlayCount : {2}\nAll Time Average : {3}sec";

        object[] args = { 5, PlayTimeAverage, AllPlayCount, AllTimeAverage };
        str = string.Format(str, args);
        text.text = str;
    }

    int ClickedCircleCount = 0;
    float AllPlayTime = 0.0f;
    float PlayStartTime = 0.0f;

    public enum EGameStep
    {
        Begin = 0,
        Count,
        Play,
        CalcPoint,
    }
    EGameStep GameStep = EGameStep.Begin;
    int Count;

    GameObject TextObj;
    Text text;
    GameObject CircleObj;

    int AllPlayCount = 0;
    float AllTimeAverage = 0.0f;
}
