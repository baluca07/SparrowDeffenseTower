using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class FrozeTextScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] GameManagement manager;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        manager = FindObjectOfType<GameManagement>();
        StartCoroutine(manager.CountDownFreezeTime(text,gameObject));
    }


}
