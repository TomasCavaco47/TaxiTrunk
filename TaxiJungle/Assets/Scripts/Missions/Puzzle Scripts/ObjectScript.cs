using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ObjectScript : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private int _width, _height;
    [SerializeField] private int _size;
    Vector2Int _itemSize;
    private int _baseSize=150;
    RectTransform _rectTransform;
    CanvasGroup _canvasGroup;
    Canvas _canvas;
    Vector2 _initialPos;
    [SerializeField] List<Slots> _currentSlot ;
    [SerializeField] Slots _oldSlot;
    [SerializeField]private RectTransform _pointToDrag;

    public Vector2Int ItemSize { get => _itemSize; set => _itemSize = value; }
    public Vector2 InitialPos { get => _initialPos; set => _initialPos = value; }
    public List<Slots> CurrentSlot { get => _currentSlot; set => _currentSlot = value; }
    public Slots OldSlot { get => _oldSlot; set => _oldSlot = value; }
    public int Width { get => _width; set => _width = value; }
    public int Height { get => _height; set => _height = value; }
    public int Size { get => _size; set => _size = value; }

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvas = GetComponentInParent<Canvas>();
        //_pointToDrag = GetComponentInChildren<RectTransform>();    
        _rectTransform.sizeDelta = new Vector2(150 * Width, 150 * Height);
        ItemSize = new Vector2Int(Width, Height);
    }
    void Start()
    {
       
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        _canvasGroup.alpha = .6f;
        _canvasGroup.blocksRaycasts = false;
        InitialPos =_rectTransform.anchoredPosition;
        gameObject.transform.position =Input.mousePosition;
        //_slotManager.CurrentMovingItem = gameObject;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.alpha = 1f;

        _canvasGroup.blocksRaycasts = true;
        if (eventData.pointerCurrentRaycast.gameObject)
        {
            if (eventData.pointerCurrentRaycast.gameObject.CompareTag("Slots"))
            {

            }
           if (eventData.pointerCurrentRaycast.gameObject.CompareTag("Object"))
            {
                _rectTransform.anchoredPosition = _initialPos;

            }

        }
        else
        {
            for (int i = 0; i < _currentSlot.Count; i++)
            {
                _currentSlot[i].Ocupied = false;

            }
            _currentSlot.Clear();
            Debug.Log("fdsfdsfdsa");

        }
    }
}
