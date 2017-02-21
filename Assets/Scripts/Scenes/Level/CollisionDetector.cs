using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{

    public LayerMask collisionMask;

    const float skinWidth = 0.0f;
    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;

    float horizontalRaySpacing;
    float verticalRaySpacing;

    BoxCollider2D boxCollider;
    RaycastOrigins raycastOrigins;
    public CollisionInfo collisions;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        CalculateRaySpacing();
    }

    public void Detect()
    {
        UpdateRaycastOrigins();
        collisions.Reset();

        HorizontalCollisions();
        VerticalCollisions();
    }

    void HorizontalCollisions()
    {
        float rayLength = skinWidth;

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = raycastOrigins.bottomLeft;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, 
                                                 Vector2.left, 
                                                 rayLength + 1, 
                                                 collisionMask);

            Debug.DrawRay(rayOrigin, 
                          Vector2.left * (rayLength + 1), 
                          Color.red);

            if (hit)
            {
                collisions.left = true;
            }
        }

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin,
                                                 Vector2.right,
                                                 rayLength + 1,
                                                 collisionMask);

            Debug.DrawRay(rayOrigin,
                          Vector2.right * (rayLength + 1),
                          Color.red);

            if (hit)
            {
                collisions.right = true;
            }
        }
    }

    void VerticalCollisions()
    {
        //float rayLength = Mathf.Abs(velocity.y) + skinWidth;
        float rayLength = skinWidth;

        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = raycastOrigins.bottomLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, 
                                                 Vector2.down, 
                                                 rayLength + 1, 
                                                 collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.down * (rayLength + 1), Color.red);

            if (hit)
            {
                collisions.below = true;
            }
        }

        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin,
                                                 Vector2.up,
                                                 rayLength + 1,
                                                 collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.up * (rayLength + 1), Color.red);

            if (hit)
            {
                collisions.above = true;
            }
        }
      
    }

    void UpdateRaycastOrigins()
    {
        Bounds bounds = boxCollider.bounds;
        bounds.Expand(skinWidth * -2);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    void CalculateRaySpacing()
    {
        Bounds bounds = boxCollider.bounds;
        bounds.Expand(skinWidth * -2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }

    struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }

    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;

        public bool climbingSlope;
        public bool descendingSlope;
        public float slopeAngle, slopeAngleOld;

        public void Reset()
        {
            above = below = false;
            left = right = false;
            climbingSlope = false;
            descendingSlope = false;

            slopeAngleOld = slopeAngle;
            slopeAngle = 0;
        }
    }
}
