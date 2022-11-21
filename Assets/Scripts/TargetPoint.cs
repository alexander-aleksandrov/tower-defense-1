using UnityEngine;

public class TargetPoint : MonoBehaviour
{
    const int enemyLayerMask = 1 << 6;
    public Enemy Enemy { get; private set; }
    public float ColliderSize { get; private set; }
    public Vector3 Position => transform.position;

    static Collider[] _targetsBuffer = new Collider[100];
    public static int BufferedCount { get; private set; }

    public static bool FillBuffer(Vector3 position, float range)
    {
        Vector3 top = position;
        top.y += 3f;
        BufferedCount = Physics.OverlapCapsuleNonAlloc(position, top, range, _targetsBuffer, enemyLayerMask);
        return BufferedCount > 0;
    }
    public static TargetPoint GetBuffered(int index)
    {
        var target = _targetsBuffer[index].GetComponent<TargetPoint>();
        return target;
    }

    public static TargetPoint GetRandomTarget => GetBuffered(Random.Range(0, BufferedCount));

    private void Awake()
    {
        Enemy = transform.root.GetComponent<Enemy>();
        ColliderSize = GetComponent<SphereCollider>().radius * transform.localScale.x;
        Debug.Assert(gameObject.layer == 6, "Target Point on wrong Layer", this);
    }
}
