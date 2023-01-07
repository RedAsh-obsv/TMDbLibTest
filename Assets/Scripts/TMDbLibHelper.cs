using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using TMDbLib.Client;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;

public class TMDbLibHelper : MonoBehaviour
                        //引用自项目 https://github.com/LordMike/TMDbLib
                        //Nuget页面 https://www.nuget.org/packages/TMDbLib
{
    public TMDbLibTestUI TMDbLibTestUI;
    private TMDbClient client;
    private Thread SeartchThread;
    private string SearchMovieName = "";

    void Start()
    {
        client = new TMDbClient(TMDbAPIkey.key);        //在TMDB账户设置中>API>API 密钥中找到
        TMDbLibTestUI.AddLog("TMDbClient initialized.");
    }

    private void GetMovieTitle()
    {
        Movie movie = client.GetMovieAsync(47964, MovieMethods.Credits).Result;

        TMDbLibTestUI.AddLog($"Movie title: {movie.Title}");
        foreach (Cast cast in movie.Credits.Cast)
            TMDbLibTestUI.AddLog($"{cast.Name} - {cast.Character}");
    }
    public void StartSearch(string movieName)
    {
        if (SeartchThread != null)
            SeartchThread.Abort();
        SearchMovieName = movieName;
        SeartchThread = new Thread(SearchMovie);
        SeartchThread.Start();
        TMDbLibTestUI.Load(true);
        TMDbLibTestUI.AddLog("-----------------------------");
        TMDbLibTestUI.AddLog("Search \"" + movieName + "\" thread starting...");
    }
    public void StopSearch()
    {
        if (SeartchThread == null)
        {
            TMDbLibTestUI.AddLog("There is no thread running.");
            return;
        }
        SeartchThread.Abort();
        SeartchThread = null;
        TMDbLibTestUI.Load(false);
        TMDbLibTestUI.AddLog("Search thread aborted.");
    }
    private void SearchMovie()
    {
        try
        {
            SearchContainer<SearchMovie> results = client.SearchMovieAsync(SearchMovieName).Result;
            string logcontent = $"Got {results.Results.Count:N0} of {results.TotalResults:N0} results";
            TMDbLibTestUI.AddLog(logcontent);
            TMDbLibTestUI.Load(false);
            foreach (SearchMovie result in results.Results)
                TMDbLibTestUI.AddLog("《"+result.Title+"》");
            TMDbLibTestUI.AddLog("Search success.");
        }
        catch
        {
            TMDbLibTestUI.AddLog("Search failed. Check network connection.");
            TMDbLibTestUI.Load(false);
        }
    }

    private void OnDisable()
    {
        if (SeartchThread != null)
            SeartchThread.Abort();
        TMDbLibTestUI.AddLog("Search thread aborted.");
    }
}
