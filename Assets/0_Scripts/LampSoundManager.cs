using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampSoundManager : MonoBehaviour {
    public AudioSource[] audioSources;
    public AudioClip[] audioClips;

    // 0-4 : right-left-top-down
    public void TurnOnSound(int light) {
        audioSources[light].clip = audioClips[0];
        audioSources[light].Play();
    }

    // 0-4 : right-left-top-down
    public void TurnOffSound(int light) {
        audioSources[light].clip = audioClips[1];
        audioSources[light].Play();
    }
}
