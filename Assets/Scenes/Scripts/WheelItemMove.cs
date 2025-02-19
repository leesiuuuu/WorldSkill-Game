using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WheelItemMove : ItemSelete
{
	public Transform Camera;
	private int MAX = 4;
	private float zPos =1.75f;
	public override void MoveLeft()
	{
		if(index > 0) --index;
		Move();
	}

	public override void MoveRight()
	{
		if (index < MAX-1) ++index;
		Move();
	}
	void Move()
	{
		Camera.position =
			new Vector3(
				Camera.position.x,
				Camera.position.y,
				zPos + (index * -3f) + 7.24f);
	}
}
