using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class ProgramEntryPoint
{

    public static void SetSpiderSceneUp()
    {
        List<UnityEditor.EditorBuildSettingsScene> currentSettings = new List<UnityEditor.EditorBuildSettingsScene>(UnityEditor.EditorBuildSettings.scenes);

        currentSettings.Insert(0, new UnityEditor.EditorBuildSettingsScene("Assets/UISpider/UITest.unity", true));
        
        UnityEditor.EditorBuildSettings.scenes = currentSettings.ToArray();
    }
}
