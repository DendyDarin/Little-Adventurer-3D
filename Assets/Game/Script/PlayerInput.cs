using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float horizontalInput;
    public float verticalInput;
    public bool mouseButtonDown;

    // Update is called once per frame
    void Update()
    {
        if (!mouseButtonDown && Time.timeScale != 0)
        {
            mouseButtonDown = Input.GetMouseButtonDown(0);
        }
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void OnDisable() 
    {
        mouseButtonDown = false;
        horizontalInput = 0;
        verticalInput = 0;
    }
}
