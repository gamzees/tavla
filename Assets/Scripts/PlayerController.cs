﻿using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public GameObject GF;
    private bool isTouch = false;

    public GameObject buket,babyBear,gift;
    public Text GirlsCountText;
    private Rigidbody2D _rb;
    public Vector2 moveVelocity;
    public static bool isTalking = false;

    public static int GirlsCount = 0;
    public Animator _animator;

    
    private int lvl1 = 5;
    private int lvl2;
    private int lvl3;
    
    [SerializeField] private float speed=10f;
    void Start()
    {
        _animator = GetComponent<Animator>();
        
        _rb = gameObject.GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        GirlsCountText.text = GirlsCount.ToString();
        
        if (UIController.isGameStart && !UIController.isGamePasue)
        {
            _animator.enabled = true;

            BuketOpen();
            BabyBearOpen();
            GiftOpen();

            #region Controller

#if UNITY_EDITOR || UNITY_STANDALONE
            float horizontalInput = Input.GetAxis("Horizontal");

            if (horizontalInput == 0)
            {
                _animator.SetBool("isMoving", false);
            }
            else
            {
                Flip(Math.Sign(horizontalInput));

                _animator.SetBool("isMoving", true);
            }

            if (horizontalInput < 0 && transform.position.x < -22)
            {
                horizontalInput = 0;
            }
            else if (horizontalInput > 0 && transform.position.x > 22)
            {
                horizontalInput = 0;
            }

            Vector2 moveInput = new Vector2(horizontalInput, 0);

            moveVelocity = moveInput * speed * Time.deltaTime;
#endif

#if UNITY_ANDROID

        if (Input.touchCount > 0)
        {
             Touch touch = Input.GetTouch(0);
             
             if (touch.phase == TouchPhase.Began)
             {
                 isTouch = true;

             } else if (touch.phase == TouchPhase.Ended)
             {
                 isTouch = false;
             }
             
             if (isTouch)
             {
                 if (Screen.width / 2 < touch.position.x)
                 {
                     Vector2 moveMobileInput;
                     
                     if ( transform.position.x > 22)
                     {
                         moveMobileInput = new Vector2(0,0);  
                     }
                     else
                     {
                         moveMobileInput = new Vector2(1,0);
                     }

                     moveVelocity = moveMobileInput * (speed / 2)  * Time.deltaTime;
            
                     Flip(Math.Sign(1));
                        
                     _animator.SetBool("isMoving", true);
                 }
                 else if (Screen.width / 2 > touch.position.x)
                 {
                     Flip(Math.Sign(-1));
                        
                     _animator.SetBool("isMoving", true);
            
                     Vector2 moveMobileInput;
                     
                     if ( transform.position.x < -22)
                     {
                         moveMobileInput = new Vector2(0,0);  
                     }
                     else
                     {
                         moveMobileInput = new Vector2(-1,0);
                     }
            
                     moveVelocity = moveMobileInput * (speed / 2)  * Time.deltaTime;
                 }
             }
             else
             {
                 Vector2 moveMobileInput = new Vector2(0,0);
            
                 moveVelocity = moveMobileInput * (speed / 2)  * Time.deltaTime;
                 
                 _animator.SetBool("isMoving", false);
             }
            
             }
#endif

            #endregion

        }
        else
        {
            _animator.enabled = false;
        }

    }

    private void FixedUpdate()
    {
        if (UIController.isGameStart && !UIController.isGamePasue)
        {
            
            _rb.MovePosition(_rb.position + moveVelocity);
        }

    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Girl"))
        {
            isTalking = true;
        }
    }

    void Flip(float direction){
        Vector3 theScale = transform.localScale;
        theScale.x = -direction / 2;
        transform.localScale = theScale;
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Girl"))
        {
            isTalking = false;
        }
    }
    
    private void BuketOpen()
    {
        if (PowerUp.isTaked && PowerUp.isBuket)
        {
            CancelInvoke(nameof(BuketClose));
            buket.SetActive(true);
            Invoke(nameof(BuketClose),5);
        }
    }

    public void BuketClose()
    {
        PowerUp.isTaked = false;
        PowerUp.isBuket = false;
        buket.SetActive(false);
    }
    
    private void BabyBearOpen()
    {
        if (PowerUp.isTaked && PowerUp.isBabyBear)
        {
            CancelInvoke(nameof(BabyBearClose));
            
            babyBear.SetActive(true);
            
            Invoke(nameof(BabyBearClose),5);
        }
    }
    
    public void BabyBearClose()
    {
   
        PowerUp.isTaked = false;
        PowerUp.isBabyBear = false;
        
       babyBear.SetActive(false);
    }
    
    private void GiftOpen()
    {
        if (PowerUp.isTaked && PowerUp.isGift)
        {
            CancelInvoke(nameof(GiftClose));
            gift.SetActive(true);
            
            Invoke(nameof(GiftClose),5);
        }
    }
    
    public void GiftClose()
    {
   
        PowerUp.isTaked = false;
        PowerUp.isGift = false;
        
        gift.SetActive(false);
    }
    
}