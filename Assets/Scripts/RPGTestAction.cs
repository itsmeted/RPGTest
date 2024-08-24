using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class RPGTestAction
{
    [SerializeField]
    string m_name;

    [Tooltip("Specifies whether the action should be used upon an enemy or upon a ally")]
    [SerializeField]
    bool m_offensive = true;

    [Tooltip("The amount to change health per update (i.e if duration is 5 and delta is 2 then the total change is 5 x 2 = 10")]
    [SerializeField]
    float m_healthDelta;

    [Tooltip("The amount to change attack per update (i.e if duration is 5 and delta is 2 then the total change is 5 x 2 = 10")]
    [SerializeField]
    float m_attackDelta;

    [Tooltip("The amount to change defense per update (i.e if duration is 5 and delta is 2 then the total change is 5 x 2 = 10")]
    [SerializeField]
    float m_defenseDelta;

    [Tooltip("The amount to change speed per update (i.e if duration is 5 and delta is 2 then the total change is 5 x 2 = 10")]
    [SerializeField]
    int m_speedDelta;

    [Range(0, 100)]
    [SerializeField]
    int m_duration;

    public string Name
    {
        get { return m_name; }
    }

    public bool Offensive
    {
        get { return m_offensive; }
        set { m_offensive = value; }
    }

    public float HealthDelta
    {
        get { return m_healthDelta; }
        set { m_healthDelta = value; }
    }

    public float AttackDelta
    {
        get { return m_attackDelta; }
        set { m_attackDelta = value; }
    }

    public float DefenseDelta
    {
        get { return m_defenseDelta; }
        set { m_defenseDelta = value; }
    }

    public int SpeedDelta
    {
        get { return m_speedDelta; }
        set { m_speedDelta = value; }
    }

    public int Duration
    {
        get { return m_duration; }
        set { m_duration = value; }
    }

    public RPGTestAgent GetTarget(RPGTestAgent[] allies, RPGTestAgent[] enemies)
    {
        // could apply some smarter heuristics here for target selection
        if (m_offensive)
        {
            return FindAliveAgent(enemies);
        }
        return FindAliveAgent(allies);
    }

    RPGTestAgent FindAliveAgent(RPGTestAgent[] agents)
    {
        // try not to create garbage so we just iterate twice
        // once to get the count of alive agents and once to select them
        int numAlive = 0;
        for (int i = 0; i < agents.Length; ++i)
        {
            if (agents[i].IsAlive)
            {
                ++numAlive;
            }
        }

        if (numAlive == 0)
        {
            return null;
        }

        int chosenIndex = Random.Range(0, numAlive);
        int currentIndex = 0;
        for (int i = 0; i < agents.Length; ++i)
        {
            if (agents[i].IsAlive)
            {
                if (chosenIndex == currentIndex)
                {
                    return agents[i];
                }
                ++currentIndex;
            }
        }

        return null;
    }

    public void Simulate(RPGTestAgent sourceAgent, RPGTestAgent targetAgent)
    {
        if (m_healthDelta < 0.0f)
        {
            // formula for damage is (sourceAgent.Attack - targetAgent.Defense) / 10.0f
            // make sure we always do a little bit of damage
            float attackDamage = Mathf.Max(sourceAgent.Attack - targetAgent.Defense, 0.0f) / 10.0f * m_healthDelta;
            targetAgent.Health = Mathf.Clamp(targetAgent.Health + attackDamage, 0, 100);
        }
        else
        {
            targetAgent.Health = Mathf.Clamp(targetAgent.Health + m_healthDelta, 0, 100);
        }

        // don't allow attack to reach zero otherwise we might end up in a stalemate
        targetAgent.Attack  = Mathf.Max(0.0f, targetAgent.Attack  + m_attackDelta);

        targetAgent.Defense = Mathf.Clamp(targetAgent.Defense + m_defenseDelta, 0.0f, 100.0f);
        targetAgent.Speed   = Mathf.Max(0, targetAgent.Speed   + m_speedDelta);
    }
}
