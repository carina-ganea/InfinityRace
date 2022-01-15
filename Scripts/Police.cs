using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Police : MonoBehaviour
{
    // Defining variables
    public WheelCollider wheelFR;
    public WheelCollider wheelFL;
    public WheelCollider wheelBR;
    public WheelCollider wheelBL;
    public GameObject wheelFRg;
    public GameObject wheelFLg;
    public GameObject wheelBRg;
    public GameObject wheelBLg;
    public float throttle;
    public float drivingforce;
    public float wheelangle;
    public Rigidbody rb;
    public Vector3 center;
    public Vector3 wheelpos;
    public Quaternion wheelrot;
    public Transform target;
    public Transform targeter;
    public float throttlecontrol;
    public float directional;
    public Vector3 reflection;
    public Transform right;
    public Transform left;
    public GameObject red;
    public GameObject blue;
    public bool lightchange;
    public float lighttime;
    public GameObject player;
    public float maxSpeed = 26f;
    public float lastCheckTime = 0;
    public Vector3 lastCheckPos;
    public double xSeconds = 5.0;
    public double yMuch = 1.0;
    //config params

    Vector3 spawnDistance = new Vector3(-25f,1f,-25f);

    //params
    Vector3 PlayerPosition;

    
    // Start is called before the first frame update
    void Start()
    {
        rb.centerOfMass = center;
        
    }

    
    GameObject getPlayerPosition(){
    
    player = GameObject.FindGameObjectWithTag("Player");
    Debug.Log(player.transform.position);
        return player;
    }
    
    
    void RespawnPolice(){
        maxSpeed = maxSpeed+0.5f;
        Debug.Log(maxSpeed);
        Vector3 spawnpoint = getPlayerPosition().transform.TransformPoint(spawnDistance);
        GameObject police = GameObject.FindGameObjectWithTag("Police");
        police.transform.position=spawnpoint;

        
    }
    // Update is called once per frame
    void Update()
    {
        GameObject police = GameObject.FindGameObjectWithTag("Police");
        directional=0;
        //check if police is not moving for respawn
        if ((Time.time - lastCheckTime) > xSeconds)
        {
            if ((police.transform.position - lastCheckPos).magnitude < yMuch){
                RespawnPolice();
                Debug.Log("RESPAWN POLICE ymuch");

            }
            if((police.transform.position - target.position).magnitude > 200){
                RespawnPolice();
                Debug.Log("RESPAWN POLICE distanta");
            }
            lastCheckPos = police.transform.position;
            lastCheckTime = Time.time;
        }


        // This makes the car slow down if making a directional change more than 10 degrees
        if (Mathf.Abs(directional) < 10)
        {
            throttlecontrol = 1;
        }
        else
        {
            throttlecontrol = 0.7f;
        }
        // Targeter looks at player
        

        targeter.LookAt(target);
        // Figures out which direction the car is facing
        // if (targeter.localEulerAngles.y <= 180)
        // {
        //     directional = Mathf.Clamp(targeter.localEulerAngles.y, -25, 25);
        // }
        // else
        // {
        //     directional = Mathf.Clamp(targeter.localEulerAngles.y - 360, -25, 25);
        // }
        // Looks left and right for obstacles, if it finds any, it will make adjustments, adjustments are more intense the closer to the obstacle
        RaycastHit hit;
        if (Physics.Raycast(right.position, right.forward, out hit, 10))
        {
            directional = -10 / (hit.distance / 2);
        }
        if (Physics.Raycast(left.position, left.forward, out hit, 10))
        {
            directional = 10 / (hit.distance / 2);
        }
        // Puts the car into reverse if looking directly at a wall, throttle is increase to 5 times the normal value to help the process
        if (Physics.Raycast(transform.position, transform.forward, out hit, 4))
        {
            
            directional = -directional;
            throttlecontrol = -0.3f;
        }
        // Applies the throttle force
        drivingforce = throttlecontrol * throttle * 50;
        wheelFR.motorTorque = drivingforce;
        wheelFL.motorTorque = drivingforce;
        wheelBR.motorTorque = drivingforce;
        wheelBL.motorTorque = drivingforce;
        //limit police speed
        if( rb.velocity.magnitude > maxSpeed)
            {
                rb.velocity = rb.velocity.normalized * maxSpeed;
            }

        // Sets the direciton of the wheels
        wheelangle = directional;

        wheelFR.steerAngle = wheelangle;
        wheelFL.steerAngle = wheelangle;

        // Sets the graphical model of the wheels to the correct positions/rotations
        wheelFR.GetWorldPose(out wheelpos, out wheelrot);
        wheelFRg.transform.position = wheelpos;
        wheelFRg.transform.rotation = wheelrot;


        wheelFL.GetWorldPose(out wheelpos, out wheelrot);
        wheelFLg.transform.position = wheelpos;
        wheelFLg.transform.rotation = wheelrot;


        wheelBR.GetWorldPose(out wheelpos, out wheelrot);
        wheelBRg.transform.position = wheelpos;
        wheelBRg.transform.rotation = wheelrot;


        wheelBL.GetWorldPose(out wheelpos, out wheelrot);
        wheelBLg.transform.position = wheelpos;
        wheelBLg.transform.rotation = wheelrot;
    }
    void FixedUpdate()
    {
        // Adds a downforce to prevent the car from lifting up too easily, more force is applied the faster the car is going
        // rb.AddForce(-transform.up * rb.velocity.magnitude * 7);
        // Simple script to flash the lights, probably a better way to do this
        lighttime += 1;
        if (lighttime > 10)
        {
            lighttime = 0;
            if (lightchange == false)
            {
                lightchange = true;
                // blue.SetActive(true);
                // red.SetActive(false);
            }
            else
            {
                lightchange = false;
                // blue.SetActive(false);
                // red.SetActive(true);
            }
        }
    }
}

