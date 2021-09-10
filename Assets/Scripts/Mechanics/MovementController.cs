using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField]
    GameObject movingObject; //Our object that will move

    float movingObjectY; //To prevent y axis move
    bool move = false; //Movement ended
    Vector3 targetPosition; //Move position
    TouchManager touchManager; //We will take touch input from here

    void Start()
    {
        touchManager = TouchManager.GetInstance();
        movingObjectY = movingObject.transform.position.y;
        movingObject.GetComponent<Rigidbody>().isKinematic=true; //Other colliders shouldnt effect our object
    }

    void Update()
    {
        if (touchManager.touchDown) //User touched
        {
            touchManager.touchDown = false;
            targetPosition = touchManager.touchPosition;
            move = true;
        }
        if (move)
        {
            movingObject.transform.position = Vector3.MoveTowards(movingObject.transform.position, targetPosition, 0.02f); //Moving
            movingObject.transform.position = new Vector3(movingObject.transform.position.x, movingObjectY, movingObject.transform.position.z); //Prevent y axis movement

            if (Vector3.Distance(targetPosition, movingObject.transform.position) < 0.1f) //We are close we no longer need to move
                move = false;

            Vector3 distance = movingObject.transform.position - targetPosition; //Get distance between moving object and target
            Quaternion rotation = Quaternion.Slerp(movingObject.transform.rotation, Quaternion.LookRotation(distance), 10f * Time.deltaTime); //Calculate rotation with distance
            movingObject.transform.rotation = rotation; //Rotate object
            movingObject.transform.eulerAngles = new Vector3(0, movingObject.transform.eulerAngles.y, 0); //Prevent rotating in x and z axis
        }
    }
}