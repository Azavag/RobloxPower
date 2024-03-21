
using UnityEngine;
using UnityEngine.UI;

public class ScriptableObjectFill : MonoBehaviour
{
    [SerializeField]
    private SkinScriptableObject[] scriptables;

    [SerializeField]
    private Sprite[] sprites;

    private void OnValidate()
    {
        FillImagesAndNames();
    }

    void FillImagesAndNames()
    {
        for (int i = 0; i < scriptables.Length; i++)
        {
            scriptables[i].sprite = sprites[i];
            scriptables[i].skinName = i+ "_" + sprites[i].name;
            scriptables[i].idNumber = i;

        }
    }
}
