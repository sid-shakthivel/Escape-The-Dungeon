using System.Collections;
using System.Collections.Generic;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine;

public class LightingManager : MonoBehaviour
{
    public List<Light2D> Lights;

    private void OnPreCull()
    {
        foreach (Light2D light in Lights)
        {
            light.enabled = false;
        }
    }

    private void OnPostRender()
    {
        foreach (Light2D light in Lights)
        {
            light.enabled = true;
        }
    }
}
