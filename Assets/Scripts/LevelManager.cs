using UnityEngine;
using System;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    int currentLevel = 1;
    int requiredXP;
    [SerializeField] float xp;

    [SerializeField] AnimationCurve xpRequirementCurve;

    [SerializeField] HatLevelRequirement[] hatRequirements;

    public event Action<int, int> OnXPGain;
    bool xpGained;

    [ContextMenu("Add 100 XP")]
    void Add100XP()
    {
        AddXP(100);
    }

    void Awake()
    {
        Instance = this;

        requiredXP = GetXPToNextLevel();

        CheckForHatUnlock();

        Debug.Log("Level manager awake");
    }

    void LateUpdate()
    {
        if (xpGained) OnXPGain?.Invoke((int)xp, requiredXP);
    }

    public void AddXP(float xpToAdd)
    {
        xpGained = true;

        xp += xpToAdd;

        //Debug.Log($"Adding {xpToAdd}XP, current level: {currentLevel}");
        while (xp >= requiredXP)
        {
            LevelUp();

            //Debug.Log($"Reached level {currentLevel}, XP to next level: {requiredXP}");
        }

        //Debug.Log($"Final level: {currentLevel}");
    }

    void LevelUp()
    {
        currentLevel++;
        xp -= requiredXP;

        requiredXP = GetXPToNextLevel();

        CheckForHatUnlock();
    }

    int GetXPToNextLevel()
    {
        return (int)xpRequirementCurve.Evaluate(currentLevel + 1);
    }

    void CheckForHatUnlock()
    {
        foreach (HatLevelRequirement hatRequirement in hatRequirements)
        {
            if (currentLevel == hatRequirement.level) Book.Instance.UnlockHat(hatRequirement.hat.ID);
        }
    }
}

[System.Serializable]
public class HatLevelRequirement
{
    public int level;
    public HatSO hat;
}