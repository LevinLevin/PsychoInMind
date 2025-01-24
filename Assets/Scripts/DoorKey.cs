using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorKey : MonoBehaviour
{
    private MeshRenderer meshRenderer;

    [SerializeField]
    private ParticleSystem PSStars;

    [SerializeField]
    private KeyEnum keyName;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();

        LeanTween.moveY(gameObject, transform.position.y + 0.3f, 1f).setEase(LeanTweenType.easeInOutSine).setLoopPingPong();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PSStars.Play();
            meshRenderer.enabled = false;
            FinishDoorSingleton.Instance.AddKey(keyName);
            Invoke(nameof(DeactivateKey), 5f);
        }
    }

    void DeactivateKey()
    {
        meshRenderer.enabled = true;
        gameObject.SetActive(false);
    }
}
