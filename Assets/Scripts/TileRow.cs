using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileRow : MonoBehaviour
{
    //keeps track of cell in row
    public TileCell[] cells { get; private set; }


    private void Awake()
    {
        //searches for component type tileCell on object this script is connected to
        cells = GetComponentsInChildren<TileCell>();
    }
}
