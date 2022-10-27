using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    [SerializeField] float hp;
    [SerializeField] float maxHP = 100f;
    [SerializeField, Tooltip("In seconds")] float deathSequenceLength = 3f;
    [SerializeField] AudioClip deathClip;
    [SerializeField] ParticleSystem explosionParticles;

    Movement movementScript;
    AudioSource audioSource;

    bool isAlive = true;

    private void Awake()
    {
        movementScript = GetComponent<Movement>();
        audioSource = GetComponent<AudioSource>();
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
            if (isAlive) StartCoroutine(Death());
        }
    }
    IEnumerator Death()
    {
        isAlive = false;

        DisableMovements();

        explosionParticles.Play();

        audioSource.PlayOneShot(deathClip);

        yield return new WaitForSeconds(deathSequenceLength);

        // todo add a respawn instead of reload
        ReloadLevel();
    }
    private void DisableMovements()
    {
        if (movementScript != null)
        {
            movementScript.enabled = false;
        }
    }
    private static void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
