using System.Collections.Generic;
using UnityEngine;

public class CellManager : MonoBehaviour
{
    List<List<Cell>> cells = new List<List<Cell>>(); //All cells

    [SerializeField]
    List<GameObject> foodTypes; //Food types that we will spawn

    [SerializeField]
    int totalRow, totalColumn; //Cell size

    [SerializeField]
    float cellsLeftLimit, cellsRightLimit, cellsBottomLimit, cellsTopLimit; //To create nodes nodes between cell borders (we will use that nodes to spawn objects and place cells)

    float columnGap, rowGap; //Gap between columns and rows

    TouchManager touchManager; //We will get touches
    Vector3 firstTouchLocation, finalTouchLocation; //To determine swipe
    bool swipeStarted, isSwipeFailed; //If swipe failed than try again
    bool swipeCompleted = true; //Dont swipe before current swipe complete
    float swipeDuration = 1f; //Swipe animation
    Cell firstTouchedCell, finalTouchedCell; //Cells that will swipe

    void Start()
    {
        touchManager = TouchManager.GetInstance();

        columnGap = (cellsRightLimit - cellsLeftLimit) / (totalColumn - 1); //Calculate gap
        rowGap = (cellsTopLimit - cellsBottomLimit) / (totalRow - 1);

        int[] matrix = GlobalAttributes.GetMatrix(); //Get matrix from json file
        for (int i = 0; i < totalRow; i++)
        {
            cells.Add(new List<Cell>());
            for (int j = 0; j < totalColumn; j++)
            {
                cells[i].Add(new Cell(new Vector3(cellsLeftLimit + (columnGap * j), foodTypes[matrix[(i * totalColumn) + j]].transform.position.y, cellsTopLimit - (rowGap * i)))); //Add cells to calculated nodes
                cells[i][j].currentCellObject = Instantiate(foodTypes[matrix[(i * totalColumn) + j]], cells[i][j].cellLocation, Quaternion.identity); //Spawn object to cell location

                Destroy(cells[i][j].currentCellObject.GetComponent<Collider>()); //We dont need these
                Destroy(cells[i][j].currentCellObject.GetComponent<Rigidbody>());

                cells[i][j].currentCellObject.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f); //Lets make them smaller :)

                if (i > 0) //Adding neighbors
                {
                    cells[i - 1][j].verticalNeighbor.Add(cells[i][j]);
                    cells[i][j].verticalNeighbor.Add(cells[i - 1][j]);
                }
                if (j > 0)
                {
                    cells[i][j - 1].horizontalNeighbor.Add(cells[i][j]);
                    cells[i][j].horizontalNeighbor.Add(cells[i][j - 1]);
                }
            }
        }
    }


    void Update()
    {
        if (swipeStarted) //Swipe animation
        {
            finalTouchedCell.currentCellObject.transform.position = Vector3.MoveTowards(finalTouchedCell.currentCellObject.transform.position, firstTouchedCell.cellLocation, 0.01f); //We are moving food locations
            firstTouchedCell.currentCellObject.transform.position = Vector3.MoveTowards(firstTouchedCell.currentCellObject.transform.position, finalTouchedCell.cellLocation, 0.01f);

            if (Vector3.Distance(firstTouchedCell.currentCellObject.transform.position, finalTouchedCell.cellLocation) < 0.05f) //They arrived to thier location
            {
                swipeDuration -= Time.deltaTime; //Lets give them some time to check matches
                if (swipeDuration < 0)
                {
                    swipeDuration = 1f;
                    swipeStarted = false; //Cell animation over
                    SwipeComplete(); //Swipe completed lets check matches
                }
            }
        }
        else //We cant touch until cell animation over
        {
            if (touchManager.touchDown) //First touch
            {
                firstTouchLocation = touchManager.touchPosition;
                touchManager.touchDown = false;
            }
            if (touchManager.touchUp)
            {
                finalTouchLocation = touchManager.touchPosition;
                touchManager.touchUp = false; //Swipe starting, Probably (if cells arent adjacent swipe wont start)
                Swipe(); //Check cells and swipe if they are adjacent
            }
        }
    }

    void Swipe()
    {
        if (GetTouchedCells() && swipeCompleted) //Check if cells are neighbor and current swipe complete
        {
            swipeStarted = true; //Start swipe animation
            swipeCompleted = false; //Swipe just started
        }
    }

    void SwipeComplete()
    {
        GameObject temp = null; //Lets switch objects between two selected cell
        temp = finalTouchedCell.currentCellObject;
        finalTouchedCell.currentCellObject = firstTouchedCell.currentCellObject;
        firstTouchedCell.currentCellObject = temp;

        if (!CellsMatch()) //No match lets get back to our own positions
        {
            if (!isSwipeFailed)
            {
                swipeStarted = true; //Start swipe animation again and we will return back to our own positions
                isSwipeFailed = true; //To prevent loop
            }
            else
            {
                isSwipeFailed = false;
                swipeCompleted = true; //Swipe completed so we can swipe again
            }
        }
    }

    bool CellsMatch()
    {
        List<GameObject> objectsToDestroy = new List<GameObject>(); //We will destroy objects that matched

        for (int i = 0; i < totalRow; i++)
        {
            for (int j = 0; j < totalColumn; j++)
            {
                if (cells[i][j].currentCellObject) //We dont need to check if there isnt any object
                {
                    if (cells[i][j].horizontalNeighbor.Count > 1) //If there is enough neighbor
                    {
                        if (cells[i][j].horizontalNeighbor[0].currentCellObject && cells[i][j].horizontalNeighbor[1].currentCellObject) //If neighbors have objects
                        {
                            if (cells[i][j].horizontalNeighbor[0].currentCellObject.tag == cells[i][j].currentCellObject.tag && cells[i][j].horizontalNeighbor[1].currentCellObject.tag == cells[i][j].currentCellObject.tag) //If these objects has same tag
                            {
                                if (!objectsToDestroy.Contains(cells[i][j].currentCellObject)) //If these objects hasnt already added to death note
                                    objectsToDestroy.Add(cells[i][j].currentCellObject); //Than destroy them
                                if (!objectsToDestroy.Contains(cells[i][j].horizontalNeighbor[0].currentCellObject))
                                    objectsToDestroy.Add(cells[i][j].horizontalNeighbor[0].currentCellObject);
                                if (!objectsToDestroy.Contains(cells[i][j].horizontalNeighbor[1].currentCellObject))
                                    objectsToDestroy.Add(cells[i][j].horizontalNeighbor[1].currentCellObject);
                            }
                        }
                    }
                    if (cells[i][j].verticalNeighbor.Count > 1)
                    {
                        if (cells[i][j].verticalNeighbor[0].currentCellObject && cells[i][j].verticalNeighbor[1].currentCellObject)
                        {
                            if (cells[i][j].verticalNeighbor[0].currentCellObject.tag == cells[i][j].currentCellObject.tag && cells[i][j].verticalNeighbor[1].currentCellObject.tag == cells[i][j].currentCellObject.tag)
                            {
                                if (!objectsToDestroy.Contains(cells[i][j].currentCellObject))
                                    objectsToDestroy.Add(cells[i][j].currentCellObject);
                                if (!objectsToDestroy.Contains(cells[i][j].verticalNeighbor[0].currentCellObject))
                                    objectsToDestroy.Add(cells[i][j].verticalNeighbor[0].currentCellObject);
                                if (!objectsToDestroy.Contains(cells[i][j].verticalNeighbor[1].currentCellObject))
                                    objectsToDestroy.Add(cells[i][j].verticalNeighbor[1].currentCellObject);
                            }
                        }
                    }
                }
            }
        }

        if (objectsToDestroy.Count > 0) //We have something to destroy
        {
            GameUI.levelPercentage += objectsToDestroy.Count * 100 / (totalRow * totalColumn); //Calculate percentage of level

            for (int i = 0; i < objectsToDestroy.Count; i++)
                Destroy(objectsToDestroy[i]); //Destroy matched objects
            swipeCompleted = true; //Swipe completed
            return true;
        }
        
        return false;
    }

    bool GetTouchedCells()
    {
        for (int i = 0; i < totalRow; i++)
        {
            for (int j = 0; j < totalColumn; j++)
            {
                if (Vector3.Distance(cells[i][j].cellLocation, firstTouchLocation) < 0.3f) //We found first touched cell
                {
                    firstTouchedCell = cells[i][j];
                    if (!firstTouchedCell.currentCellObject) //There isnt any object so we dont need to touch it
                        return false;
                }
            }
        }
        for (int i = 0; i < totalRow; i++)
        {
            for (int j = 0; j < totalColumn; j++)
            {
                if (Vector3.Distance(cells[i][j].cellLocation, finalTouchLocation) < 0.3f)
                {
                    if (cells[i][j].horizontalNeighbor.Contains(firstTouchedCell) || cells[i][j].verticalNeighbor.Contains(firstTouchedCell)) //We only swipe if we touched the neighbor cell of first touched cell
                    {
                        finalTouchedCell = cells[i][j];
                        if (!finalTouchedCell.currentCellObject)
                            return false;
                        return true;
                    }
                }
            }
        }
        return false;
    }
}
