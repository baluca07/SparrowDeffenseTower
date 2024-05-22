using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public ParticleSystem hit;
    private void Start()
    {
        var rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.down * 2;
        transform.SetParent(null);
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag != "Player")
        {
            StartCoroutine(Destroy());
        }
    }
    private IEnumerator Destroy()
    {
        hit.Play();
        yield return new WaitForSeconds(0.35f);
        GameObject.Destroy(gameObject);
    }
}
