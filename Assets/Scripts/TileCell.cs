using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCell : MonoBehaviour
{
    //setting and getting tile coordinates
    public Vector2Int coordinates {  get; set; }

    //sets which tile object on the grid cell
    public Tile tile { get; set; }

    //determines if cell is emplty or occupied
    public bool empty => tile == null;

    public bool occupied => tile != null;


}
