using UnityEngine;

public class ItemRot : MonoBehaviour
{
    private float i = 0;
    void Update()
    {
		Vector3 rotation = transform.eulerAngles;
		rotation.y += Time.unscaledDeltaTime * 30f;
        transform.eulerAngles = rotation;
    }
}
