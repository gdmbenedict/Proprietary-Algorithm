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
        cityGenerator.GenerateCity(cityGenerator.seed);
    }

    // Update is called once per frame
    void Update()
    {
        //Regenerate city scape on space bar down
        if (Input.GetKeyDown(KeyCode.Space))
        {
            cityGenerator.GenerateCity(cityGenerator.seed);
        }
    }
}
