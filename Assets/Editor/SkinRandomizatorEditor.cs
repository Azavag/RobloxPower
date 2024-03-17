using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SkinRandomizator))]
public class SkinRandomizatorEditor : Editor
{

    public override void OnInspectorGUI()
    {
        SkinRandomizator skinRandomizator = (SkinRandomizator)target;
        DrawDefaultInspector();

            
        if (GUILayout.Button("Randomize Hat"))
        {
            skinRandomizator.RandomHat();
        }
        if (GUILayout.Button("Randomize Hair"))
        {
            skinRandomizator.RandomHair();
        }
        if (GUILayout.Button("Randomize Bag"))
        {
            skinRandomizator.RandomBag();
        }
        if (GUILayout.Button("Randomize Attach"))
        {
            skinRandomizator.RandomAttach();
        }
    }

}
