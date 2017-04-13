using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    public static AudioManager audioManager = null;

    void Start()
    {
        audioManager = this;
    }
    void Awake()
    {

        if (audioManager == null)
            audioManager = this;
        else if (audioManager != this)
            Destroy(gameObject);
        DontDestroyOnLoad(this);
    }
}
