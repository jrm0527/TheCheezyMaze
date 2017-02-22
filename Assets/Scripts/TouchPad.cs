using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TouchPad : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public float smoothing;

    private Vector2 origin;
    private Vector2 direction;
    private Vector2 smoothDirection;

    private Vector3 dir;

    void Awake()
    {
        direction = Vector2.zero;
    }
    
    public void OnPointerDown(PointerEventData data)
    {
//        if (!touched)
//        {
//            touched = true;
//            pointerID = data.pointerId;
            origin = data.position;
//        }
    }
    public void OnDrag(PointerEventData data)
    {
//        if (data.pointerId == pointerID)
//        {
            Vector2 currentPosition = data.position;
            Vector2 directionRaw = currentPosition - origin;
            direction = directionRaw.normalized;
        //        }

        smoothDirection = Vector3.MoveTowards(smoothDirection, direction, smoothing);
//        return smoothDirection;

        dir.x = smoothDirection.x;
        dir.z = smoothDirection.y;

        //        dir.x = direction.x;
        //        dir.z = direction.y;

        smoothDirection = Vector3.MoveTowards(smoothDirection, direction, smoothing);

    }
    public void OnPointerUp(PointerEventData data)
    {
//        if (data.pointerId == pointerID)
//        {
            direction = Vector2.zero;
        dir.x = 0;
        dir.z = 0;


//            touched = false;
//        }
    }

    public float Horizontal()
    {
        if (dir.x != 0)
            return dir.x;
        else
            return Input.GetAxis("Horizontal");
    }

    public float Vertical()
    {
        if (dir.z != 0)
            return dir.z;
        else
            return Input.GetAxis("Vertical");
    }
}
