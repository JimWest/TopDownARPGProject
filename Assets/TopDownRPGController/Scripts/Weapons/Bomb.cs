using UnityEngine;
using System.Collections;

namespace TopDown
{
    public class Bomb : MonoBehaviour
    {
        [SerializeField]
        float _timer = 5f;
        [SerializeField]
        GameObject _explodeEffect;
        [SerializeField]
        Color _glowColor = Color.red;

        protected Renderer _mainRenderer;
        protected Color _oldColor;
        protected Color _newColor;

        // Use this for initialization
        void Start()
        {
            _mainRenderer = GetComponentInChildren<Renderer>();
            _oldColor = _mainRenderer.material.color;
            _newColor = _oldColor;
            StartCoroutine("StartTimer");
            StartCoroutine("ChangeColor");
        }

        // Update is called once per frame
        void Update()
        {
            Color lerpedColor = Color.Lerp(_mainRenderer.material.color,
                        _newColor, Time.deltaTime * 5);

            _mainRenderer.material.color = lerpedColor;
        }

        IEnumerator StartTimer()
        {
            for (float i = 0; i < _timer; i += Time.deltaTime)
                yield return 0;

            if (_explodeEffect)
                Instantiate(_explodeEffect, transform.position, transform.rotation);

            DestroyObject(this.gameObject);
        }

        IEnumerator ChangeColor()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.5f);
                _newColor = _glowColor;
                yield return new WaitForSeconds(0.5f);
                _newColor = _oldColor;
            }
        }
    }
}
