using TMPro;
using UnityEngine;

public class StatElement : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statNameText;
    [SerializeField] private TextMeshProUGUI statValueText;

    public void Set(string name, string value)
    {
        if (statNameText == null || statValueText == null)
        {
            Debug.LogError("StatElement null");
            return;
        }

        statNameText.text = name;
        statValueText.text = value;
    }
}