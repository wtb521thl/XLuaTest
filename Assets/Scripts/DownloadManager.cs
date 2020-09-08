using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class DownloadManager : MonoBehaviour
{
    static DownloadManager instance;
    public static DownloadManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = GameObject.Find("Single_" + typeof(DownloadManager).ToString());
                if (go != null)
                {
                    instance = go.GetComponent<DownloadManager>();

                }
                else
                {
                    go = new GameObject("Single_" + typeof(DownloadManager).ToString());
                    instance = go.AddComponent<DownloadManager>();
                }
            }
            return instance;
        }
    }

    public void DownloadLua(string path, LuaEvnItem luaEvnItem, int funcIndex, Action<LuaEvnItem, int, string> FinishAction)
    {
        if (!string.IsNullOrEmpty(path) && path.ToLower() != "null")
        {
            StartCoroutine(DownloadLuaFunc(path, luaEvnItem, funcIndex, FinishAction));
        }
        else
        {
            if (FinishAction != null)
            {
                FinishAction(luaEvnItem, funcIndex, null);
            }
        }
    }

    IEnumerator DownloadLuaFunc(string path, LuaEvnItem luaEvnItem, int funcIndex, Action<LuaEvnItem, int, string> FinishAction)
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(path);
        yield return webRequest.SendWebRequest();
        if (!string.IsNullOrEmpty(webRequest.error))
        {
            Debug.Log("下载Lua失败==" + webRequest.error);
            if (FinishAction != null)
            {
                FinishAction(luaEvnItem, funcIndex, null);
            }
        }
        else
        {
            if (FinishAction != null)
            {
                FinishAction(luaEvnItem, funcIndex, webRequest.downloadHandler.text);
            }
        }
    }

    public void DownloadText(string path, Action<string> FinishAction)
    {
        if (!string.IsNullOrEmpty(path) && path.ToLower() != "null")
        {
            StartCoroutine(DownloadTextFunc(path, FinishAction));
        }
        else
        {
            if (FinishAction != null)
            {
                FinishAction(null);
            }
        }
    }

    IEnumerator DownloadTextFunc(string path, Action<string> FinishAction)
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(path);
        yield return webRequest.SendWebRequest();
        if (!string.IsNullOrEmpty(webRequest.error))
        {
            Debug.Log("下载Text失败==" + webRequest.error);
            if (FinishAction != null)
            {
                FinishAction(null);
            }
        }
        else
        {
            if (FinishAction != null)
            {
                FinishAction(webRequest.downloadHandler.text);
            }
        }
    }
}

