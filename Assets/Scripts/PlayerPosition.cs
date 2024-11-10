using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPosition : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        if (SceneController.lastPosition != Vector3.zero && SceneController.lastPosition != Vector3.zero)
        {
            transform.position = SceneController.lastPosition;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
