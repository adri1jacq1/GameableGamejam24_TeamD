using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag : MonoBehaviour {

    private Vector3 screenPoint;
    private Vector3 offset;
    private float depth;
    public Vector3 initScale;
    public bool isHovering = false;



    public TurnSystem system;

    public Hand hand;

    void OnMouseDown() {
        if (TurnSystem.isPlayerTurn) {
            hand.RemoveCard(gameObject);
            offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
            depth = transform.position.z;
            transform.position = new Vector3(transform.position.x, transform.position.y, -2f);
        }
    }

    void OnMouseDrag() {
        if (TurnSystem.isPlayerTurn) {
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z - 2f);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
            transform.position = curPosition;
        }

    }


    void OnMouseUp() {
        if (TurnSystem.isPlayerTurn) {

            if (Input.mousePosition.y > 300) {
                TurnSystem.isPlayerTurn = false;
                StartCoroutine(system.UseCard(Input.mousePosition.x, gameObject));
                
            } else {
                hand.AddCard(gameObject);
                transform.position = new Vector3(transform.position.x, transform.position.y, depth);
            }
        }
    }


    void OnMouseEnter() {
        if (TurnSystem.isPlayerTurn) {
            if (!isHovering) {
                isHovering = true;
            }
        }
    }

    void OnMouseExit() {
        if (TurnSystem.isPlayerTurn) {
            isHovering = false;
        }
    }


}
