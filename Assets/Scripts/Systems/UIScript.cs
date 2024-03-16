using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_tickRateTMP;
    [SerializeField] private TextMeshProUGUI m_tickIntervalText;
    [SerializeField] private TextMeshProUGUI m_canvasText;
    [SerializeField] private CanvasDrawer m_canvasDrawer;

    private float m_tickRate;
    private float m_tickInterval;

    private void OnEnable()
    {
        CanvasRenderingSystem.onRenderUpdate.AddListener(UpdateTickRate);
    }

    private void Update()
    {
        m_tickRateTMP.text = m_tickRate.ToString("0.0") + " tps";
        m_tickIntervalText.text = m_tickInterval.ToString("0.") + " ms";
        m_canvasText.text = $"{m_canvasDrawer.size.x} x {m_canvasDrawer.size.y}";
    }

    private void OnDisable()
    {
        CanvasRenderingSystem.onRenderUpdate.RemoveListener(UpdateTickRate);
    }


    private void UpdateTickRate(float tick_rate)
    {
        m_tickRate = tick_rate;
        m_tickInterval = (1.0f / m_tickRate) * 1000;
    }

}
