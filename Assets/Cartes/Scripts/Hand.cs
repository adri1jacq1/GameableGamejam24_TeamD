using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    private List<GameObject> hand = new();
    public float spacing = 0;
    public float rotFactor = 0;
    public float yBalancing = 0;
    public float yUpHovered = 0;

    private void updateHand() {
        for (int index = 0; index < hand.Count; index++) {
            GameObject card = hand[index];
            float dynamicSpacing = spacing / hand.Count;
            float dynamicRot = rotFactor / hand.Count;
            float xPos = (transform.position.x + index * dynamicSpacing) - (hand.Count - 1) * dynamicSpacing / 2;
            float yPos;
            Drag drag = card.GetComponent<Drag>();
            if (drag.isHovering) {
                yPos = (transform.position.y - Mathf.Abs((index * yBalancing) - (hand.Count - 1) * yBalancing / 2)) + yUpHovered;
                card.transform.localScale = new Vector3(drag.initScale.x * 1.5f, drag.initScale.y * 1.5f, drag.initScale.z);
            } else {
                yPos = (transform.position.y - Mathf.Abs((index * yBalancing) - (hand.Count - 1) * yBalancing / 2));
                card.transform.localScale = drag.initScale;
            }
            card.transform.position = new Vector3(xPos, yPos, transform.position.z - index*0.1f);
            card.transform.eulerAngles = new Vector3(0f, 0f, index * dynamicRot - (hand.Count - 1) * dynamicRot / 2);
        }
    }

    public void AddCard(GameObject card) {
        hand.Add(card);
    }


    public void RemoveCard(GameObject card) {
        hand.Remove(card);
    }

    public void Update() {
        updateHand();
    }
}
