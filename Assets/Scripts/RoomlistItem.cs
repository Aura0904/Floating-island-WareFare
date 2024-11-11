using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoomlistItem : MonoBehaviour
{
    [SerializeField] TMP_Text text;

    public RoomInfo Info;
    public void SetUp(RoomInfo _info)
    {
        Info = _info;
        text.text = _info.Name;
    }

    public void OnClick()
    {
        Launcher.instance.JoinRoom(Info);
    }
}
