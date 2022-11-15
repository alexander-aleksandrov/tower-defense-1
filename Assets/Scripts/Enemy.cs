using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyFactory OrigignFactory { get; set; }

    public void SpawnOn(Tile tile)
    {
        transform.localPosition = tile.transform.localPosition;
    }
}
