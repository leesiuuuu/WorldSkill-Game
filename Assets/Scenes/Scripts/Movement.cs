using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
	public WheelCollider[] wheels = new WheelCollider[4];
	public Transform[] wheels_transform;

	public float torque = 100;
	public float Power = 2000;
	public float driftDeceleration = 0.90f;

	public CameraFollowPlayer cmf;

	float slipRate = 2f;
	float handBreakSlipRate = 0.1f;

	float _Boost = 1f;

	private WheelFrictionCurve[] Frictions = new WheelFrictionCurve[8];

	public TrailRenderer[] tr;
	public TrailRenderer[] skid;

	float angle = 20;
	private bool isDrifting = false;
	private bool isBoosting = false;

	private int direction;

	private Rigidbody rb;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();

		for (int i = 0; i < 4; i++)
		{
			Frictions[i * 2] = wheels[i].forwardFriction;
			Frictions[i * 2 + 1] = wheels[i].sidewaysFriction;
		}
	}

	private void Update()
	{
		for (int i = 0; i < 4; i++)
		{
			if (isDrifting)
			{
				wheels[i].motorTorque = Input.GetAxis("Vertical") * torque * driftDeceleration * _Boost * Power;
			}
			else
				wheels[i].motorTorque = Input.GetAxis("Vertical") * torque * _Boost * Power;

			float n = Input.GetAxis("Horizontal");
			direction = (int)n;
			if (i == 0 || i == 1)
			{
				wheels[i].steerAngle = n * angle;
			}

			var pos = transform.position;
			var rot = transform.rotation;
			wheels[i].GetWorldPose(out pos, out rot);
			wheels_transform[i].position = pos;
			wheels_transform[i].rotation = rot;
		}

		if (Input.GetKeyDown(KeyCode.Space))
		{
			foreach (var i in wheels)
			{
				i.brakeTorque = 2000000;
			}
		}
		if (Input.GetKeyUp(KeyCode.Space))
		{
			foreach (var i in wheels)
			{
				i.brakeTorque = 0;
			}
		}

		if (Input.GetKeyDown(KeyCode.LeftShift))
		{
			ApplyDrift(true);
		}
		if (Input.GetKeyUp(KeyCode.LeftShift))
		{
			ApplyDrift(false);
		}
		if (Input.GetKeyDown(KeyCode.LeftControl) && !isBoosting)
		{
			StartCoroutine(Boost());
		}
	}

	private void ApplyDrift(bool drifting)
	{
		isDrifting = drifting;

		skid[0].emitting = drifting;
		skid[1].emitting = drifting;

		if(direction != 0)
		{
			if(direction > 0)
			{
				rb.AddForce(Vector3.right * 100f, ForceMode.Impulse);
			}
			if (direction < 0) 
			{
				rb.AddForce(Vector3.left * 100f, ForceMode.Impulse);
			}
		}

		for (int i = 0; i < 4; i++)
		{
			WheelFrictionCurve forwardFriction = wheels[i].forwardFriction;
			WheelFrictionCurve sidewaysFriction = wheels[i].sidewaysFriction;

			if (drifting)
			{
				if (i < 2) // 앞바퀴
				{
					forwardFriction.stiffness = slipRate;
					sidewaysFriction.stiffness = slipRate * 0.6f;
				}
				else // 뒷바퀴
				{
					forwardFriction.stiffness = slipRate;
					sidewaysFriction.stiffness = handBreakSlipRate;
				}
			}
			else // 드리프트 해제 시 원래 값으로 복원
			{
				forwardFriction.stiffness = 1;
				sidewaysFriction.stiffness = 1;
			}

			wheels[i].forwardFriction = forwardFriction;
			wheels[i].sidewaysFriction = sidewaysFriction;
		}

	}

	IEnumerator Boost()
	{
		_Boost = 20f;
		isBoosting = true;

		tr[0].emitting = true;
		tr[1].emitting = true;
		cmf.distance = 12;

		yield return new WaitForSeconds(5f);

		_Boost = 1f;
		isBoosting = false;
		cmf.distance = 8;

		tr[0].emitting = false;
		tr[1].emitting = false;
	}
}
