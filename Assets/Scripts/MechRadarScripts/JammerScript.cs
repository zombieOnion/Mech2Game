using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JammerScript : MonoBehaviour
{
    public RadarHitList<Transform> JammingRadarBlips;
    public event EventHandler JammedEnemy;

    // Blip settings
    public SendRadarPulseAndCreateRadarEchoes PulseSender;
    public float BlipTimeOut = 0.5f;
    public int BlipSize = 20;
    public bool IsJamming { set; get; }
    private Guid? jammTargetGuid = Guid.Empty;
    private RadarTargetScript jammingTargetScript = null;
    private RadarTargetScript[] paintedTargetOnUs = null;
    private int blipCount = 2;
    void Awake()
    {
        PulseSender = gameObject.GetComponent<SendRadarPulseAndCreateRadarEchoes>();
    }

    public void SetUpJamming(Guid? radarSignature, RadarTargetScript targetToJamm)
    {
        jammTargetGuid = radarSignature;
        jammingTargetScript = targetToJamm;
        JammingRadarBlips = PulseSender.InstantiateRadarBlips(BlipSize, BlipTimeOut, jammTargetGuid.Value);
        Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2, Quaternion.identity, 1 << 8);
        paintedTargetOnUs = hitColliders.Select( c => c.gameObject.GetComponent<RadarTargetScript>()).ToArray();
        JammedEnemy?.Invoke(this, EventArgs.Empty);
    }

    public bool JammTarget()
    {
        if (jammTargetGuid == null || jammingTargetScript == null) {
            Console.WriteLine("Trying to jamm without supplied radarSignature");
            IsJamming = false;
            return false;
        }
        var heading = (jammingTargetScript.transform.position - gameObject.transform.root.position).normalized;
        var blip = JammingRadarBlips.AdvanceNext();
        var direction = jammingTargetScript.transform.position + heading;

        //var randomShootDispersionFactor = UnityEngine.Random.Range(-20, 20);
        var newVector = new Vector3(gameObject.transform.position.x + UnityEngine.Random.Range(10, 60), 
            gameObject.transform.position.y, 
            gameObject.transform.position.z + UnityEngine.Random.Range(-30, 30));
        blip.transform.position = newVector + heading * 1 * blipCount;
        blip.gameObject.SetActive(true);
        blip.GetComponent<RadarBlipScript>().ResetAppearTime();

        foreach(var target in paintedTargetOnUs)
        {
            target.ReceiveNewRadarHitOnTarget(blip);
        }
        if (blipCount < BlipSize)
            blipCount++;
        else
            blipCount = 2;
        return true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (IsJamming) JammTarget();
    }

    private void SetColourOfBlip(RadarBlipScript blipScript) => blipScript.gameObject.GetComponent<Renderer>().materials[0].color = Color.grey;
}
