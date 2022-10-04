using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Coin : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip audioClip;
    public Text coinstText;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == Hero.Instance.gameObject)
        {
            Hero.Instance.AddCoin();
            Destroy(gameObject);
            coinstText.text = "Coins: " + Hero.Instance.coins;
            audioSource.PlayOneShot(audioClip);
        }
    }
}
