using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridBuilder : MonoBehaviour
{
    [SerializeField] private int _width, _height;
    [SerializeField] GameObject[,] slotGrid;

    [SerializeField] GameObject _tilePrefab;
    GridLayoutGroup _gridLayoutGroup;

    public GameObject[,] SlotGrid { get => slotGrid; set => slotGrid = value; }
    public int Width { get => _width; set => _width = value; }
    public int Height { get => _height; set => _height = value; }

    private void Awake()
    {
        SlotGrid = new GameObject[Width, Height];

    }
    private void Start()
    {
        _gridLayoutGroup = GetComponent<GridLayoutGroup>();
        // GenerateGrid();
        CreateSlots();
    }
    private void CreateSlots()
    {
        _gridLayoutGroup.constraintCount = Width;
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {

                SlotGrid[x, y] = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity);
                SlotGrid[x, y].transform.GetComponentInChildren<Slots>().PassObjectArr = SlotGrid;
                SlotGrid[x, y].transform.name = "slot[" + x + "," + y + "]";
                SlotGrid[x, y].transform.SetParent(this.transform);
                SlotGrid[x, y].GetComponent<RectTransform>().localScale = Vector3.one;
                SlotGrid[x, y].GetComponent<Slots>().SlotArray = new Vector2Int(x, y);
            }
        }

    }
}
