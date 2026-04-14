using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SaveData
{
    public CharacterType ActiveCharacterType;
    public CharacterProgress SwordProgress;
    public CharacterProgress MageProgress;
    public List<string> GlobalChoiceIds = new();
}