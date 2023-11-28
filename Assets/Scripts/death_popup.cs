using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class death_popup : MonoBehaviour
{
    public GameObject deathPopup;
    //public Canvas blackCanvas;
    public GameObject countDown;
    public GameObject black;
    public GameObject save_data;
    public GameObject ads;
    public GameObject player;
    public GameObject UI;
    public TextMeshProUGUI continueBtn;
    public TextMeshProUGUI score_display;
    public TextMeshProUGUI gem_display;


    void Start(){
        deathPopup.SetActive(false);
        black.SetActive(false);
        gem_display.text = "";
    }

    public void onDead(){
        deathPopup.SetActive(true);
        //blackCanvas.planeDistance = 90f;
        black.SetActive(true);
        countDown.SetActive(false);
        save_data.GetComponent<SaveData>().SaveToJson();

        refreshPopup();
    }

    private void Restart(){
        SceneManager.LoadScene("tile_run");
    }

    public void Continue(){
        print("continue");
        int [] saved_data = save_data.GetComponent<SaveData>().LoadFromJson();
        //print("in death popup continue, gemnum: " + saved_data[1]);

        if(saved_data[1] < 5){
            ads.GetComponent<InterstitialAd>().LoadAd();
            save_data.GetComponent<SaveData>().add5Gem();
        } 
        else{
            refreshPopup();
            save_data.GetComponent<SaveData>().deductGem();
            deathPopup.SetActive(false);
            black.SetActive(false);
            countDown.SetActive(true);
            gem_display.text = "";
            player.GetComponent<acceleratemeter_input>().StartCoroutine("revive");
            UI.GetComponent<tile_run_UI>().StartCoroutine("startCountDown");
        }
    }

    public void refreshPopup(){
        print("in refreshPopup");
        int [] temp = save_data.GetComponent<SaveData>().LoadFromJson();
        print("temp: " + temp[0] + "; " + temp[1]);
        if(temp[1] <5){
            continueBtn.fontSize = 35; 
            continueBtn.text = "Watch ads to get GEM";
        }else{
            continueBtn.fontSize = 45; 
            continueBtn.text = "Continue!";
        }
        score_display.text = "HIGHEST SCORE: " + temp[0];
        gem_display.text = "GEM: " + temp[1];
    }

    private void Quit(){
        //print("clicked quit");
        SceneManager.LoadScene("start_menu");
    }


    /*private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");
                //
                // YOUR CODE TO REWARD THE GAMER
                // Give coins etc.
                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                break;
        }
    }*/
}
