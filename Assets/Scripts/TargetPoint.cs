using UnityEngine;

public class TargetPoint : MonoBehaviour
{
    public Enemy Enemy { get; private set; }
    public float ColliderSize { get; private set; }
    public Vector3 Position => transform.position;

    private void Awake()
    {
        Enemy = transform.root.GetComponent<Enemy>();
        ColliderSize = GetComponent<SphereCollider>().radius * transform.localScale.x;
        Debug.Assert(gameObject.layer == 6, "Target Point on wrong Layer", this);
    }
}
