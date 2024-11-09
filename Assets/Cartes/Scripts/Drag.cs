using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag : MonoBehaviour {

    private Vector3 screenPoint;
    private Vector3 offset;
    private float depth;

    public Hand hand;

    void OnMouseDown() {
        hand.RemoveCard(gameObject);
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
        depth = transform.position.z;
        //transform.position = new Vector3(transform.position.x, transform.position.y, -2f);
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
}
