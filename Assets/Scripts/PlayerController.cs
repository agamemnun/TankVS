using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float turretRotationSpeed = 30.0f;
    public float bulletVelocity = 300.0f;
    public float firePower = 0.0f;
    public float maxFirePower = 15.0f;
    float chargeMultiplier = 10.0f;
    [SerializeField] GameObject turret;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject spawnLocation;
    private bool chargingStarted = false;
    private bool isCharging = false;



    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
            rotateTurretUp();
        else if (Input.GetKey(KeyCode.DownArrow))
            rotateTurretDown();

        if (Input.GetKeyDown(KeyCode.Space))
            chargingStarted = true;

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

        if (firePower >= maxFirePower)
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
    }

}
