using UnityEngine;

public class CarController : MonoBehaviour
{
    private Vector3 MoveForce;
    public float MoveSpeed;
    public float MAX_SPEED = 15f;
    public float Drag = 0.98f;
    public float SteerAngle = 20;
    public float Traction = 1;
	public float BoostMultiply;
	public float MAX_BOOST = 10;
	public float Gravity;

	private float BoostSpeed = 1f;
    private Rigidbody rb;
	private void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			rb.velocity = Vector3.zero;
		}
		if (Input.GetKey(KeyCode.LeftControl) && BoostSpeed < MAX_BOOST)
		{
			BoostSpeed += BoostMultiply;
		}
	}

	void FixedUpdate()
    {
        float moveInput = Input.GetAxis("Vertical");
        float steerInput = Input.GetAxis("Horizontal");

        Vector3 moveForce = transform.forward * moveInput * BoostSpeed * MoveSpeed;
        rb.AddForce(moveForce);

        transform.Rotate(Vector3.up * steerInput * SteerAngle * Time.fixedDeltaTime);

		rb.velocity = Vector3.ClampMagnitude(rb.velocity, MAX_SPEED);

		rb.velocity = Vector3.Lerp(rb.velocity.normalized, transform.forward, Traction * Time.fixedDeltaTime) * rb.velocity.magnitude;

		rb.AddForce(Vector3.down * Gravity, ForceMode.Acceleration);
	}
}
