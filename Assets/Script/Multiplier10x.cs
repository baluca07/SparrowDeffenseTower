using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multiplier10x : MonoBehaviour
{
    [SerializeField] GameManagement manager;
    private void Start()
    {
        manager = FindObjectOfType<GameManagement>();
        StartCoroutine(DestroyTimer());
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            manager.StartCoroutine(manager.MultiplierTimer(10, gameObject));
            Destroy(gameObject);
        }

    }
    public IEnumerator DestroyTimer()
    {
        yield return new WaitForSeconds(manager.GetUtilityLifetime());
        Destroy(gameObject);
    }
}
