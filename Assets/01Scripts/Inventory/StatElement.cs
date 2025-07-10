using TMPro;
using UnityEngine;

public class StatElement : MonoBehaviour
{
    public TextMeshProUGUI statNameText;
    public TextMeshProUGUI statValueText;

    public void Set(string name, string value)
    {
        if (statNameText == null || statValueText == null)
        {
            Debug.LogError("StatElement: Text components are not assigned!");
            return;
        }

        statNameText.text = name;
        statValueText.text = value;
    }
}