﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using Valve.VR.InteractionSystem;

public class TitleScreenBehaviour : MonoBehaviour
{
    public GameObject text;
    private Dictionary<string, GameObject> dict;

    public GameObject htcplayer_prefab;
    public GameObject teleporterPrefab;

    public GameObject nonvrplayer_prefab;
    public GameObject defaultEventSystem_prefab;

    //public GameObject oculusplayer_prefab;
    //public GameObject UIHelpers_prefab;

    void Start()
    {
        dict = new Dictionary<string, GameObject>() {
            { "" , nonvrplayer_prefab },
            { "OpenVR", htcplayer_prefab }
        };
        StartCoroutine(Coroutine());
    }

    IEnumerator Coroutine()
    {
        GameObject new_player = Instantiate(dict[XRSettings.loadedDeviceName]);
        new_player.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        if (XRSettings.loadedDeviceName == "OpenVR")
        {
            XRSettings.enabled = true;
            GameObject.Find("/Ground").GetComponent<TeleportArea>().enabled = true;
            Instantiate(teleporterPrefab);
        }
        yield return new WaitForSeconds(3);
        text.GetComponent<Text>().CrossFadeAlpha(0.0f, 3.0f, false);
        GetComponent<Image>().CrossFadeAlpha(0.0f, 3.0f, false);
        yield return new WaitForSeconds(3);
        gameObject.SetActive(false);
    }
}
