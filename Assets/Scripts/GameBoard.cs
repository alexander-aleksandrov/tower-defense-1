using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
	[SerializeField]
	private Transform _ground;
	[SerializeField]
	private Tile _tilePrefab;

	private Tile[] _tiles;

	private Vector2Int _size;

	private Queue<Tile> _searchFrontier = new Queue<Tile>();
	private TileContentFactory _contentFactory;
	private List<Tile> _spawPoints = new List<Tile>();
	private List<TileContent> _updatingContent = new List<TileContent>();

	public int SpawnPointCount => _spawPoints.Count;

	public void GameUpdate()
	{
		for (int i = 0; i < _updatingContent.Count; i++)
		{
			_updatingContent[i].GameUpdate();
		}
	}

	public void Initialyze(Vector2Int size, TileContentFactory contentFactory)
	{
		_size = size;
		_ground.localScale = new Vector3(size.x, size.y, 1f);

		Vector2 offset = new Vector2((size.x - 1) * 0.5f, (size.y - 1) * 0.5f);

		_tiles = new Tile[size.x * size.y];
		_contentFactory = contentFactory;
		for (int i = 0, y = 0; y < size.y; y++)
		{
			for (int x = 0; x < size.x; x++, i++)
			{
				Tile tile = _tiles[i] = Instantiate(_tilePrefab);
				tile.transform.SetParent(transform, false);
				tile.transform.localPosition = new Vector3(x - offset.x, 0f, y - offset.y);

				if (x > 0)
				{
					Tile.MakeEastWestNeighbors(tile, _tiles[i - 1]);
				}
				if (y > 0)
				{
					Tile.MakeNorthSouthNeighbors(tile, _tiles[i - size.x]);
				}

				tile.IsAlternative = (x & 1) == 0;
				if ((y & 1) == 0)
				{
					tile.IsAlternative = !tile.IsAlternative;
				}
				tile.Content = _contentFactory.Get(TileContentType.Empty);
			}
		}
		ToggleDestination(_tiles[_tiles.Length / 2]);
		ToggleSpawnPoint(_tiles[0]);
	}

	public bool FindPath()
	{
		foreach (var t in _tiles)
		{
			if (t.Content.Type == TileContentType.Destination)
			{
				t.BecomeDestination();
				_searchFrontier.Enqueue(t);
			}
			else
			{
				t.ClearPath();
			}
		}

		if (_searchFrontier.Count == 0)
		{
			return false;
		}

		while (_searchFrontier.Count > 0)
		{
			Tile tile = _searchFrontier.Dequeue();
			if (tile != null)
			{
				if (tile.IsAlternative)
				{
					_searchFrontier.Enqueue(tile.GrowPathNorth());
					_searchFrontier.Enqueue(tile.GrowPathSouth());
					_searchFrontier.Enqueue(tile.GrowPathEast());
					_searchFrontier.Enqueue(tile.GrowPathWest());
				}
				else
				{
					_searchFrontier.Enqueue(tile.GrowPathWest());
					_searchFrontier.Enqueue(tile.GrowPathEast());
					_searchFrontier.Enqueue(tile.GrowPathSouth());
					_searchFrontier.Enqueue(tile.GrowPathNorth());
				}
			}

		}

		foreach (var t in _tiles)
		{
			if (!t.HasPath)
			{
				return false;
			}
		}

		foreach (var tile in _tiles)
		{
			tile.ShowPath();
		}

		return true;
	}

	public void ToggleDestination(Tile tile)
	{
		if (tile.Content.Type == TileContentType.Destination)
		{
			tile.Content = _contentFactory.Get(TileContentType.Empty);
			if (!FindPath())
			{
				tile.Content = _contentFactory.Get(TileContentType.Destination);
				FindPath();
			}
			FindPath();
		}
		else if (tile.Content.Type == TileContentType.Empty)
		{
			tile.Content = _contentFactory.Get(TileContentType.Destination);
			FindPath();
		}
	}

	public void ToggleWall(Tile tile)
	{
		if (tile.Content.Type == TileContentType.Wall)
		{
			tile.Content = _contentFactory.Get(TileContentType.Empty);
			if (!FindPath())
			{
				tile.Content = _contentFactory.Get(TileContentType.Wall);
				FindPath();
			}
			FindPath();
		}
		else if (tile.Content.Type == TileContentType.Empty)
		{
			tile.Content = _contentFactory.Get(TileContentType.Wall);
			if (!FindPath())
			{
				tile.Content = _contentFactory.Get(TileContentType.Empty);
				FindPath();
			}
		}
	}

	public void ToggleTower(Tile tile)
	{
		if (tile.Content.Type == TileContentType.Tower)
		{
			_updatingContent.Remove(tile.Content);
			tile.Content = _contentFactory.Get(TileContentType.Empty);
			FindPath();
		}
		else if (tile.Content.Type == TileContentType.Empty)
		{
			tile.Content = _contentFactory.Get(TileContentType.Tower);
			if (FindPath())
			{
				_updatingContent.Add(tile.Content);
			}
			else
			{
				tile.Content = _contentFactory.Get(TileContentType.Empty);
				FindPath();
			}
		}
		else if (tile.Content.Type == TileContentType.Wall)
		{
			tile.Content = _contentFactory.Get(TileContentType.Tower);
			_updatingContent.Add(tile.Content);
		}
	}

	public void ToggleSpawnPoint(Tile tile)
	{
		if (tile.Content.Type == TileContentType.SpawnPoint)
		{
			if (_spawPoints.Count > 1)
			{
				_spawPoints.Remove(tile);
				tile.Content = _contentFactory.Get(TileContentType.Empty);
			}
		}
		else if (tile.Content.Type == TileContentType.Empty)
		{
			tile.Content = _contentFactory.Get(TileContentType.SpawnPoint);
			_spawPoints.Add(tile);
		}

	}

	public Tile GetTile(Ray ray)
	{
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, float.MaxValue, 1))
		{
			int x = (int)(hit.point.x + _size.x * 0.5f);
			int y = (int)(hit.point.z + _size.y * 0.5f);
			if (x >= 0 && x < _size.x && y >= 0 && y < _size.y)
			{
				return _tiles[x + y * _size.x];
			}
		}
		return null;
	}

	public Tile GetSpawnPoint(int index)
	{
		return _spawPoints[index];
	}
}
