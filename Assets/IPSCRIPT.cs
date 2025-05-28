using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfusionPumpUI : MonoBehaviour
{
    [Header("UI Text Elements")]
    public TextMeshProUGUI rateText;
    public TextMeshProUGUI volumeText;
    public TextMeshProUGUI doseText;

    [Header("Control Buttons")]
    public Button increaseRateButton;
    public Button decreaseRateButton;
    public Button startButton;
    public Button stopButton;

    // Data
    private float rate = 20f;        // ml/hr
    private float volume = 100f;     // ml
    private float dose = 500f;       // mg
    private float dosePerMl = 5f;    // mg/ml
    private bool isRunning = false;

    void Start()
    {
        // زر زيادة/نقصان السرعة
        increaseRateButton.onClick.AddListener(() => ChangeRate(+1f));
        decreaseRateButton.onClick.AddListener(() => ChangeRate(-1f));

        // زر التشغيل والإيقاف
        startButton.onClick.AddListener(StartInfusion);
        stopButton.onClick.AddListener(StopInfusion);

        UpdateUI();
    }

    void Update()
    {
        if (!isRunning || volume <= 0) return;

        float delta = Time.deltaTime;
        float infusedVolume = rate * (delta / 3600f); // ml/s

        volume = Mathf.Max(volume - infusedVolume, 0);
        dose = volume * dosePerMl;

        if (volume <= 0)
            StopInfusion();

        UpdateUI();
    }

    void ChangeRate(float delta)
    {
        rate = Mathf.Clamp(rate + delta, 1f, 100f); // الحدود بين 1 و100
        UpdateUI();
    }

    void StartInfusion() => isRunning = true;
    void StopInfusion() => isRunning = false;

    void UpdateUI()
    {
        rateText.text = $"Rate: {rate:F1} ml/hr";
        volumeText.text = $"Volume: {volume:F1} ml";
        doseText.text = $"Dose: {dose:F1} mg";
    }
}