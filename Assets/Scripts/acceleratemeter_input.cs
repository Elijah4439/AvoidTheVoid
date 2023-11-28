using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class acceleratemeter_input : MonoBehaviour
{
    public GameObject player;
    public Vector3 movement;
    public int canMove;
    string deviceType;
    public TextMeshProUGUI gem_display;
    private int acc;

    private int count = 0;


    void Start(){
        player.transform.position = new Vector3(7f,-2.5f,-10f);
        setCanMove(0);
        //give control after 4 secondes: 1 second empty, 3 secondes countdown; player fall from second 0
        StartCoroutine("waitToStart");

        if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            deviceType = "Desktop";
        }
        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            deviceType = "Handheld";
        }
        acc = 1;
    }


    IEnumerator revive(){
        print("revive");
        yield return new WaitForSeconds(4f);
        setCanMove(1);
    }


    IEnumerator waitToStart(){
        print("in wait to start");
        yield return new WaitForSeconds(6f);
        gem_display.text = "";
        setCanMove(1);
    }


    public void setCanMove(int dead){
        if(dead == 1){
            canMove = 1;
        }else{
            canMove = 0;
        }
    }

    void Update()
    {
        if(canMove == 1 &&  deviceType == "Desktop"){
            if(player.transform.position.x <= -1f){
                player.transform.position = new Vector3(-1f,player.transform.position.y,player.transform.position.z);
            }
            if(player.transform.position.x >= 15f){
                player.transform.position = new Vector3(15f,player.transform.position.y,player.transform.position.z);
            }
            if(player.transform.position.y >= 17.89f){
                player.transform.position = new Vector3(player.transform.position.x,-2.95f,player.transform.position.z);
            }
            if (Input.GetKey("space"))
            {
                acc = 2;
                Debug.Log("space key was pressed");
            }else{
                acc = 1;
            }
            movement = new Vector3(acc * Input.GetAxisRaw("Horizontal")* 6f * Time.deltaTime, 6f * Time.deltaTime, 0f);
            transform.Translate(movement);
        }
        //print("deviceType" + deviceType);
        if(canMove == 1 && deviceType == "Handheld"){
            if(player.transform.position.x <= -1f){
                player.transform.position = new Vector3(-1f,player.transform.position.y,player.transform.position.z);
            }
            if(player.transform.position.x >= 15f){
                player.transform.position = new Vector3(15f,player.transform.position.y,player.transform.position.z);
            }
            if(player.transform.position.y >= 17.89f){
                player.transform.position = new Vector3(player.transform.position.x,-2.95f,player.transform.position.z);
            }
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (Input.touchCount > 0 && Input.GetTouch(count).phase == TouchPhase.Began)
                {
                    acc = 4;

                }
                if(Input.GetTouch(count).phase == TouchPhase.Ended){
                    acc = 2;
                    count++;
                }
            }else{
                acc = 2;
            }

            print("input acc: " + Input.acceleration);
            movement = new Vector3(acc * Input.acceleration.x* 6f * Time.deltaTime, 4f * Time.deltaTime, 0f);
            transform.Translate(movement);
        }
    }
}
