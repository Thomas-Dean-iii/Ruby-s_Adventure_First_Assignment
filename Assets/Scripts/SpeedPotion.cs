using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPotion : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip Slurp;
    public ParticleSystem DrinkEffect;

    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();

        if (controller != null)
        {
            if (controller.speed < controller.maxSpeed)
            {
                controller.ChangeSpeed(2);
                Destroy(gameObject);
                controller.PlaySound(Slurp);
                Instantiate(DrinkEffect, transform.position + Vector3.up * 0.5f, Quaternion.identity);
            }
        }
    }
}
