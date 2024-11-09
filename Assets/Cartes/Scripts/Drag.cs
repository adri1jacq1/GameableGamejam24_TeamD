using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag : MonoBehaviour {

    private Vector3 screenPoint;
    private Vector3 offset;
    private float depth;
    public Vector3 initScale;
    public bool isHovering = false;

    public Hand hand;


    void OnMouseDown() {
        hand.RemoveCard(gameObject);
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
        depth = transform.position.z;
        //transform.position = new Vector3(transform.position.x, transform.position.y, -2f);
        transform.eulerAngles = new Vector3(0f, 0f, 0f);
    }

    void OnMouseDrag() {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z - 2f);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        transform.position = curPosition;

    }

    void OnMouseUp() {
        hand.AddCard(gameObject);
        transform.position = new Vector3(transform.position.x, transform.position.y, depth);
    }

    void OnMouseOver() {
        if (!isHovering) {
            isHovering = true;
            initScale = transform.localScale;
        }
    }


    void OnMouseExit() {
        transform.localScale = initScale;
        isHovering = false;
    }
}
