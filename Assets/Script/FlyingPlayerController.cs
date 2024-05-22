using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlyingPlayerController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody rb; 
    private GameManagement manager;

    [SerializeField] float rotateSpeed = 1.0f;

    [SerializeField] AudioSource audioPlayer;

    [SerializeField] RawImage dashImage;

    private bool canDash = true;

    private bool canAttack = true;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        manager = FindObjectOfType<GameManagement>();
        dashImage = GameObject.Find("Dash Image").GetComponent<RawImage>();

    }
    void Update()
    {
        if (manager.IsAlive())
        {
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");

            if (vertical != 0 || horizontal != 0)
            {
                //Going forward,backward and rotate
                transform.Rotate(0, horizontal * rotateSpeed * Time.deltaTime, 0);
                transform.Translate(Vector3.forward * manager.GetSpeed() * vertical * Time.deltaTime);
            }
            if (Input.GetKeyDown(KeyCode.E) && canDash)
            {
                //Dash with cool down
                if (vertical < 0)
                {
                    rb.AddForce(transform.forward * -manager.GetDashStrength(), ForceMode.Impulse);
                    animator.SetTrigger("dash");
                    //Debug.Log("Dashing forward");
                    StartCoroutine(Dashing());
                }
                else
                {
                    //Dash backward
                    rb.AddForce(transform.forward * manager.GetDashStrength(), ForceMode.Impulse);
                    animator.SetTrigger("dash");
                    //Debug.Log("Dashing backward");
                    StartCoroutine(Dashing());
                }
            }
            if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) && canAttack))
            {
                //Attack from air
                //Debug.Log("Attack!");
                manager.ShootProjectile(transform);
                StartCoroutine(Attack());
            }
        }
        else
        {
            animator.SetBool("alive", false);
        }
    }

    private IEnumerator Attack()
    {

        audioPlayer.Play();
        canAttack = false;
        yield return new WaitForSeconds(manager.GetAttackCooldown());
        canAttack = true;
    }
    private IEnumerator Dashing()
    {
        dashImage.color = new Color(1, 1, 1, 0.4f);
        canDash = false;
        //Debug.Log("Can't dash");
        yield return new WaitForSeconds(manager.GetDashCooldown());
        dashImage.color = new Color(1, 1, 1, 1);
        canDash = true;
        //Debug.Log("Can dash");

    }
}
