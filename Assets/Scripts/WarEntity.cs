public class WarEntity : GameBehavior
{
    private WarFactory _originFactory;
    public WarFactory OriginFactory
    {
        get => _originFactory;
        set { _originFactory = value; }
    }
    public override void Recycle()
    {
        _originFactory.Reclaim(this);
    }
}
