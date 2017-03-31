using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadLevel : MonoBehaviour {

    public static void LoadScene(string Destination)
    {
        Application.LoadLevel(Destination);
    }

}
