using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// It is the class that works on character buttons.
/// </summary>
public class CharacterButtonScript : MonoBehaviour
{
    private Button thisButton;
    private Text thisChildrenText;
    private string characterName;

    private void OnEnable()
    {
        thisButton = this.gameObject.GetComponent<Button>();
        thisButton.onClick.AddListener(OnSelectCharacter);
        thisChildrenText = this.gameObject.GetComponentInChildren<Text>();
    }

    /// <summary>
    /// It is the method that sets the character name.
    /// </summary>
    /// <param name="_characterName">It is the variable that we specify the character name.</param>
    public void SetCharacterName(string _characterName)
    {
        characterName = _characterName;
        thisChildrenText.text = _characterName;
    }

    /// <summary>
    /// It is a method for selecting characters.
    /// </summary>
    private void OnSelectCharacter()
    {
        PlayerPrefs.SetString("CharacterName", characterName);
        Debug.Log(characterName);
    }

}
