using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineItemMove : ItemSelete
{
	public Transform Camera;
	private int MAX = 3;
	private float zPos = 3.75f;
	public override void MoveLeft()
	{
		if (index > 0) --index;
		Move();
	}

	public override void MoveRight()
	{
		if (index < MAX - 1) ++index;
		Move();
	}
	void Move()
	{
		Camera.position =
			new Vector3(
				Camera.position.x,
				Camera.position.y,
				zPos + (index * -3f) + 5.28f);
	}
}
