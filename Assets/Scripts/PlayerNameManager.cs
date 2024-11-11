using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;

public class PlayerNameManager : MonoBehaviour
{
    [SerializeField] TMP_InputField UserNameInput;

    private void Start()
    {
        if (PlayerPrefs.HasKey("username"))
        {
            UserNameInput.text = PlayerPrefs.GetString("username");
            PhotonNetwork.NickName = PlayerPrefs.GetString("username");
        }
        else
        {
            UserNameInput.text = "player" + Random.Range(0, 10000).ToString("0000");
            OnUsernameInputValueChanged();
        }
    }

    public void OnUsernameInputValueChanged()
    {
        PhotonNetwork.NickName = UserNameInput.text;
        PlayerPrefs.SetString("username", UserNameInput.text);
    }
}
