using System.Collections;
using System.ComponentModel.Design;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class KartController : TerrainDetect
{
	Rigidbody rb;

	[Header("Wheel Colliders")]
	public WheelCollider frontLeftWheel;
	public WheelCollider frontRightWheel;
	public WheelCollider backLeftWheel;
	public WheelCollider backRightWheel;

	[Header("Meshes")]
	public Transform frontLeftMesh;
	public Transform frontRightMesh;
	public Transform backLeftMesh;
	public Transform backRightMesh;

	[Header("Movement Settings")]
	public float maxMotorTorque = 1500f; // 기본 속도
	public float maxSteerAngle = 30f; // 기본 조향각
	public float boostMultiplier = 2f; // 부스터 시 가속 배율
	public float driftFactor = 0.8f; // 드리프트 시 마찰력 감소
	public float boostGauge = 0f;
	public float boostDuration;

	[Header("Effects")]
	public TrailRenderer[] skid;
	public TrailRenderer[] boost;
	public CameraFollowPlayer cmf;
	public GameObject BoostEffect;
	public GameObject HitEffect;

	[Header("Rigidbody Property")]
	public float MoveSpeed;
	public float MAX_SPEED = 15f;
	public float Drag = 0.98f;
	public float SteerAngle = 20;
	public float Traction = 1;
	public float MAX_BOOST = 10;
	public float Gravity;

	[Header("UI")]
	public Slider boostSlider;
	public Text boostText;
	public GameObject ItemLog;

	[Header("Game Management")]
	public GameManage GameManage;

	[Header("Map Setting")]
	public GameSystem.WheelType _wheelType;

	private float BoostSpeed = 1f;

	private float moveInput;
	private float turnInput;
	private bool isBoosting = false;
	private bool isDrifting = false;

	private float DriftValue = 100f;
	private float MoveValue = 100f;

	[HideInInspector]
	public float SpeedCheck;

	private Vector3 moveSpeed;
	void Awake()
	{
		rb = GetComponent<Rigidbody>();
	}

	void Update()
	{

		if (Input.GetKeyDown(KeyCode.F2))
		{
			GameManage.Cheat2();
		}

		if (GameManage.isStore)
		{
			return;
		}

		if (Input.GetKeyDown(KeyCode.F1))
		{
			GameManage.Cheat1();
		}
		else if (Input.GetKeyDown(KeyCode.F3))
		{
			GameManage.Cheat3();
		}
		else if (Input.GetKeyDown(KeyCode.F4))
		{
			GameManage.Cheat4();
		}
		else if (Input.GetKeyDown(KeyCode.F5))
		{
			GameManage.Cheat5();
		}

		moveInput = Input.GetAxis("Vertical");   // ↑↓ 입력값
		turnInput = Input.GetAxis("Horizontal"); // ←→ 입력값

		boostSlider.value = boostGauge / 100;


		// 드리프트 시작/종료
		if (Input.GetKeyDown(KeyCode.LeftShift))
		{
			StartDrift();
		}
		if (Input.GetKeyUp(KeyCode.LeftShift))
		{
			EndDrift();
		}

		// 드리프트 중 부스터 게이지 충전
		if (isDrifting)
		{
			if (isBoosting)
			{
				boostGauge += Time.deltaTime * (backRightWheel.motorTorque >= 0 ? backRightWheel.motorTorque : 0) / 50;
			}
			else
			{
				if (boostGauge <= 100f)
				{
					boostGauge += Time.deltaTime * (backRightWheel.motorTorque >= 0 ? backRightWheel.motorTorque : 0) / DriftValue;
				}
			}
		}

		if (Input.GetKeyDown(KeyCode.Space))
		{
			frontLeftWheel.brakeTorque = 5000f;
			frontRightWheel.brakeTorque = 5000f;
			backLeftWheel.brakeTorque = 5000f;
			backRightWheel.brakeTorque = 5000f;
		}
		if (Input.GetKeyUp(KeyCode.Space))
		{
			frontLeftWheel.brakeTorque = 0f;
			frontRightWheel.brakeTorque = 0f;
			backLeftWheel.brakeTorque = 0f;
			backRightWheel.brakeTorque = 0f;
		}

		if (Input.GetKeyDown(KeyCode.LeftControl) && boostGauge > 100 && !isBoosting)
		{
			StartCoroutine(Boost());
		}

		if(moveInput != 0 && boostGauge <= 100) boostGauge += Time.deltaTime * (backRightWheel.motorTorque >= 0 ? backRightWheel.motorTorque : 0) / MoveValue;
	}
	public void Startboost()
	{
		frontLeftWheel.motorTorque = 100f;
		frontRightWheel.motorTorque = 100f;
		backLeftWheel.motorTorque = 100f;
		backRightWheel.motorTorque = 100f;
		rb.AddForce(Vector3.right * 40, ForceMode.VelocityChange);
	}

	private void FixedUpdate()
	{
		if (GameManage.isStore)
		{
			return;
		}
		if (backLeftWheel.isGrounded)
		{
			SetSkid(true, 0);
		}
		else SetSkid(false, 0);

		if (backRightWheel.isGrounded)
		{
			SetSkid(true, 1);
		}
		else SetSkid(false, 1);

		ApplySteering();
		ApplyAcceleration();
		ApplyDriftSteering();
		UpdateWheelMeshes();
		RigidMovement();
	}

	//Rigidbody 움직임 관리
	void RigidMovement()
	{
		moveSpeed = transform.forward * moveInput * BoostSpeed * MoveSpeed;
		if (GetTerrainAtPosition(transform.position) == 1 && !GameSystem.instance.wheeltype.Equals(_wheelType)) rb.AddForce(-moveSpeed * 0.5f);
		else rb.AddForce(moveSpeed);

		transform.Rotate(Vector3.up * turnInput * SteerAngle * Time.fixedDeltaTime);

		rb.velocity = Vector3.ClampMagnitude(rb.velocity, MAX_SPEED);

		rb.velocity = Vector3.Lerp(rb.velocity.normalized, transform.forward, Traction * Time.fixedDeltaTime) * rb.velocity.magnitude;

		rb.AddForce(Vector3.down * Gravity, ForceMode.Acceleration);

		boostText.text = (rb.velocity.magnitude * 7).ToString("000") + " km / h";
	}

	//부스터
	public IEnumerator Boost()
	{
		isBoosting = true;
		BoostSpeed = MAX_BOOST;
		boostGauge = 0;
		cmf.distance = 14;
		boost[0].emitting = true;
		boost[1].emitting = true;
		BoostEffect.SetActive(true);
		rb.AddForce(transform.forward * BoostSpeed, ForceMode.VelocityChange);
		yield return new WaitForSeconds(boostDuration);
		BoostEffect.SetActive(false);
		cmf.distance = 10;
		boost[0].emitting = false;
		boost[1].emitting = false;
		isBoosting = false;
		BoostSpeed = 1f;
	}

	//매쉬 업데이트
	void UpdateWheelMeshes()
	{
		UpdateWheelPosition(frontLeftWheel, frontLeftMesh);
		UpdateWheelPosition(frontRightWheel, frontRightMesh);
		UpdateWheelPosition(backLeftWheel, backLeftMesh);
		UpdateWheelPosition(backRightWheel, backRightMesh);
	}

	//바퀴 휠 업데이트
	void UpdateWheelPosition(WheelCollider col, Transform mesh)
	{
		Vector3 pos;
		Quaternion rot;
		col.GetWorldPose(out pos, out rot);
		mesh.position = pos;
		mesh.rotation = rot;
	}

	//바퀴로 움직임 관리
	void ApplyAcceleration()
	{
		float motorTorque = moveInput * maxMotorTorque;

		backLeftWheel.motorTorque = motorTorque;
		backRightWheel.motorTorque = motorTorque;
	}

	//앞바퀴 각도 조절
	void ApplySteering()
	{
		float steer = turnInput * maxSteerAngle;

		if (isBoosting)
		{
			steer *= 0.9f; // 부스터 중엔 조향 감소
		}

		frontLeftWheel.steerAngle = steer;
		frontRightWheel.steerAngle = steer;
	}

	//드리프트 시 앞바퀴 각도 조절
	void ApplyDriftSteering()
	{
		if (isDrifting)
		{
			float driftSteer = turnInput * maxSteerAngle * 1.5f;
			frontLeftWheel.steerAngle = driftSteer;
			frontRightWheel.steerAngle = driftSteer;

			transform.Rotate(Vector3.up * turnInput * 0.3f);
		}
	}

	//드리프트 상태 시작
	void StartDrift()
	{
		isDrifting = true;
		AdjustWheelFriction(driftFactor);
	}

	//드리프트 상태 끝내기
	void EndDrift()
	{
		isDrifting = false;
		AdjustWheelFriction(1.5f); // 마찰력 원래대로 복구
	}

	//스키드 상태 조절
	void SetSkid(bool _skid, int index)
	{
		if (isDrifting) skid[index].emitting = _skid;
		else skid[index].emitting = false;
	}

	//휠 마찰력 조절
	void AdjustWheelFriction(float factor)
	{
		WheelFrictionCurve friction = backLeftWheel.sidewaysFriction;
		friction.stiffness = factor;

		backLeftWheel.sidewaysFriction = friction;
		backRightWheel.sidewaysFriction = friction;
	}

	//충돌 방향 감지
	private void OnCollisionEnter(Collision collision)
	{

		ContactPoint contact = collision.contacts[0];

		Vector3 pos = contact.point;

		Vector3 normal = contact.normal;
		Vector3 collisionDirection = -normal;
		Debug.Log($"{collision.gameObject.name}과 충돌함");

		if(Vector3.Dot(collisionDirection, new Vector3(0,0,-1)) > 0.5f)
		{
			Debug.DrawRay(contact.point, normal, Color.red, 2f); // 법선 방향
			Debug.DrawRay(contact.point, collisionDirection, Color.green, 2f); // 충돌 방향
			AdjustSpeed(pos);
		}
		else if(Vector3.Dot(collisionDirection, new Vector3(-1,0,0)) > 0.5f)
		{
			Debug.DrawRay(contact.point, normal, Color.red, 2f); // 법선 방향
			Debug.DrawRay(contact.point, collisionDirection, Color.green, 2f); // 충돌 방향
			AdjustSpeed(pos);
		}
		else if (Vector3.Dot(collisionDirection, new Vector3(1, 0, 0)) > 0.5f)
		{
			Debug.DrawRay(contact.point, normal, Color.red, 2f); // 법선 방향
			Debug.DrawRay(contact.point, collisionDirection, Color.green, 2f); // 충돌 방향
			AdjustSpeed(pos);
		}
		else if (Vector3.Dot(collisionDirection, new Vector3(0, 0, 1)) > 0.5f)
		{
			Debug.DrawRay(contact.point, normal, Color.red, 2f); // 법선 방향
			Debug.DrawRay(contact.point, collisionDirection, Color.green, 2f); // 충돌 방향
			AdjustSpeed(pos, true);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("ItemCube"))
		{
			other.gameObject.SetActive(false);
			ItemLog.SetActive(true);
		}
	}

	//지정 방향으로 속도 조절
	void AdjustSpeed(Vector3 collisionDirection, bool isIncrease = false)
	{

		if (isIncrease)
		{
			rb.AddForce(rb.velocity.normalized * 500f, ForceMode.Impulse);
		}
		else
		{
			var n = Instantiate(HitEffect, collisionDirection, Quaternion.identity);
			rb.AddForce(-rb.velocity.normalized * 500f, ForceMode.Impulse);
			rb.AddForce(-rb.transform.forward * 50f);
			Destroy(n, 0.4f);
		}
	}
	public void ItemSet()
	{
		//GameSystem에 저장된 값에 따라 속도 및 다른 변수 감소
		if (GameSystem.instance.transmission.Equals(GameSystem.Transmission.EnforcedTransmission)) DriftValue = 80f;
		else if (GameSystem.instance.transmission.Equals(GameSystem.Transmission.AutoTransmission)) DriftValue = 75f;
		else DriftValue = 100f;

		if (GameSystem.instance.engine.Equals(GameSystem.Engine._6Engine)) MoveValue = 90f;
		else if (GameSystem.instance.engine.Equals(GameSystem.Engine._8Engine)) MoveValue = 80f;
		else MoveValue = 100f;

		if (GameSystem.instance.wheeltype.Equals(GameSystem.WheelType.Sand) && SceneManager.GetActiveScene().name == "Stage2") MoveSpeed = 110;
		else if (GameSystem.instance.wheeltype.Equals(GameSystem.WheelType.Mountain) && SceneManager.GetActiveScene().name == "Stage1") MoveSpeed = 110;
		else if (GameSystem.instance.wheeltype.Equals(GameSystem.WheelType.Road) && SceneManager.GetActiveScene().name == "Stage3") MoveSpeed = 110;
		else MoveSpeed = 100f;

		GameSystem.instance.SaveStoreData();
		GameSystem.instance.SaveItemData();
	}
}
