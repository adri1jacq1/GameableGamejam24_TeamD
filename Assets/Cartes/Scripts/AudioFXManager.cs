using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFXManager : MonoBehaviour
{
    public static AudioFXManager Instance;

    public AudioClip punchSound;
    public AudioClip healSound;
    public AudioClip shieldSound;

    public AudioSource source;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    public void PlaySound(string sound) {
        switch (sound) {
            case "shield":
                source.PlayOneShot(shieldSound);
                return;
            case "heal":
                source.PlayOneShot(healSound);
                return;
            case "damage":
                source.PlayOneShot(punchSound);
                return;
            default:
                break;
        }
    }
}
