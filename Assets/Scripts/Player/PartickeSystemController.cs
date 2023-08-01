using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartickeSystemController : MonoBehaviour
{
    [SerializeField]
    private float _lifetime = 2f;

    void Start()
    {
        StartCoroutine(DieAfterSec(_lifetime));
    }

    private IEnumerator DieAfterSec(float second)
    {
        yield return new WaitForSeconds(second);
        Destroy(this.gameObject);
    }
}
