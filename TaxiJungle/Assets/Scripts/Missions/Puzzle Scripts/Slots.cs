using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slots : MonoBehaviour,IPointerEnterHandler,IDropHandler, IPointerExitHandler
{
    Vector2Int _slotArray;
    [SerializeField] bool _ocupied;
    [SerializeField] private GameObject[,] _passObjectArr;
   [SerializeField] private GameObject _storedItem;



    public Vector2Int SlotArray { get => _slotArray; set => _slotArray = value; }
    public bool Ocupied { get => _ocupied; set => _ocupied = value; }
    public GameObject[,] PassObjectArr { get => _passObjectArr; set => _passObjectArr = value; }
    public GameObject StoredItem { get => _storedItem; set => _storedItem = value; }


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {

            ObjectScript itemOject = eventData.pointerDrag.GetComponent<ObjectScript>();
            int ocupiedByOther = 0;
            bool passedTheArray = false;
            if (itemOject.ItemSize.x > 0 || itemOject.ItemSize.y > 0)
            {
                for (int x = 0; x < itemOject.ItemSize.x; x++)
                {
                    for (int y = 0; y < itemOject.ItemSize.y; y++)
                    {
                        Vector2Int possitionToOcupie = new Vector2Int(_slotArray.x + x, _slotArray.y + y);
                        if (possitionToOcupie.x < _passObjectArr.GetLength(0)  && possitionToOcupie.y < _passObjectArr.GetLength(1) )
                        {
                            if (_passObjectArr[possitionToOcupie.x, possitionToOcupie.y].GetComponent<Slots>().Ocupied == true)
                            {
                                Debug.Log("1");

                                if (_passObjectArr[possitionToOcupie.x, possitionToOcupie.y].GetComponent<Slots>().StoredItem == itemOject.gameObject)
                                {
                                    Debug.Log("2");

                                    _passObjectArr[possitionToOcupie.x, possitionToOcupie.y].GetComponent<Image>().color = Color.yellow;

                                }
                                else
                                {
                                    Debug.Log("3");
                                    ocupiedByOther++;
                                    _passObjectArr[possitionToOcupie.x, possitionToOcupie.y].GetComponent<Image>().color = Color.red;
                                }
                            }
                            else
                            {
                                _passObjectArr[possitionToOcupie.x, possitionToOcupie.y].GetComponent<Image>().color = Color.green;
                            }
                        }
                        else
                        {
                            passedTheArray = true;
                        }                  
                    }
                }
                for (int x = 0; x < itemOject.ItemSize.x; x++)
                {
                    for (int y = 0; y < itemOject.ItemSize.y; y++)
                    {
                        Vector2Int possitionToOcupie = new Vector2Int(_slotArray.x + x, _slotArray.y + y);
                        if (possitionToOcupie.x < _passObjectArr.GetLength(0) && possitionToOcupie.y < _passObjectArr.GetLength(1))
                        {
                            if(passedTheArray==true)
                            {
                                _passObjectArr[possitionToOcupie.x, possitionToOcupie.y].GetComponent<Image>().color = Color.red;

                            }
                            else if (ocupiedByOther>0)
                                {
                                    _passObjectArr[possitionToOcupie.x, possitionToOcupie.y].GetComponent<Image>().color = Color.red;

                                }
                               else
                                {

                                    if (_passObjectArr[possitionToOcupie.x, possitionToOcupie.y].GetComponent<Slots>().StoredItem == itemOject.gameObject)
                                    {
                                        Debug.Log("2");

                                        _passObjectArr[possitionToOcupie.x, possitionToOcupie.y].GetComponent<Image>().color = Color.yellow;

                                    }
                                    else
                                    {
                                            _passObjectArr[possitionToOcupie.x, possitionToOcupie.y].GetComponent<Image>().color = Color.green;

                                    }

                               }                         
                        }                       
                    }
                }
            }
        }   
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {

            ObjectScript itemOject = eventData.pointerDrag.GetComponent<ObjectScript>();

            if (itemOject.ItemSize.x > 0 || itemOject.ItemSize.y > 0)
            {
                for (int x = 0; x < itemOject.ItemSize.x; x++)
                {
                    for (int y = 0; y < itemOject.ItemSize.y; y++)
                    {
                        Vector2Int possitionToOcupie = new Vector2Int(_slotArray.x + x, _slotArray.y + y);
                        if (possitionToOcupie.x < _passObjectArr.GetLength(0) && possitionToOcupie.y < _passObjectArr.GetLength(1))
                        {
                            if (_passObjectArr[possitionToOcupie.x, possitionToOcupie.y].GetComponent<Slots>().Ocupied == true)
                            {

                                if (_passObjectArr[possitionToOcupie.x, possitionToOcupie.y].GetComponent<Slots>().StoredItem == itemOject.gameObject)
                                {
                                    _passObjectArr[possitionToOcupie.x, possitionToOcupie.y].GetComponent<Image>().color = Color.white;

                                }
                                else
                                {
                                    _passObjectArr[possitionToOcupie.x, possitionToOcupie.y].GetComponent<Image>().color = Color.white;
                                }
                            }
                            else
                            {
                                _passObjectArr[possitionToOcupie.x, possitionToOcupie.y].GetComponent<Image>().color = Color.white;

                            }
                        }

                    }
                }
            }
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
   
        int ocupiedSlots= 0;

        ObjectScript itemOject = eventData.pointerDrag.GetComponent<ObjectScript>();

            if (itemOject.CurrentSlot.Count == 0)
            {
                if (itemOject.ItemSize.x > 0 || itemOject.ItemSize.y > 0)
                {
                    for (int x = 0; x < itemOject.ItemSize.x; x++)
                    {
                        for (int y = 0; y < itemOject.ItemSize.y; y++)
                        {
                            Vector2Int possitionToOcupie = new Vector2Int(_slotArray.x + x, _slotArray.y + y);
                            if(possitionToOcupie.x > _passObjectArr.GetLength(0)-1 || possitionToOcupie.y > _passObjectArr.GetLength(1)-1)
                            {
                                itemOject.GetComponent<RectTransform>().anchoredPosition = itemOject.InitialPos;
                                return;
                            }
                            if (_passObjectArr[possitionToOcupie.x, possitionToOcupie.y].GetComponent<Slots>().Ocupied == true)
                            {
                                ocupiedSlots++;
                            }
                        }
                    }

                if (ocupiedSlots > 0)
                {
                    itemOject.GetComponent<RectTransform>().anchoredPosition = itemOject.InitialPos;
                }
                else
                {
                    itemOject.transform.SetParent(gameObject.transform);
                    itemOject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
                        itemOject.transform.parent=gameObject.transform.parent.parent;
                    for (int i = 0; i < itemOject.CurrentSlot.Count; i++)
                    {
                            
                        itemOject.CurrentSlot[i].Ocupied = false;
                    }
                    itemOject.CurrentSlot.Clear();
                    for (int x = 0; x < itemOject.ItemSize.x; x++)
                    {
                        for (int y = 0; y < itemOject.ItemSize.y; y++)
                        {

                            Vector2Int itemslotstoocupie = new Vector2Int(_slotArray.x + x, _slotArray.y + y);
                            itemOject.CurrentSlot.Add(_passObjectArr[itemslotstoocupie.x, itemslotstoocupie.y].GetComponent<Slots>());
                            _passObjectArr[itemslotstoocupie.x, itemslotstoocupie.y].GetComponent<Slots>().Ocupied = true;
                            _passObjectArr[itemslotstoocupie.x, itemslotstoocupie.y].GetComponent<Slots>().StoredItem = itemOject.gameObject;
                        }
                    }
                }
            }
            else
            {
                itemOject.CurrentSlot.Add(this);
                itemOject.CurrentSlot[0].Ocupied = true;
                _storedItem = itemOject.gameObject;

                itemOject.transform.SetParent(gameObject.transform);
                itemOject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
                itemOject.transform.parent = gameObject.transform.parent.parent;
            }
            

    }
            else
            {
                if (itemOject.ItemSize.x > 0 || itemOject.ItemSize.y > 0)
                {
                    //   Debug.Log("3333");
                    for (int x = 0; x < itemOject.ItemSize.x; x++)
                    {
                    //  Debug.Log("123");
                        for (int y = 0; y < itemOject.ItemSize.y; y++)
                        {

                            Vector2Int possitionToOcupie = new Vector2Int(_slotArray.x + x, _slotArray.y + y);
                            if (possitionToOcupie.x > _passObjectArr.GetLength(0) - 1 || possitionToOcupie.y > _passObjectArr.GetLength(1) - 1)
                            {
                                itemOject.GetComponent<RectTransform>().anchoredPosition = itemOject.InitialPos;
                                return;

                            }

                            if (_passObjectArr[possitionToOcupie.x, possitionToOcupie.y].GetComponent<Slots>().Ocupied == true)
                            {
                                // Debug.Log("the slot " + _passObjectArr[possitionToOcupie.x, possitionToOcupie.y].name + " is ocupied");
                                if(_passObjectArr[possitionToOcupie.x, possitionToOcupie.y].GetComponent<Slots>().StoredItem == itemOject.gameObject)
                                {
                                Debug.Log(_passObjectArr[possitionToOcupie.x, possitionToOcupie.y] + " tem este item");
                                }
                                else
                                {
                                    ocupiedSlots++;
                                Debug.Log(_passObjectArr[possitionToOcupie.x, possitionToOcupie.y] + " tem outro item");


                                }
                            }
                        }
                    }
                    if (ocupiedSlots > 0)
                    {
                        itemOject.GetComponent<RectTransform>().anchoredPosition = itemOject.InitialPos;
                    }
                    else
                    {
                        itemOject.transform.SetParent(gameObject.transform);
                        itemOject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
                        itemOject.transform.parent = gameObject.transform.parent.parent;
                        for (int i = 0; i < itemOject.CurrentSlot.Count; i++)
                        {
                            
                            itemOject.CurrentSlot[i].Ocupied = false;
                        }
                    itemOject.CurrentSlot.Clear();

                    for (int x = 0; x < itemOject.ItemSize.x; x++)
                        {
                            for (int y = 0; y < itemOject.ItemSize.y; y++)
                            {

                                Vector2Int itemslotstoocupie = new Vector2Int(_slotArray.x + x, _slotArray.y + y);
                                itemOject.CurrentSlot.Add(_passObjectArr[itemslotstoocupie.x, itemslotstoocupie.y].GetComponent<Slots>());
                                _passObjectArr[itemslotstoocupie.x, itemslotstoocupie.y].GetComponent<Slots>().Ocupied = true;
                            _passObjectArr[itemslotstoocupie.x, itemslotstoocupie.y].GetComponent<Slots>().StoredItem = itemOject.gameObject;
                            }
                        }
                    }
                }
                else
                {
                    itemOject.CurrentSlot.Add(this);
                    itemOject.CurrentSlot[0].Ocupied = true;
                _storedItem = itemOject.gameObject;

                itemOject.transform.SetParent(gameObject.transform);
                    itemOject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
                    itemOject.transform.parent = gameObject.transform.parent.parent;
                }
            }    
            for (int x = 0; x < itemOject.ItemSize.x; x++)
            {
                for (int y = 0; y < itemOject.ItemSize.y; y++)
                {
                    Vector2Int possitionToOcupie = new Vector2Int(_slotArray.x + x, _slotArray.y + y);
                    if (possitionToOcupie.x < _passObjectArr.GetLength(0) && possitionToOcupie.y < _passObjectArr.GetLength(1))
                    {
                        if (_passObjectArr[possitionToOcupie.x, possitionToOcupie.y].GetComponent<Slots>().Ocupied == true)
                        {

                            if (_passObjectArr[possitionToOcupie.x, possitionToOcupie.y].GetComponent<Slots>().StoredItem == itemOject.gameObject)
                            {
                                _passObjectArr[possitionToOcupie.x, possitionToOcupie.y].GetComponent<Image>().color = Color.white;

                            }
                            else
                            {
                                _passObjectArr[possitionToOcupie.x, possitionToOcupie.y].GetComponent<Image>().color = Color.white;
                            }
                        }
                        else
                        {
                            _passObjectArr[possitionToOcupie.x, possitionToOcupie.y].GetComponent<Image>().color = Color.white;

                        }
                    }

                }
            }
        }
    }
}
