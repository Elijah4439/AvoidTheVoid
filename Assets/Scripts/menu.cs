using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  
using TMPro;
using UnityEngine.UI;

public class menu : MonoBehaviour
{
    public TextMeshProUGUI highestScore;
    public TextMeshProUGUI numGem;

    public GameObject Black;
    Image black_panel;
    public GameObject SaveData;
    public AudioSource Sound1;


    void Start(){
        int [] saved_data = SaveData.GetComponent<SaveData>().LoadFromJson();
        highestScore.text = ""+saved_data[0];
        numGem.text = "" + saved_data[1];
        black_panel = Black.GetComponent<Image>();

        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }


    IEnumerator fadingProcess(){    
        //yield return new WaitForSeconds(1f);
        // fade from opaque to transparent
            // loop over 1 second backwards
        for (float i = 255; i >= 0; i -= Time.deltaTime)
        {
            // set color with i as alpha
            black_panel.color = new Color(0, 0, 0, i);
            yield return null;
        }
        // fade from transparent to opaque
        /*else
        {
            // loop over 1 second
            for (float i = 0; i <= 255; i += Time.deltaTime)
            {
                // set color with i as alpha
                black_panel.color = new Color(0, 0, 0, i);
                yield return null;
            }
        }*/
    }


     public static IEnumerator audioFadeOut (AudioSource audioSource, float FadeTime) {
        float startVolume = audioSource.volume;
 
        while (audioSource.volume > 0) {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;
            yield return null;
        }
        audioSource.Stop();
        SceneManager.LoadScene("tile_run");  
    }


    public void StartGame(){
        print("pressed");
        StartCoroutine(fadingProcess());

        IEnumerator fadeSound1 = audioFadeOut (Sound1, 3f);
        StartCoroutine(fadeSound1);
        //SceneManager.LoadScene("tile_run"); 
    }
}
