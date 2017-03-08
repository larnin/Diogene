using UnityEngine;
using System.Collections;

public class MonsterComportement : MonoBehaviour
{
    public float DetectionRadius;
    public Vector3 EndPosition;
    public float Speed;

    bool _started = false;
    bool _ended = false;
    Vector3 _startPosition;
    float _time = 0;
    float _totalTime;
	
    void Start()
    {
        _startPosition = transform.localPosition;
        _totalTime = Mathf.Abs(EndPosition.y - _startPosition.y) / Speed;
    }

	void Update ()
    {
        if (_ended)
            return;
	    if(!_started)
        {
            var collisions = Physics.OverlapSphere(transform.position, DetectionRadius);
            foreach(var c in collisions)
                if(c.gameObject.tag == "Player")
                {
                    _started = true;
                    break;
                }
            return;
        }

        _time += Time.deltaTime;
        if (_time > _totalTime)
            _ended = true;

        var result = Vector3.Slerp(new Vector3(_startPosition.x, 0, _startPosition.z), new Vector3(EndPosition.x, 0, EndPosition.z), _time / _totalTime);
        result.y = Mathf.Lerp(_startPosition.y, EndPosition.y, _time / _totalTime);
        transform.localPosition = result;
	}
}
