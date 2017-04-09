using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

public class CameraTrigger : MonoBehaviour
{
    #region Variables

    public bool onEnterCanMoveLeft;
    public bool onEnterCanMoveRight;
    public bool onEnterCanMoveUp;
    public bool onEnterCanMoveDown;


    public bool onExitCanMoveLeft;
    public bool onExitCanMoveRight;
    public bool onExitCanMoveUp;
    public bool onExitCanMoveDown;

    #endregion


    #region MonoBehaviour



    // Called when the Collider other enters the trigger.
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "MainCamera")
        {
            var camera = other.GetComponent<LevelCamera>();

            camera.canMoveLeft = onEnterCanMoveLeft;
            camera.canMoveRight = onEnterCanMoveRight;
            camera.canMoveUp = onEnterCanMoveUp;
            camera.canMoveDown = onEnterCanMoveDown;

            var cameraPosition = camera.transform.position;
            var triggerPosition = this.transform.position;

            var cameraBounds = camera.GetComponent<Collider2D>().bounds;
            var triggerBounds = GetComponent<Collider2D>().bounds;


            if(cameraPosition.x > triggerPosition.x)
            {
                cameraPosition.x = triggerPosition.x + 
                                    cameraBounds.extents.x +
                                    triggerBounds.extents.x;
            }

            if(cameraPosition.x < triggerPosition.x)
            {
                cameraPosition.x = triggerPosition.x -
                                    cameraBounds.extents.x -
                                    triggerBounds.extents.x;

            }


            currentCamera = camera;
            this.switching = true;

            switchTargetPosition = new Vector3(cameraPosition.x, 
                                               cameraPosition.y, 
                                               cameraPosition.z);


            switchStartTime = Time.time;
            //camera.transform.position = cameraPosition;

        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "MainCamera")
        {
            var camera = other.GetComponent<LevelCamera>();

            camera.canMoveLeft = onExitCanMoveLeft;
            camera.canMoveRight = onExitCanMoveRight;
            camera.canMoveUp = onExitCanMoveUp;
            camera.canMoveDown = onExitCanMoveDown;

            currentCamera = null;
            this.switching = false;
        }
    }

    LevelCamera currentCamera = null;

    Vector3 switchTargetPosition = Vector3.zero;


    float cameraSwitchDuration = 1.0f;
    float switchStartTime = 0;

    bool switching = false;


    private void Update()
    {
        if(currentCamera != null && this.switching)
        {
            float cameraTime = (Time.time - switchStartTime) / cameraSwitchDuration;

            float cameraX = Mathf.SmoothStep(currentCamera.transform.position.x,
                                             switchTargetPosition.x,
                                             cameraTime);

            float cameraY = Mathf.SmoothStep(currentCamera.transform.position.y,
                                             switchTargetPosition.y,
                                             cameraTime);

            float cameraZ = currentCamera.transform.position.z;


            currentCamera.transform.position = new Vector3(cameraX, cameraY, cameraZ);


            if (currentCamera.transform.position == switchTargetPosition)
            {
                this.switching = false;
            }

        }


    }

    #endregion
}

