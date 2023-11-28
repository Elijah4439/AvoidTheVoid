using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using TMPro;

public class SaveData: MonoBehaviour
{
    public Account player = new Account();
    public TextMeshProUGUI numGem;


    void Start(){
        int [] temp = new int [2];
        temp = LoadFromJson();
        player.highest_score = temp[0];
        player.gem_num = temp[1];
    }

    
    public void setHighestScore(int score){
        player.highest_score = score;
        player.gem_num += (int)(score/1000) * 5;
        SaveToJson();
    }


    public void addGemNum(int gemNum){
        player.gem_num += gemNum;
        SaveToJson();
    }


    public void deductGem(){
        player.gem_num -= 5;
        SaveToJson();
    }


    public void add5Gem(){
        //for testing button
        print("in add5gem, player.gemnum: " + player.gem_num);
        player.gem_num += 5;
        print("after added: " + player.gem_num);
        SaveToJson();
        numGem.text = "" + player.gem_num;
    }


    public void SaveToJson(){
        print("in savetojson, player score: " + player.highest_score + ", player gemNum: " + player.gem_num);
        if(player.highest_score < 0){
            player.highest_score = 0;
        }
        if(player.gem_num < 0){
            player.gem_num = 0;
        }
        string score = JsonUtility.ToJson(player);
        string filePath = Application.persistentDataPath + "/score.json";
        //Debug.Log(temp);
        System.IO.File.WriteAllText(filePath, score);
    }


    public int[] LoadFromJson(){
        string filePath = Application.persistentDataPath + "/score.json";
        string temp = System.IO.File.ReadAllText(filePath);
        Debug.Log(temp);
        Account player = JsonUtility.FromJson<Account>(temp);

        int [] a = new int [2];
        a[0] = player.highest_score;
        a[1] = player.gem_num;

        return a;
    }
}


[System.Serializable]
public class Account{
    public int highest_score;
    public int gem_num;
}