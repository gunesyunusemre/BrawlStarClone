using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class RandomCustomPropertyGenerator : MonoBehaviour
{
   // [SerializeField]
   // private Text _text;


    private ExitGames.Client.Photon.Hashtable _myCustomProperties = new ExitGames.Client.Photon.Hashtable();

    private void SetCustomNumber()
    {
        System.Random rnd = new System.Random();
        int result = rnd.Next(0, 99);

       // _text.text = result.ToString();

        _myCustomProperties["RandomNumber"] = result;
        PhotonNetwork.SetPlayerCustomProperties(_myCustomProperties);
        /*if (result%2==0)
        {
            PlayerPrefs.SetString("Team", "A");
        }
        else
        {
            PlayerPrefs.SetString("Team", "B");
        }*/
    }
 


    public void OnClick_SetCustomNumber()
    {
        SetCustomNumber();
    }

}
