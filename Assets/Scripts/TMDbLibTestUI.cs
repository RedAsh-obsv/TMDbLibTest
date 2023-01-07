using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TMDbLibTestUI : MonoBehaviour
{
    [SerializeField]
    private InputField MovieNameField;
    [SerializeField]
    private Text DebugLogText;
    [SerializeField]
    private Animator LoadingAnim;
    [SerializeField]
    private TMDbLibHelper TMDbLibHelper;
    private string logContent = "";
    private int logIndex = 0;
    private bool isLoading = false;
    private void FixedUpdate()
    {
        UpdateState();
    }
    private void UpdateState()
    {
        DebugLogText.text = logContent;
        if (isLoading)
            StartLoadingAnim();
        else
            StopLoadingAnim();
    }
    public void Load(bool isloading)
    {
        this.isLoading = isloading;
    }
    public void AddLog(string log)
    {
        string newlog = $"[Log{logIndex}] " + log;
        logIndex++;
        logContent = newlog + "\n" + logContent;
        Debug.Log(newlog);
    }

    public void Send()
    {
        if (MovieNameField.text == "")
            AddLog("Must enter movie name.");
        else
            TMDbLibHelper.StartSearch(MovieNameField.text);
    }

    public void SendWithName(string name)
    {
        TMDbLibHelper.StartSearch(name);
    }
    public void Stop()
    {
        TMDbLibHelper.StopSearch();
    }
    public void Quit()
    {
        Application.Quit();
    }
    private void StartLoadingAnim()
    {
        LoadingAnim.speed = 1;
    }
    private void StopLoadingAnim()
    {
        LoadingAnim.Play("Loading", 0, 0);
        LoadingAnim.speed = 0;
    }
}
