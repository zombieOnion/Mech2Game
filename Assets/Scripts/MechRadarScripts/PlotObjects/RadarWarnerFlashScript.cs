using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class RadarWarnerFlashScript : NetworkBehaviour, EnableDisableRendererInterface
{
    private TextMeshProUGUI textRenderer;

    void Awake()
    {
        textRenderer = gameObject.GetComponent<TextMeshProUGUI>();
    }

    public void EnableRenderer()
    {
        textRenderer.enabled = true;
    }

    public void DisableRenderer()
    {
        textRenderer.enabled = false;
    }
}
