using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CollisionHandler : MonoBehaviour
{
    [SerializeField] AudioClip impactClip;
    [SerializeField] float minSoundVolumeImpactSpeed = 0.5f;
    [SerializeField] float maxSoundVolumeImpactSpeed = 10f;
    [SerializeField] ParticleSystem successParticles;

    Health healthScript;
    Rigidbody myRigidbody;
    AudioSource audioSource;

    bool hasCollided = false;
    Vector3 currentSpeed = Vector3.zero;
    Vector3 speedBeforeCollision;

    private void Awake()
    {
        healthScript = GetComponent<Health>();
        myRigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        currentSpeed = myRigidbody.velocity;

        if (hasCollided)
        {
            hasCollided = false;

            float deltaSpeed = Mathf.Abs(speedBeforeCollision.magnitude - currentSpeed.magnitude); Debug.Log("Speed difference before-after collision = " + deltaSpeed);

            if (healthScript != null)
            {
                healthScript.Damage(deltaSpeed);
            }
        }
    }

    void OnCollisionEnter(Collision other)
    {
        switch (other.gameObject.tag)
        {
            case "Player": //Debug.Log(gameObject.name + " has collided with another player: " + other.gameObject.name + "." );
                ProcessPhysicalCollision();
                break;
            case "Ground": //Debug.Log(gameObject.name + " has collided with the ground: " + other.gameObject.name + ".");
                ProcessPhysicalCollision();
                break;
            case "Finish":
                FinishSequence();
                break;
            case "Untagged": //Debug.Log(gameObject.name + " has collided with an object without tag: " + other.gameObject.name + ".");
                break;
            default: //Debug.Log(gameObject.name + " has collided with an object (" + other.gameObject.name + ") with unidentified tag name: " + other.gameObject.tag + ".");
                break;
        }
    }
    void FinishSequence()
    {
        successParticles.Play();
        LoadNextLevel();
    }
    void LoadNextLevel() // Temporary function for the finish sequence
    {
        int nextSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1; //Debug.Log("Next Scene Index = " + nextSceneIndex);
        int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings; //Debug.Log("Scene count = " + sceneCount);
        if (nextSceneIndex >= sceneCount)
        {
            nextSceneIndex -= sceneCount;
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneIndex);
    }
    void ProcessPhysicalCollision()
    {
        speedBeforeCollision = currentSpeed;
        hasCollided = true;
        PlayImpactSound(speedBeforeCollision.magnitude);
        //TODO this system doesn't take into account for example if you're not moving on a wall and some ship collide you. You should take damage but your movement difference would be almost none.
    }

    private void PlayImpactSound(float speedBeforeCollision)
    {
        float speedRatio = speedBeforeCollision / maxSoundVolumeImpactSpeed;
        float impactSoundVolume = Mathf.Clamp(speedRatio, minSoundVolumeImpactSpeed, 1);

        audioSource.PlayOneShot(impactClip, impactSoundVolume);
    }
}
