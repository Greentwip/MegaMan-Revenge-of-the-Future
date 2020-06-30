using UnityEngine;
using System.Collections;

public class LevelCamera : MonoBehaviour
{
    public Transform Player;

    // Public Properties

    public bool canMoveLeft;
    public bool canMoveRight;
    public bool canMoveUp;
    public bool canMoveDown;

    // Private Instance Variables
    private Vector3 playerPos;
    private Vector3 deltaPos;


    public float xTolerance = 16.0f;
    public float yTolerance = 16.0f;

    public bool switching = false;
    bool playerSwitching = false;

    Vector3 switchTargetPosition = Vector3.zero;
    Vector3 playerSwitchTargetPosition = Vector3.zero;
    Vector3 playerVelocity = Vector3.zero;
    Vector3 cameraVelocity = Vector3.zero;

    float cameraSwitchDuration = 1.0f;
    float playerSwitchDuration = 0.5f;

    float switchStartTime = 0;


    // Update is called once per frame
    protected void Update()
    {
        if (switching)
        {
            float cameraTime = (Time.time - switchStartTime) / cameraSwitchDuration;

            float cameraX = Mathf.SmoothStep(transform.position.x,
                                             switchTargetPosition.x,
                                             cameraTime);

            float cameraY = Mathf.SmoothStep(transform.position.y, 
                                             switchTargetPosition.y,
                                             cameraTime);

            float cameraZ = transform.position.z;




            transform.position = new Vector3(cameraX, cameraY, cameraZ);


            if (this.transform.position == switchTargetPosition)
            {
                this.switching = false;
            }

            if(Player.transform.position == playerSwitchTargetPosition &&
                this.playerSwitching)
            {
                this.playerSwitching = false;

                Player.GetComponent<Player>().frozen = false;
                Player.GetComponent<Rigidbody2D>().simulated = true;
                Player.GetComponent<Rigidbody2D>().velocity = playerVelocity;
            }
            else if(this.playerSwitching)
            {
                float playerTime = (Time.time - switchStartTime) / playerSwitchDuration;

                float playerX = Mathf.SmoothStep(Player.transform.position.x,
                                                 playerSwitchTargetPosition.x,
                                                 playerTime);

                float playerY = Mathf.SmoothStep(Player.transform.position.y,
                                                 playerSwitchTargetPosition.y,
                                                 playerTime);

                float playerZ = Player.transform.position.z;

                Player.transform.position = new Vector3(playerX, playerY, playerZ);
            }
        }
        else
        {
            // Make the camera follow the player...
            playerPos = Player.transform.position;
            deltaPos = playerPos - transform.position;
            Vector3 targetPosition = Vector3.zero;

            if (deltaPos.x > xTolerance && canMoveRight)
            {
                Vector3 position = transform.position;

                position.x = playerPos.x - (xTolerance * Mathf.Sign(deltaPos.x));

                targetPosition = position;
            }
            

            if (deltaPos.x < -xTolerance && canMoveLeft)
            {
                Vector3 position = transform.position;

                position.x = playerPos.x - (xTolerance * Mathf.Sign(deltaPos.x));

                targetPosition = position;
            }

            // Check the y pos 
            if (Mathf.Abs(deltaPos.y) > yTolerance && (canMoveDown || canMoveUp))
            {
                Vector3 position = transform.position;

                position.y = playerPos.y - (yTolerance * Mathf.Sign(deltaPos.y));

                targetPosition = position;
            }

            if(targetPosition != Vector3.zero)
            {
                //var smoothPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref cameraVelocity, 2 * Time.deltaTime);

                var smoothPosition = Vector3.Lerp(transform.position, targetPosition, 1f / 30f);
                //var rigidBody = GetComponent<Rigidbody2D>();
                //var trunkPos= new Vector3(Mathf.FloorToInt(smoothPosition.x), Mathf.FloorToInt(smoothPosition.y), smoothPosition.z);
                //rigidBody.MovePosition(smoothPosition);
                //rigidBody.MovePosition(smoothPosition);

                transform.position = smoothPosition;

            }

            if (!canMoveRight)
            {
                var verticalExtent = GetComponent<Camera>().orthographicSize;
                var horizontalExtent = verticalExtent * Screen.width / Screen.height;


                if (playerPos.x >
                    transform.position.x + horizontalExtent)
                {
                    switching = true;
                    playerSwitching = true;
                    switchStartTime = Time.time;

                    Player.GetComponent<Player>().frozen = true;
                    Player.GetComponent<Rigidbody2D>().simulated = false;

                    playerVelocity = Player.GetComponent<Rigidbody2D>().velocity;
                    playerVelocity.x = 0;

                    Vector3 playerPosition = Player.transform.position;
                    playerPosition.x += 32;
                    playerSwitchTargetPosition = playerPosition;


                    Vector3 position = transform.position;
                    position.x += 400;
                    switchTargetPosition = position;
                }

            }

            if (!canMoveLeft)
            {
                var verticalExtent = GetComponent<Camera>().orthographicSize;
                var horizontalExtent = verticalExtent * Screen.width / Screen.height;


                if (playerPos.x <
                    transform.position.x - horizontalExtent)
                {
                    switching = true;
                    playerSwitching = true;
                    switchStartTime = Time.time;

                    Player.GetComponent<Player>().frozen = true;
                    Player.GetComponent<Rigidbody2D>().simulated = false;

                    playerVelocity = Player.GetComponent<Rigidbody2D>().velocity;
                    playerVelocity.x = 0;

                    Vector3 playerPosition = Player.transform.position;
                    playerPosition.x -= 32;
                    playerSwitchTargetPosition = playerPosition;

                    Vector3 position = transform.position;
                    position.x -= 400;
                    switchTargetPosition = position;
                }

            }

            if (!canMoveDown)
            {

                var verticalExtent = GetComponent<Camera>().orthographicSize;

                if (playerPos.y <
                    transform.position.y - verticalExtent)
                {
                    if (Player.GetComponent<Player>().isInPassage)
                    {
                        switching = true;
                        playerSwitching = true;
                        switchStartTime = Time.time;

                        Player.GetComponent<Player>().frozen = true;
                        Player.GetComponent<Rigidbody2D>().simulated = false;

                        playerVelocity = Player.GetComponent<Rigidbody2D>().velocity;
                        
                        Vector3 playerPosition = Player.transform.position;
                        playerPosition.y += 32;
                        playerSwitchTargetPosition = playerPosition;

                        Vector3 position = transform.position;
                        position.y -= 224;
                        switchTargetPosition = position;

                    }
                }                
            }

            if (!canMoveUp)
            {

                var verticalExtent = GetComponent<Camera>().orthographicSize;

                if (playerPos.y >
                    transform.position.y + verticalExtent)
                {
                    if (Player.GetComponent<Player>().climbing)
                    {
                        switching = true;
                        playerSwitching = true;
                        switchStartTime = Time.time;
                        Vector3 position = transform.position;
                        position.y += 224;
                        switchTargetPosition = position;

                    }
                }
            }


        }


    }

}
