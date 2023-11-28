using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class tile_trigger : MonoBehaviour{

    public AudioSource audio;
    public AudioClip do1;
    public AudioClip re;
    public AudioClip mi;
    public AudioClip so;
    public AudioClip la;

    public AudioClip beat;

    public Color deadColour = new Color(0,0,0);
    public Color[,] palette = new Color[5,5];
    public string[,] colours = new string[5,5]{
        //pink pastel
        {"#ffe5ec","#ffc2d1","#ffb3c6","#ff8fab","#fb6f92"},
        //blue pastel
        {"#edfsfb","#d7e3fc","#ccdbfd","#c1d3fe","#abc4ff"},
        //orange blue
        {"#69d2e7","#a7dbd8","#e0e4cc","#F38630","#FA6900"},
        //red white blue
        {"#E63946","#F7A6A4","#FAF3EE","#45789D","#1D3557"},
        //brick green
        {"#FEDACD","#D06F39","#F8E4A1","#B0C688","#3D5723"}
    };
    public Color gapColour = new Color(18,17,19,199);

    public GameObject player;
    public int score;
    public TextMeshProUGUI score_display;
    public TextMeshProUGUI gem_display;
    public TextMeshProUGUI death_popup_score_display;
    public GameObject SaveData;


    void Start(){
        init_materials();
        audio = GetComponent<AudioSource>();
        gem_display.text = "";
        score = -5;
    }


    void init_materials(){
        System.Random rdm = new System.Random();
        Color newCol;
        //0 to palette_size - 1 is colour palette
        for(int j = 0; j < 5; j++){
            for(int i = 0; i < 5; i++){
                if (ColorUtility.TryParseHtmlString(colours[j,i], out newCol)){
                    palette[j,i] = newCol;
                }
            }
        }
    }

    IEnumerator waitToDeath(){
        print("in wait");
        yield return new WaitForSeconds(0.2f);
        player.GetComponent<generate_level>().playerDead();
    }


    void OnCollisionEnter(Collision collision){
        if(collision.rigidbody.GetComponent<Renderer>().material.color == deadColour){
    			//print("dead");
                death_popup_score_display.text = "" + score;
                int [] saved_data = SaveData.GetComponent<SaveData>().LoadFromJson();
                if(score > saved_data[0]){
                    SaveData.GetComponent<SaveData>().setHighestScore(score);
                }

                StartCoroutine("waitToDeath");
        }else{
            print("score: " + score);
            score += 5;
            print("after adding, score is: " + score);
            score_display.text = "CURRENT SCORE: " + score;

            if(collision.rigidbody.GetComponent<Renderer>().material.color == palette[0,0] || 
            collision.rigidbody.GetComponent<Renderer>().material.color == palette[1,4] ||
            collision.rigidbody.GetComponent<Renderer>().material.color == palette[2,3] ||
            collision.rigidbody.GetComponent<Renderer>().material.color == palette[3,2] ||
            collision.rigidbody.GetComponent<Renderer>().material.color == palette[4,1]){
                audio.PlayOneShot(do1);
            }
            else if(collision.rigidbody.GetComponent<Renderer>().material.color == palette[0,1] || 
            collision.rigidbody.GetComponent<Renderer>().material.color == palette[1,0] ||
            collision.rigidbody.GetComponent<Renderer>().material.color == palette[2,4] ||
            collision.rigidbody.GetComponent<Renderer>().material.color == palette[3,3] ||
            collision.rigidbody.GetComponent<Renderer>().material.color == palette[4,2]){
                audio.PlayOneShot(re);
            }
            else if(collision.rigidbody.GetComponent<Renderer>().material.color == palette[0,2] || 
            collision.rigidbody.GetComponent<Renderer>().material.color == palette[1,1] ||
            collision.rigidbody.GetComponent<Renderer>().material.color == palette[2,0] ||
            collision.rigidbody.GetComponent<Renderer>().material.color == palette[3,4] ||
            collision.rigidbody.GetComponent<Renderer>().material.color == palette[4,3]){
                audio.PlayOneShot(mi);
            }
            else if(collision.rigidbody.GetComponent<Renderer>().material.color == palette[0,3] || 
            collision.rigidbody.GetComponent<Renderer>().material.color == palette[1,2] ||
            collision.rigidbody.GetComponent<Renderer>().material.color == palette[2,1] ||
            collision.rigidbody.GetComponent<Renderer>().material.color == palette[3,0] ||
            collision.rigidbody.GetComponent<Renderer>().material.color == palette[4,4]){
                audio.PlayOneShot(so);
            }
            else if(collision.rigidbody.GetComponent<Renderer>().material.color == palette[0,4] || 
            collision.rigidbody.GetComponent<Renderer>().material.color == palette[1,3] ||
            collision.rigidbody.GetComponent<Renderer>().material.color == palette[2,2] ||
            collision.rigidbody.GetComponent<Renderer>().material.color == palette[3,1] ||
            collision.rigidbody.GetComponent<Renderer>().material.color == palette[4,0]){
                audio.PlayOneShot(la);
            }
            else{
                print("Gap");
                audio.PlayOneShot(beat);
            }
        }
    }

}
