using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileBoard : MonoBehaviour
{
    public GameManager gameManager;
    public Tile tilePrefab;
    public TileState[] tileStates;
    //public Sprite[] tileSprites;
    private TileGrid grid;
    private List<Tile> tiles;
    private bool waiting;
    public float highestElementNum;

    private void Awake()
    {
        grid =   GetComponentInChildren<TileGrid>();
        tiles = new List<Tile>(16);
        //tileSprites = new List<Sprite>(16);

    }

    public void ClearBoard()
    {
        foreach (var cell in grid.cells)
        {
            cell.tile = null;
        }

        foreach (var tile in tiles)
        {
            Destroy(tile.gameObject);
        }

        tiles.Clear();
    }

    public void CreateTile()
    {
        
        Tile tile = Instantiate(tilePrefab, grid.transform);
        //tile.setState(tileStates[0]);
        // tileImages[0]

        tile.setState(tileStates[0]);

        tile.Spawn(grid.GetRandomEmptyCell());
        tiles.Add(tile);
    }

    private void Update()
    {
        if (!waiting)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
               // MoveTiles(Vector2Int.up, 0, 1, 1, 1);
                MoveTiles(Vector2Int.left, 1, 1, 0, 1);
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
               // MoveTiles(Vector2Int.right, grid.width - 2, -1, 0, 1);
                MoveTiles(Vector2Int.down, 0, 1, grid.height - 2, -1);
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
               // MoveTiles(Vector2Int.down, 0, 1, grid.height - 2, -1);
                MoveTiles(Vector2Int.right, grid.width - 2, -1, 0, 1);
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
               // MoveTiles(Vector2Int.left, 1, 1, 0, 1);
                MoveTiles(Vector2Int.up, 0, 1, 1, 1);
            }
        }

    }

    //makes logic less duplicated
    private void MoveTiles(Vector2Int direction, int startX, int incrementX, int startY, int incrementY)
    {
        bool changed = false;

        for (int x = startX; x >= 0 && x < grid.width; x += incrementX)
        {
            for (int y = startY; y >= 0 && y < grid.height; y += incrementY) 
            {
                TileCell cell = grid.GetCell(x, y);

                if (cell.occupied)
                {
                    changed |= MoveTile(cell.tile, direction);
                }
            }
        }

        if (changed)
        {
            StartCoroutine(WaitForChanges());
        }
    }

    private bool CanMerge(Tile a, Tile b)
    {
        return a.state == b.state && !b.locked;
    }

    //main game logic
    private bool MoveTile(Tile tile, Vector2Int direction) 
    {
        TileCell newCell = null;
        TileCell adjacent = grid.getAdjacentCell(tile.cell, direction);

        while (adjacent != null)
        {
            if (adjacent.occupied)
            {
                //meging tiles
                if(CanMerge(tile, adjacent.tile))
                {
                    Merge(tile, adjacent.tile);
                    return true;
                }
                break;
            }
            
            newCell = adjacent;
            adjacent = grid.getAdjacentCell(adjacent, direction);
        }

        if (newCell != null)
        {
            tile.MoveTo(newCell);
            return true;
        }

        return false;
    }

    private void Merge(Tile a, Tile b)
    {
        tiles.Remove(a);
        a.Merge(b.cell);

        int index = Mathf.Clamp(IndexOf(b.state) + 1, 0, tileStates.Length -1);
        //move image up 1

        b.setState(tileStates[index]);
        gameManager.IncreaseScore((index + 1) * 10);

        if (index == highestElementNum)
        {
            gameManager.GameWon();
        }
    }

    private int IndexOf(TileState state)
    {
        for (int i = 0; i < tileStates.Length; i++)
        {
            if(state == tileStates[i])
            {
                return i;
            }
        }

        return -1;
    }

    private IEnumerator WaitForChanges()
    {
        waiting = true;

        yield return new WaitForSeconds(0.1f);

        waiting = false;

        foreach (var tile in tiles)
        {
            tile.locked = false;
        }

        //create new tile

        if (tiles.Count != grid.size)
        {
            CreateTile();
        }

        //check game over 
        if (CheckForGameOver())
        {
            gameManager.GameOver();
        }
    }

       /* private bool CheckForGameWon() 
        {
            //check each sprite and see if sprite in sprite renderer is set to 
        }*/

        private bool CheckForGameOver()
        {
            if (tiles.Count != grid.size)
            {
                return false;
            }

            foreach (var tile in tiles)
            {
                TileCell up = grid.getAdjacentCell(tile.cell, Vector2Int.up);
                TileCell down = grid.getAdjacentCell(tile.cell, Vector2Int.down);
                TileCell left = grid.getAdjacentCell(tile.cell, Vector2Int.left);
                TileCell right = grid.getAdjacentCell(tile.cell, Vector2Int.right);

                if(up != null && CanMerge(tile, up.tile))
                {
                    return false;
                }

                if (down != null && CanMerge(tile, down.tile))
                {
                    return false;
                }

                if (left != null && CanMerge(tile, left.tile))
                {
                    return false;
                }

                if (right != null && CanMerge(tile, right.tile))
                {
                    return false;
                }
            }

            

            return true;
        }
}
    