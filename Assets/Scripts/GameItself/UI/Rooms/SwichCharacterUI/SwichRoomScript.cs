using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwichRoomScript : MonoBehaviour
{

    [SerializeField]
    private Button SwichCharacterButton;
    [SerializeField]
    private GameObject SwichCharacterCanvas;
    [SerializeField]
    private Text characterName;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetString("CharacterName")==null)
        {
            PlayerPrefs.SetString("CharacterName", SwichCharacterCanvas.GetComponentInChildren<CharacterListing>().characterList[0]); 
        }

        SwichCharacterButton.onClick.AddListener(OnSwichCharacterButton);
        characterName.text = PlayerPrefs.GetString("CharacterName");
    }


    private void OnSwichCharacterButton()
    {
        SwichCharacterCanvas.SetActive(true);
    }

}
