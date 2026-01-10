using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
using System;
using System.IO;

public class BuildScript
{
    public static void PerformBuild()
    {

        string[] scenes = {
            "Assets/Scenes/menu.unity",
            "Assets/Scenes/settings.unity",
            "Assets/Scenes/gameplay.unity",
            "Assets/Scenes/win.unity",
        };

        string aabPath = "SplashCatch.aab";
        string apkPath = "SplashCatch.apk";

        string keystoreBase64 = "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";
        string keystorePass ="splash";
        string keyAlias = "splash";
        string keyPass = "splash";


        string tempKeystorePath = null;

        if (!string.IsNullOrEmpty(keystoreBase64))
        {

            tempKeystorePath = Path.Combine(Path.GetTempPath(), "TempKeystore.jks");
            File.WriteAllBytes(tempKeystorePath, Convert.FromBase64String(keystoreBase64));

            PlayerSettings.Android.useCustomKeystore = true;
            PlayerSettings.Android.keystoreName = tempKeystorePath;
            PlayerSettings.Android.keystorePass = keystorePass;
            PlayerSettings.Android.keyaliasName = keyAlias;
            PlayerSettings.Android.keyaliasPass = keyPass;

            Debug.Log("Android signing configured from Base64 keystore.");
        }
        else
        {
            Debug.LogWarning("Keystore Base64 not set. APK/AAB will be unsigned.");
        }

        BuildPlayerOptions options = new BuildPlayerOptions
        {
            scenes = scenes,
            target = BuildTarget.Android,
            options = BuildOptions.None
        };

        EditorUserBuildSettings.buildAppBundle = true;
        options.locationPathName = aabPath;

        Debug.Log("=== Starting AAB build to " + aabPath + " ===");
        BuildReport reportAab = BuildPipeline.BuildPlayer(options);
        if (reportAab.summary.result == BuildResult.Succeeded)
            Debug.Log("AAB build succeeded! File: " + aabPath);
        else
            Debug.LogError("AAB build failed!");

        EditorUserBuildSettings.buildAppBundle = false;
        options.locationPathName = apkPath;

        Debug.Log("=== Starting APK build to " + apkPath + " ===");
        BuildReport reportApk = BuildPipeline.BuildPlayer(options);
        if (reportApk.summary.result == BuildResult.Succeeded)
            Debug.Log("APK build succeeded! File: " + apkPath);
        else
            Debug.LogError("APK build failed!");

        Debug.Log("=== Build script finished ===");

        if (!string.IsNullOrEmpty(tempKeystorePath) && File.Exists(tempKeystorePath))
        {
            File.Delete(tempKeystorePath);
            Debug.Log("Temporary keystore deleted.");
        }
    }
}