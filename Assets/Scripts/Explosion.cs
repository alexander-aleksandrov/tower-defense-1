using UnityEngine;

public class Explosion : WarEntity
{
    [SerializeField, Range(0f, 1f)]
    private float _duration = 0.5f;
    private float _age;
    [SerializeField]
    private AnimationCurve _opacity;
    [SerializeField]
    private AnimationCurve _scaleCurve;

    //Explosion animation
    private static int _colorPropertyID = Shader.PropertyToID("_Color");
    private static MaterialPropertyBlock _propertyBlock;
    private float _scale;
    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    public void Initialize(Vector3 position, float blastRadius, float damage)
    {
        if (damage > 0)
        {
            TargetPoint.FillBuffer(position, blastRadius);
            for (int i = 0; i < TargetPoint.BufferedCount; i++)
            {
                TargetPoint.GetBuffered(i).Enemy.TakeDamage(damage);
            }

        }
        transform.localPosition = position;
        _scale = 2f * blastRadius;
    }

    public override bool GameUpdate()
    {
        _age += Time.deltaTime;
        if (_age >= _duration)
        {
            OriginFactory.Reclaim(this);
            return false;
        }
        if (_propertyBlock == null)
        {
            _propertyBlock = new MaterialPropertyBlock();
        }
        float t = _age / _duration;
        Color c = Color.yellow;
        c.a = _opacity.Evaluate(t);
        _propertyBlock.SetColor(_colorPropertyID, c);
        _meshRenderer.SetPropertyBlock(_propertyBlock);
        transform.localScale = Vector3.one * (_scale * _scaleCurve.Evaluate(t));
        return true;
    }
}
