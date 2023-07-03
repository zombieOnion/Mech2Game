using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using static PhysicalSpaceLibrary;

public class RadarSweepScript : NetworkBehaviour
{
    // general variables
    private RadarTargetComputer targetProcessor;
    private bool radarOn = true;
    public bool RadarOn { get => radarOn; set => radarOn = value; }
    private RadarTargetScript _currentlyTrackedTarget = null; //cache variable for hits on the same targetc

    // cache variables and blips
    public SendRadarPulseAndCreateRadarEchoes PulseSender;
    private MechShootAtPlayer mechShootAtPlayer;
    [SerializeField] protected GameObject RadarBlipLocalePreFab;
    public RadarHitList<Transform> HitList;
    protected int blipCount = 100;
    protected float blipTimeOut = 5;
    public readonly Guid RadarSignature = new Guid();

    // radar rotation and speed
    private float xSweepRotationAngle;
    public float SweepSpeedChangeNumber = 30f;
    public int MaxSweepSpeed = 150;
    public int MinimumSweepSpeed = 30;

    // sector sweep and change direction
    public float SweepSpeed = 60;
    public bool IsSectorSweeping { get; private set; } = false;
    private float xSectorSweepStart = 0f;
    private float xSectorSweepEnd = 90f;
    private float xSectorSweepAngle = 90f;
    private NetworkObjectPoolSpawner spawner;
    private SpawnPlayerManager playerSpawnManager;
    private ClientRpcParams clientRpcParams;
    private bool hasCreatedClientRpcParams;
    private bool playerSpawningIsDone = false;

    void Awake()
    {
        targetProcessor = gameObject.GetComponent<RadarTargetComputer>();
        PulseSender = gameObject.GetComponent<SendRadarPulseAndCreateRadarEchoes>();
        mechShootAtPlayer = gameObject.transform.root.GetComponentInChildren<MechShootAtPlayer>();
    }

    public override void OnNetworkSpawn()
    {
        CreateTargetCache();
        SpawnPlayerManager.Singleton.AllPlayersHaveSpawned.OnValueChanged += OnAllPlayerSpawnedValueChanged;
        if (!IsServer)
        {
            base.OnNetworkDespawn();
            return;
        }
        base.OnNetworkDespawn();
    }

    public override void OnNetworkDespawn()
    {
        SpawnPlayerManager.Singleton.AllPlayersHaveSpawned.OnValueChanged -= OnAllPlayerSpawnedValueChanged;
        if (!IsServer)
        {
            base.OnNetworkDespawn();
            return;
        }
        base.OnNetworkDespawn();
    }

    private void OnAllPlayerSpawnedValueChanged(bool oldValue, bool newValue) => playerSpawningIsDone = newValue;

    [ClientRpc]
    public void countRadarBlipsClientRpc(int blips)
    {
        Debug.Log(blips);
    }

    public void FixedUpdate()
    {
        if (!IsServer || !playerSpawningIsDone) return;
        if (!radarOn)
            return;
        var lastXAngle = xSweepRotationAngle;
        xSweepRotationAngle += (Time.fixedDeltaTime/2f) * SweepSpeed;
        transform.rotation = Quaternion.Euler(0, xSweepRotationAngle, 90);
        if(IsSectorSweeping)
        {
            if(360 < xSectorSweepStart + xSectorSweepAngle && 0 < xSweepRotationAngle && xSweepRotationAngle < xSectorSweepEnd)
            {

            }
            else if (xSweepRotationAngle > xSectorSweepStart+ xSectorSweepAngle || xSweepRotationAngle < xSectorSweepStart)
            {
                xSweepRotationAngle = xSectorSweepStart;
            }
        }
        if (lastXAngle <= 360f && xSweepRotationAngle > 360f)
            xSweepRotationAngle = 0;
        var hits = SendAndCreateTargets();
        if (hasCreatedClientRpcParams == false && mechShootAtPlayer == null)
        {
            var clientId = SpawnPlayerManager.Singleton.ClientsObject[gameObject.transform.root.GetComponent<NetworkObject>().NetworkObjectId];
            var ewoOwnerId = gameObject.transform.root.GetComponentInChildren<EwoGameObjectReference>().EwoRefeence.GetComponent<NetworkObject>().OwnerClientId;
            if (clientId != ewoOwnerId)
                clientRpcParams = GameObjectUtilityFunctions.CreateSrvParaWithClientId(new ulong[] { clientId , ewoOwnerId });
            else
                clientRpcParams = GameObjectUtilityFunctions.CreateSrvParaWithClientId(clientId);
            hasCreatedClientRpcParams = true;
        }
        if (hits.Length > 0)
        {
            foreach (var hit in hits)
            {
                AddRadarHit(hit);
                if(mechShootAtPlayer == null)
                    CreateLocaleRadarBlipClientRpc(hit.transform.position, clientRpcParams);
            }
        }
    }
    protected virtual void CreateTargetCache()
    { //RadarSignature
        var currentPos = transform.position;
        HitList = DisappearTimerLocaleScript.InstantiatePrefabWithDisappearsGeneralLocale(blipCount, blipTimeOut, new Vector3(currentPos.x, currentPos.y-10, currentPos.z), RadarBlipLocalePreFab.transform);
    }

