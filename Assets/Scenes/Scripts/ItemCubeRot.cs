using UnityEngine;

public class ItemCubeRot : MonoBehaviour
{
    private float i = 0;
    void Update()
    {
		Vector3 rotation = transform.eulerAngles;
		rotation.y += Time.deltaTime * 150f;
        transform.eulerAngles = rotation;
    }
}
