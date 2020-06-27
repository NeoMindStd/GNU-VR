﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour 
{
    public static GameObject player;

    // Status
    [SerializeField] private int hp;

    // 스피드 조정 변수
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float crouchSpeed;

    private float applySpeed;

    [SerializeField] private float jumpForce;

    // 상태 변수
    private bool isWalk = false;
    private bool isRun = false;
    private bool isCrouch = false;
    private bool isGround = true;
    private bool isHitting = false;
    private bool isGetting = false;
    private bool isDead = false;

    // 움직임 체크 변수
    private Vector3 lastPos;

    // 앉았을 때 얼마나 앉을지 결정하는 변수.
    [SerializeField] private float crouchPosY;
    private float originPosY;
    private float applyCrouchPosY;

    // 땅 착지 여부
    private CapsuleCollider capsuleCollider;

    // 민감도
    [SerializeField] private float lookSensitivity;

    // 카메라 한계
    [SerializeField] private float cameraRotationLimit;
    private float currentCameraRotationX = 0;

    // 필요한 컴포넌트
    [SerializeField] private Camera theCamera;
    private Rigidbody myRigid;
    private Crosshair theCrosshair;
    [SerializeField] public GunController theGunController;
    [SerializeField] private DeadScreenController deadScreenController;

    // 카운터
    public static float aliveTimer = 0f;

    private void Awake() 
    {
        player = gameObject;
    }

	void Start () 
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        myRigid = GetComponent<Rigidbody>();
        theCrosshair = FindObjectOfType<Crosshair>();

        // 초기화.
        applySpeed = walkSpeed;
        originPosY = theCamera.transform.localPosition.y;
        applyCrouchPosY = originPosY;
    }

	void Update () 
    {
        if(!isDead)
        {
            aliveTimer += Time.deltaTime;
            IsGround();
            TryJump();
            TryRun();
            TryCrouch();
            Move();
            MoveCheck();
            CameraRotation();
            CharacterRotation();
        }
	}

    public int GetHp()
    {
        return hp;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "enemy" && !isHitting)
        {
            isHitting = true;
            StartCoroutine(HitCoroutine(other));
        }
        if(other.gameObject.tag == "item" && !isGetting)
        {
            isGetting = true;
            StartCoroutine(GetItemCoroutine(other));
        }
    }

    IEnumerator HitCoroutine(Collider collider)
    {
        int damage = Random.Range(5,25);
        damage += collider.gameObject.GetComponent<Enemy>().type == Enemy.Type.zombear ? 20 : 0;
        ReceiveDamage(damage);

        yield return new WaitForSeconds(1);
        isHitting = false;
    }
    IEnumerator GetItemCoroutine(Collider collider)
    {
        collider.SendMessage("OnGetItem", this);

        yield return new WaitForSeconds(1);
        isGetting = false;
    }

    private void GameOver()
    {
        isDead = true;
        GunController.isActivate = false;
        Time.timeScale = 0f;
        Settings.MouseUnlock();
        deadScreenController.Show();
    }

    // 대미지 입음
    public void ReceiveDamage(int damage)
    {
        hp = Mathf.Max(0,hp-damage);
        if(hp <= 0) GameOver();
    }

    // 회복
    public void Heal(int healingPoint)
    {
        hp = Mathf.Min(100,hp+healingPoint);
    }

    // 앉기 시도
    private void TryCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl)) Crouch();
    }


    // 앉기 동작
    private void Crouch()
    {
        isCrouch = !isCrouch;
        theCrosshair.CrouchingAnimation(isCrouch);

        if (isCrouch)
        {
            applySpeed = crouchSpeed;
            applyCrouchPosY = crouchPosY;
        }
        else
        {
            applySpeed = walkSpeed;
            applyCrouchPosY = originPosY;
        }

        StartCoroutine(CrouchCoroutine());

    }


    // 부드러운 동작 실행.
    IEnumerator CrouchCoroutine()
    {

        float posY = theCamera.transform.localPosition.y;
        int count = 0;

        while(posY != applyCrouchPosY)
        {
            count++;
            posY = Mathf.Lerp(posY, applyCrouchPosY, 0.3f);
            theCamera.transform.localPosition = new Vector3(0, posY, 0);
            if (count > 15) break;
            yield return null;
        }
        theCamera.transform.localPosition = new Vector3(0, applyCrouchPosY, 0f);
    }


    // 지면 체크.
    private void IsGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);
        theCrosshair.JumpingAnimation(!isGround);
    }


    // 점프 시도
    private void TryJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround) Jump();
    }


    // 점프
    private void Jump()
    {

        // 앉은 상태에서 점프시 앉은 상태 해제를 원할 시 아래 주석 해제.
        // if (isCrouch) Crouch();

        myRigid.velocity = transform.up * jumpForce;
    }


    // 달리기 시도
    private void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift)) Running();
        if (Input.GetKeyUp(KeyCode.LeftShift)) RunningCancel();
    }


    // 달리기 실행
    private void Running()
    {
        if (isCrouch) Crouch();

        theGunController.CancelFineSight();

        isRun = true;
        theCrosshair.RunningAnimation(isRun);
        applySpeed = runSpeed;
    }


    // 달리기 취소
    private void RunningCancel()
    {
        isRun = false;
        theCrosshair.RunningAnimation(isRun);
        applySpeed = walkSpeed;
    }


    // 움직임 실행
    private void Move()
    {
        float moveDirX = Input.GetAxisRaw("Horizontal");
        float moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 moveHorizontal = transform.right * moveDirX;
        Vector3 moveVertical = transform.forward * moveDirZ;

        Vector3 velocity = (moveHorizontal + moveVertical).normalized * applySpeed;

        myRigid.MovePosition(transform.position + velocity * Time.deltaTime);
    }


    // 움직임 체크
    private void MoveCheck()
    {
        if (!isRun && !isCrouch && isGround)
        {
            if (Vector3.Distance(lastPos, transform.position) >= 0.01f) isWalk = true;
            else isWalk = false;

            theCrosshair.WalkingAnimation(isWalk);
            lastPos = transform.position;
        }
    }


    // 좌우 캐릭터 회전
    private void CharacterRotation()
    {
        float yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 characterRotationY = new Vector3(0f, yRotation, 0f) * lookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(characterRotationY));
    }



    // 상하 카메라 회전
    private void CameraRotation()
    {
        float xRotation = Input.GetAxisRaw("Mouse Y");
        float cameraRotationX = xRotation * lookSensitivity;
        currentCameraRotationX -= cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }


    // 상태 변수 값 반환
    public bool GetRun()
    {
        return isRun;
    }

    public bool GetWalk()
    {
        return isWalk;
    }

    public bool GetCrouch()
    {
        return isCrouch;
    }

    public bool GetIsGround()
    {
        return isGround;
    }
}