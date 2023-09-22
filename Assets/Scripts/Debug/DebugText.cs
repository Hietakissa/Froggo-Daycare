using TMPro;
using UnityEngine;

public class DebugText : MonoBehaviour
{
    public static DebugText Instance;

    TextMeshProUGUI text;

    string textString;

    void Awake()
    {
        Instance = this;

        text = GetComponent<TextMeshProUGUI>();
    }

    void LateUpdate()
    {
        text.text = textString;
        textString = string.Empty;
    }

    public void AddText(string newText)
    {
        textString += newText + "\n";
    }
}
