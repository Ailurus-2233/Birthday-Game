using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Player : MonoBehaviour {
    public float WalkSpeed;
    public float JumpSpeed;
    public Animator cameraAnim;
    public Camera camera;
    
    private Rigidbody2D rb2d;
    private Animator anim;
    private BoxCollider2D playerFeet;
    private CapsuleCollider2D playerBody;
    private bool isGround;
    private bool isSuperJump;
    private bool isMiddleJump;
    private bool isGameOver;
    public int move;

    private bool[][] flag;
    private bool[] door;
    private bool isReadyTel;
    private Collider2D doorObj;

    private Collider2D endTorch;

    // Start is called before the first frame update
    void Start() {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerFeet = GetComponent<BoxCollider2D>();
        playerBody = GetComponent<CapsuleCollider2D>();
        flag = new[] {new[] {false, false}, new[] {false, false}, new[] {false, false}};
        door = new[] {false, false, false};
        isReadyTel = false;
        isGameOver = false;
    }

    // Update is called once per frame
    void Update() {
        Walk();
        Flip();
        Jump();
        CheckGrounded();
        OpenDoor();
        Teleport();
        GameOver();
    }

    void OnTriggerEnter2D(Collider2D other) {
        string name = other.gameObject.name;
        switch (name) {
            case "key1":
                other.gameObject.SetActive(false);
                flag[0][0] = true;
                break;
            case "key2":
                other.gameObject.SetActive(false);
                flag[0][1] = true;
                break;
            case "key3":
                other.gameObject.SetActive(false);
                flag[1][0] = true;
                break;
            case "key4":
                other.gameObject.SetActive(false);
                flag[1][1] = true;
                break;
            case "key5":
                other.gameObject.SetActive(false);
                flag[2][0] = true;
                break;
            case "key6":
                other.gameObject.SetActive(false);
                flag[2][1] = true;
                break;
            case "door1":
            case "door2":
            case "door3":
                isReadyTel = true;
                doorObj = other;
                break;
            case "torch":
                Fire(other);
                break;
            case "torch-end":
                isGameOver = true;
                endTorch = other;
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        isReadyTel = false;
        this.doorObj = null;
        isGameOver = false;
        endTorch = null;
    }

    void Walk() {
        // float move = Input.GetAxis("Horizontal");
        move = Global.move;
        Vector2 playerVel = new Vector2(move * WalkSpeed, rb2d.velocity.y);
        rb2d.velocity = playerVel;
        bool playerHasXSpeed = Mathf.Abs(rb2d.velocity.x) > Mathf.Epsilon;
        anim.SetBool("IsWalk", playerHasXSpeed);
    }


    void Flip() {
        bool playerHasXSpeed = Mathf.Abs(rb2d.velocity.x) > Mathf.Epsilon;
        if (playerHasXSpeed) {
            if (rb2d.velocity.x > 0.1f) {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }

            if (rb2d.velocity.x < -0.1f) {
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }

    public void Jump() {
        if (Global.isJump) {
            if (isSuperJump) {
                this.JumpSpeed = 7;
            }
            else if (isMiddleJump) {
                this.JumpSpeed = 5;
            }
            else {
                this.JumpSpeed = 2;
            }
            if (isGround) {
                anim.SetBool("IsJump", true);
                Vector2 JumpVel = new Vector2(0, JumpSpeed);
                rb2d.velocity = Vector2.up * JumpVel;
            }
        }
    }

    void CheckGrounded() {
        isGround =
            playerFeet.IsTouchingLayers(LayerMask.GetMask("Ground")) ||
            playerFeet.IsTouchingLayers(LayerMask.GetMask("SuperJump")) ||
            playerFeet.IsTouchingLayers(LayerMask.GetMask("MiddleJump"));
        isSuperJump =
            playerFeet.IsTouchingLayers(LayerMask.GetMask("SuperJump"));
        isMiddleJump =
            playerFeet.IsTouchingLayers(LayerMask.GetMask("MiddleJump"));
        if (isGround) {
            anim.SetBool("IsJump", false);
        }
    }

    void OpenDoor() {
        if (flag[0][0] && flag[0][1]) door[0] = true;
        if (flag[1][0] && flag[1][1]) door[1] = true;
        if (flag[2][0] && flag[2][1]) door[2] = true;
    }

    void Teleport() {
        if (isReadyTel) {
            switch (doorObj.name) {
                case "door1":
                    if (this.door[0] && Global.isJump) {
                        transform.position = new Vector3(2.0f, 1.7f, 0);
                        isReadyTel = false;
                        this.doorObj = null;
                    }

                    break;
                case "door2":
                    if (this.door[1] && Global.isJump) {
                        transform.position = new Vector3(0, 2.0f, 0);
                        isReadyTel = false;
                        this.doorObj = null;
                    }

                    break;
                case "door3":
                    if (this.door[2] && Global.isJump) {
                        transform.position = new Vector3(-2f, -2f, 0);
                        isReadyTel = false;
                        this.doorObj = null;
                    }

                    break;
                default:
                    break;
            }
        }
    }

    void Fire(Collider2D torch) {
        foreach (Transform child in torch.transform) {
            child.gameObject.SetActive(true);
        }
    }

    void GameOver() {
        if (isGameOver) {
            if (Global.isJump) {
                Fire(endTorch);
                foreach (Transform controller in camera.transform) {
                    controller.gameObject.SetActive(false);
                }
                cameraAnim.Play("Camera");
                Global.isGameOver = true;
            }
        }
    }
}