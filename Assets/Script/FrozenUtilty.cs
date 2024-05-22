using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrozenUtilty : MonoBehaviour
{
    [SerializeField] GameManagement manager;
    [SerializeField] ParticleSystem freeze;
    private void Start()
    {
        manager = FindObjectOfType<GameManagement>();
        StartCoroutine(DestroyTimer());
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            manager.StartCoroutine(manager.FrozenTimer(gameObject));
            Destroy(gameObject);
        }

    }
    public IEnumerator DestroyTimer()
    {
        yield return new WaitForSeconds(manager.GetUtilityLifetime());
        Destroy(gameObject);
    }
}
