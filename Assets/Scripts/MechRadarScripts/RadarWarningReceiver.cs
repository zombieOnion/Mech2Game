using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;

public class RadarWarningReceiver : NetworkBehaviour
{
    LayerMask radarBlipsMask;
    LayerMask uiMask;
    private NetworkObjectPoolSpawner spawner;
    private SpawnPlayerManager playerSpawnManager;

    public RadarHitList<Transform> LineGos { get; private set; }
    public DisappearTimerLocaleScript RadarWarningReceiverUITimerScript { get; private set; }

    public GameObject RadarWarningReceiverUI;
    float radarHitTime = 1f;
    public Guid? RadarSignatureOfHit = null;
    float maxRadarReciverTime = 2f;
    float countUpSenseHit = 0f;
    float triggerActivationRestPeriod = 0.2f;
    float triggerActivationRestPeriodCount = 0f;
    private GameObjectUtilityFunctions utility;
    private EwoGameObjectReference ewoRefScript = null;
    private Transform canvasGo;
    private GameObject ewoGoId;
    [SerializeField] GameObject RadarWarningLinePreFab;
    private string RadarWarnerText = null;

    private ClientRpcParams clientRpcParams;
    private bool hasCreatedClientRpcParams = false;

    // Start is called before the first frame update

    void Awake()
    {
        //utility = FindAnyObjectByType<GameObjectUtilityFunctions>();
        ewoRefScript = transform.root.GetComponent<EwoGameObjectReference>();
    }
    void Start()
    {
        radarBlipsMask = LayerMask.NameToLayer("PLOT");
        uiMask = LayerMask.NameToLayer("UI");
    }
    public override void OnNetworkSpawn()
    {
        utility = FindAnyObjectByType<GameObjectUtilityFunctions>();
        LineGos = DisappearTimerLocaleScript.InstantiateRadarBlipsGeneral(30, 2, transform.position, RadarWarningLinePreFab.transform);
        //SendRadarPulseAndCreateRadarEchoes.SerParentList(LineGos, gameObject.transform);
        transform.root.GetComponent<EwoGameObjectReference>().EwoRefeenceId.OnValueChanged += OnEwoGoIdChanged;
        if (!IsServer)
        {
            base.OnNetworkSpawn();
            return;
        }
        spawner = FindAnyObjectByType<NetworkObjectPoolSpawner>();
        playerSpawnManager = FindAnyObjectByType<SpawnPlayerManager>();
        base.OnNetworkSpawn();
    }
    public override void OnNetworkDespawn()
    {
        if (!IsServer)
        {
            base.OnNetworkSpawn();
            return;
        }
        transform.root.GetComponent<EwoGameObjectReference>().EwoRefeenceId.OnValueChanged -= OnEwoGoIdChanged;
        base.OnNetworkSpawn();
    }

    private void OnEwoGoIdChanged(ulong previous, ulong current)
    {
        //if (!IsServer) return;
        canvasGo = utility.FindGameObjectByNetworkObjectId(current).transform.Find("Canvas nav");
        ewoGoId = utility.FindGameObjectByNetworkObjectId(current);
        RadarWarningReceiverUI = utility.FindGameObjectByNetworkObjectId(current).transform.Find("Canvas nav/RWR").gameObject;
        RadarWarningReceiverUI.GetComponent<TextMeshProUGUI>().enabled = false;
        RadarWarningReceiverUITimerScript = RadarWarningReceiverUI.GetComponent<DisappearTimerLocaleScript>();
        RadarWarningReceiverUITimerScript.DisappearTimerMax = 2f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsServer) return;
        if (other.gameObject.layer != radarBlipsMask || !other.name.Equals("RadarBlip(Clone)"))
            return;
        if(triggerActivationRestPeriodCount < triggerActivationRestPeriod)
            return;
        else
            triggerActivationRestPeriodCount = 0;
        RadarSignatureOfHit = (other.GetComponent<RadarBlipScript>()).radarSignature;
        Vector3 hit = new Vector3(other.transform.position.x, 1, other.transform.position.z);
        Vector3 org = new Vector3(transform.position.x, 1, transform.position.z);
        //Debug.Log(hit + " " + org);
        Vector3 dir = (hit - org).normalized;
        //Debug.DrawLine(transform.position, transform.position + dir * 10, Color.red, Mathf.Infinity);
        //Debug.Log(other.name +" "+ dir);
        if (hasCreatedClientRpcParams == false)
        {
            var clientId = playerSpawnManager.ClientsObject[gameObject.transform.root.GetComponent<NetworkObject>().NetworkObjectId];
            clientRpcParams = GameObjectUtilityFunctions.CreateSrvParaWithClientId(clientId);
            hasCreatedClientRpcParams = true;
        }
        DrawRadarDirectionClientRpc(dir, clientRpcParams);
        //StartCoroutine(TurnOffRWR());
    }

    private void Update()
    {
        triggerActivationRestPeriodCount += Time.deltaTime;
    }

    private IEnumerator TurnOffRWR()
    {
        yield return new WaitForSeconds(radarHitTime);
        if (countUpSenseHit > maxRadarReciverTime)
        {
            RadarWarningReceiverUI.GetComponent<MeshRenderer>().enabled = false;
            countUpSenseHit = 0;
        }
    }

    [ClientRpc]
    private void DrawRadarDirectionClientRpc(Vector3 dir, ClientRpcParams clientRpcParams = default)
    {
        Vector3 offsetFromText = new Vector3(0, 0, -20);

        var lineGo = LineGos.AdvanceNext();
        var rendererControlScript = lineGo.GetComponent<DisappearTimerLocaleScript>();
        rendererControlScript.DisappearTimerMax = 2;
        lineGo.GetComponent<RadarWarningLineScript>().ResetPos(RadarWarningReceiverUI.transform.position + offsetFromText, RadarWarningReceiverUI.transform.position + offsetFromText + dir * 20);
        rendererControlScript.StartCountDown();
        RadarWarningReceiverUITimerScript.StartCountDown();
        //lineGo.GetComponent<NetworkObject>().TrySetParent(ewoGoId, true);
    }
}