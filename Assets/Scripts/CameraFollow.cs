using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Camera cam;
    [SerializeField] private Transform jet;
    [SerializeField] Vector3 camPos;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }
    private void Update()
    {
        cam.transform.position = jet.transform.position + camPos;
    }
}
