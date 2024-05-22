using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class TileGrid : MonoBehaviour
{

    //gets and sets rows 
    public TileRow[] rows { get; private set; }

    //gets and sets cells
    public TileCell[] cells { get; private set; }

    //the size of grid is amount of cells
    public int size => cells.Length;

    //the height of grid is amt of rows
    public int height =>rows.Length;

    public int width => size / height;

    private void Awake()
    {
        rows = GetComponentsInChildren<TileRow>();
        cells = GetComponentsInChildren<TileCell>();
    }

    //assign coordinates for each cell
    private void Start()
    {
        for (int y = 0; y < rows.Length; y++) 
        {
            for (int x = 0; x < rows[y].cells.Length; x++) 
            {
                rows[y].cells[x].coordinates = new Vector2Int(x, y);
            }
        }
    }

    public TileCell GetRandomEmptyCell()
    {
        int index = Random.Range(0, cells.Length);
        int startingIndex = index;

        while (cells[index].occupied) 
        {
            index++;
            if (index >= cells.Length)
            {
                index = 0;
            }

            if (index == startingIndex)
            {
                return null;
            }
        }

        return cells[index];
    }

    public TileCell GetCell(int x, int y)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            return rows[y].cells[x];
        }
        else
        {
            return null;
        }
    }

    public TileCell GetCell(Vector2Int coordinates)
    {
        return GetCell(coordinates.x, coordinates.y);
    }

    public TileCell getAdjacentCell(TileCell cell, Vector2Int direction)
    {
        Vector2Int coordinates = cell.coordinates;
        coordinates.x += direction.x; 
        coordinates.y -= direction.y;

        return GetCell(coordinates);
    }
}
