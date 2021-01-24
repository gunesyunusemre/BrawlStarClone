using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class allows you to create the necessary buttons.
/// </summary>
public class CharacterListing : MonoBehaviour
{
    //-------------
    [Header("Character Exchange Objects")]
    [SerializeField]
    private Button LeaveButton;
    [SerializeField]
    private GameObject SwichCharacterCanvas;
    [SerializeField]
    private Text characterNameText;
    [SerializeField]
    private Transform content;
    [SerializeField]
    private CharacterButtonScript characterButtonScript;
    //-------------

    //-------------
    [Header("Add New Character")]
    [Tooltip("Used to add new characters. Make sure that there is a character with the same name in the 'Resource' file.")]
    public List<string> characterList;
    List<CharacterButtonScript> Buttons = new List<CharacterButtonScript>();
    //-------------

    private void OnEnable()
    {
        LeaveButton.onClick.AddListener(OnLeaveSwichCharacterCanvas);
        GetCharacterButtons();
    }

    private void OnDisable()
    {
        DestroyCharacterButtons();
    }

    /// <summary>
    /// It is the method that works when the page(Swich Character Canvas) is opened.
    /// Creates buttons.
    /// </summary>
    private void GetCharacterButtons()
    {
        foreach (string characterName in characterList)
        {
            CharacterButtonScript listing = Instantiate(characterButtonScript, content);
            listing.SetCharacterName(characterName);
            Buttons.Add(listing);
        }
    }

    /// <summary>
    /// It is the method that works when the page(Swich Character Canvas) is closed.
    /// Destroys created buttons.
    /// </summary>
    private void DestroyCharacterButtons()
    {
        for (int i = 0; i < Buttons.Count; i++)
        {
            Destroy(Buttons[i].gameObject);
        }
        Buttons.Clear();
    }

    /// <summary>
    /// This method is working when click Leave Button
    /// </summary>
    private void OnLeaveSwichCharacterCanvas()
    {
        characterNameText.text = PlayerPrefs.GetString("CharacterName");
        SwichCharacterCanvas.SetActive(false);
    }

}
