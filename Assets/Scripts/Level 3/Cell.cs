using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public List<Cell> verticalNeighbor=new List<Cell>(); //Neighbors
    public List<Cell> horizontalNeighbor=new List<Cell>();

    public Vector3 cellLocation; //Location of our imaginary cell
    public GameObject currentCellObject; //Cell object

    public Cell(Vector3 location)
    {
        cellLocation=location;
    }
}