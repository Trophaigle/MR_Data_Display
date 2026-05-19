using TMPro;
using UnityEngine;

public class KPIPanel : MonoBehaviour
{
    public TextMeshProUGUI kpiTitle;
    public TextMeshProUGUI kpiValue;
   // public float spawnDistance = 1.5f;

    public void UpdateKPI(string title, string value)
    {
        kpiTitle.text = title;
        kpiValue.text = value;
    }

   /* void Start()
{
    Transform head = Camera.main.transform;

    // position devant la tête
    Vector3 spawnPos =
        head.position +
        head.forward * spawnDistance;

    // option : légèrement plus bas
    spawnPos.y -= 0.2f;

    // appliquer
    transform.position = spawnPos;

    // regarder le joueur
    transform.LookAt(head);

    // IMPORTANT :
    // évite rotation verticale bizarre
    Vector3 rot = transform.eulerAngles;
    rot.x = 0;
    rot.z = 0;
    transform.eulerAngles = rot;
}*/
}
