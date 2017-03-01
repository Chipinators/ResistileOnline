using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadLevel : MonoBehaviour {

    public string Destination;

    public void LoadScene()
    {
        Application.LoadLevel(Destination);
    }

}
