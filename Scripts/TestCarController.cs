using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class TestCarController : MonoBehaviour
{
    [Header("Center of mass")]
    public Vector3 CenterOfMass;
    public GameObject Car;
    private Rigidbody carRigidbody;
    private bool going = true;
    private bool brake = false;
    public float topSpeedForward = 15.0f;
    public float topSpeedBackward = 5.0f;
    public enum Drivetrain
    {
        FWD,
        RWD,
        FourWD
    };

    private float horizontalInput, verticalInput;
    private float steeringAngle;

    public AudioSource accelerationSound;
    private float pitch = 0.9f;

    [Header("Car Settings")]
    public Drivetrain drivetrain = Drivetrain.FWD;
    public float maxSteeringAngle = 30;
    public float torque = 60;
    public float brakeForce = 60;

    [Header("Wheel Colliders")]
    public WheelCollider frontRightWheel;
    public WheelCollider frontLeftWheel;
    public WheelCollider rearRightWheel;
    public WheelCollider rearLeftWheel;

    [Header("Wheels")]
    public Transform frontRightTransform;
    public Transform frontLeftTransform;
    public Transform rearRightTransfrom;
    public Transform rearLeftTransform;

    private void GetInput()
    {
        float verticalInputCopy = verticalInput;
        
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");




        if (Math.Sign(verticalInput) != Math.Sign(verticalInputCopy))
        {
            brake = false;
        }
        
        if (Math.Sign(verticalInput) ==0)
        {
            brake = true;
        }
        if (Math.Sign(verticalInput) == Math.Sign(verticalInputCopy) )
        {
            if(!brake){
                going = true;
            }
        }
        else
        {
            if (!brake)
            {
                going = true;
            }
            else 
            {
                going = false;
            }

        }

        if(Math.Sign(verticalInputCopy)==0 && Math.Sign(verticalInput)==0){
            going=false;
            brake=true;
        }
    }


    private void Movement()
    {
        if (going)
        {
            
                frontLeftWheel.brakeTorque = 0.0f;
                frontRightWheel.brakeTorque = 0.0f;
                rearLeftWheel.brakeTorque = 0.0f;
                rearRightWheel.brakeTorque = 0.0f;

                switch (drivetrain)
                {

                    case Drivetrain.FWD:
                        frontLeftWheel.motorTorque = torque * verticalInput;
                        frontRightWheel.motorTorque = torque * verticalInput;
                        break;
                    case Drivetrain.RWD:
                        rearLeftWheel.motorTorque = torque * verticalInput;
                        rearRightWheel.motorTorque = torque * verticalInput;
                        break;
                    case Drivetrain.FourWD:
                        frontLeftWheel.motorTorque = torque * verticalInput;
                        frontRightWheel.motorTorque = torque * verticalInput;
                        rearLeftWheel.motorTorque = torque * verticalInput;
                        rearRightWheel.motorTorque = torque * verticalInput;
                        break;
                }

            if(verticalInput >= 0 && carRigidbody.velocity.magnitude > topSpeedForward)
            {
                carRigidbody.velocity = carRigidbody.velocity.normalized * topSpeedForward;

                ScoreManager.instance.AddPoint();
            }

            if (verticalInput < 0 && carRigidbody.velocity.magnitude > topSpeedBackward)
            {
                carRigidbody.velocity = carRigidbody.velocity.normalized * topSpeedBackward;
                
            }

            

        }
        else
        {
            brake = true;

            frontLeftWheel.motorTorque = 0.0f;
            frontRightWheel.motorTorque = 0.0f;
            rearLeftWheel.motorTorque = 0.0f;
            rearRightWheel.motorTorque = 0.0f;

            frontLeftWheel.brakeTorque = brakeForce * Math.Abs(verticalInput);
            frontRightWheel.brakeTorque = brakeForce * Math.Abs(verticalInput);
            rearLeftWheel.brakeTorque = brakeForce * Math.Abs(verticalInput);
            rearRightWheel.brakeTorque = brakeForce * Math.Abs(verticalInput);

        }

        if (carRigidbody.velocity.magnitude < 0.1f)
        {
            brake = false;
        }
    }
    private void Accelerate()
    {
        if (verticalInput != 0)
        {
            Movement();

        }
        else
        {

            frontLeftWheel.motorTorque = 0.0f;
            frontRightWheel.motorTorque = 0.0f;
            rearLeftWheel.motorTorque = 0.0f;
            rearRightWheel.motorTorque = 0.0f;

            if(carRigidbody.velocity.magnitude > topSpeedForward)
            {
                carRigidbody.velocity = carRigidbody.velocity.normalized * topSpeedForward;

                ScoreManager.instance.AddPoint();
            }
            accelerationSound.Stop();
            frontLeftWheel.brakeTorque = brakeForce * 1.1f;
            frontRightWheel.brakeTorque = brakeForce * 1.1f;
            rearLeftWheel.brakeTorque = brakeForce * 1.1f;
            rearRightWheel.brakeTorque = brakeForce * 1.1f;
        }


    }

    private void Steer()
    {
        steeringAngle = maxSteeringAngle * horizontalInput;
        frontLeftWheel.steerAngle = steeringAngle;
        frontRightWheel.steerAngle = steeringAngle;
    }

    private void UpdateWheelPoses()
    {
        UpdateWheelPose(frontLeftWheel, frontLeftTransform);
        UpdateWheelPose(frontRightWheel, frontRightTransform);
        UpdateWheelPose(rearLeftWheel, rearLeftTransform);
        UpdateWheelPose(rearRightWheel, rearRightTransfrom);
    }

    private void UpdateWheelPose(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos = wheelTransform.position;
        Quaternion rot = wheelTransform.rotation;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.position = pos;
        wheelTransform.rotation = rot;
    }

    private void Start()
    {
        Car = GameObject.FindGameObjectWithTag("Player");
        accelerationSound.Play();
        carRigidbody = Car.GetComponent<Rigidbody>();

        carRigidbody.centerOfMass = CenterOfMass;
    }
    private void FixedUpdate()
    {
        GetInput();
        Steer();
        Accelerate();
        UpdateWheelPoses();
        pitch = carRigidbody.velocity.magnitude / topSpeedForward;
        Debug.Log(pitch);
        accelerationSound.pitch = pitch;
        
    }

    //Detect collisions between the GameObjects with Colliders attached
    void OnCollisionEnter(Collision collision)
    {
        //Check for a match with the specified name on any GameObject that collides with your GameObject
/*        if (collision.gameObject.tag == "Terrain" && onRoad == true)
        {
            //If the GameObject's name matches the one you suggest, output this message in the console
            onRoad = false;
        }
        else if (collision.gameObject.tag == "Terrain"&& onRoad == false)
        {
            ScoreManager.instance.LoseLife();
        }

        if (collision.gameObject.tag == "Road")
        {
            onRoad = true;
        }*/
        //Check for a match with the specific tag on any GameObject that collides with your GameObject
        if (collision.gameObject.tag == "Police")
        {
            ScoreManager.instance.LoseLife();
        }

        int layerMask = 1 << 3;
        RaycastHit hit;
        // Does the ray intersect any objects in the tree layer
        if (Physics.Raycast(Car.transform.position, Car.transform.TransformDirection(Vector3.forward), out hit, 100, layerMask))
        {
            ScoreManager.instance.LoseLife();
        }
    }
}
