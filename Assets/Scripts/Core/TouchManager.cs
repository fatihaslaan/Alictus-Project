using UnityEngine;

public class TouchManager : MonoBehaviour
{
    [HideInInspector]
    public bool touchDown, touchUp;
    [HideInInspector]
    public Vector3 touchPosition;

    static TouchManager instance;
    bool isEditor;
    Camera camera;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            isEditor = Application.isEditor;
            camera = Camera.main;
        }
    }

    void Update()
    {
        if (isEditor) //We are using editor
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0))
            {
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 10))
                {
                    if (hit.transform.tag == "Platform") //We will move on platform
                    {
                        touchPosition = hit.point;
                        if (Input.GetMouseButtonDown(0))
                        {
                            touchDown = true;
                        }
                        if (Input.GetMouseButtonUp(0))
                        {
                            touchUp = true;
                        }
                    }
                }

            }
        }
        else
        {
            if (Input.touchCount == 1)
            {
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 10))
                {
                    if (hit.transform.tag == "Platform")
                    {
                        touchPosition = hit.point;
                        if (Input.GetTouch(0).phase == TouchPhase.Began)
                        {
                            touchDown = true;
                        }
                        if (Input.GetTouch(0).phase == TouchPhase.Ended)
                        {
                            touchUp = true;
                        }
                    }
                }
            }
        }
    }

    public static TouchManager GetInstance()
    {
        return instance;
    }
}