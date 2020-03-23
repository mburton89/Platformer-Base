using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flyer : Enemy
{
    float xSpeed;
    float ySpeed;
    float maxSpeed;
    float sightDistance;

    public enum WalkerState
    {
        Idle,
        Chase
    }

    public WalkerState CurrentState;

    void Start()
    {
        xSpeed = 0f;
        ySpeed = 0f;
        maxSpeed = 1.5f;
        sightDistance = 180;
    }

    void Update()
    {
        switch (CurrentState)
        {
            //case WalkerState.Idle:
            //    image_index = s_bat_idle;
            //    if (instance_exists(o_player))
            //    {
            //        var dis = point_distance(x, y, o_player.x, o_player.y);
            //        if (dis < sight)
            //        {
            //            state = bat.chase;
            //        }
            //    }
            //    break;
            //case WalkerState.Chase:
            //    if (instance_exists(o_player))
            //    {
            //        var dir = point_direction(x, y, o_player.x, o_player.y);
            //        xspeed = lengthdir_x(max_speed, dir);
            //        yspeed = lengthdir_y(max_speed, dir);
            //        sprite_index = s_bat_fly;
            //        if (xspeed != 0)
            //        {
            //            image_xscale = sign(xspeed);
            //        }

            //        move(o_solid);
            //    }
            //    break;
        }
    }
}
