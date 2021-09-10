using System.Collections.Generic;
using UnityEngine;

public class BrickSpawner : MonoBehaviour
{
    [SerializeField]
    List<GameObject> brickTypes; //Brick types that we want to spawn

    [SerializeField]
    int totalRow, totalColumn; //Brick size

    [SerializeField]
    float brickAreaLeftLimit, brickAreaRightLimit, brickAreaBottomLimit, brickAreaTopLimit; //To create nodes in birck area

    float columnGap, rowGap; //Gap between columns and rows

    void Start()
    {
        Ball.totalBricks=0;
        for (int i = 0; i < totalRow; i++)
        {
            for (int j = 0; j < totalColumn; j++)
            {
                columnGap = (brickAreaRightLimit - brickAreaLeftLimit) / (totalColumn - 1); //Calculate gap
                rowGap = (brickAreaTopLimit - brickAreaBottomLimit) / (totalRow - 1);

                Instantiate(brickTypes[Random.Range(0, brickTypes.Count)], new Vector3(brickAreaLeftLimit + (columnGap * j), 0, brickAreaTopLimit - (rowGap * i)), Quaternion.identity).GetComponent<Rigidbody>().isKinematic=true;
                Ball.totalBricks++;
            }
        }
    }
}