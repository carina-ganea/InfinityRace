using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{

    private float m_horizontalInput;
    private float m_verticalInput;
    private float m_steeringAngle;

    public WheelCollider frontRightW, frontLeftW;
    public WheelCollider backRightW, backLeftW;
    public Transform frontRightT, frontLeftT;
    public Transform backRightT, backLeftT;

    public Rigidbody frontRightR, frontLeftR;
    public Rigidbody backRightR, backLeftR;

    public float maxSteerAngle = 30;
    public float motorForce;

    public void Start()
    {
        frontRightR.maxAngularVelocity = 20;
        frontLeftR.maxAngularVelocity = 20;
        backRightR.maxAngularVelocity = 20;
        backLeftR.maxAngularVelocity = 20;
    }

    public void GetInput()
    {
        m_horizontalInput = Input.GetAxis("Horizontal");
        m_verticalInput = Input.GetAxis("Vertical");
    }

    private void Steer()
    {
        m_steeringAngle = maxSteerAngle * m_horizontalInput;
        frontLeftW.steerAngle = m_steeringAngle;
        frontRightW.steerAngle = m_steeringAngle;
    }

    private void Accelerate()
    {

        frontLeftW.motorTorque = m_verticalInput * motorForce ;
        frontRightW.motorTorque = m_verticalInput * motorForce;
    }

    private void UpdateWheelPoses()
    {
        UpdateWheelPose(frontLeftW, frontLeftT);
        UpdateWheelPose(frontRightW, frontRightT);
        UpdateWheelPose(backLeftW, backLeftT);
        UpdateWheelPose(backRightW, backRightT);
    }

    private void UpdateWheelPose(WheelCollider _collider, Transform _transform)
    {
        Vector3 _pos = _transform.position;
        Quaternion _quat = _transform.rotation;

        _collider.GetWorldPose(out _pos, out _quat);

        _transform.position = _pos;
        _transform.rotation = _quat;
    }


    private void FixedUpdate()
    {
        GetInput();
        Steer();
        Accelerate();
        UpdateWheelPoses();

    }

}
