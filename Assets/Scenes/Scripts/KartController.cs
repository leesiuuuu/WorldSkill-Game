using System.Collections;
using UnityEngine;

public class KartController : MonoBehaviour
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

	[Header("Rigidbody Property")]
	public float MoveSpeed;
	public float MAX_SPEED = 15f;
	public float Drag = 0.98f;
	public float SteerAngle = 20;
	public float Traction = 1;
	public float MAX_BOOST = 10;
	public float Gravity;

	private float BoostSpeed = 1f;

	private float moveInput;
	private float turnInput;
	private bool isBoosting = false;
	private bool isDrifting = false;

	private bool isGroundL = false;
	private bool isGroundR = false;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	void Update()
	{
		moveInput = Input.GetAxis("Vertical");   // ↑↓ 입력값
		turnInput = Input.GetAxis("Horizontal"); // ←→ 입력값


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
				boostGauge += Time.deltaTime * backRightWheel.motorTorque / 100;
			}
			else
			{
				if (boostGauge <= 100f)
				{
					boostGauge += Time.deltaTime * backRightWheel.motorTorque / 85;
				}
			}
		}

		if (Input.GetKeyDown(KeyCode.Space))
		{
			rb.velocity = Vector3.zero;
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
		if(moveInput != 0 && boostGauge <= 100) boostGauge += Time.deltaTime * backLeftWheel.motorTorque / 100;
	}
	private void FixedUpdate()
	{
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
	void RigidMovement()
	{
		Vector3 moveForce = transform.forward * moveInput * BoostSpeed * MoveSpeed;
		rb.AddForce(moveForce);

		transform.Rotate(Vector3.up * turnInput * SteerAngle * Time.fixedDeltaTime);

		rb.velocity = Vector3.ClampMagnitude(rb.velocity, MAX_SPEED);

		rb.velocity = Vector3.Lerp(rb.velocity.normalized, transform.forward, Traction * Time.fixedDeltaTime) * rb.velocity.magnitude;

		rb.AddForce(Vector3.down * Gravity, ForceMode.Acceleration);
	}
	IEnumerator Boost()
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

	void UpdateWheelMeshes()
	{
		UpdateWheelPosition(frontLeftWheel, frontLeftMesh);
		UpdateWheelPosition(frontRightWheel, frontRightMesh);
		UpdateWheelPosition(backLeftWheel, backLeftMesh);
		UpdateWheelPosition(backRightWheel, backRightMesh);
	}

	void UpdateWheelPosition(WheelCollider col, Transform mesh)
	{
		Vector3 pos;
		Quaternion rot;
		col.GetWorldPose(out pos, out rot);
		mesh.position = pos;
		mesh.rotation = rot;
	}

	void ApplyAcceleration()
	{
		float motorTorque = moveInput * maxMotorTorque;

		backLeftWheel.motorTorque = motorTorque;
		backRightWheel.motorTorque = motorTorque;
	}

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

	void StartDrift()
	{
		isDrifting = true;
		AdjustWheelFriction(driftFactor);
	}

	void EndDrift()
	{
		isDrifting = false;
		AdjustWheelFriction(1.5f); // 마찰력 원래대로 복구
	}
	void SetSkid(bool _skid, int index)
	{
		if (isDrifting) skid[index].emitting = _skid;
		else skid[index].emitting = false;
	}

	void AdjustWheelFriction(float factor)
	{
		WheelFrictionCurve friction = backLeftWheel.sidewaysFriction;
		friction.stiffness = factor;

		backLeftWheel.sidewaysFriction = friction;
		backRightWheel.sidewaysFriction = friction;
	}
}
