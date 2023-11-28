using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class tile_run_UI : MonoBehaviour
{

    public TextMeshProUGUI countDown;
    public float period = 0.0f;
    int count = 0;
    string[] timeMessage = {"3", "2", "1", "GO!", ""};
    int startCounting = 0;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("startCountDown");
    }


    IEnumerator startCountDown(){   
        count = 0; 
        //print("in tile run UI ienumerator");
        yield return new WaitForSeconds(1f);
        //displayCountDown();
        startCounting = 1;
    }


    /*public void displayCountDown(){
        //print("in displayCountDown");
        startCounting = 1;
    }*/
    

    void Update(){
        if(startCounting == 1){
            //print("period: " + period + "count: " + count);
            if(period > 1f && count < 5){
                countDown.text = timeMessage[count];
                count += 1;
                period = 0.0f;
            }
            period += UnityEngine.Time.deltaTime;
        }
    }
}
