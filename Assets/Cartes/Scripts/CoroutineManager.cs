using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineManager : MonoBehaviour {
    public static CoroutineManager Instance;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    public void RunCoroutine(IEnumerator coroutine) {
        StartCoroutine(coroutine);
    }
}
