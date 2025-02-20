using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierFollow : MonoBehaviour
{
    [SerializeField]
    private Transform[] routes;

    private int routeToGo;

    private float tParam;

    private Vector3 targetPosition;
    [SerializeField]
    private float speedModifier;

    private bool coroutineAllowed;

    private Rigidbody rb;

	public float Gravity;

	private void Start()
	{
        rb = GetComponent<Rigidbody>();
		routeToGo = 0;
        tParam = 0f;
        coroutineAllowed = true;
	}

	private void Update()
	{
        if (coroutineAllowed && routeToGo + 1 <= routes.Length) StartCoroutine(GoByTheRoute(routeToGo));
        else GetComponent<BezierFollow>().enabled = false;
	}
	private void FixedUpdate()
	{
		rb.AddForce(Vector3.down * Gravity, ForceMode.Acceleration);
	}

	private IEnumerator GoByTheRoute(int routeNumber)
    {
        coroutineAllowed = false;

        Vector3 p0 = routes[routeNumber].GetChild(0).position;
        Vector3 p1 = routes[routeNumber].GetChild(1).position;
        Vector3 p2 = routes[routeNumber].GetChild(2).position;
        Vector3 p3 = routes[routeNumber].GetChild(3).position;

        while(tParam < 1)
        {
            tParam += Time.deltaTime * speedModifier;

            targetPosition = Mathf.Pow(1 - tParam, 3) * p0 +
                3 * Mathf.Pow(1 - tParam, 2) * tParam * p1 +
                3 * (1 - tParam) * Mathf.Pow(tParam, 2) * p2 +
                Mathf.Pow(tParam, 3) * p3;

            float nextTParam = Mathf.Min(tParam + 0.05f, 1);

			Vector3 NexttargetPosition = Mathf.Pow(1 - nextTParam, 3) * p0 +
	            3 * Mathf.Pow(1 - nextTParam, 2) * nextTParam * p1 +
	            3 * (1 - nextTParam) * Mathf.Pow(nextTParam, 2) * p2 +
	            Mathf.Pow(nextTParam, 3) * p3;

            Vector3 direction = (NexttargetPosition - targetPosition).normalized;

			Quaternion targetRot = Quaternion.LookRotation(direction);
			Quaternion smoothedRotation = Quaternion.Slerp(rb.rotation, targetRot, Time.deltaTime * speedModifier * 50000f);
			rb.MoveRotation(smoothedRotation);

			Vector3 newPos = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);

            rb.MovePosition(newPos);
            yield return new WaitForFixedUpdate();
        }

        tParam = 0f;

        routeToGo += 1;

        coroutineAllowed = true;
    }
}
