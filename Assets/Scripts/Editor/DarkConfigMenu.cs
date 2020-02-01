using DarkConfig;
using UnityEngine;
using UnityEditor;

public class DarkConfigMenu {
    public static string GetRelPath() {
        return "/Resources/Configs";
    }

    public static string GetFilePath() {
        return Application.dataPath + GetRelPath();
    }

    [MenuItem ("Assets/DarkConfig/Autogenerate Index")]
    static void MenuGenerateIndex() {
        EditorUtils.GenerateIndex(GetRelPath());
        AssetDatabase.Refresh();
    }
}
