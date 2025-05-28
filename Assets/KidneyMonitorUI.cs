using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class KidneyMonitorUI : MonoBehaviour
{
    [Header("IV Fluid")]
    public TMP_Dropdown fluidDropdown;
    public Button administerButton;

    [Header("Urine Output Display")]
    public TMP_Text outputPerHourText;
    public TMP_Text totalOutputText;
    public TMP_Text colorText;
    public TMP_Text gravityText;
    public TMP_Text pHText;

    [Header("Control Buttons")]
    public Button startButton;
    public Button stopButton;
    public Button resetButton;
    public Button saveButton;

    private bool isMonitoring = false;
    private float simulatedTotal = 0f;

    void Start()
    {
        startButton.onClick.AddListener(StartMonitoring);
        stopButton.onClick.AddListener(StopMonitoring);
        resetButton.onClick.AddListener(ResetMonitor);
        administerButton.onClick.AddListener(AdministerFluid);
    }

    void StartMonitoring()
    {
        isMonitoring = true;
        StartCoroutine(UpdateSimulation());
    }

    void StopMonitoring()
    {
        isMonitoring = false;
        StopAllCoroutines();
    }

    void ResetMonitor()
    {
        isMonitoring = false;
        StopAllCoroutines();
        simulatedTotal = 0f;
        UpdateUI(0f, 0f, "Clear", "1.010", "6.5");
    }

    void AdministerFluid()
    {
        string selectedFluid = fluidDropdown.options[fluidDropdown.value].text;
        Debug.Log("Administered: " + selectedFluid);

        // مثال: إذا المحلول يحتوي "Saline" زد الإخراج
        if (selectedFluid.ToLower().Contains("saline"))
        {
            simulatedTotal += 50f;
        }
    }

    IEnumerator UpdateSimulation()
    {
        while (isMonitoring)
        {
            SimulateData();
            yield return new WaitForSeconds(1f); // تحديث كل ثانية
        }
    }

    void SimulateData()
    {
        float hourlyOutput = Random.Range(30f, 80f);
        simulatedTotal += hourlyOutput / 60f; // كأن كل دقيقة تمر (معدل بطيء وطبيعي)
        string color = GetRandomColor();
        string gravity = Random.Range(1.010f, 1.030f).ToString("F3");
        string ph = Random.Range(4.5f, 8f).ToString("F1");

        UpdateUI(hourlyOutput, simulatedTotal, color, gravity, ph);
    }

    void UpdateUI(float hourOut, float total, string color, string gravity, string ph)
    {
        outputPerHourText.text = $"Output/hour (ml/h): {hourOut:F0}";
        totalOutputText.text = $"Total output: {total:F0} ml / 24h";
        colorText.text = $"Color: {color}";
        gravityText.text = $"Specific gravity: {gravity}";
        pHText.text = $"pH: {ph}";
    }

    string GetRandomColor()
    {
        string[] colors = { "Clear", "Amber", "Dark Yellow", "Red-Tint" };
        return colors[Random.Range(0, colors.Length)];
    }
}