using UnityEngine;
using TMPro;

public class DebugFuriousFrogText : MonoBehaviour
{
    TextMeshProUGUI text;

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    void UpdateText()
    {
        if (GameManager.FuriousFrogCount == 0) text.text = "";
        else text.text = $"Furious Frogs: {GameManager.FuriousFrogCount}";
    }

    void OnEnable()
    {
        GameManager.OnFuriousChange += UpdateText;
    }

    void OnDisable()
    {
        GameManager.OnFuriousChange -= UpdateText;
    }
}
