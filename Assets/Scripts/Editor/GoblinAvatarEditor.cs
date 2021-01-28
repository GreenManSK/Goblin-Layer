using Controllers.Goblin;
using Services;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GoblinAvatarController))]
public class GoblinAvatarEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var goblinAvatar = (GoblinAvatarController) target;
        if (GUILayout.Button("Generate"))
        {
            goblinAvatar.data = GoblinGenerator.Generate();
        }
        goblinAvatar.UpdateDesign();
    }
}