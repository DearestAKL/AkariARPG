using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public class GenerateUIScriptEditor
{
    /// <summary>
    /// 从UIView自动生成 HotFix项目组件脚本
    /// </summary>
    [MenuItem("Assets/自动生成UI脚本", false, 1050)]
    static void GenerateUIScript()
    {
        //var templateUIBaseFile = Application.dataPath + "/../ScriptMould/UIBaseTemplate.cs";
        //var templateUIBaseCellFile = Application.dataPath + "/../ScriptMould/UIBaseCellTemplate.cs";

        var templateUIBaseFile = "Assets/GameMain/Configs/UIBaseTemplate.txt";
        var templateUIBaseCellFile = "Assets/GameMain/Configs/UIBaseTemplate.txt";

        var outputPath = Application.dataPath + "/GameMain/Scripts/UI/_GenerateScript/";

        var selectObj = Selection.activeGameObject;
        if (!selectObj)
        {
            Debug.Log("请选择要生成的UI资源！");
            return;
        }
        var uiCtrl = selectObj.GetComponent<ReferenceCollector>();
        if (!uiCtrl)
        {
            Debug.Log($"name = {selectObj.name} 没有添加脚本 ReferenceCollector");
            return;
        }

        //输出VS项目引用配置
        //var csprojStr = "";
        //if (File.Exists(outputProCSProj))
        //{
        //    csprojStr = File.ReadAllText(outputProCSProj);
        //}
        //else
        //{
        //    Debug.LogError("csproj文件未找到，生成引用失败！");
        //}

        #region 读取导出代码模板

        var classStrPre = "";
        var tarTemplateFilePath = "";

        tarTemplateFilePath = templateUIBaseCellFile;

        if (File.Exists(tarTemplateFilePath))
        {
            classStrPre = File.ReadAllText(tarTemplateFilePath);
        }
        else
        {
            Debug.LogError("生成失败：模板文件不存在！");
            return;
        }

        #endregion

        #region 生成代码

        var classStr = classStrPre;

        //构造成员变量 和 初始化代码
        var uiDataList = uiCtrl.data;

        var className = selectObj.name + "Sign";//类名
        var memberStr = "";//成员变量构造
        var initStr = "";//初始化变量

        uiDataList.ForEach(t =>
        {
            var memberName = t.key;
            var typeStr = t.gameObject.GetType();

            memberStr += string.Format("protected {0} {1} = null;\r\n\t\t", typeStr, memberName);
            initStr += string.Format("{0} = rc.Get<{1}>(\"{2}\");\r\n\t\t\t", memberName, typeStr, memberName);
        });
        classStr = classStr.Replace("__CREATE_TIME__", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
        classStr = classStr.Replace("__NAME_SPACE__", "Akari");
        classStr = classStr.Replace("__CLASS_NAME__", className);
        classStr = classStr.Replace("__MEMBER_VARIABLES__", memberStr);
        classStr = classStr.Replace("__INIT__", initStr);

        //写入.cs文件
        var outputFileFullPath = string.Format(outputPath + "{0}.cs", className);
        FileStream stream = new FileStream(outputFileFullPath, FileMode.Create, FileAccess.Write);

        StreamWriter fileW = new StreamWriter(stream, System.Text.Encoding.UTF8);
        fileW.Write(classStr);
        fileW.Flush();
        fileW.Close();
        stream.Close();
        Debug.Log("创建脚本 " + outputFileFullPath + "成功！"); 

        #endregion

        //生成csproj配置文件引用
        //if (!string.IsNullOrEmpty(csprojStr))
        //{
        //    if (!Regex.IsMatch(csprojStr, className + ".cs"))
        //    {
        //        var compileStr = string.Format("<!--UIView-->\r\n\t<Compile Include=\"Game\\UI\\_GenerateUIScript\\{0}\" />", className + ".cs");
        //        stream = new FileStream(outputProCSProj, FileMode.Open, FileAccess.Write);
        //        csprojStr = csprojStr.Replace("<!--UIView-->", compileStr);

        //        fileW = new StreamWriter(stream);
        //        fileW.Write(csprojStr);
        //        fileW.Flush();
        //        fileW.Close();
        //        stream.Close();
        //        Debug.Log("生成csproj配置文件引用成功！");
        //    }
        //}
    }

}
