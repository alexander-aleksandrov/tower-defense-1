using UnityEngine;

public class TileContent : MonoBehaviour
{
    [SerializeField]
    private TileContentType _type;
    public TileContentType Type => _type;

    public TileContentFactory OriginFactory { get; set; }

    public void Recycle()
    {
        OriginFactory.Reclaim(this);
    }
}

public enum TileContentType
{
    Empty,
    Destination,
    Wall
}