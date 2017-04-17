using UnityEngine;
using System.Collections;
using TopDown;

public class InteractionLogic : MonoBehaviour
{

    [SerializeField]
    GameObject _interactionCanvas;

    Pawn _player;
    IUseable _oldUseableObject;
    public IUseable UseableObject { get; private set; }

    GameObject _canvasInstance;
    CanvasGroup _canvasGroup;

    void Start()
    {
        _player = GetComponentInParent<Pawn>();
        if (_interactionCanvas)
        {
            _canvasInstance = Instantiate(_interactionCanvas);
            _canvasGroup = _canvasInstance.GetComponentInChildren<CanvasGroup>();

            if (!_canvasGroup)
                Debug.LogError("Can't fade InteractionCanvas, needs a CanvasGroup");

            if (_canvasGroup)
                _canvasGroup.alpha = 0;
            //_interactionCanvasInstance.SetActive(false);
        }
    }

    void OnDisable()
    {
        _oldUseableObject = null;
        UseableObject = null;

        if (_canvasInstance)
        {
            if (_canvasGroup)
                _canvasGroup.alpha = 0;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        IUseable tempUseable = GetUseableObject(other.gameObject);
        if (tempUseable != null && _oldUseableObject != tempUseable && tempUseable.CanBeUsed(_player.gameObject))
        {
            _oldUseableObject = tempUseable;
            UseableObject = tempUseable;

            if (_canvasInstance)
            {
                _canvasInstance.transform.SetParent(other.transform);
                _canvasInstance.transform.position = other.transform.position;
                if (_canvasGroup)
                    _canvasGroup.alpha = 1;
            }

        }
    }

    void OnTriggerExit(Collider other)
    {
        IUseable tempUseable = GetUseableObject(other.gameObject);
        if (tempUseable != null && _oldUseableObject == tempUseable)
        {
            _oldUseableObject = null;
            UseableObject = null;

            if (_canvasInstance)
            {
                _canvasInstance.transform.SetParent(null);
                _canvasInstance.transform.position = Vector3.zero;

                if (_canvasGroup)
                    _canvasGroup.alpha = 0;
            }
        }
    }


    private IUseable GetUseableObject(GameObject go)
    {
        IUseable tempUseable = (IUseable)go.gameObject.GetComponentInChildren(typeof(IUseable));

        // if it isnt in the children, maybe in the parnet?
        if (tempUseable == null)
            tempUseable = (IUseable)go.gameObject.GetComponentInParent(typeof(IUseable));

        return tempUseable;

    }

}
