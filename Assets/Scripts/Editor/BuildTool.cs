using System.Collections;
using System.Collections.Generic;
using System.IO;
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

        string[] files = Directory.GetFiles(PathUtil.BuildResourcesPath, "*", SearchOption.AllDirectories);

        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].EndsWith(".meta"))
            {
                continue;
            }
            
            AssetBundleBuild assetBundle = new AssetBundleBuild();

            string fileName = PathUtil.GetStanderPath(files[i]);
            Debug.Log("file:"+fileName);
            
            string assetName = PathUtil.GetUnityPath(fileName);
            assetBundle.assetNames = new string[] { assetName };

            string bundleName = files[i].Replace(PathUtil.BuildResourcesPath, "").ToLower();
            assetBundle.assetBundleName = bundleName + ".ab";
            assetBundleBuilds.Add(assetBundle);
        }

        if (Directory.Exists(PathUtil.BundleOutpath))
        {
            Directory.Delete(PathUtil.BundleOutpath, true);
        }

        Directory.CreateDirectory(PathUtil.BundleOutpath);
        
        BuildPipeline.BuildAssetBundles(PathUtil.BundleOutpath, assetBundleBuilds.ToArray(),
            BuildAssetBundleOptions.None, target);
    }
}