    protected virtual Transform[] SendAndCreateTargets()
    {
        return PulseSender.SendAndRecieveRadarPulse(HitList);
    } 

    private void AddRadarHit(Transform radarHit)
    {
        Collider[] alreadyExistingRadarTargetsAtHitLocation = CheckForOverlap(radarHit, targetProcessor.TargetLayermask);
        if (alreadyExistingRadarTargetsAtHitLocation != null && alreadyExistingRadarTargetsAtHitLocation.Length > 0)
        {
            var target = alreadyExistingRadarTargetsAtHitLocation[0];
            // if already tracking target don't look it up in the targetList again
            if (_currentlyTrackedTarget != null && target.transform.GetInstanceID() == _currentlyTrackedTarget.transform.GetInstanceID())
            {
                _currentlyTrackedTarget.ReceiveNewRadarHitOnTarget(radarHit);
            }
            else
            {
                foreach (var alreadyPlottedTarget in targetProcessor.Targets)
                {
                    if (target.transform.GetInstanceID() == alreadyPlottedTarget.GetInstanceID())
                    {
                        _currentlyTrackedTarget = target.transform.GetComponent<RadarTargetScript>(); //cache tracked target
                        _currentlyTrackedTarget.ReceiveNewRadarHitOnTarget(radarHit);
                        break;
                    }
                }
            }
        }
        //hit something but no overlapp with anything, not enough signal to create a target
        else
        {
            _currentlyTrackedTarget = null;
        }
    }

    [ClientRpc]
    private void CreateLocaleRadarBlipClientRpc(Vector3 dir, ClientRpcParams clientRpcParams = default)
    {
        DisappearTimerLocaleScript.CreateLocaleRadarBlip(HitList, dir);
    }

    public void ChangeSweepDirection() {
        SweepSpeed *= -1;
    }

    public void IncreaseSweepSpeed() {
        if (Math.Abs(MaxSweepSpeed) <= Math.Abs(SweepSpeed+SweepSpeedChangeNumber))
        {
            SweepSpeed = MaxSweepSpeed;
            return;
        }            
        else
        {
            if(SweepSpeed > 0)
                SweepSpeed += SweepSpeedChangeNumber;
            else
                SweepSpeed -= SweepSpeedChangeNumber;
        }   
    }

    public void DecreaseSweepSpeed() {
        if (Math.Abs(MinimumSweepSpeed) >= Math.Abs(SweepSpeed- SweepSpeedChangeNumber))
        {
            SweepSpeed = MinimumSweepSpeed;
            return;
        }
        else
        {
            if (SweepSpeed > 0)
                SweepSpeed -= SweepSpeedChangeNumber;
            else
                SweepSpeed += SweepSpeedChangeNumber;
        }
    }

    public void ToggleSectorSweep()
    {
        IsSectorSweeping = !IsSectorSweeping;
        if(SweepSpeed < 0)
            SweepSpeed = Math.Abs(SweepSpeed);
    }
    public void RotateSectorSweepForward()
    {
        float rotateAngleDistance = 45;
        if (xSectorSweepStart + rotateAngleDistance > 360f)
        {
            xSectorSweepStart = xSectorSweepStart + rotateAngleDistance - 360f;
        }
        else
        {
            xSectorSweepStart += rotateAngleDistance;
        }

        if (xSectorSweepEnd + rotateAngleDistance > 360f)
        {
            xSectorSweepEnd = xSectorSweepEnd + rotateAngleDistance - 360f;
        }
        else
        {
            xSectorSweepEnd += rotateAngleDistance;
        }
    }
    public void RotateSectorSweepBackward()
    {
        float rotateAngleDistance = 45;
        if (xSectorSweepStart - rotateAngleDistance < 0)
        {
            xSectorSweepStart = 360f + xSectorSweepStart - rotateAngleDistance;
        }
        else
        {
            xSectorSweepStart -= rotateAngleDistance;
        }

        if (xSectorSweepEnd - rotateAngleDistance < 0)
        {
            xSectorSweepEnd = 360f + xSectorSweepEnd - rotateAngleDistance;
        }
        else
        {
            xSectorSweepEnd -= rotateAngleDistance;
        }
    }

    public void IncreaseSectorSweep()
    {
        if (xSectorSweepAngle == 180)
            return;
        xSectorSweepAngle += 45;
        if (xSectorSweepEnd + 45 > 360)
            xSectorSweepEnd = xSectorSweepEnd - 360 + 45;
        else
            xSectorSweepEnd += 45;
    }
    public void DecreaseSectorSweep()
    {
        if (xSectorSweepAngle == 45)
            return;
        xSectorSweepAngle -= 45;
        if (xSectorSweepEnd - 45 < 0)
            xSectorSweepEnd = 360 + xSectorSweepEnd -  45;
        else
            xSectorSweepEnd -= 45;
    }
}
