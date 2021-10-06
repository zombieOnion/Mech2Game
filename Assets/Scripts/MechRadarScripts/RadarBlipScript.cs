using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarBlipScript : MonoBehaviour
{

    private float DisappearTimer;
    public float DisappearTimerMax;

    // Update is called once per frame
    void Update()
    {
        DisappearTimer += Time.deltaTime;
        if(DisappearTimer >= DisappearTimerMax)
            Destroy(gameObject);
    }
}
