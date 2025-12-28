using System;
using UnityEngine;

public class Floating : MonoBehaviour
{
    [SerializeField] private float amplitude = 0.5f;
    [SerializeField] private float frequency = 1f;
    
    private float startPos;
    private Vector3 curPos;
    
    private bool floatable = true;

    private void Awake()
    {
        startPos = transform.position.y;
    }

    private void Update()
    {
        if (floatable)
        {
            curPos = transform.position;
            curPos.y += amplitude * Mathf.Sin(Time.time * frequency);
            transform.position = curPos;
        }
        else
        {
            curPos = transform.position;
            curPos.y = startPos;
            transform.position = Vector3.Lerp(transform.position, curPos, Time.deltaTime);
        }
    }

    public void SetFloatable(bool value)
    {
        floatable = value;
    }
}
