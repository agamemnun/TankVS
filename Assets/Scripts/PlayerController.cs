using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float turretRotationSpeed = 30.0f;
    public float bulletVelocity = 300.0f;
    public float firePower = 0.0f;
    float chargeMultiplier = 10.0f;
    private const float MAX_FIRE_POWER = 15.0f;
    [SerializeField] GameObject turret;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject spawnLocation;
    [SerializeField] GameObject fireLocation;
    private bool chargingStarted = false;
    private bool isCharging = false;

    private Vector3 powerIndicatorStartPos;
    private Vector3 powerIndicatorEndPos;
    private LineRenderer lr;
    private Vector3 turretDir;
    [SerializeField] AnimationCurve powerIndicatorAC;


    // Start is called before the first frame update
    void Start()
    {
        powerIndicatorStartPos = fireLocation.transform.position;
        powerIndicatorEndPos = spawnLocation.transform.position;

        initializePowerIndicator();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isCharging)
        {
            if (Input.GetKey(KeyCode.UpArrow))
                rotateTurretUp();
            else if (Input.GetKey(KeyCode.DownArrow))
                rotateTurretDown();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            powerIndicatorEndPos = spawnLocation.transform.position;
            turretDir = spawnLocation.transform.position - fireLocation.transform.position;
            chargingStarted = true;
        }

        if (chargingStarted && Input.GetKey(KeyCode.Space))
            chargeFire();

        if (isCharging && Input.GetKeyUp(KeyCode.Space))
            fire();
    }

    void rotateTurretUp()
    {
        if (turret.transform.rotation.z < 0 && turret.transform.rotation.z > -0.84f)
            turret.transform.Rotate(0, 0, turretRotationSpeed * Time.deltaTime);
    }

    void rotateTurretDown()
    {
        if (turret.transform.rotation.z < 0.10f && turret.transform.rotation.z > -0.82f)
            turret.transform.Rotate(0, 0, -turretRotationSpeed * Time.deltaTime);
    }

    void chargeFire()
    {
        isCharging = true;

        firePower += Time.deltaTime * chargeMultiplier;

        showFirePowerIndicator();

        if (firePower >= MAX_FIRE_POWER)
            fire();
    }

    GameObject spawnBullet()
    {
        return Instantiate(bullet, spawnLocation.transform.position, bullet.transform.rotation);
    }

    void fire()
    {
        chargingStarted = false;
        isCharging = false;

        GameObject bulletInstance = spawnBullet();
        Rigidbody2D bulletRb = bulletInstance.GetComponent<Rigidbody2D>();
        bulletRb.gravityScale = 1;
        bulletRb.AddForce(turret.transform.up * firePower, ForceMode2D.Impulse);
        firePower = 0;

        hideFirePowerIndicator();
    }

    void initializePowerIndicator()
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

    void showFirePowerIndicator()
    {
        lr.enabled = true;
        lr.positionCount = 2;
        lr.widthCurve = powerIndicatorAC;
        lr.numCapVertices = 10;

        lr.SetPosition(0, fireLocation.transform.position);
        lr.useWorldSpace = true;

        powerIndicatorEndPos += (turretDir * 0.02f);

        //Debug.Log($"start pos: {fireLocation.transform.position}");
        Debug.Log($"end pos: {powerIndicatorEndPos}");

        lr.SetPosition(1, powerIndicatorEndPos);
    }

    void hideFirePowerIndicator()
    {
        lr.enabled = false;
    }

}
