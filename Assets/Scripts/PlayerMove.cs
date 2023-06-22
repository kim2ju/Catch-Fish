using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;

public class PlayerMove : MonoBehaviour
{
    public GameObject bluetoothManagerObject;

    private BluetoothManager bluetoothManager_;
    private string arduino_str_;
    private int arduino_num_ = 0;
    private bool isNumber_ = false;

    public float minY;
    public float maxY;
    public float fishCatchDistanceX = 2.0f; // X축 거리만 고려
    public KeyCode catchKey = KeyCode.Z;
    public TextMesh scoreText;
    Animator anim;

    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    CapsuleCollider2D capsuleCollider;
    public int score = 0;
    bool canCatchFish = false;
    bool isCatchingFish = false; // 물고기를 잡는 중인지 여부를 나타내는 변수
    GameObject caughtFish;
    bool isColliding = false;
    float collisionDuration = 2f;

    public AudioClip audioAttack;
    AudioSource audioSource;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        anim = GetComponent<Animator>();
        bluetoothManager_ = bluetoothManagerObject.GetComponent<BluetoothManager>();
        audioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        if (rigid.position.y > maxY)
        {
            rigid.velocity = new Vector2(0, 0);
        }
        else if (rigid.position.y < minY)
        {
            rigid.position = new Vector2(-6, minY);
        }
    }

    void Update()
    {
        arduino_str_ = bluetoothManager_.received_message_;
        isNumber_ = int.TryParse(arduino_str_, out arduino_num_);

        if (isCatchingFish)
            return; // 물고기를 잡는 중일 때는 추가적인 입력을 받지 않음

        if (Input.GetKeyDown(catchKey) || arduino_num_ == 7 || arduino_num_ == 3)
        {
            if (canCatchFish)
            {
                StartCoroutine(CatchFishCoroutine());
            }
        }
        if (isColliding)
        {
            rigid.velocity = new Vector2(0, 0);
        }
        if (Input.GetMouseButtonDown(0) || arduino_num_ == 7 || arduino_num_ == 5)
        {
            if (!isColliding)
            {
                rigid.velocity = new Vector2(0, 2);
            }
        }
        if ((Input.GetKey(KeyCode.Z) || arduino_num_ == 7 || arduino_num_ == 3) && !anim.GetCurrentAnimatorStateInfo(0).IsName("attack"))
        {
            if (!isColliding)
            {
                audioSource.clip = audioAttack;
                audioSource.Play();
                anim.SetBool("attack", true);
            }
        }
        else if (!(Input.GetKey(catchKey) || arduino_num_ == 7 || arduino_num_ == 3) && anim.GetCurrentAnimatorStateInfo(0).IsName("attack"))
        {
                anim.SetBool("attack", false);
        }
    }

    IEnumerator CatchFishCoroutine()
    {
        isCatchingFish = true; // 물고기를 잡는 상태로 변경

        audioSource.clip = audioAttack;
        audioSource.Play();
        anim.SetBool("attack", true); // 애니메이션 실행

        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length); // 애니메이션이 끝날 때까지 대기

        anim.SetBool("attack", false); // 애니메이션 종료

        if (caughtFish != null)
        {
            caughtFish.SetActive(false);
            score++;
            scoreText.text = "SCORE: " + score;
            canCatchFish = false;
            caughtFish = null;
        }

        isCatchingFish = false; // 물고기 잡기 완료 후 상태 변경
        isColliding = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Fish"))
        {
            float distanceX = Mathf.Abs(transform.position.x - other.transform.position.x);

            if (distanceX <= fishCatchDistanceX)
            {
                canCatchFish = true;
                caughtFish = other.gameObject;
            }
        }
        if (other.CompareTag("Object"))
        {
            float distanceX = Mathf.Abs(transform.position.x - other.transform.position.x);


            if (distanceX <= fishCatchDistanceX)
            {
                StartCoroutine(SetCollidingCoroutine());
            } 
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        //&& other.gameObject == caughtFish
        if (other.CompareTag("Fish"))
        {
            canCatchFish = false;
            caughtFish = null;
        }
    }
    IEnumerator SetCollidingCoroutine()
    {
        isColliding = true;
        canCatchFish = false;

        anim.SetBool("collision", true);

        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);


        isColliding = false;
        canCatchFish = true;
        anim.SetBool("collision", false);
    }
}