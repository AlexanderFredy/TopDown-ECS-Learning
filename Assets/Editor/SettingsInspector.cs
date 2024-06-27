using UnityEditor;
using UnityEngine;
using Zenject;

public class SettingsInspector : EditorWindow
{
    //int index = 0;
    
    //[Inject]
    //private GameManager _gameManager;

    //[MenuItem("Game settings/Player settings")]
    //static void ShowWindow()
    //{
    //    GetWindow<SettingsInspector>("Player settings");
    //}

    //void OnGUI()
    //{
    //    var settingsList = AssetDatabase.FindAssets("t:PlayerSettings");
    //    var content = new string[settingsList.Length];
    //    int i = 0;

    //    foreach (var settings in settingsList)
    //    {
    //        var parts = AssetDatabase.GUIDToAssetPath(settings).Split('/');
    //        content[i] = parts[parts.Length-1];         
    //        i++;
    //    }
        
    //    EditorGUILayout.LabelField("Settings found: " + settingsList?.Length.ToString(), EditorStyles.boldLabel);
    //    EditorGUILayout.Space(10f);
    //    index = EditorGUILayout.Popup(index, content);

    //    GUILayout.Space(20f);

    //    if (GUILayout.Button("Apply"))
    //    {
    //        _gameManager.PlayerSettings = AssetDatabase.LoadAssetAtPath<PlayerSettings>(AssetDatabase.GUIDToAssetPath(settingsList[index]));
    //        _gameManager.SetPlayerStats();
    //        Debug.Log(content[index] + " was applied");
    //    }
    //}

}
