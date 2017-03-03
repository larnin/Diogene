using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChunckSpawner : MonoBehaviour
{
    public class ChunkInfo
    {
        public int holesCount = 0;
        public int ringCount = 0;
        public int trapCount = 0;
        public int armCount = 0;
    }

    public GameObject startChunkPrefab;
    public List<GameObject> chunkPrefabs;
    public float distanceToDelChunk;
    public float distanceToLoadChunk;
    public int dontRepeatCount;

    private SubscriberList _subscriberList = new SubscriberList();
    private List<ChunkData> _chunkDatas = new List<ChunkData>();
    private List<Chunk> _chunks = new List<Chunk>();
    private List<int> _lastChunckSpawn = new List<int>();
	private bool _initialized = false;
    private float _currentHeight;

    void Awake()
    {
        _subscriberList.Add(new Event<CameraMovedEvent>.Subscriber(OnCameraMove));
        _subscriberList.Add(new Event<InitializeEvent>.Subscriber(OnInitialize));
		_subscriberList.Add(new Event<ResetEvent>.Subscriber(OnReset));
        _subscriberList.Subscribe();

        CreateChunkData();
    }

    void OnCameraMove(CameraMovedEvent e)
    {
		if (!_initialized)
			return;

        _currentHeight = e.pos.y;
		
        while (_chunks[_chunks.Count - 1].gameObject.transform.position.y - _chunks[_chunks.Count - 1].datas.height > e.pos.y - distanceToLoadChunk)
            addChunk();
        while (_chunks[0].gameObject.transform.position.y - _chunks[0].datas.height > e.pos.y + distanceToLoadChunk)
            delChunk();
    }

    void OnInitialize(InitializeEvent e)
    {
		_initialized = true;
        var data = GetDataFromChunk(startChunkPrefab);

        var chunk = Instantiate(startChunkPrefab);
        chunk.transform.position = new Vector3(0, chunk.transform.FindChild("Start").transform.position.y, 0);
        chunk.transform.Rotate(0, -data.startRotation, 0);

        _chunks.Add(new Chunk(chunk, data, -data.startRotation, false));

        while (_chunks[_chunks.Count - 1].gameObject.transform.position.y - _chunks[_chunks.Count - 1].datas.height > e.pos.y - distanceToLoadChunk)
            addChunk();
    }

	void OnReset(ResetEvent e)
	{
		foreach(var chunk in _chunks)
			Destroy(chunk.gameObject);
		_chunks.Clear ();
		_initialized = false;
	}

    void CreateChunkData()
    {
        _chunkDatas.Clear();

        foreach (var chunk in chunkPrefabs)
            _chunkDatas.Add(GetDataFromChunk(chunk));
    }

    ChunkData GetDataFromChunk(GameObject o)
    {
        var start = o.transform.FindChild("Start");
        var end = o.transform.FindChild("End");
        if (start == null)
            Debug.Log("Can't find the start point !");
        if (end == null)
            Debug.Log("Can't find the end point !");
        if (start != null && end != null)
        {
            var endProprieties = end.GetComponent<ChunkEndProprieties>();
            bool endFliped = endProprieties == null ? false : endProprieties.reversed;
            return new ChunkData(Mathf.Abs(end.position.y - start.position.y), endFliped
                          , start.transform.localRotation.eulerAngles.y
                          , end.transform.localRotation.eulerAngles.y);
        }
        return null;
    }

    void addChunk()
    {
        int index = 0;
        for(int i = 0; i < 10; i++)
        {
            index = G.Sys.random.Next(chunkPrefabs.Count);
            bool repeated = false;
            for(int j = Mathf.Max(0, _lastChunckSpawn.Count - dontRepeatCount); j < _lastChunckSpawn.Count; j++)
                if(_lastChunckSpawn[j] == index)
                {
                    repeated = true;
                    break;
                }
            if (!repeated)
                break;
        }
        _lastChunckSpawn.Add(index);
        var o = Instantiate(chunkPrefabs[index]);
        var currentChunk = _chunks[_chunks.Count - 1];
        float rotation = currentChunk.blockRotation + currentChunk.datas.endRotation - _chunkDatas[index].startRotation;
        bool fliped = (currentChunk.fliped != currentChunk.datas.endFliped);
        if (fliped)
        {
            o.transform.localScale = new Vector3(1, 1, -1);
            rotation += 2 * _chunkDatas[index].startRotation;
        }

        if(currentChunk.fliped)
        {
            rotation -= 2 * _chunkDatas[index].endRotation;
        }

        o.transform.position = new Vector3(0, currentChunk.gameObject.transform.position.y - currentChunk.datas.height, 0);
        o.transform.Rotate(0, rotation, 0);
        _chunks.Add(new Chunk(o, _chunkDatas[index], rotation, fliped));
    }

    void delChunk()
    {
        if (_chunks.Count == 0)
            return;
        Destroy(_chunks[0].gameObject);
        _chunks.RemoveAt(0);
    }

    private class ChunkData
    {
        public ChunkData()
        {
            height = 0;
            endFliped = false;
            startRotation = 0;
            endRotation = 0;
        }

        public ChunkData(float _height, bool _endFliped, float _startRotation, float _endRotation)
        {
            height = _height;
            endFliped = _endFliped;
            startRotation = _startRotation;
            endRotation = _endRotation;
        }

        public readonly float height;
        public readonly bool endFliped;
        public readonly float startRotation;
        public readonly float endRotation;
    }

    private class Chunk
    {
        public Chunk(GameObject _gameObject, ChunkData _datas, float _blockRotation, bool _fliped)
        {
            gameObject = _gameObject;
            datas = _datas;
            blockRotation = _blockRotation;
            fliped = _fliped;
        }

        public GameObject gameObject;
        public ChunkData datas;
        public float blockRotation;
        public bool fliped;
    }

    public int chunkCount()
    {
        int count = 0;
        float currentHeight = 0;
        foreach (var value in _lastChunckSpawn)
        {
            currentHeight += _chunkDatas[value].height;
            count++;
            if (currentHeight >= _currentHeight)
                return count;
        }
        return _lastChunckSpawn.Count - 1;
    }

    public int currentChunkID()
    {
        return _lastChunckSpawn[chunkCount()];
    }

    public ChunkInfo allDatas()
    {
        ChunkInfo datas = new ChunkInfo();

        for(int i = 0; i < chunkCount(); i++)
        {
            var infos = chunkPrefabs[_lastChunckSpawn[i]].GetComponent<ChunkInfos>();
            if(infos != null)
            {
                datas.armCount += infos.armCount;
                datas.holesCount += infos.holesCount;
                datas.ringCount += infos.ringCount;
                datas.trapCount += infos.trapCount;
            }
        }
        return datas;
    }
}
