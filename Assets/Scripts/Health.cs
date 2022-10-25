using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    [SerializeField] float hp;
    [SerializeField] float maxHP = 100f;
    [SerializeField, Tooltip("In seconds")] float deathSequenceLength = 1f;
    Movement movementScript;

    private void Awake()
    {
        movementScript = GetComponent<Movement>();
    }

    private void Start()
    {
        hp = maxHP;
    }

    public void Damage(float amount)
    {
        hp = Mathf.FloorToInt(hp - amount);
        if (hp <= 0)
        {
            hp = 0;
            StartCoroutine(Death());
        }
    }

    IEnumerator Death()
    {
        if (movementScript != null)
        {
            movementScript.enabled = false;
        }

        // todo add SFX upon death
        // todo add particles effect upon death

        yield return new WaitForSeconds(deathSequenceLength);
        // todo add a respawn instead of reload
        ReloadLevel();
    }

    private static void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}