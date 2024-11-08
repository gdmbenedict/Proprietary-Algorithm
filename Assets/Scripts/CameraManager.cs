using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("Camera Movement Variables")]
    [SerializeField]
    [Range(0.5f, 5f)] private float transitionTime;
    [SerializeField] 
    [Range(-90,90)] private float viewingAngle;
    [SerializeField]
    [Range(0f, 1f)] private float HBSP; //Horizontal Buffer Space Percentage
    [SerializeField]
    [Range(0f, 1f)] private float VBSP; //Verticle Buffer Space Percentage
    [SerializeField] private float minHeight = 10f;

    private float currentRotation;
    private bool rotatingCamera = false;
    private float rotationVelocity = 0f;

    // Start is called before the first frame update
    void Start()
    {
        currentRotation = transform.rotation.x;      
    }

    // Update is called once per frame
    void Update()
    {
        currentRotation = Mathf.SmoothDampAngle(currentRotation, viewingAngle, ref rotationVelocity, transitionTime);
        transform.rotation = Quaternion.Euler(currentRotation, 0f, 0f);
    }

    public void SetViewingAngle(float targetAngle)
    {
        viewingAngle = targetAngle;
    }

    public IEnumerator moveCameraToPosition(Vector2 position)
    {
        float timer = 0f;

        //calculating actual target position
        float targetPosZ = position.x * (1 + HBSP); //multiply with percentage wanted for the buffer zone
        float targetPosY = position.y * (1 + VBSP); //multiply with percentage wanted for the buffer zone

        if (targetPosY < minHeight)
        {
            targetPosY = minHeight;
        }

        Vector3 targetPos = new Vector3(0, targetPosY, -targetPosZ);
        Vector3 startPos = transform.position;

        while (timer <= transitionTime)
        {
            float t = timer / transitionTime;
            t = EaseInOunt(t);

            transform.position = Vector3.Lerp(startPos, targetPos, t);
            
            timer += Time.deltaTime;
            //Debug.Log("Movement Change still being called");
            yield return null;
        }

        transform.position = targetPos;
    }

    public float EaseInOunt(float t)
    {
        return 0.5f - 0.5f * Mathf.Cos(t * Mathf.PI);
    }
}
