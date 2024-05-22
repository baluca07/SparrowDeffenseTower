using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

public class EnemySnakeBehavior : MonoBehaviour
{   
    public GameManagement manager;
    [SerializeField] GameObject[] targets;
    [SerializeField] GameObject target;

    [SerializeField] float health = 20;

    public Animator animator;
    public AudioSource audioPlayer;

    //particles
    [SerializeField] ParticleSystem nestHit;

    //sounds
    [SerializeField] AudioClip damage;
    [SerializeField] AudioClip die;

    private void Start()
    {
        targets[0] = GameObject.Find("Nest");
        targets[1] = GameObject.Find("Nest1");
        targets[2] = GameObject.Find("Nest2");
        manager = FindObjectOfType<GameManagement>();
        animator = GetComponent<Animator>();
        audioPlayer = GetComponent<AudioSource>();
        target = targets[0];
        if (SceneManager.GetActiveScene().name == "Level 3")
        {
            target = targets[Random.Range(1, 3)];
        }

    }

    private void Update()
    {
        if (health <= 0)
        {
            animator.SetBool("alive", false);
            StartCoroutine(DeathAnimation());
            StartCoroutine(DestroyDelay());
        }
        else if (!manager.IsAlive())
        {
            transform.LookAt(target.transform);
            animator.SetTrigger("game over");
        }
        else
        {
            transform.LookAt(target.transform);
            transform.Translate(Vector3.forward * manager.enemySpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Projectile")
        {
            //Debug.Log("Enemy damaged");
            manager.DamageEnemy(ref health);
            audioPlayer.PlayOneShot(damage, 0.5f);
            if(health <= 0)
            {
                manager.KillEnemy();
            }
        }
        if(other.tag == "Target")
        {
            Debug.Log("Entered to target");
            nestHit.Play();
            AudioSource.PlayClipAtPoint(die,transform.position);
            manager.DamagePlayer();
            GameObject.Destroy(gameObject);
        }
    }

    private IEnumerator DeathAnimation()
    {
        yield return new WaitForSeconds(0.35f);
        AudioSource.PlayClipAtPoint(die,transform.position);
        gameObject.SetActive(false);
    }
    private IEnumerator DestroyDelay()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }

}
