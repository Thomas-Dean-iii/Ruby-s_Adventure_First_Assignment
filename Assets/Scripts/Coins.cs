using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coins : MonoBehaviour
{

    public AudioClip ChaChingClip;
    public ParticleSystem collectedEffect;

    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();

        if (controller != null)
        {
            if (controller.CoinsCollected < controller.CoinMax)
            {
                controller.ChangeCoins(1);
                Destroy(gameObject);
                controller.PlaySound(ChaChingClip);
                Instantiate(collectedEffect, transform.position + Vector3.up * 0.5f, Quaternion.identity);
            }
        }
    }

    private void Update()
    {
        
    }

}
