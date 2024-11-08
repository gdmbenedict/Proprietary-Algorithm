using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimManager : MonoBehaviour
{
    [SerializeField] private CityGenerator cityGenerator;
    [SerializeField] private CameraManager cameraManager;

    // Start is called before the first frame update
    void Start()
    {
        Vector2 targetCamPos = new Vector2(cityGenerator.GetLargestSide(), cityGenerator.GetMaxHeight());
        StartCoroutine(cameraManager.moveCameraToPosition(targetCamPos));
        cityGenerator.GenerateCity();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
