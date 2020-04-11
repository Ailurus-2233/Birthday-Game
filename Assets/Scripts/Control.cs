using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour {

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
    }

    private void OnMouseDown() {
        switch (name) {
            case "Left":
                Global.move = -1;
                break;
            case "Right":
                Global.move = 1;
                break;
            case "Jump":
                Global.isJump = true;
                break;
        }
    }

    private void OnMouseUp() {
        switch (name) {
            case "Left":
            case "Right":
                Global.move = 0;
                break;
            case "Jump":
                Global.isJump = false;
                break;
        }
    }
}