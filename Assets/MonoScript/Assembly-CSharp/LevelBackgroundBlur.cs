using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public sealed class LevelBackgroundBlur : MonoBehaviour
{
	private const int MinKeyFrameNum = 2;

	private const string ShaderName = "CustomShader/UI/BackGroudWitMask";

	private const string EnableMaskPropertyName = "_EnableMask";

	private const string MaskIntensityPropertyName = "_Intensity";

	[SerializeField]
	private Material _material;

	[SerializeField]
	private AnimationCurve _dragIntensityCurve;

	[SerializeField]
	private float _time;

	[SerializeField]
	private float _duration;

	public void Enable()
	{
		if (_dragIntensityCurve == null || _dragIntensityCurve.length < 2 || _material == null || !_material.shader.name.Equals("CustomShader/UI/BackGroudWitMask"))
		{
			base.enabled = false;
			return;
		}
		_dragIntensityCurve.postWrapMode = WrapMode.ClampForever;
		_time = _dragIntensityCurve[_dragIntensityCurve.length - 1].time;
		_duration = 0f;
		StopAllCoroutines();
		StartCoroutine(EaseInBlur());
	}

	private void OnDestroy()
	{
		if (_material != null)
		{
			_material.SetInt("_EnableMask", 0);
			_material.SetInt("_EnableMask", 0);
		}
	}

	public void Disable()
	{
		StopAllCoroutines();
		StartCoroutine(EaseOutBlur());
	}

	private IEnumerator EaseInBlur()
	{
		_material.SetInt("_EnableMask", 1);
		while (_duration <= _time)
		{
			_material.SetFloat("_Intensity", _dragIntensityCurve.Evaluate(_duration));
			_duration += Time.deltaTime;
			yield return null;
		}
		_duration = _time;
		_material.SetFloat("_Intensity", _dragIntensityCurve.Evaluate(_duration));
	}

	private IEnumerator EaseOutBlur()
	{
		while (_duration > 0f)
		{
			_material.SetFloat("_Intensity", _dragIntensityCurve.Evaluate(_duration));
			_duration -= Time.deltaTime;
			yield return null;
		}
		_duration = 0f;
		_material.SetFloat("_Intensity", _dragIntensityCurve.Evaluate(_duration));
		_material.SetInt("_EnableMask", 0);
	}
}
