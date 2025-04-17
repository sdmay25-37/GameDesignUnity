using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class EnemyTrapReactor : TrapReactorBase
{
    public override void Trapped(Vector3 trapPos)
    {
        StartCoroutine(Kill(trapPos));
    }

    private IEnumerator Kill(Vector3 trapPos)
    {
        trapPos -= new Vector3(0, 0.1f, 0);
        Vector3 position = transform.position;
        Vector3 diff;
        while(transform.localScale.x > 0.1)
        {
            diff = trapPos - position;
            position = position + diff * 0.03f;
            transform.position = position;
            transform.localScale *= 0.99f;
            yield return null;
        }
        Destroy(gameObject);
    }
}
