using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject _particlePrefab;
    [SerializeField] private AudioSource _deathAudio;
    public SpriteRenderer spriteRenderer;
    [HideInInspector] public float spriteSize;

    private void Awake()
    {
        spriteSize = spriteRenderer.transform.localScale.x;
    }

    private void OnDestroy()
    {
        int randomInt = Random.Range(4, 8);
        for (int i = 0; i < randomInt; i++)
        {
            int randomX = Random.Range(-4, 4);
            int randomY = Random.Range(-4, 4);
            Vector3 randomPosition = new Vector3(randomX, randomY);
            GameObject particle = Instantiate(_particlePrefab, randomPosition, this.transform.rotation);
            //TODO Make particle color match enemy color
            //TODO Make particle animate and destroy on animation end
            _deathAudio.Play();
        }
    }
}
