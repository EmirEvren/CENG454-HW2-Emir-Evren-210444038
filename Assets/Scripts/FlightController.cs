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

    [Header("Audio Settings")]
    [SerializeField] private AudioClip engineSoundClip;
    [SerializeField] private float idlePitch = 0.5f;   
    [SerializeField] private float maxPitch = 2.0f;    
    [SerializeField] private float idleVolume = 0.3f;  
    [SerializeField] private float maxVolume = 1.0f;   
    [SerializeField] private float audioTransitionSpeed = 2f; 

    private Rigidbody rb;
    private float pitchInput, yawInput, rollInput, thrustInput;
    private AudioSource engineAudioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; 
        rb.linearDamping = 1.5f;
        rb.angularDamping = 12f; 

        if (engineSoundClip != null)
        {
            engineAudioSource = gameObject.AddComponent<AudioSource>();
            engineAudioSource.clip = engineSoundClip;
            engineAudioSource.loop = true;
            engineAudioSource.spatialBlend = 0f;
            engineAudioSource.Play();
        }
    }

    void Update()
    {
        pitchInput = Input.GetAxis("Vertical");
        yawInput = Input.GetAxis("Horizontal");

        rollInput = 0f;
        if (Input.GetKey(KeyCode.Q)) rollInput = 1f;
        if (Input.GetKey(KeyCode.E)) rollInput = -1f;

        thrustInput = Input.GetKey(KeyCode.Space) ? 1f : 0f;

        HandleEngineSound();
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

    private void HandleEngineSound()
    {
        if (engineAudioSource == null) return;

        float forwardVelocity = transform.InverseTransformDirection(rb.linearVelocity).z;
        
        float speedFactor = Mathf.Clamp01(forwardVelocity / (thrustPower / 3f)); 

        float targetPitch = idlePitch + (thrustInput * 0.4f) + (speedFactor * (maxPitch - idlePitch - 0.4f));
        float targetVolume = idleVolume + (thrustInput * 0.3f) + (speedFactor * (maxVolume - idleVolume - 0.3f));

        engineAudioSource.pitch = Mathf.Lerp(engineAudioSource.pitch, targetPitch, Time.deltaTime * audioTransitionSpeed);
        engineAudioSource.volume = Mathf.Lerp(engineAudioSource.volume, targetVolume, Time.deltaTime * audioTransitionSpeed);
    }
}