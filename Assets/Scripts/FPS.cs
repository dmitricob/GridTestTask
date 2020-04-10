using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPS : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //gameObject.GetComponent<Text>().text = Mathf.Round(1f / Time.unscaledDeltaTime) + "";
        gameObject.GetComponent<Text>().text = Mathf.Round(Time.frameCount / Time.time) + "";
    }
}
