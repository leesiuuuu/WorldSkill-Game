using UnityEngine;

public abstract class ItemSelete : MonoBehaviour
{
    protected int index = 0;
    public abstract void MoveLeft();
    public abstract void MoveRight();

    public abstract void SaveIndex();
    public abstract void LoadIndex();

}
