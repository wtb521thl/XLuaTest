using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[XLua.Hotfix]
public class ClickBtn : MonoBehaviour
{
    public GameObject btnObj;

    public GameObject image;

    // Start is called before the first frame update
    void Awake()
    {
        btnObj.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (ClickAction != null)
                ClickAction(new object[] {123 });
            //ClickBtnFunc();
        });
        LuaManager.Instance.ToDoAction += ToDoAction;
        Injection[] injections = new Injection[] {
                new Injection("image",image)
            };
        LuaManager.Instance.WriteByLua(this, "LuaTest", "TestFunction", injections);
        LuaManager.Instance.LuaHotFix(this, "CS.ClickBtn", "ClickBtnFunc");
    }

    public void ClickBtnFunc()
    {
        Debug.Log("NoHotFix");
    }

    Action<object[]> ClickAction;
    private void ToDoAction(string arg1, string arg2, Action<object[]> arg3)
    {
        if (arg1 == "LuaTest" && arg2 == "TestFunction")
        {
            ClickAction = arg3;
        }
    }
}
