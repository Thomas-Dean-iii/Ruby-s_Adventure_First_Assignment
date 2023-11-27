using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectable : MonoBehaviour
{
    public AudioClip collectedClip;
    public ParticleSystem FixedEffect;

    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();

        if (controller != null)
        {
            if (controller.health < controller.maxHealth)
            {
                controller.ChangeHealth(1);
                Destroy(gameObject);
                controller.PlaySound(collectedClip);
                Instantiate(FixedEffect, transform.position + Vector3.up * 0.5f, Quaternion.identity);
            }
        }
    }
}