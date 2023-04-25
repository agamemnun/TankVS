using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float turretRotationSpeed = 30.0f;
    private float turretRotation = -50f;
    [SerializeField] int turretRotationUpAngleLimit = -100;
    [SerializeField] int turretRotationDownAngleLimit = -10;

    public float bulletVelocity = 300.0f;
    public float firePower = 0.0f;
    float chargeMultiplier = 10.0f;
    private const float MAX_FIRE_POWER = 15.0f;
    private bool isAlive = false;
    [SerializeField] GameObject turret;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject spawnLocation;
    [SerializeField] GameObject fireLocation;
    [SerializeField] bool isPlayerTurn = false;

    private bool chargingStarted = false;
    private bool isCharging = false;


    private Vector3 powerIndicatorStartPos;
    private Vector3 powerIndicatorEndPos;



    public void StartTurn()
    {
        isPlayerTurn = true;
    }

    public void EndTurn()
    {
        if (IsAlive())
        {
            if (isCharging)
            {
                Fire();
            }

        }

        isPlayerTurn = false;
    }

    private void EndTurnByPlayer()
    {
        isPlayerTurn = false;
    }

    public void RevivePlayer()
    {
        isAlive = true;
    }

    public bool IsAlive()
    {
        return isAlive;
    }

    public bool IsPlayerTurn()
    {
        return isPlayerTurn;
    }

    private LineRenderer lr;
    private Vector3 turretDir;
    [SerializeField] AnimationCurve powerIndicatorAC;


    // Start is called before the first frame update
    void Start()
    {
        powerIndicatorStartPos = fireLocation.transform.position;
        powerIndicatorEndPos = spawnLocation.transform.position;

        InitializePowerIndicator();
        RotateTurret();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlayerTurn)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            powerIndicatorEndPos = spawnLocation.transform.position;
            turretDir = spawnLocation.transform.position - fireLocation.transform.position;
            chargingStarted = true;
        }

        if (chargingStarted && Input.GetKey(KeyCode.Space))
            ChargeFire();

        if (isCharging && Input.GetKeyUp(KeyCode.Space))
            Fire();

    }

    private void FixedUpdate()
    {
        if (!isPlayerTurn)
            return;

        if (!isCharging)
        {
            if (Input.GetKey(KeyCode.W))
            {
                RotateTurret(1);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                RotateTurret(-1);
            }
        }
    }

    void RotateTurret(int direction = 1)
    {
        turretRotation = turretRotation + (turretRotationSpeed * Time.deltaTime * direction);

        turretRotation = Mathf.Clamp(turretRotation, turretRotationUpAngleLimit, turretRotationDownAngleLimit);

        var rot = turret.transform.localEulerAngles;
        rot.z = turretRotation;

        turret.transform.localEulerAngles = rot;
    }


    void ChargeFire()
    {
        isCharging = true;

        firePower += Time.deltaTime * chargeMultiplier;

        ShowFirePowerIndicator();

        if (firePower >= MAX_FIRE_POWER)
            Fire();
    }

    GameObject SpawnBullet()
    {
        return Instantiate(bullet, spawnLocation.transform.position, bullet.transform.rotation);
    }

    void Fire()
    {
        chargingStarted = false;
        isCharging = false;

        GameObject bulletInstance = SpawnBullet();
        Rigidbody2D bulletRb = bulletInstance.GetComponent<Rigidbody2D>();

        bulletRb.gravityScale = 1;
        bulletRb.AddForce(turret.transform.up * firePower, ForceMode2D.Impulse);
        firePower = 0;

        HideFirePowerIndicator();
        EndTurnByPlayer();
    }

    void CancelFire()
    {
        chargingStarted = false;
        isCharging = false;

        HideFirePowerIndicator();
    }

    void InitializePowerIndicator()
    {
        if (lr == null)
            lr = gameObject.AddComponent<LineRenderer>();

        lr.material = new Material(Shader.Find("Sprites/Default"));

        // A simple 2 color gradient with a fixed alpha of 1.0f.
        float alpha = 1.0f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.yellow, 0.0f), new GradientColorKey(Color.red, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
        );
        lr.colorGradient = gradient;
    }

    void ShowFirePowerIndicator()
    {
        lr.enabled = true;
        lr.positionCount = 2;
        lr.widthCurve = powerIndicatorAC;
        lr.numCapVertices = 10;

        lr.SetPosition(0, fireLocation.transform.position);
        lr.useWorldSpace = true;

        powerIndicatorEndPos += (turretDir * 0.02f);

        //Debug.Log($"start pos: {fireLocation.transform.position}");
        //Debug.Log($"end pos: {powerIndicatorEndPos}");

        lr.SetPosition(1, powerIndicatorEndPos);
    }

    void HideFirePowerIndicator()
    {
        lr.enabled = false;
    }

}
