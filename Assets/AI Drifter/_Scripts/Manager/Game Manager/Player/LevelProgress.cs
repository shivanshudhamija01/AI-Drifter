using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelProgress
{
    public int id;
    public int reward;
    public int completed; // 0 = not complete, 1 = complete
}
[System.Serializable]
public class LevelProgressCollection
{
    public List<LevelProgress> levels = new();
}
