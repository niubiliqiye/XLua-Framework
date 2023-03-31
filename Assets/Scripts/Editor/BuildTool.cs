using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class BuildTool : Editor
{
    [MenuItem("Tools/Build Windows Bundle")]
    static void BundleWindowsBuild()
    {
        Build(BuildTarget.StandaloneWindows);
    }
    
    [MenuItem("Tools/Build Android Bundle")]
    static void BundleAndroidBuild()
    {
        Build(BuildTarget.Android);
    }
    
    [MenuItem("Tools/Build Iphone Bundle")]
    static void BundleIphoneBuild()
    {
        Build(BuildTarget.iOS);
    }
    
    static void Build(BuildTarget target)
    {
        List<AssetBundleBuild> assetBundleBuilds = new List<AssetBundleBuild>();

        //文件信息列表
        List<string> bundleInfos = new List<string>();

        string[] files = Directory.GetFiles(PathUtil.BuildResourcesPath, "*", SearchOption.AllDirectories);

        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].EndsWith(".meta"))
            {
                continue;
            }
            
            AssetBundleBuild assetBundle = new AssetBundleBuild();

            string fileName = PathUtil.GetStanderPath(files[i]);
            Debug.Log("file:" + fileName);
            
            string assetName = PathUtil.GetUnityPath(fileName);
            assetBundle.assetNames = new string[] { assetName };

            string bundleName = files[i].Replace(PathUtil.BuildResourcesPath, "").ToLower();
            assetBundle.assetBundleName = bundleName + ".ab";
            assetBundleBuilds.Add(assetBundle);

            //添加文件和依赖信息
            List<string> dependenceInfo = GetDependence(assetName);
            string bundleInfo = assetName + "|" + bundleName;

            if (dependenceInfo.Count>0)
            {
                bundleInfo = bundleInfo + "|" + string.Join("|", dependenceInfo);
            }
            
            bundleInfos.Add(bundleInfo);
        }
        
        if (Directory.Exists(PathUtil.BundleOutpath))
        {
            Directory.Delete(PathUtil.BundleOutpath, true);
        }

        Directory.CreateDirectory(PathUtil.BundleOutpath);
        
        BuildPipeline.BuildAssetBundles(PathUtil.BundleOutpath, assetBundleBuilds.ToArray(),
            BuildAssetBundleOptions.None, target);

        File.WriteAllLines(PathUtil.BundleOutpath + "/" + AppConst.FileListName, bundleInfos);
        
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 获取依赖文件列表
    /// </summary>
    /// <param name="curFile"></param>
    /// <returns></returns>
    static List<string> GetDependence(string curFile)
    {
        List<string> dependence = new List<string>();
        string[] files = AssetDatabase.GetDependencies(curFile);
        dependence = files.Where(file => !file.EndsWith(".cs") && !file.Equals(curFile)).ToList();
        return dependence;
    }
}
