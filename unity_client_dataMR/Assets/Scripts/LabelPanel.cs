using TMPro;
using UnityEngine;

public class LabelPanel : MonoBehaviour
{
    public TextMeshProUGUI label;


    public void UpdateLabel(string label)
    {
        this.label.text = label;
    }
}
