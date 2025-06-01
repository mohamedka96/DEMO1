using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VitalSignsMonitor : MonoBehaviour
{
    public TextMeshProUGUI bpText, prText, rrText, tempText, spo2Text;
    public LineRenderer ecgLine;

    private Queue<float> ecgData = new Queue<float>();
    private float timeCounter = 0f;
    private int ecgResolution = 150;

    void Start()
    {
        for (int i = 0; i < ecgResolution; i++)
            ecgData.Enqueue(0f);

        InvokeRepeating(nameof(UpdateVitals), 0f, 1.0f); // كل ثانية
    }

    void Update()
    {
        UpdateECG();
    }

    void UpdateVitals()
    {
        int systolic = Random.Range(115, 125);
        int diastolic = Random.Range(75, 85);
        bpText.text = $"BP: {systolic}/{diastolic} mmHg";

        prText.text = $"PR: {Random.Range(65, 80)} bpm";
        rrText.text = $"RR: {Random.Range(12, 18)} rpm";
        tempText.text = $"Temp: {(36.5f + Random.Range(-0.3f, 0.3f)).ToString("F1")} °C";
        spo2Text.text = $"SpO2: {Random.Range(96, 99)}%";
    }

    void UpdateECG()
    {
        float ecgPoint = Mathf.Sin(timeCounter * 6f) * 0.5f + Random.Range(-0.05f, 0.05f);
        ecgData.Enqueue(ecgPoint);

        if (ecgData.Count > ecgResolution)
            ecgData.Dequeue();

        ecgLine.positionCount = ecgData.Count;

        int i = 0;
        foreach (float val in ecgData)
        {
            ecgLine.SetPosition(i, new Vector3(i * 0.05f, val, 0));
            i++;
        }

        timeCounter += Time.deltaTime;
    }
}