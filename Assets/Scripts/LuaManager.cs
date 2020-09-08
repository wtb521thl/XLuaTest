using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;


public class LuaManager : MonoBehaviour
{
    static LuaManager instance;

    public static LuaManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = GameObject.Find("Single_"+typeof(LuaManager).ToString());
                if (go != null)
                {
                    instance = go.GetComponent<LuaManager>();

                }
                else
                {
                    go = new GameObject("Single_" + typeof(LuaManager).ToString());
                    instance = go.AddComponent<LuaManager>();
                }
            }
            return instance;
        }
    }

    public static LuaEnv luaEnv = new LuaEnv();

    List<LuaEvnItem> luaEvnItems = new List<LuaEvnItem>();

    public Action<string, string, Action<object[]>> ToDoAction;

    public void WriteByLua<T>(T obj, string fileName, string funcName, Injection[] injections = null)
    {
        Action<object[]> InvokeActtion = null;
        LuaEvnItem luaEvnItem = luaEvnItems.Find((tempItem) => { return tempItem.fileName == fileName; });
        if (luaEvnItem == null)
        {
            LuaTable luaTable = luaEnv.NewTable();
            LuaTable tempLuaTb = luaEnv.NewTable();
            tempLuaTb.Set("__index", luaEnv.Global);
            luaTable.SetMetaTable(tempLuaTb);
            tempLuaTb.Dispose();
            luaTable.Set("self", obj);
            if (injections != null && injections.Length > 0)
            {
                for (int i = 0; i < injections.Length; i++)
                {
                    luaTable.Set(injections[i].key, injections[i].value);
                }
            }
            luaEvnItem = new LuaEvnItem(fileName, new List<string>() { funcName }, "", injections, luaTable, tempLuaTb);
            luaEvnItems.Add(luaEvnItem);
        }
        string path = "";
        if (Application.platform == RuntimePlatform.WebGLPlayer && Application.platform != RuntimePlatform.WindowsEditor)
        {
            path = "./StreamingAssets/LuaScripts/" + fileName + ".lua";
        }
        else
        {
            path = Application.streamingAssetsPath + "/LuaScripts/" + fileName + ".lua";
        }

        string downloadInfo = System.IO.File.ReadAllText(path);

        if (!string.IsNullOrEmpty(downloadInfo))
        {
            luaEnv.DoString(downloadInfo, luaEvnItem.fileName, luaEvnItem.luaTable);
        }
        else
        {
            luaEnv.DoString(Resources.Load<TextAsset>(luaEvnItem.fileName).text, luaEvnItem.fileName, luaEvnItem.luaTable);
        }
        luaEvnItem.luaData = downloadInfo;
        luaEvnItem.luaTable.Get(funcName, out InvokeActtion);
        if (ToDoAction != null)
        {
            ToDoAction(luaEvnItem.fileName, funcName, InvokeActtion);
        }
    }

    public void LuaHotFix<T>(T obj,string assFullName, string funcName)
    {
        string path = "";
        if (Application.platform == RuntimePlatform.WebGLPlayer && Application.platform != RuntimePlatform.WindowsEditor)
        {
            path = "./StreamingAssets/LuaScripts/" + assFullName.Replace('.', '_') + '_' + funcName + ".lua";
        }
        else
        {
            path = Application.streamingAssetsPath + "/LuaScripts/" + assFullName.Replace('.', '_') + '_' + funcName + ".lua";
        }
        Debug.Log(path);
        string downloadInfo = System.IO.File.ReadAllText(path);

        Debug.Log(string.Format("xlua.hotfix({0},'{1}',{2})", assFullName, funcName, downloadInfo));
        luaEnv.DoString(string.Format("xlua.hotfix({0},'{1}',{2})", assFullName, funcName, downloadInfo));
    }
}

[Serializable]
public class Injection
{
    public string key;
    public GameObject value;
    public Injection(string _key, GameObject _value)
    {
        key = _key;
        value = _value;
    }
}

public class LuaEvnItem
{
    public string fileName;
    public List<string> funcNames = new List<string>();
    public string luaData;
    public LuaTable luaTable;
    public LuaTable luaChildTable;
    public Injection[] injections;
    public LuaEvnItem(string _fileName, List<string> _funcNames, string _luaData, Injection[] _injections, LuaTable _luaTable, LuaTable _luaChildTable)
    {
        fileName = _fileName;
        funcNames = _funcNames;
        luaData = _luaData;
        injections = _injections;
        luaTable = _luaTable;
        luaChildTable = _luaChildTable;
    }
}
