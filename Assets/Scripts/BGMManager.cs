using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BGMManager : MonoBehaviour
{

    private string[] scene_list = {
        "KidHouseBGM",
        "WW2BGM",
        "WW2BarBGM",
        "NightStreetBGM",
        "GrandpaHouseBGM",
        "BackyardBGM",
        "SexyTimeClosetBGM",
        "BabyRoomGrampsBGM"
    };
    private Dictionary<string, GameObject> scene_bgms =
        new Dictionary<string, GameObject>();

    // Use this for initialization
    void Start()
    {
        foreach (string scene_name in scene_list)
        {
            GameObject bgm = GameObject.Find(scene_name);
            bgm.GetComponent<AudioSource>().volume = 0;
            scene_bgms.Add(scene_name, bgm);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
