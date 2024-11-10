using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public List<GameObject> hand = new();
    public float spacing = 0;
    public float rotFactor = 0;
    public float yBalancing = 0;
    public float yHovered = 0;
    public float scaleIncrease = 0;

    public Deck deck;

    private void updateHand() {
        for (int index = 0; index < hand.Count; index++) {
            GameObject card = hand[index];
            Drag drag = card.GetComponent<Drag>();
            float dynamicSpacing = spacing / hand.Count;
            float dynamicRot = rotFactor / hand.Count;
            float xPos = (transform.position.x + index * dynamicSpacing) - (hand.Count - 1) * dynamicSpacing / 2;
            float yPos;
            float zPos;
            if (drag.isHovering) {
                yPos = (transform.position.y - yBalancing * Mathf.Abs(index - (-1.0f + hand.Count) / 2)) + yHovered;
                card.transform.localScale = drag.initScale * scaleIncrease;
                zPos = transform.position.z - hand.Count * 0.1f;
            } else {
                yPos = (transform.position.y - yBalancing * Mathf.Abs(index - (-1.0f + hand.Count)/ 2));
                card.transform.localScale = drag.initScale;
                zPos = transform.position.z - index * 0.1f;
            }
            card.transform.position = new Vector3(xPos, yPos, zPos);
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
