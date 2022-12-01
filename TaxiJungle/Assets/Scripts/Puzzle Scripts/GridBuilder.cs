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

    private void Awake()
    {
        SlotGrid = new GameObject[_width, _height];

    }
    private void Start()
    {
        _gridLayoutGroup = GetComponent<GridLayoutGroup>();
        // GenerateGrid();
        CreateSlots();
    }
    private void GenerateGrid()
    {
        _gridLayoutGroup.constraintCount = _width;
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                var spawnTile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity);
                spawnTile.transform.SetParent(this.transform);
                spawnTile.GetComponent<Slots>().SlotArray = new Vector2Int(x, y);
                spawnTile.name = $"Tile {x} {y}]";

            }
        }

    }
    private void CreateSlots()
    {
        _gridLayoutGroup.constraintCount = _width;
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
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
