using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    private float _secondsBetweenFrames;
    private float _movementSpeed;
    private Vector2 _direction;
    private SpriteRenderer _spriteRenderer;
    private List<Sprite> _sprites;
    private Rigidbody2D _rigidbody2D;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _secondsBetweenFrames = Random.Range(0.05f, 0.2f);
        _movementSpeed = Random.Range(2f, 4f);
        _direction = new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f)).normalized;
        _rigidbody2D.AddForce(_direction * _movementSpeed);
        StartCoroutine(LoopThruSprites());
    }

    IEnumerator LoopThruSprites()
    {
        for (int i = 0; i < _sprites.Count; i++)
        {
            _spriteRenderer.sprite = _sprites[i];
            yield return new WaitForSeconds(_secondsBetweenFrames);
        }
        Destroy(gameObject);
    }
}
