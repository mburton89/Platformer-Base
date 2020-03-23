using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    #region Player Variables
    int maxHp = 3;
    float maxSpeed = 4f;
    float xSpeed = 0f;
    float ySpeed = 0f;
    float acceleration = 1f;
    float gravityAcceleration = 0.5f;
    float jumpHeight = -9f;
    float grabWidth = 18f;
    #endregion

    #region Raycasting Variables
    [SerializeField] private GroundCheck _groundCheck;
    [SerializeField] private WallCheck _wallCheckRight;
    [SerializeField] private WallCheck _wallCheckLeft;
    #endregion

    enum PlayerState
    {
        Moving,
        LedgeGrab,
        Door,
        Hurt,
        Death
    }

    PlayerState CurrentState;

    #region KeyCode Control Bools
    bool right;
    bool left;
    bool up;
    bool down;
    bool up_release;
    #endregion

    #region Sprites & Animations for Player
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private List<Sprite> _idleSprites;
    [SerializeField] private List<Sprite> _runSprites;
    [SerializeField] private List<Sprite> _jumpSprites;
    [SerializeField] private List<Sprite> _fallSprites;
    [SerializeField] private List<Sprite> _ledgeGrabSprites;
    [SerializeField] private List<Sprite> _doorSprites;
    [SerializeField] private List<Sprite> _hurtSprites;
    [SerializeField] private List<Sprite> _deathSprites;
    private List<Sprite> _currentSprites;
    private float _animationSpeed;
    #endregion

    #region Audio for Player
    [SerializeField] AudioSource _jumpAudio;
    [SerializeField] AudioSource _stepAudio;
    [SerializeField] AudioSource _hurtAudio;
    #endregion

    private Rigidbody2D _rigidbody2D;

    void Awake()
    {
        CurrentState = PlayerState.Moving;
        _animationSpeed = 0.15f;
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        #region Set up controls for the player
        right = Input.GetKeyDown(KeyCode.D);
        left = Input.GetKeyDown(KeyCode.A);
        up = Input.GetKeyDown(KeyCode.W);
        down = Input.GetKeyDown(KeyCode.S);
        up_release = Input.GetKeyUp(KeyCode.W);
        #endregion
    }

    void HandleState()
    {
        #region State Machine
        switch (CurrentState)
        {
            #region Move State
            case PlayerState.Moving:
                if (xSpeed == 0)
                {
                    PlayAnimationWithSprites(_idleSprites);
                }
                else
                {
                    PlayAnimationWithSprites(_runSprites);
                }
                //Check if player is on the ground
                if (!_groundCheck.isTouchingGround)
                {
                    ySpeed += gravityAcceleration;

                    //Player is in the air
                    if (_rigidbody2D.velocity.y > 0)
                    {
                        PlayAnimationWithSprites(_jumpSprites);
                    }
                    else
                    {
                        PlayAnimationWithSprites(_fallSprites);
                    }

                    //Control the jump height
                    if (up_release && _rigidbody2D.velocity.y < -6)
                    {
                        _rigidbody2D.velocity = new Vector3(_rigidbody2D.velocity.x, -3);
                        ySpeed = -3;
                    }
                }
                else
                {
                    ySpeed = 0;

                    //Jumping code
                    if (up)
                    {
                        ySpeed = jumpHeight;
                        _jumpAudio.Play();
                    }
                }
                //Change direction of sprite
                if (xSpeed != 0)
                {
                    if (xSpeed > 0)
                    {
                        _spriteRenderer.transform.localScale = new Vector3(1, 1, 0);
                    }
                    else
                    {
                        _spriteRenderer.transform.localScale = new Vector3(-1, 1, 0);
                    }
                }

                //Check for moving left or right
                if (right || left)
                {
                    xSpeed += (Convert.ToInt32(right) - Convert.ToInt32(left)) * acceleration;
                    xSpeed = Mathf.Clamp(xSpeed, -maxSpeed, maxSpeed);
                }
                else
                {
                    ApplyFriction(acceleration);
                }

                if (_groundCheck.isTouchingGround && ySpeed > 0)
                {
                    _stepAudio.Play();
                }

                Move();

                //Check for ledge grab state
                //var falling = y - yprevious > 0;
                //var wasnt_wall = !position_meeting(x + grabWidth * image_xscale, yprevious, o_solid);
                //var is_wall = position_meeting(x + grabWidth * image_xscale, y, o_solid);

                //if (falling and wasnt_wall and is_wall) {
                //    xSpeed = 0;
                //    ySpeed = 0;

                //    //Move against the ledge
                //    while (!place_meeting(x + image_xscale, y, o_solid))
                //    {
                //        x += image_xscale;
                //    }

                //    //Check vertical position
                //    while (position_meeting(x + grabWidth * image_xscale, y - 1, o_solid))
                //    {
                //        y -= 1;
                //    }

                //    //Change sprite and state
                //    sprite_index = s_player_ledge_grab;
                //    state = player.ledge_grab;

                //    audio_play_sound(a_step, 6, false);
                //}

                break;
            #endregion
            #region Ledge Grab state
            case PlayerState.LedgeGrab:
                //if (down)
                //{
                //    state = player.moving;
                //}
                //if (up)
                //{
                //    state = player.moving;
                //    ySpeed = jumpHeight;
                //}
                //break;
            #endregion
            #region Door state
            case PlayerState.Door:
                //sprite_index = s_player_exit;
                ////Fade out
                //if (image_alpha > 0)
                //{
                //    image_alpha -= .05;
                //}
                //else
                //{
                //    //Go to the next room
                //    room_goto_next();
                //}
                //break;
            #endregion
            #region Hurt state
            case PlayerState.Hurt:
                PlayAnimationWithSprites(_hurtSprites);
                //Change direction as we fly around
                if (xSpeed != 0)
                {
                    _spriteRenderer.transform.localScale = new Vector3(Mathf.Sign(xSpeed), 1, 0);
                }
                if (!_groundCheck.isTouchingGround)
                {
                    ySpeed += gravityAcceleration;
                }
                else
                {
                    ySpeed = 0;
                    ApplyFriction(acceleration);
                }
                //TODO implement DirectionMoveBounce
                //DirectionMoveBounce(o_solid);

                //Change back to the other states
                if (xSpeed == 0 && ySpeed == 0) 
                {
                    //Check health
                    if (PlayerStats.hp <= 0)
                    {
                        CurrentState = PlayerState.Death;
                    }
                    else
                    {
                        //image_blend = c_white;
                        CurrentState = PlayerState.Moving;
                    }
                }

                break;
            #endregion
            #region Death state
            case PlayerState.Death:
                PlayerStats.hp = maxHp;
                PlayerStats.coins = 0;

                GameManager.Instance.RestartScene();
                break;
                #endregion
        }
        #endregion
    }

    void Move()
    {
        //Horizontal Collisions
        //TODO Implement Pixel Perfect Collision
        //if (place_meeting(x + xSpeed, y, collisionObject))
        //{
        //    while (!place_meeting(x + sign(xSpeed), y, collisionObject))
        //    {
        //        x += Mathf.Sign(xSpeed);
        //    }
        //    xSpeed = 0;
        //}

        //TODO replace this with pixel perfect collision
        if (_wallCheckLeft.isApproachingWall || _wallCheckRight.isApproachingWall)
        {
            xSpeed = 0;
        }

        //TODO Used Rigidbody2D
        transform.position = new Vector3(transform.position.x + xSpeed, transform.position.y);

        //Vertical Collisions
        //TODO Implement Pixel Perfect Collision
        //if (place_meeting(x, y + ySpeed, collisionObject))
        //{
        //    while (!place_meeting(x, y + sign(ySpeed), collisionObject))
        //    {
        //        y += Mathf.Sign(ySpeed);
        //    }
        //    ySpeed = 0;
        //}

        //TODO replace this with pixel perfect collision
        if (_groundCheck.isTouchingGround)
        {
            ySpeed = 0;
        }

        //TODO Used Rigidbody2D
        transform.position = new Vector3(transform.position.x, transform.position.y + ySpeed);
    }

    void DirectionMoveBounce(GameObject collisionObject)
    {
        //Horizontal Collisions
        //TODO Implement Pixel Perfect Collision
        //if (place_meeting(x + xSpeed, y, collisionObject))
        //{
        //    while (!place_meeting(x + Mathf.Sign(xSpeed), y, collisionObject))
        //    {
        //        //TODO Used Rigidbody2D
        //        transform.position = new Vector3(transform.position.x + Mathf.Sign(xSpeed), transform.position.y);
        //    }
        //    xSpeed = -(xSpeed / 4);
        //}

        //TODO Used Rigidbody2D
        transform.position = new Vector3(transform.position.x + xSpeed, transform.position.y);

        //Vertical Collisions
        //TODO Implement Pixel Perfect Collision
        //if (place_meeting(x, y + ySpeed, collisionObject))
        //{
        //    while (!place_meeting(x, y + Mathf.Sign(ySpeed), collisionObject))
        //    {
        //        y += Mathf.Sign(ySpeed);
        //    }
        //    ySpeed = -(ySpeed / 4);
        //    if (Mathf.Abs(ySpeed) < 2)
        //    {
        //        ySpeed = 0;
        //    }
        //}

        //TODO Used Rigidbody2D
        transform.position = new Vector3(transform.position.x, transform.position.y + ySpeed);
    }

    void ApplyFriction(float frictionAmount)
    {
        //First check to see if we're moving
        if (xSpeed != 0)
        {
            if (Mathf.Abs(xSpeed) - frictionAmount > 0)
            {
                xSpeed -= frictionAmount * _spriteRenderer.transform.localScale.x;
            }
            else
            {
                xSpeed = 0;
            }
        }
    }

    void PlayAnimationWithSprites(List<Sprite> playerSpriteList)
    {
        StopCoroutine(nameof(LoopPlayerSprites));
        _currentSprites = playerSpriteList;
        StartCoroutine(LoopPlayerSprites());
    }

    IEnumerator LoopPlayerSprites()
    {
        for (int i = 0; i < _currentSprites.Count; i++)
        {
            _spriteRenderer.sprite = _currentSprites[i];
            yield return new WaitForSeconds(_animationSpeed);
        }
    }
}
