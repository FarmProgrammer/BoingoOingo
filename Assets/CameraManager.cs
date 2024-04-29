using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public List<Transform> cameraPositions;
    private float camInterval = 8f;
    public Transform player;
    public Transform cam;

    // Update is called once per frame
    void Update()
    {
        float abspos = player.position.y + 4;
        cam.position = new Vector3(0, cameraPositions[Mathf.FloorToInt(abspos / 8)].position.y, -10f);
    }
}
