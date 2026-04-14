using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class GameState
{
    public CharacterType ActiveCharacterType;
    public CharacterProgress SwordProgress;
    public CharacterProgress MageProgress;
    public List<string> GlobalChoiceIds = new();

    public GameState()
    {
        ActiveCharacterType = CharacterType.None;
        SwordProgress = new CharacterProgress(CharacterType.Sword);
        MageProgress = new CharacterProgress(CharacterType.Mage);
    }
}