using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Zenject;

public class OdinSettingsSelector : MonoBehaviour
{
    [Inject]
    private GameManager _gameManager;

    [AssetList]
    [LabelText("Player settings")]
    [InlineEditor(InlineEditorModes.FullEditor)]
    
    public PlayerSettings curSettings;

    [Button(ButtonSizes.Medium),GUIColor(0.6f,1f,0.6f)]
    public void ApplySetting()
    {
        _gameManager.PlayerSettings = curSettings;
        _gameManager.SetPlayerStats();
        Debug.Log(curSettings + " was applied");
    }

}
