using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CollisionHandler : MonoBehaviour
{
    Health healthScript;
    Rigidbody myRigidbody;
    bool hasCollided = false;
    Vector3 currentSpeed = Vector3.zero;
    Vector3 speedBeforeCollision;

    private void Awake()
    {
        healthScript = GetComponent<Health>();
        myRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        currentSpeed = myRigidbody.velocity;

        if (healthScript == null) return;
        if (hasCollided)
        {
            float deltaSpeed = Mathf.Abs(speedBeforeCollision.magnitude - currentSpeed.magnitude); Debug.Log("Speed difference before-after collision = " + deltaSpeed);
            healthScript.Damage(deltaSpeed);
            hasCollided = false;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        switch (other.gameObject.tag)
        {
            case "Player": //Debug.Log(gameObject.name + " has collided with another player: " + other.gameObject.name + "." );
                AnalyseCollision();
                break;
            case "Ground": //Debug.Log(gameObject.name + " has collided with the ground: " + other.gameObject.name + ".");
                AnalyseCollision();
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
        LoadNextLevel();
    }
    private void LoadNextLevel() // Temporary function for the finish sequence
    {
        int nextSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1; //Debug.Log("Next Scene Index = " + nextSceneIndex);
        int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings; //Debug.Log("Scene count = " + sceneCount);
        if (nextSceneIndex >= sceneCount)
        {
            nextSceneIndex -= sceneCount;
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneIndex);
    }

    void AnalyseCollision()
    {
        speedBeforeCollision = currentSpeed;
        hasCollided = true;

        //TODO this system doesn't take into account for example if you're not moving on a wall and some ship collide you. You should take damage but your movement difference would be almost none.
    }
}
