using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scoree : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Text>().text = "Your score: " + PlayerPrefs.GetInt("score").ToString();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
