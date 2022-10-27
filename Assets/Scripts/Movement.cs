using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour
{
    [SerializeField] float mainThrustPower = 1250f;
    [SerializeField] float rotationThrustPower = 300f;
    [SerializeField] GameObject mainThrustObject;
    [SerializeField] ParticleSystem mainThrustParticles, leftThrustParticles, rightThrustParticles;
    [SerializeField] float mass = 1f;
    [SerializeField] float drag = 0.5f;
    [SerializeField] float angularDrag = 3f;
    [SerializeField] float mainThrustSoundStartVolume = 0.3f;
    [SerializeField] float volumeIncreaseSpeed = 1f;
    [SerializeField] float volumeDecreaseSpeed = 5f;

    Rigidbody myRigidbody;
    AudioSource audioSource;

    bool leftKey, rightKey, thrustKey;

    private void Awake()
    {
        SetRigidbody();

        audioSource = mainThrustObject.GetComponent<AudioSource>();
    }
    void Update()
    {
        ProcessInput();
    }
    private void FixedUpdate()
    {
        ProcessRotation();
        ProcessThrust();
    }
    void SetRigidbody()
    {
        myRigidbody = GetComponent<Rigidbody>();

        myRigidbody.mass = mass;
        myRigidbody.drag = drag;
        myRigidbody.angularDrag = angularDrag;

        myRigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        myRigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }
    void ProcessInput()
    {
        leftKey = Input.GetKey(KeyCode.A);
        rightKey = Input.GetKey(KeyCode.D);
        thrustKey = Input.GetKey(KeyCode.Space);
    }
    void ProcessRotation()
    {
        if (leftKey && rightKey) 
        {
            //Debug.Log("Both left and right key pressed, not rotating");
            rightThrustParticles.Stop();
            leftThrustParticles.Stop();
            return; 
        }

        if (leftKey)
        {
            RotateLeft();
        }
        else
        {
            rightThrustParticles.Stop();
        }

        if (rightKey)
        {
            RotateRight();
        }
        else
        {
            leftThrustParticles.Stop();
        }
    }
    void RotateRight()
    {
        RotateRigidbody(new Vector3(0, 0, -1 * rotationThrustPower));
        leftThrustParticles.Play();
    }
    void RotateLeft()
    {
        RotateRigidbody(new Vector3(0, 0, rotationThrustPower));
        rightThrustParticles.Play();
    }
    void RotateRigidbody(Vector3 eulerAngle)
    {
        Quaternion deltaRotation = Quaternion.Euler(eulerAngle * Time.fixedDeltaTime);
        myRigidbody.MoveRotation(myRigidbody.rotation * deltaRotation);
        myRigidbody.angularVelocity = Vector3.zero;
    }
    void ProcessThrust()
    {
        if (thrustKey)
        {
            ActivateMainThrust();
        }
        else
        {
            StopMainThrust();
        }
    }
    void ActivateMainThrust()
    {
        myRigidbody.AddRelativeForce(mainThrustPower * Vector3.up * Time.fixedDeltaTime);

        if (!audioSource.isPlaying)
        {
            audioSource.volume = mainThrustSoundStartVolume;
            audioSource.Play();
        }
        else
        {
            audioSource.volume += volumeIncreaseSpeed * Time.deltaTime;
        }

        mainThrustParticles.Play();
    }
    void StopMainThrust()
    {
        if (audioSource.volume < 0.01f)
        {
            audioSource.Stop();
        }
        else
        {
            audioSource.volume -= volumeDecreaseSpeed * Time.deltaTime;
        }

        mainThrustParticles.Stop();
    }
}