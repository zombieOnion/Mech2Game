using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class JammerScript : NetworkBehaviour
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
    private List<RadarTargetScript> paintedTargetOnUs = null;
    private int blipCount = 2;
    void Awake()
    {
        PulseSender = gameObject.GetComponent<SendRadarPulseAndCreateRadarEchoes>();
    }

    public void SetUpJamming(Guid? radarSignature, RadarTargetScript targetToJamm)
    {
        if(!radarSignature.HasValue)
        {
            IsJamming = false;
            jammingTargetScript = null;
            return;
        }
        jammTargetGuid = radarSignature;
        jammingTargetScript = targetToJamm;
        JammingRadarBlips = PulseSender.InstantiateRadarBlips(BlipSize, BlipTimeOut, jammTargetGuid.Value);
        Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2, Quaternion.identity, 1 << 8);
        paintedTargetOnUs = hitColliders.Select( c => c.gameObject.GetComponent<RadarTargetScript>()).ToList();
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
        blip.GetComponent<RadarBlipScript>().DisappearTimerMax.Value = 3f;
        blip.GetComponent<RadarBlipScript>().ResetAppearTime();
        // TODO efter att målet har flyttat sig så hittar vi det inte längre och kan då inte skicka in fler störmål
        // Koll varje gång efter nya mål och ifall de fortfarande har oss som mål, uppdatera listan
        Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2, Quaternion.identity, 1 << 8);
        var newTargets = hitColliders.Select(c => c.gameObject.GetComponent<RadarTargetScript>()).ToList();
        newTargets = newTargets.Where(t => !paintedTargetOnUs.Contains(t)).ToList();
        paintedTargetOnUs.AddRange(newTargets);
        paintedTargetOnUs = paintedTargetOnUs.Where(p => p != null).ToList();
        foreach (var target in paintedTargetOnUs)
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
        if (!IsServer) return;
        if (IsJamming) JammTarget();
    }

    private void SetColourOfBlip(RadarBlipScript blipScript) => blipScript.gameObject.GetComponent<Renderer>().materials[0].color = Color.grey;
}
