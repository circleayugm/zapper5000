using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneCtrl : MonoBehaviour
{
    [SerializeField]
    public MainSceneCtrl MAIN;
    [SerializeField]
    public ObjectManager MANAGE;
    [SerializeField]
    public ObjectCtrl OBJ_TONARI_NIKI_HAND;
    [SerializeField]
    public ObjectCtrl OBJ_WILL;

    int count = 0;


    Vector3 pos = new Vector3(0, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        MAIN = GameObject.Find("root_game").GetComponent<MainSceneCtrl>();
        MANAGE = GameObject.Find("root_game").GetComponent<ObjectManager>();
        while(MANAGE.SW_BOOT == false)
        {

		}
    }

    // Update is called once per frame
    void Update()
    {
        if (count == 0)
        {
        }


        if (((count) % 50) == 0)
        {
            MANAGE.Set(ObjectManager.TYPE.WILL, 0, new Vector3(Random.Range(-4f,4f), Random.Range(-3f,3f), 30), 0, 0);

        }
        





        count++;
    }
}
