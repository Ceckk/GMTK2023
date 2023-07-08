using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapToDestroy : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit info;
            if (Physics.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward, out info, 100))
            {
                if (info.collider.CompareTag("Obstacle"))
                {
                    Destroy(info.collider.gameObject);
                }
            }
        }
    }
}
