using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfusionPumpWithValidation : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_InputField doseInputField;
    public TMP_InputField rateInputField;
    public TMP_Dropdown fluidTypeDropdown;
    public TextMeshProUGUI volumeText;
    public TextMeshProUGUI doseText;
    public TextMeshProUGUI rateText;
    public TextMeshProUGUI fluidText;
    public TextMeshProUGUI warningText;

    public Button confirmButton;
    public Button startButton;
    public Button stopButton;

    private float rate = 0f;       // ml/hr
    private float volume = 0f;     // ml
    private float dose = 0f;       // mg
    private bool isRunning = false;
    private bool isConfirmed = false;

    private float dosePerMl = 5f; // mg/ml
    private string selectedFluid = "";

    void Start()
    {
        warningText.text = "";
        fluidText.text = "";

        confirmButton.onClick.AddListener(ConfirmSettings);
        startButton.onClick.AddListener(StartInfusion);
        stopButton.onClick.AddListener(StopInfusion);

        UpdateUI();
    }

    void Update()
    {
        if (!isRunning || !isConfirmed || volume <= 0) return;

        float delta = Time.deltaTime;
        float infusedVolume = rate * (delta / 3600f);

        volume = Mathf.Max(volume - infusedVolume, 0);
        dose = volume * dosePerMl;

        if (volume <= 0)
            StopInfusion();

        UpdateUI();
    }

    void ConfirmSettings()
    {
        if (!float.TryParse(doseInputField.text, out float enteredDose) ||
            !float.TryParse(rateInputField.text, out float enteredRate))
        {
            warningText.text = "Please enter valid numbers.";
            isConfirmed = false;
            return;
        }

        if (enteredDose < 50f || enteredDose > 1000f)
        {
            warningText.text = "WARNING: Dose out of safe range (50 - 1000 mg)!";
            isConfirmed = false;
            return;
        }

        selectedFluid = fluidTypeDropdown.options[fluidTypeDropdown.value].text;

        dose = enteredDose;
        rate = enteredRate;
        volume = dose / dosePerMl;

        isConfirmed = true;
        warningText.text = "Settings confirmed. You can start the infusion.";
        UpdateUI();
    }

    void StartInfusion()
    {
        if (!isConfirmed)
        {
            warningText.text = "Please confirm settings first!";
            return;
        }

        isRunning = true;
        warningText.text = "Infusion running...";
    }

    void StopInfusion()
    {
        isRunning = false;
        warningText.text = "Infusion stopped.";
    }

    void UpdateUI()
    {
        rateText.text = $"{rate:F1}";
        doseText.text = $"{dose:F1}";
        volumeText.text = $"{volume:F1}";
        fluidText.text = $"{selectedFluid}";
    }
}