using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class BuildAssetBundles : MonoBehaviour
{
    // Create an AssetBundle for Windows.
    [MenuItem("Assets/Build Asset Bundles")]
    static void BuildABs()
    {
        string outputPath = "build/ABs/" + EditorUserBuildSettings.activeBuildTarget;
        if (!Directory.Exists(outputPath))
        {
            Directory.CreateDirectory(outputPath);
        }
        Debug.LogFormat("Build ABs output path: {0}", outputPath);
        // Put the bundles in a folder called "ABs" within the Assets folder.
        var menifest = BuildPipeline.BuildAssetBundles(outputPath, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
        Debug.Log(menifest);
    }
}
