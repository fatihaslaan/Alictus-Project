using UnityEngine;

public class Ball : MonoBehaviour
{
    public static int totalBricks = 0;

    TouchManager touchManager;

    float zSpeed, xSpeed; //Ball variables
    bool wobbling;
    float wobblingDuration = 0.1f;

    float wobblingStrength = 0.04f; //Brick effects
    float biggerBallStrength = 1.15f;
    float padLenthUpStrenth = 1.2f;
    float speedUpStregth = 0.01f;

    GameObject pad; //Pad variables
    float padTargetXAxis;
    Vector3 targetLocation;
    bool padMoves, padReversed;

    void Start()
    {
        touchManager = TouchManager.GetInstance();

        zSpeed = 0.01f;
        xSpeed = Random.Range(0, 2) == 1 ? 0.01f : -0.01f;

        pad = GameObject.FindGameObjectWithTag("Hotdog");
    }

    void Update()
    {
        transform.Translate(xSpeed, 0, zSpeed); //Move ball

        if (touchManager.touchDown) //User touched
        {
            touchManager.touchDown = false;
            padTargetXAxis = touchManager.touchPosition.x;
            if (padReversed)
                padTargetXAxis *= -1; //Move reversed
            targetLocation = new Vector3(padTargetXAxis, pad.transform.position.y, pad.transform.position.z);
            padMoves = true;
        }
        if (padMoves)
        {
            pad.transform.position = Vector3.MoveTowards(pad.transform.position, targetLocation, 0.02f); //Move pad

            if (Vector3.Distance(targetLocation, pad.transform.position) < 0.1f) //We are close we no longer need to move
                padMoves = false;
        }
        if (wobbling) //Ball is shaking
        {
            wobblingDuration -= Time.deltaTime;
            if (wobblingDuration < 0)
            {
                wobblingDuration = 0.1f;
                transform.position = new Vector3(transform.position.x + Random.Range(-wobblingStrength, wobblingStrength), 0, transform.position.z + Random.Range(-wobblingStrength, wobblingStrength)); //Change position of ball to give shake effect
            }
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.transform.tag == "Bottom Barrier")
        {
            //Game over
        }
        else if (col.transform.tag == "Right Barrier" || col.transform.tag == "Left Barrier") //Change direction when hitting barriers
        {
            xSpeed *= -1;
        }
        else if (col.transform.tag == "Top Barrier" || col.transform.tag == "Hotdog")
        {
            zSpeed *= -1;
        }
        else if (col.transform.tag != "Platform") //We hit one of bricks
        {
            zSpeed *= -1;
            GameUI.levelPercentage += 100f / totalBricks;

            if (col.transform.tag == "Cherry") //Speed ball
            {
                zSpeed += (zSpeed > 0) ? speedUpStregth : -speedUpStregth;
                xSpeed += (xSpeed > 0) ? speedUpStregth : -speedUpStregth;
            }
            if (col.transform.tag == "Banana") //Length up pad
            {
                pad.transform.localScale = new Vector3(pad.transform.localScale.x * padLenthUpStrenth, pad.transform.localScale.y, pad.transform.localScale.z);
            }
            if (col.transform.tag == "Hamburger") //Reversed pad
            {
                padReversed = !padReversed;
            }
            if (col.transform.tag == "Cheese") //Shake ball
            {
                wobbling = !wobbling;
            }
            if (col.transform.tag == "Watermelon") //Ball is bigger now
            {
                transform.localScale = new Vector3(transform.localScale.x * biggerBallStrength, transform.localScale.y * biggerBallStrength, transform.localScale.z * biggerBallStrength);
            }
            
            Destroy(col.gameObject); //Destroy brick
        }
    }
}