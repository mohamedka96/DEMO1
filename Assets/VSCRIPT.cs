using UnityEngine;
using TMPro;

public class VentilatorSimulator : MonoBehaviour
{
    [Header("المؤشرات الحيوية")]
    public TMP_Text respiratoryRateText; // معدل التنفس (RR)
    public TMP_Text spo2Text;            // تشبع الأكسجين (SpO2)
    public TMP_Text peepText;            // ضغط نهاية الزفير (PEEP)
    public TMP_Text ieRatioText;         // نسبة الشهيق إلى الزفير (I:E)

    [Header("الإعدادات")]
    public float updateInterval = 2f;    // معدل تحديث القراءات
    public bool isAutoUpdate = true;     // التحديث التلقائي

    // القيم الطبيعية الأساسية
    private float normalRR = 14f;        // معدل التنفس الطبيعي (14-20 للبالغين)
    private float normalSpO2 = 97.3f;    // تشبع الأكسجين الطبيعي (>95%)
    private float normalPEEP = 5f;       // ضغط نهاية الزفير الطبيعي (5-10 cmH2O)
    private float normalIERatio = 2f;    // نسبة الزفير إلى الشهيق (1:1.5 إلى 1:3)

    // القيم الحالية
    private float currentRR;
    private float currentSpO2;
    private float currentPEEP;
    private float currentIERatio;

    void Start()
    {
        // تهيئة القيم الابتدائية
        ResetToNormalValues();
        
        // بدء التحديث التلقائي إذا كان مفعلاً
        if(isAutoUpdate)
        {
            InvokeRepeating("UpdateVitals", 0f, updateInterval);
        }
    }

    void UpdateVitals()
    {
        // محاكاة التغيرات الطبيعية في التنفس
        SimulateNormalBreathing();
        
        // تحديث العرض
        UpdateDisplay();
    }

    void SimulateNormalBreathing()
    {
        // تغيرات طفيفة لمحاكاة التنفس الطبيعي
        currentRR += Random.Range(-0.5f, 0.5f);
        currentSpO2 += Random.Range(-0.1f, 0.1f);
        currentPEEP += Random.Range(-0.2f, 0.2f);
        currentIERatio += Random.Range(-0.05f, 0.05f);
        
        // التأكد من بقاء القيم في نطاقات طبيعية
        ClampValues();
    }

    void ClampValues()
    {
        currentRR = Mathf.Clamp(currentRR, normalRR - 2f, normalRR + 2f);
        currentSpO2 = Mathf.Clamp(currentSpO2, normalSpO2 - 1f, normalSpO2 + 0.5f);
        currentPEEP = Mathf.Clamp(currentPEEP, normalPEEP - 1f, normalPEEP + 1f);
        currentIERatio = Mathf.Clamp(currentIERatio, normalIERatio - 0.3f, normalIERatio + 0.3f);
    }

    void UpdateDisplay()
    {
        // تحديث النصوص مع التنسيق المناسب
        respiratoryRateText.text = $"RR {currentRR:F0}";
        spo2Text.text = $"{currentSpO2:F1}%";
        peepText.text = $"PEEP\n{currentPEEP:F1} / 1:{currentIERatio:F1}";
        ieRatioText.text = $"I:E Ratio\n1:{currentIERatio:F2}";
    }

    // إعادة التعيين إلى القيم الطبيعية
    public void ResetToNormalValues()
    {
        currentRR = normalRR;
        currentSpO2 = normalSpO2;
        currentPEEP = normalPEEP;
        currentIERatio = normalIERatio;
        UpdateDisplay();
    }

    // تبديل التحديث التلقائي
    public void ToggleAutoUpdate()
    {
        isAutoUpdate = !isAutoUpdate;
        if(isAutoUpdate)
        {
            InvokeRepeating("UpdateVitals", 0f, updateInterval);
        }
        else
        {
            CancelInvoke("UpdateVitals");
        }
    }
}