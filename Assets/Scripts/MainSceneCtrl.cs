using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MainSceneCtrl : MonoBehaviour
{
    [SerializeField]
    public MainSceneCtrl MAIN;
    [SerializeField]
    public ObjectManager MANAGE;
    [SerializeField]
    Text MSG_TIME;
    [SerializeField]
    Text MSG_SATS;
    [SerializeField]
    GameObject ROOT_START;
    [SerializeField]
    InputField INP_COPY;
    [SerializeField]
    Text MSG_PRESS_START;
    [SerializeField]
    HandCtrl OBJ_HAND;
    [SerializeField]
    GameObject OBJ_PLAYER;



    // uGUI表示

    enum MODE
    {
        DEMO,
        PLAYING,
        OVER,
    }
    readonly int[] timer_msec = new int[60]
    {
        00,01,03,05,07,09,
        10,12,14,16,17,19,
        20,21,23,25,27,29,
        30,32,34,35,37,39,
        40,41,43,45,47,49,
        50,52,54,56,58,59,
        60,61,63,65,67,69,
        70,72,74,76,78,79,
        80,81,83,85,87,89,
        90,92,94,96,98,99
    };
    static int SATS_START = 10000;   // スタート時の資産





    int count = 0;
    MODE mode = MODE.DEMO;



    string msg_send = "";

    public int player_sats = 10000;
    int player_time = 0;

    ObjectManager.TYPE type = ObjectManager.TYPE.WILL;
    Vector3 pos = new Vector3(0, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        MAIN = GameObject.Find("root_game").GetComponent<MainSceneCtrl>();
        MANAGE = GameObject.Find("root_game").GetComponent<ObjectManager>();
        ROOT_START.gameObject.SetActive(false);
        OBJ_PLAYER.gameObject.SetActive(false);
        while (MANAGE.SW_BOOT == false)
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (mode)
        {
            case MODE.DEMO:
                if (count == 0)
                {
                    ROOT_START.gameObject.SetActive(true);
                }

                if (((count) % 50) == 0)
                {
                    switch (Random.Range(0, 10))
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                            type = ObjectManager.TYPE.WILL;
                            break;
                        case 5:
                        case 6:
                        case 7:
                            type = ObjectManager.TYPE.TONARI_NIKI;
                            break;
                        case 8:
                        case 9:
                            type = ObjectManager.TYPE.JACK;
                            break;
                        default:
                            type = ObjectManager.TYPE.WILL;
                            break;
                    }
                    MANAGE.Set(type, 0, new Vector3(Random.Range(0, 9) - 4, Random.Range(0, 2) - 1, 30), 0, 0);
                }
                // スタート待ち
                if (Input.GetKeyDown(KeyCode.Space) == true)
                {
                    INP_COPY.gameObject.SetActive(false);
                    ROOT_START.gameObject.SetActive(false);
                    mode = MODE.PLAYING;
                    count = -1;
                }

                break;
            case MODE.PLAYING:
                {
                    if (count == 0)
                    {
                        player_time = 0;
                        player_sats = SATS_START;
                        OBJ_PLAYER.gameObject.SetActive(true);
                        OBJ_HAND.Set();
                        MANAGE.ResetAll();

                    }
                    if (((count) % 50) == 0)
                    {
                        switch (Random.Range(0, 10))
                        {
                            case 0:
                            case 1:
                            case 2:
                            case 3:
                            case 4:
                                type = ObjectManager.TYPE.WILL;
                                break;
                            case 5:
                            case 6:
                            case 7:
                                type = ObjectManager.TYPE.TONARI_NIKI;
                                break;
                            case 8:
                            case 9:
                                type = ObjectManager.TYPE.JACK;
                                break;
                            default:
                                type = ObjectManager.TYPE.WILL;
                                break;
                        }
                        MANAGE.Set(type, 0, new Vector3(Random.Range(0, 9) - 4, Random.Range(0, 2) - 1, 30), 0, 0);
                        //MANAGE.Set(type, 0, new Vector3(0,0, 30), 0, 0);
                    }

                    player_time = count;
                    if (player_sats > 100000)
                    {
                        OBJ_PLAYER.gameObject.SetActive(false);
                        OBJ_HAND.Reset();
                        mode = MODE.OVER;
                    }
                    if (player_sats <= 0)
                    {
                        OBJ_PLAYER.gameObject.SetActive(false);
                        OBJ_HAND.Reset();
                        mode = MODE.OVER;
                    }
                }
                break;
            case MODE.OVER:
                {
                    if (count == 0)
                    {
                        if (player_sats <= 0)
                        {
                            msg_send = "CLEAR TIME=" + (player_time / 3600).ToString("D4") + ":" + ((player_time / 60) % 60).ToString("D2") + "." + timer_msec[player_time % 60].ToString("D2") + " / https://howto_nostr.info/zapper5000";
                        }
                        else
                        {
                            msg_send = "Success! / https://howto-nostr.info/zapper5000";
                        }
                        INP_COPY.text = msg_send;
                        INP_COPY.gameObject.SetActive(true);
                    }
                    if (count > 60)
                    {
                        mode = MODE.DEMO;
                        count = -1;
                    }
                    break;

                }



        }




        count++;

        MSG_TIME.text = "TIME=" + (player_time / 3600).ToString("D4") + ":" + ((player_time / 60) % 60).ToString("D2") + "." + timer_msec[player_time % 60].ToString("D2");
        MSG_SATS.text = "Sats=" + player_sats;

    }
}
