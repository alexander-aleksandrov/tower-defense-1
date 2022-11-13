using UnityEngine;

public class GameBoard : MonoBehaviour
{
	[SerializeField]
	private Transform _ground;
	[SerializeField]
	private Tile _tilePrefab;

	private Tile[] _tiles;

	private Vector2Int _size;

	public void Initialyze(Vector2Int size)
	{
		_size = size;
		_ground.localScale = new Vector3(size.x, size.y, 1f);

		Vector2 offset = new Vector2((size.x - 1) * 0.5f, (size.y - 1) * 0.5f);

		_tiles = new Tile[size.x * size.y];
		for (int i = 0, y = 0; y < size.y; y++)
		{
			for (int x = 0; x < size.x; x++)
			{
				Tile tile = _tiles[i] = Instantiate(_tilePrefab);
				tile.transform.SetParent(transform, false);
				tile.transform.localPosition = new Vector3(x - offset.x, 0f, y - offset.y);
			}
		}
	}
}
