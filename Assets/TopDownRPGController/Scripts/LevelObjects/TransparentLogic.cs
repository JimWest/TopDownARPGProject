using UnityEngine;
using System.Collections;

public class TransparentLogic : MonoBehaviour {
    	
	Renderer _renderer;
	float _oldMode;
	int _oldSrcBlend;
	int _oldDstBlend;
	int _oldZWrite;
	int _renderQueue;
	bool _alphatest;
	bool _alphablend;
	bool _alphapremultiply;
    bool _fadedOut = false;

    void Start () {
		_renderer = GetComponentInChildren<Renderer>();
		_oldMode = _renderer.material.GetFloat ("_Mode");
		_oldSrcBlend = _renderer.material.GetInt ("_SrcBlend");
		_oldDstBlend = _renderer.material.GetInt ("_DstBlend");
		_oldZWrite = _renderer.material.GetInt ("_ZWrite");
		_renderQueue = _renderer.material.renderQueue;

		_alphatest = _renderer.material.IsKeywordEnabled ("_ALPHATEST_ON");
		_alphablend = _renderer.material.IsKeywordEnabled ("_ALPHABLEND_ON");
		_alphapremultiply = _renderer.material.IsKeywordEnabled ("_ALPHAPREMULTIPLY_ON");
	}

	
	public void FadeOut()
	{
		if (!_fadedOut) {
			StartCoroutine ("FadeOutCoroutine");	
			_fadedOut = true;
		}
	}

	public void FadeIn()
	{
		if (_fadedOut) {
			StartCoroutine ("FadeInCoroutine");	
			_fadedOut = false;
		}
	}

	IEnumerator FadeOutCoroutine() {
		_renderer.material.SetFloat("_Mode", 2);
		_renderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
		_renderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
		_renderer.material.SetInt("_ZWrite", 0);

		_renderer.material.DisableKeyword("_ALPHATEST_ON");
		_renderer.material.EnableKeyword("_ALPHABLEND_ON");
		_renderer.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");

		_renderer.material.renderQueue = 3000;
		
		for (float f = 1f; f >= 0; f -= 0.1f) {
			Color c = _renderer.material.color;
			c.a = f;
			_renderer.material.color = c;
			yield return null;
		}
	}

	IEnumerator FadeInCoroutine() {
		_renderer.material.SetFloat("_Mode", _oldMode);
		_renderer.material.SetInt("_SrcBlend", _oldSrcBlend);
		_renderer.material.SetInt("_DstBlend", _oldDstBlend);
		_renderer.material.SetInt("_ZWrite", _oldZWrite);

		if (_alphatest) {
			_renderer.material.EnableKeyword ("_ALPHATEST_ON");
		} else {
			_renderer.material.DisableKeyword ("_ALPHATEST_ON");
		}

		if (_alphablend) {
			_renderer.material.EnableKeyword ("_ALPHATEST_ON");
		} else {
			_renderer.material.DisableKeyword ("_ALPHATEST_ON");
		}

		if (_alphapremultiply) {
			_renderer.material.EnableKeyword("_ALPHATEST_ON");
		} else {
			_renderer.material.DisableKeyword("_ALPHATEST_ON");
		}

		_renderer.material.renderQueue = _renderQueue;
		
		for (float f = 0f; f <= 1.0f; f += 0.1f) {
			Color c = _renderer.material.color;
			c.a = f;
			_renderer.material.color = c;
			yield return null;
		}
	}

}
