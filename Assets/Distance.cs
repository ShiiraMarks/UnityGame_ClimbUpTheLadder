using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Distance : MonoBehaviour
{

    [SerializeField] Transform DistanceZone;

    [SerializeField] Text DistanceText;

    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(DistanceZone.position, transform.position) - 92;
        DistanceText.text = distance.ToString("F0");
    }
}
