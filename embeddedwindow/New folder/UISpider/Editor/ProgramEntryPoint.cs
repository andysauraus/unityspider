using UnityEngine;
using System.Collections;

public static class ProgramEntryPoint
{

    public static void SetSpiderSceneUp()
    {
        UnityEditor.EditorBuildSettingsScene[] currentSettings = UnityEditor.EditorBuildSettings.scenes;
        UnityEditor.EditorBuildSettingsScene[] mySettings = new UnityEditor.EditorBuildSettingsScene[currentSettings.Length + 1];

        mySettings[0].path = "..\\UISpider\\UITest.Unity";
        for(int i = 0; i < currentSettings.Length; i++)
        {
            mySettings[i + 1] = currentSettings[i];
        }
        UnityEditor.EditorBuildSettings.scenes = mySettings;
    }

}
