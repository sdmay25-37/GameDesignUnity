using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDetector : MonoBehaviour
{
    [SerializeField] private bool single_use;
    [SerializeField] private SpriteRenderer render;
    private bool onCooldown = false;
    private float cooldown = 10.0f;
    [SerializeField] private Animator animator;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (onCooldown) { return; }
        TrapReactorBase trapReactor = collision.GetComponent<TrapReactorBase>();

        if(trapReactor != null)
        {
            trapReactor.Trapped();
            animator.SetTrigger("Activate");
            onCooldown = true;

            if (single_use)
            {
                Destroy(this); //Should add some stuff if we do single use
            }

        }
    }

    private void EndOfAnimation()
    {
        StartCoroutine(Cooldown());
    }

    private IEnumerator Cooldown()
    {
        render.color = Color.gray;
        yield return new WaitForSeconds(cooldown);
        onCooldown = false;
        render.color = Color.white;
    }
}
