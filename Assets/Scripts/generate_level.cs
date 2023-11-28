using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generate_level : MonoBehaviour
{

    public GameObject tile;
    public GameObject gap;
    public GameObject start_platform;
    public GameObject player;
    public GameObject tempStart;

    int max_len = 3;
    int max_width = 5;
    int index;
    float tile_x = 3.5f;
    float tile_y = 3.5f;
    float gap_x = 3.5f;
    float gap_y = 3.5f;

    int start_platform_destroyed = 0;
    //Ray ray;
    //RaycastHit hit;


    //0-4 themed colour, 5 death colour, 6 gap colour
    public Material[] Material = new Material[7];
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
    public Color deadColour = new Color(0,0,0,0);
    public Color gapColour = new Color(72, 61, 139);
    public Color[] palette = new Color[5];
    public Color[,] tile_colour = new Color[5,5];
    public GameObject[,] tile_set = new GameObject[6,5];

    public GameObject save_data;


    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        init_materials();

        int [] temp = new int[2];
        temp = save_data.GetComponent<SaveData>().LoadFromJson();

        print("in generate level start(), saved_data is: score: " + temp[0] + ", numgem: " + temp[1]);

        //GameObject tempStart = Instantiate(start_platform, new Vector3(7f,-3.2f,0f), new Quaternion());
        tempStart.transform.position = new Vector3(7f,-2.75f,0f);

        // index = which line
        index = 0;
        for(int i = 0; i < max_len; i++){
            generate_line(index);
            index += 1;
        }
        index = 0;
    }

    void Update(){
        //print("in Update, tempStart: " + tempStart);
        update_tiles();
    }


    void init_materials(){
        System.Random rdm = new System.Random();
        Color newCol;
        //choose which palette to use
        int temp = rdm.Next(0,5);
        //0 to palette_size - 1 is colour palette
        for(int i = 0; i < 5; i++){
            if (ColorUtility.TryParseHtmlString(colours[temp,i], out newCol))
            {
                palette[i] = newCol;
                //print(newCol);
            }
        }

        /*if(ColorUtility.TryParseHtmlString("#332B2B", out newCol)){
            gapColour = newCol;
        }*/
    }


    IEnumerator waitToCallDeathWindow(){
        yield return new WaitForSeconds(0.3f);
        deathWindowPopup();
    }


    void deathWindowPopup(){
        player.GetComponent<death_popup>().onDead();
    }


    public void playerDead(){
        print("in player dead");
        player.GetComponent<acceleratemeter_input>().setCanMove(0);
        StartCoroutine("waitToCallDeathWindow");
    }


    void generate_line(int index){
        //index is y axis, i is x axis
        System.Random rdm = new System.Random();
        //keep track of how many dead tiles are there in the same line, need to have at least one normal tile
        int normal = 5;

        GameObject tempGap = Instantiate(gap, new Vector3(2*gap_x, (index+max_len)*gap_y + (index+max_len+1)*tile_y, 0f),  new Quaternion()); 

        for(int i = 0; i < max_width; i++){
            //generate front row of tile for look
            if(index == 0){
                GameObject tempTile4 = Instantiate(tile, new Vector3(i*tile_x, (index+max_len)*gap_y + (index+max_len)*tile_y, 0f), new Quaternion());
                tile_set[4, i] = tempTile4;

                //GameObject tempGap = Instantiate(gap, new Vector3(i*gap_x, (index+max_len)*gap_y + (index+max_len+1)*tile_y, 0f),  new Quaternion()); 
                //tempGap.GetComponent<MeshRenderer>().material.SetColor("_Color", gapColour);
            }

            //generate tiles for use
            GameObject tempTile = Instantiate(tile, new Vector3(i*tile_x, index*gap_y + index*tile_y, 0f), new Quaternion());
            tile_set[index + 1, i] = tempTile;
            int type = rdm.Next(0,6);
            //not dead tile
            if(type != 5 && type != 2){
            //if(type == 10){
                type = rdm.Next(0,5);
                tempTile.GetComponent<MeshRenderer>().material.SetColor("_Color", palette[type]);
                tile_colour[index, i] = palette[type];
            }
            //dead tile
            else{
                print("normal:" + normal);
                //don't allow dead tile on first row
                if(index == 0){
                    tempTile.GetComponent<MeshRenderer>().material.SetColor("_Color", palette[3]);
                    tile_colour[index, i] = palette[3];
                }else{
                    normal -= 1;
                    if(normal == 0){
                        print("all dead, regenerating...");
                        type = rdm.Next(0,5);
                        tempTile.GetComponent<MeshRenderer>().material.SetColor("_Color", palette[type]);
                        tile_colour[index, i] = palette[type];
                    }
                    //Debug.Log("palette[] size: " + palette.Length);
                    else{
                        tempTile.GetComponent<MeshRenderer>().material.SetColor("_Color", deadColour);
                        tile_colour[index, i] = deadColour;
                    }
                }             
            }
        }
        add_gap(index*gap_y + (index+1)*tile_y);
    }


    void add_gap(float coord){
        GameObject tempGap = Instantiate(gap, new Vector3(2*gap_x, coord, 0f),  new Quaternion()); 
        //tempGap.GetComponent<MeshRenderer>().material.SetColor("_Color", gapColour);
    }


    void update_tiles(){
        if(player.transform.position.y >= 3 && start_platform_destroyed == 0){
            destroy_object(tempStart);
            start_platform_destroyed = 1;

            for(int i = 0; i < max_width; i++){
                //GameObject tempGap = Instantiate(gap, new Vector3(i*gap_x, -3.5f, 0f),  new Quaternion()); 
                GameObject tempTileNeg1 = Instantiate(tile, new Vector3(i*tile_x, -7f, 0f), new Quaternion());
                //tempGap.GetComponent<MeshRenderer>().material.SetColor("_Color", gapColour);
                tile_set[0, i] = tempTileNeg1;
            }
            GameObject tempGap = Instantiate(gap, new Vector3(2*gap_x, -3.5f, 0f),  new Quaternion()); 
        }

        //update row1
        if(player.transform.position.y >= 8 && player.transform.position.y <= 8.5f){
            int normal = 4;
            for(int i = 0; i < max_width; i++){
                System.Random rdm = new System.Random();
                int type = rdm.Next(0,6);
                //not dead tile
                if(type != 5 && type != 2){
                    type = rdm.Next(0,5);
                    //print("before colour: " + tile_colour[1, i]);
                    tile_colour[1, i] = palette[type];
                    //print("after colour: " + tile_colour[1,i]);
                    tile_set[1, i].GetComponent<MeshRenderer>().material.SetColor("_Color", palette[type]);
                    tile_set[4, i].GetComponent<MeshRenderer>().material.SetColor("_Color", palette[type]);
                    tile_colour[4, i] = palette[type];
                }
                //dead tile
                else{
                    normal -= 1;
                    if(normal == 0){
                        print("All dead, regenerating...");
                        type = rdm.Next(0,5);
                        tile_set[1, i].GetComponent<MeshRenderer>().material.SetColor("_Color", palette[type]);
                        tile_set[4, i].GetComponent<MeshRenderer>().material.SetColor("_Color", palette[type]);
                        tile_colour[1, i] = palette[type];
                        tile_colour[4, i] = palette[type];
                        normal = 4;
                    }
                    //Debug.Log("palette[] size: " + palette.Length);
                    else{
                        tile_set[1, i].GetComponent<MeshRenderer>().material.SetColor("_Color", deadColour);
                        tile_set[4, i].GetComponent<MeshRenderer>().material.SetColor("_Color", deadColour);
                        tile_colour[1, i] = deadColour;
                        tile_colour[4, i] = deadColour;
                    }
                }
            }
        }   

        if(player.transform.position.y >=14.8f && player.transform.position.y <= 15.2f){
            int normal = 4;
            for(int i = 0; i < max_width; i++){
                System.Random rdm = new System.Random();
                int type = rdm.Next(0,6);
                //not dead tile
                if(type != 5 && type != 2){
                    type = rdm.Next(0,5);
                    tile_set[2, i].GetComponent<MeshRenderer>().material.SetColor("_Color", palette[type]);
                    tile_colour[2, i] = palette[type];
                }
                //dead tile
                else{
                    normal -= 1;
                    if(normal == 0){
                        print("all Dead, regenerating...");
                        type = rdm.Next(0,5);
                        tile_set[2, i].GetComponent<MeshRenderer>().material.SetColor("_Color", palette[type]);
                        tile_colour[2, i] = palette[type];
                        normal = 4;
                    }
                    //Debug.Log("palette[] size: " + palette.Length);
                    else{
                        tile_set[2, i].GetComponent<MeshRenderer>().material.SetColor("_Color", deadColour);
                        tile_colour[2, i] = deadColour;
                    }
                }
            }
        }

        if(player.transform.position.y >=3 &&player.transform.position.y <=3.5){
            int normal = 4;
            for(int i = 0; i < max_width; i++){
                System.Random rdm = new System.Random();
                int type = rdm.Next(0,6);
                //not dead tile
                if(type != 5 && type != 2){
                    type = rdm.Next(0,5);
                    tile_set[0, i].GetComponent<MeshRenderer>().material.SetColor("_Color", palette[type]);
                    tile_colour[0, i] = palette[type];
                    tile_set[3, i].GetComponent<MeshRenderer>().material.SetColor("_Color", palette[type]);
                    tile_colour[3, i] = palette[type];
                }
                //dead tile
                else{
                    normal -= 1;
                    if(normal == 0){
                        print("all Dead, regenerating...");
                        type = rdm.Next(0,5);
                        tile_set[0, i].GetComponent<MeshRenderer>().material.SetColor("_Color", palette[type]);
                        tile_colour[0, i] = palette[type];
                        tile_set[3, i].GetComponent<MeshRenderer>().material.SetColor("_Color", palette[type]);
                        tile_colour[3, i] = palette[type];
                        normal = 4;
                    }
                    //Debug.Log("palette[] size: " + palette.Length);
                    else{
                        tile_set[0, i].GetComponent<MeshRenderer>().material.SetColor("_Color", deadColour);
                        tile_colour[0, i] = deadColour;
                        tile_set[3, i].GetComponent<MeshRenderer>().material.SetColor("_Color", deadColour);
                        tile_colour[3, i] = deadColour;
                    }
                }
            }
        }
    }


    void destroy_object(GameObject obj){
        print("in destroy_object, obj is: " + obj);
        Destroy(obj);
    }
    
}
