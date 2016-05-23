﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MoveCar : MonoBehaviour
{
    public float CarSpeed = 50.0f;
    Vector3 position;

    public float Health = 100;
    public int GasTank = 1;
    string movementAxisHorizontal;
    string movementAxisVertical;
    string fire;
    public GameObject Sniper;
    public GameObject Shotgun;
    public GameObject MachineGun;
    bool shooting = false;
    public int barrel = 0;
    float delaySpan = 1;
    float Delay = 1;
    public float Boost;
    public bool BoostCurrent = false;
    public GameObject currentBullet;
    bool alive = true;
    string fast;

    public Slider healthSlider;
    public Text currentWeapon;

    // Use this for initialization
    void Start () {
        position = transform.position;
        if (gameObject.tag == "P1")
        {
            movementAxisHorizontal = "P1Horizontal" ;
            movementAxisVertical = "P1Vertical" ;
            fire = "Fire1";
            fast = "Fast";
        }
        if (gameObject.tag == "P2")
        {
            movementAxisHorizontal = "P2Horizontal";
            movementAxisVertical = "P2Vertical";
            fire = "Fire2";
            fast = "Fast2";
        }

        if (barrel == 0)
        {
            this.gameObject.transform.GetChild(1).transform.localScale = new Vector3(.1f, .4f, .1f);
            delaySpan = 5;
            Delay = 0;
            currentBullet = Sniper;
            currentWeapon.text = " Sniper";
        }
        else if (barrel == 1)
        {
            this.gameObject.transform.GetChild(1).transform.localScale = new Vector3(.3f, .2f, .2f);
            delaySpan = 3;
            Delay = 0;
            currentBullet = Shotgun;
            currentWeapon.text = " Shotgun";
        }
        else if (barrel == 2)
        {
            this.gameObject.transform.GetChild(1).transform.localScale = new Vector3(.2f, .2f, .2f);
            delaySpan = .1f;
            Delay = 0;
            currentBullet = MachineGun;
            currentWeapon.text = " Machine Gun";
        }
        else if (barrel == 3)
        {
            this.gameObject.transform.GetChild(1).transform.localScale = new Vector3(.2f, .4f, .01f);
            this.gameObject.GetComponentInChildren<ChainsawScript>().Owner = this.gameObject;
            currentWeapon.text = " Chainsaw";
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (alive)
        {
            if (GasTank == 1)
            {
                if (Input.GetButtonDown(fast))
                {
                    CarSpeed = 150;
                    GasTank = 1;
                }
            }
            if (CarSpeed == 150)
            {
                Boost = 5;
                CarSpeed = 99;
                BoostCurrent = true;
            }
            if (BoostCurrent == true)
            {
                Boost -= Time.deltaTime;
            }
            if (Boost <= 0)
            {
                CarSpeed = 50;
                BoostCurrent = false;
            }
            position.x += Input.GetAxis(movementAxisHorizontal) * CarSpeed * Time.deltaTime;
            position.z += Input.GetAxis(movementAxisVertical) * CarSpeed * Time.deltaTime;
            transform.position = new Vector3(position.x, transform.position.y, position.z);
            if (Input.GetButtonDown(fire))
                shooting = true;
            if (Input.GetButtonUp(fire))
                shooting = false;
            if ((shooting && (Delay <= 0)) && (barrel != 3))
            {
                GameObject Bullet = (GameObject)Instantiate(currentBullet, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
                Bullet.GetComponent<bullet>().Owner = this.gameObject;
                if (barrel == 1)
                {
                    for (int i = 1; i < 3; i++)
                    {
                        GameObject exPosBullet = (GameObject)Instantiate(currentBullet, new Vector3(transform.position.x + (5 * i), transform.position.y, transform.position.z), Quaternion.identity);
                        exPosBullet.GetComponent<bullet>().Owner = this.gameObject;
                        GameObject exNegBullet = (GameObject)Instantiate(currentBullet, new Vector3(transform.position.x - (5 * i), transform.position.y, transform.position.z), Quaternion.identity);
                        exNegBullet.GetComponent<bullet>().Owner = this.gameObject;
                    }
                }
                Delay = delaySpan;
            }
        }
        healthSlider.value = Health;
        TankDeath();
        Delay -= Time.deltaTime;
        if (GasTank == 2)
            GasTank = 1;
	}

    void TankDeath()
    {
        if (Health <= 0)
            alive = false;
    }
}
