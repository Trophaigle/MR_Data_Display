using Oculus.Interaction;
using UnityEngine;

public class TemperatureGraph : MonoBehaviour
{
     public GameObject cubePrefab;
    public LabelPanel legendLabelPrefab;
    public LabelPanel valueLabelPrefab;
    public Transform origin;
    public GameObject buttonArrowRight;
    public GameObject buttonArrowLeft;

    public int windowSize = 4;
   // public float spawnDistance = 1.5f;
    private int windowNumber = 0;

    private GameObject[] cubes;
    private LabelPanel[] legendLabels;
    private LabelPanel[] valueLabels;
    private float[] currentValues;
    private string[] currentDays;
    private string[] currentEffects;

    private float[] currentHeights;
    private float[] targetHeights;

    private bool isAnimating = false;
    private float stableTime = 0f;

    [SerializeField] private float stopThreshold = 0.3f; // 0.2 - 0.5s

    public void BuildGraph(float[] values, string[] days, string[] effects)
    {
        int count = Mathf.Min(windowSize, values.Length);

        // reset ancien graph
        if (cubes != null)
        {
            foreach (var c in cubes)
                Destroy(c);

            foreach (var l in legendLabels)
                Destroy(l.gameObject);

            foreach (var l in valueLabels)
                Destroy(l.gameObject);
        }

        cubes = new GameObject[count];
        legendLabels = new LabelPanel[count];
        valueLabels = new LabelPanel[count];
        currentHeights = new float[windowSize];
        targetHeights = new float[windowSize];
        currentEffects = new string[values.Length];

        float min = Mathf.Min(values);
        float max = Mathf.Max(values);

        currentDays = days;
        currentValues = values;
        currentEffects = effects;

        float range = max - min;
        if (range == 0) range = 1f;

        for (int i = 0; i < count; i++)
        {
            Vector3 pos = origin.position + new Vector3(i * 1.5f, 0, 0);

            GameObject bar = Instantiate(cubePrefab, transform);

            // position du pivot (BarPivot = base du graphe)
            bar.transform.position = pos;

            // récupère le Cube enfant (IMPORTANT)
            Transform cube = bar.transform.Find("Bar");

            float normalized = (values[i] - min) / range;
            float height = Mathf.Lerp(0.5f, 5f, normalized);
            currentHeights[i] = height;
            targetHeights[i] = height;

            // scale UNIQUEMENT le cube
            cube.localScale = new Vector3(1, height, 1);

            // IMPORTANT : ancrer la base du cube sur le pivot
            cube.localPosition = new Vector3(0, height / 2f, 0);

            Renderer renderer = cube.GetComponent<Renderer>();
            Color cold = new Color(0.2f, 0.4f, 1f);
            Color hot = new Color(1f, 0.3f, 0.2f);
            renderer.material.color = Color.Lerp(cold, hot, normalized);

            cubes[i] = bar;

            // 📅 LABEL JOUR
            LabelPanel label = Instantiate(legendLabelPrefab, transform);
            label.transform.position = pos + new Vector3(0, -0.5f, -0.6f);
            label.transform.localScale *= 8;
            label.UpdateLabel(
                System.DateTime.Parse(days[i]).ToString("MMM d")
            );
            legendLabels[i] = label;

            // 🌡 LABEL VALEUR
            LabelPanel valueLabel = Instantiate(valueLabelPrefab, transform);
            valueLabel.transform.position =
                pos + new Vector3(0, (height) + 0.2f, -0.6f);
            valueLabel.transform.localScale *= 8;
            valueLabel.UpdateLabel(values[i].ToString("F1") + "°C");

            valueLabels[i] = valueLabel;

            WeatherBar weatherBar = cube.GetComponent<WeatherBar>();
            weatherBar.SetEffect(ParseWeather(currentEffects[i]));
        }
        SpawnButtons();
        transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
    }

    private void SpawnButtons()
    {
        GameObject rightButton = Instantiate(buttonArrowRight, transform);
        rightButton.transform.position = origin.position + new Vector3(windowSize * 1.5f, -0.2f, -0.5f);
        rightButton.transform.localScale *= 6f;
        // Wrapper du poke
        InteractableUnityEventWrapper rightWrapper =
            rightButton.transform.GetChild(0).GetComponent<InteractableUnityEventWrapper>();
            // Appelle une fonction DE CE SCRIPT
        rightWrapper.WhenSelect.AddListener(this.NextWindow);

        GameObject leftButton = Instantiate(buttonArrowLeft, transform);
        leftButton.transform.position = origin.position + new Vector3(-1.5f, -0.2f, -0.5f);
        leftButton.transform.localScale *= 6f;
        // Wrapper du poke
        InteractableUnityEventWrapper leftWrapper =
            leftButton.transform.GetChild(0).GetComponent<InteractableUnityEventWrapper>();
            // Appelle une fonction DE CE SCRIPT
        leftWrapper.WhenSelect.AddListener(this.PreviousWindow);
    }

