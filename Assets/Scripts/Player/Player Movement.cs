using System.Collections;
using System.Collections.Generic;
using Microsoft.Win32.SafeHandles;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    int anim_dir = 0;
    Vector2 direction = Vector2.zero;
    public Vector2 final_dir;
    float footAudioTime = 0.4f;


    public Animator anim;
    public Rigidbody2D rigid;
    public PlayerCombat player_combat;

    ToolUsedSquare toolUsed;
    // public ClothesAnimator clothes;

    private void Start()
    {
        toolUsed = GetComponentInChildren<ToolUsedSquare>();
    }



    void Update()
    {

        #region ½ÇÉ«ÒÆ¶¯ÅÐ¶¨
        if (PauseController.IsGamePaused)
        {
            direction = Vector2.zero;
        }
        else
        {

            if (Input.GetKey(KeyCode.A))
            {
                if (Input.GetKey(KeyCode.S))
                {
                    direction = (Vector2.down + Vector2.left) / 1.414f;
                }
                else if (Input.GetKey(KeyCode.W))
                {
                    direction = (Vector2.up + Vector2.left) / 1.414f;
                }
                else
                {
                    direction = Vector2.left;
                }
                anim_dir = 2;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                if (Input.GetKey(KeyCode.S))
                {
                    direction = (Vector2.down + Vector2.right) / 1.414f;
                }
                else if (Input.GetKey(KeyCode.W))
                {
                    direction = (Vector2.up + Vector2.right) / 1.414f;
                }
                else
                {
                    direction = Vector2.right;
                }
                anim_dir = 3;
            }
            else if (Input.GetKey(KeyCode.W))
            {
                direction = Vector2.up;
                anim_dir = 1;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                direction = Vector2.down;
                anim_dir = 0;
            }
            else
            {
                direction = Vector2.zero;
            }
        }
        #endregion

        if (direction != Vector2.zero)
        {
            anim.SetBool("Walk", true);
            footAudioTime -= Time.deltaTime;
            if (footAudioTime <= 0)
            {
                footAudioTime = 0.4f;
                SoundEffectManager.Instance.PlaySecondAudio("Walking");
            }

            if (final_dir != direction)
            {
                anim.SetFloat("Direction", anim_dir);
                // clothes.anim_switch();
                final_dir = direction;
            }

        }
        else
        {
            anim.SetBool("Walk", false);
        }

        if (player_combat.attack == false)
        {
            rigid.velocity = direction * StatsManager.Instance.speed;


        }
        else
        {
            rigid.velocity = Vector2.zero;
        }


    }


}
