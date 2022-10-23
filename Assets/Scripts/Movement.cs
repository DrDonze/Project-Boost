using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour
{
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float rotationThrust = 30f;
    [SerializeField] float mass = 1f;
    [SerializeField] float drag = 0.5f;
    [SerializeField] float angularDrag = 3f;

    [SerializeField] float startVolume = 0.3f;
    [SerializeField] float volumeIncreaseSpeed = 1f;
    [SerializeField] float volumeDecreaseSpeed = 5f;

    Rigidbody myRigidbody;
    AudioSource audioSource;

    bool leftKey;
    bool rightKey;
    bool thrustKey;

    private void Awake()
    {
        SetRigidbody();

        audioSource = GetComponent<AudioSource>();
    }

    private void SetRigidbody()
    {
        myRigidbody = GetComponent<Rigidbody>();

        myRigidbody.mass = mass;
        myRigidbody.drag = drag;
        myRigidbody.angularDrag = angularDrag;

        myRigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        myRigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    void Update()
    {
        ProcessInput();
        ProcessThrust();
        ProcessRotation();
    }
    void ProcessInput()
    {
        leftKey = Input.GetKey(KeyCode.A);
        rightKey = Input.GetKey(KeyCode.D);
        thrustKey = Input.GetKey(KeyCode.Space);
    }

    private void FixedUpdate()
    {
        ProcessRotation();
        ProcessThrust();
    }
    void ProcessRotation()
    {
        if (leftKey && rightKey) 
        { 
            //Debug.Log("Both left and right key pressed, not rotating");
            return; 
        }

        if (leftKey)
        {
            //Debug.Log("Pressed Left - Rotating");
            RotateRigidbody(new Vector3(0, 0, rotationThrust));
        }

        if (rightKey)
        {
            //Debug.Log("Pressed Right - Rotating");
            RotateRigidbody(new Vector3(0, 0, -1 * rotationThrust));
        }
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
            //Debug.Log("Pressed SPACE - Thrusting");
            myRigidbody.AddRelativeForce(mainThrust * Vector3.up * Time.fixedDeltaTime);

            if (!audioSource.isPlaying)
            {
                audioSource.volume = startVolume;
                audioSource.Play();
            }
            else
            {
                audioSource.volume += volumeIncreaseSpeed * Time.deltaTime;
            }
        }
        else
        {
            if (audioSource.volume < 0.01f)
            {
                audioSource.Stop();
            }
            else
            {
                audioSource.volume -= volumeDecreaseSpeed * Time.deltaTime;
            }
        }
    }

}
