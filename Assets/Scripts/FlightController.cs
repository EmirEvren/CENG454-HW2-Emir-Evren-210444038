// FlightController.cs
// CENG 454 HW2: Sky-High Prototype II - Smooth Arcade Flight
// Author:  Emir Evren | Student ID: 210444038

using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FlightController : MonoBehaviour
{
    [Header("Flight Characteristics")]
    [SerializeField] private float pitchTorque = 60f; 
    [SerializeField] private float yawTorque = 40f;   
    [SerializeField] private float rollTorque = 80f;  
    [SerializeField] private float thrustPower = 150f; 
    
    [Header("Fake Gravity Settings")]
    [SerializeField] private float maxGravity = 9.81f; 
    [SerializeField] private float speedToStayAfloat = 15f; 

    private Rigidbody rb;
    private float pitchInput, yawInput, rollInput, thrustInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; 
        rb.linearDamping = 1.5f;
        rb.angularDamping = 12f; 
    }

    void Update()
    {
        pitchInput = Input.GetAxis("Vertical");
        yawInput = Input.GetAxis("Horizontal");

        rollInput = 0f;
        if (Input.GetKey(KeyCode.Q)) rollInput = 1f;
        if (Input.GetKey(KeyCode.E)) rollInput = -1f;

        thrustInput = Input.GetKey(KeyCode.Space) ? 1f : 0f;
    }

    void FixedUpdate()
    {
        HandleThrustAndFakeGravity();
        HandleRotation();
    }

    private void HandleRotation()
    {
        Vector3 torque = new Vector3(pitchInput * pitchTorque, yawInput * yawTorque, rollInput * rollTorque);
        rb.AddRelativeTorque(torque, ForceMode.Acceleration);
    }

    private void HandleThrustAndFakeGravity()
    {

        if (thrustInput > 0)
        {
            rb.AddRelativeForce(Vector3.forward * thrustPower, ForceMode.Acceleration);
        }

        float forwardVelocity = transform.InverseTransformDirection(rb.linearVelocity).z;
        float gravityMultiplier = 1f - Mathf.Clamp01(forwardVelocity / speedToStayAfloat);
        Vector3 customGravity = Vector3.down * (maxGravity * gravityMultiplier);
        rb.AddForce(customGravity, ForceMode.Acceleration);
    }
}