    public void UpdateGraphData(float[] values, string[] days, string[] effects)
    {
        currentValues = values;
        currentDays = days;
        currentEffects = effects;
    }

 public void UpdateGraph()
{
    int count = Mathf.Min(windowSize, currentValues.Length);

    float min = Mathf.Min(currentValues);
    float max = Mathf.Max(currentValues);

    float range = max - min;
    if (range == 0) range = 1f;

    for (int i = 0; i < count; i++)
    {
        int index = i + windowNumber * windowSize;

        float normalized = (currentValues[index] - min) / range;
        float targetHeight = Mathf.Lerp(0.5f, 5f, normalized);

        // 🎯 ON STOCKE JUSTE LA CIBLE
        targetHeights[i] = targetHeight;
        isAnimating = true;

        // 📅 LABEL JOUR (instant car pas critique)
        legendLabels[i].UpdateLabel(
            System.DateTime.Parse(currentDays[index]).ToString("MMM d")
        );

        // 🌡 LABEL VALEUR (texte instant)
        valueLabels[i].UpdateLabel(
            currentValues[index].ToString("F1") + "°C"
        );
        Transform bar = cubes[i].transform;
        Transform cube = bar.Find("Bar");
        WeatherBar weatherBar = cube.GetComponent<WeatherBar>();
        weatherBar.SetEffect(ParseWeather(currentEffects[index]));
    }

}

    public void NextWindow()
    {
        Debug.Log("Next window");
        windowNumber++;
        UpdateGraph();
    }

    public void PreviousWindow()
    {
        Debug.Log("Previous window");
        if (windowNumber > 0){
            windowNumber--;
            UpdateGraph();
        }
    }

    private void AnimateGraph()
{
    float smooth = 10f;
    float t = 1f - Mathf.Exp(-smooth * Time.deltaTime);

    bool hasMovement = false;

    for (int i = 0; i < windowSize; i++)
    {
        float prev= currentHeights[i];

        // 🎯 interpolation hauteur
        currentHeights[i] = Mathf.Lerp(
            currentHeights[i],
            targetHeights[i],
            t
        );

        float diff = Mathf.Abs(currentHeights[i] - targetHeights[i]);
        if (diff > stopThreshold)
            hasMovement = true;

        float h = currentHeights[i];

        // ─────────────────────
        // CUBE (BarPivot -> Cube)
        // ─────────────────────
        Transform bar = cubes[i].transform;
        Transform cube = bar.Find("Bar");

        cube.localScale = new Vector3(1, h, 1);
        cube.localPosition = new Vector3(0, h / 2f, 0);

        // 🎨 couleur (optionnel mais propre ici)
        Renderer renderer = cube.GetComponent<Renderer>();
        Color cold = new Color(0.2f, 0.4f, 1f);
        Color hot = new Color(1f, 0.3f, 0.2f);

        float normalized = Mathf.InverseLerp(0.5f, 5f, h);
        renderer.material.color = Color.Lerp(cold, hot, normalized);

        // ─────────────────────
        // LABEL VALEUR (smooth)
        // ─────────────────────
        Vector3 targetLabelPos =
            bar.localPosition + new Vector3(0, h + 0.2f, -0.6f);

        valueLabels[i].transform.localPosition = Vector3.Lerp(
            valueLabels[i].transform.localPosition,
            targetLabelPos,
            t
        );
    }
    if (hasMovement)
        {
            stableTime = 0f; //ca bouge: reset
        }
        else
        {
            stableTime += Time.deltaTime; //stable: accumulate
        }
        if(stableTime >= stopThreshold)
        {
            isAnimating = false; //stop animation
            stableTime = 0f; //reset timer
        }
}

    void Update()
    {
        if (isAnimating)
    {
        AnimateGraph();
    }
    }

    private WeatherType ParseWeather(string value)
{
    switch(value.ToLower())
    {
        case "sunny":
            return WeatherType.Sunny;

        case "cloudy":
            return WeatherType.Cloudy;

        case "rain":
            return WeatherType.Rain;

        case "storm":
            return WeatherType.Storm;
    }

    return WeatherType.Cloudy;
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
