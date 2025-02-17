using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : MonoBehaviour
{
    [SerializeField]
    private GameObject UpArrow;
    [SerializeField]
    private float Duration;

	private bool isMoving = true;
    
    private float ElapsedTime = 0f;
    // Update is called once per frame
    void Start()
    {
		StartCoroutine(Move());
    }
    IEnumerator Move()
    {
		while (isMoving)
		{
			ElapsedTime = 0f;
			float n = UpArrow.transform.position.y;
			while (ElapsedTime < Duration)
			{
				ElapsedTime += Time.deltaTime;
				float t = ElapsedTime / Duration;
				UpArrow.transform.position = Vector3.Lerp(
					UpArrow.transform.position,
					new Vector3(
						UpArrow.transform.position.x, 
						n + 0.3f, 
						UpArrow.transform.position.z),
					t);
				yield return null;
			}
			ElapsedTime = 0f;
			n = UpArrow.transform.position.y;
			while (ElapsedTime < Duration)
			{
				ElapsedTime += Time.deltaTime;
				float t = ElapsedTime / Duration;
				UpArrow.transform.position = Vector3.Lerp(
					UpArrow.transform.position,
					new Vector3(
						UpArrow.transform.position.x, 
						n - 0.3f, 
						UpArrow.transform.position.z),
					t);
				yield return null;
			}
		}
	}
}
