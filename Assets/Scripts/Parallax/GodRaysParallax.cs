using UnityEngine;
using System.Collections;
using DG.Tweening;

public class GodRaysParallax : MonoBehaviour {

	public float Speed = 0;
	public float FadeInDuration = 0.5f;
	public float ActiveDuration = 1.5f;
	public float FadeOutDuration = 0.5f;
	public float MinWaitBetweenRays = 0.5f;
	public float MaxWaitBetweenRays = 15;

	public Material _material;
	Color _newColor;
	
	void Start () {
		Color _newColor = _material.color;
		_newColor.a = 0;
		_material.color = _newColor;
		StartCoroutine (RandomAppearance ());
	}

	IEnumerator Fade()
	{
		float value = 0;

		while(value < 1)
		{
			yield return new WaitForFixedUpdate ();
			value += Time.deltaTime * (1 / FadeInDuration);
			_newColor = _material.color;
			_newColor.a = value;
			_material.color = _newColor;
		}
		_newColor = _material.color;
		_newColor.a = 1;
		_material.color = _newColor;

		yield return new WaitForSeconds (ActiveDuration);

		while(value > 0)
		{
			yield return new WaitForFixedUpdate ();
			value -= Time.deltaTime * (1 / FadeOutDuration);
			_newColor = _material.color;
			_newColor.a = value;
			_material.color = _newColor;
		}
		_newColor = _material.color;
		_newColor.a = 0;
		_material.color = _newColor;

		StartCoroutine (RandomAppearance ());
	}

	IEnumerator RandomAppearance () {
		float time = MinWaitBetweenRays + ((float)G.Sys.random.NextDouble () * (MaxWaitBetweenRays - MinWaitBetweenRays));
		yield return new WaitForSeconds (time);
		StartCoroutine (Fade ());
	}
}
