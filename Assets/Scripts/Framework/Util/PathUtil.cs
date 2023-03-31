using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathUtil
{
    /// <summary>
    /// 根目录
    /// </summary>
    public static readonly string AssetsPath = Application.dataPath;
    
    /// <summary>
    /// 需要打bundle的目录
    /// </summary>
    public static readonly string BuildResourcesPath = AssetsPath + "/BuildResources/";
    
    /// <summary>
    /// bundle 输出目录
    /// </summary>
    public static readonly string BundleOutpath = Application.streamingAssetsPath;

    /// <summary>
    /// 获取Unity相对路径
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string GetUnityPath(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            return String.Empty;
        }
        return path.Substring(path.IndexOf("Assets"));
    }

    /// <summary>
    /// 获取标准路径
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string GetStanderPath(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            return string.Empty;
        }

        return path.Trim().Replace("\\", "/");
    }
}