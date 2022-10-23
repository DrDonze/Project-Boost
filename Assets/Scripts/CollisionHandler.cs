using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    void OnCollisionEnter(Collision other)
    {
        switch (other.gameObject.tag)
        {
            case "Player":
                Debug.Log(gameObject.name + " has collided with another player: " + other.gameObject.name + "." );
                break;
            case "Ground":
                Debug.Log(gameObject.name + " has collided with the ground: " + other.gameObject.name + ".");
                break;
            case "Untagged":
                Debug.Log(gameObject.name + " has collided with an object without tag: " + other.gameObject.name + ".");
                break;
            default:
                Debug.Log(gameObject.name + " has collided with an object (" + other.gameObject.name + ") with unidentified tag name: " + other.gameObject.tag + ".");
                break;
        }
    }
}
