using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    public static AudioManager audioManager;

    void Start()
    {
        audioManager = this;
    }
    void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
