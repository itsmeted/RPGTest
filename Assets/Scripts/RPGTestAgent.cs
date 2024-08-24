using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class RPGTestAgent
{
    [SerializeField]
    string m_name;

    [SerializeField]
    float m_health;

    [SerializeField]
    float m_attack;

    [SerializeField]
    float m_defense;

    [Tooltip("The lower the value the faster this agent performs actions")]
    [SerializeField]
    int m_speed;

    RPGTestAction m_currentAction = null;
    RPGTestAgent  m_targetAgent = null;

    // since actions are stateless we need to keep track of how long we have
    // done the action for in the case of actions that are not immediate
    int m_elapsedActionTime = 0;

    // time before another action can be performed (after an action is performed this gets set to the speed)
    int m_delayActionTime = 0;

    // we need to keep track of whether the agent was alive for the given update
    // in case this agent dies this frame so we can still perform the action the
    // simulation step that they died on
    public bool IsAlive
    {
        get; set;
    } = true;

    public string Name
    { 
        get { return m_name; }
        set { m_name = value; }
    }

    public float Health
    {
        get { return m_health; }
        set {  m_health = value; }
    }

    public float Attack
    {
        get { return m_attack; }
        set { m_attack = value; }
    }

    public float Defense
    {
        get { return m_defense; }
        set { m_defense = value; }
    }

    public int Speed
    {
        get { return m_speed; }
        set { m_speed = value; }
    }

    public RPGTestAgent()
    {
    }

    public RPGTestAgent(RPGTestAgent prototype, int index)
    {
        m_name = $"{prototype.m_name}{index}";
        m_health = prototype.m_health;
        m_attack = prototype.m_attack;
        m_defense = prototype.m_defense;
        m_speed = prototype.m_speed;
        m_delayActionTime = m_speed;
    }

    public void Simulate(RPGTestAgent[] allies, RPGTestAgent[] enemies)
    {
        if (!IsAlive)
        {
            return;
        }

        if (m_currentAction == null)
        {
            // we are waiting 
            if (m_delayActionTime < m_speed)
            {
                ++m_delayActionTime;
                return;
            }

            m_currentAction = ChooseAction(allies, enemies);

            // set this to 1 since the first update we actually perform the action
            m_elapsedActionTime = 1;

            m_targetAgent = m_currentAction.GetTarget(allies, enemies);
            if (m_targetAgent == null)
            {
                m_currentAction = null;
                Debug.LogError("{m_name} Could not find alive agent to perform action!");
                return;
            }

            if (m_currentAction.Duration > 0)
            {
                //Debug.Log($"{m_name} starts performing action {m_currentAction.Name} on {m_targetAgent.Name} at time {RPGTestGame.Instance.GameTime}!\n{this}\n{m_targetAgent}");
                RPGTestGame.Instance.AddActionText(this, $"Start: {m_name} -> {m_targetAgent.Name} {m_currentAction.Name}");
            }
        }
        else
        {
            ++m_elapsedActionTime;
        }

        m_currentAction.Simulate(this, m_targetAgent);

        // if the action is finished then we delay choosing the next action until
        // we have waited a certain amount of time based on speed
        if (m_elapsedActionTime >= m_currentAction.Duration || !m_targetAgent.IsAlive)
        {
            if (!m_targetAgent.IsAlive)
            {
                //Debug.Log($"{m_name} done performing action {m_currentAction.Name} on {m_targetAgent.Name} due to death at time {RPGTestGame.Instance.GameTime}!\n{this}\n{m_targetAgent}");
                RPGTestGame.Instance.AddActionText(this, $"End: {m_name} -> {m_targetAgent.Name} {m_currentAction.Name} due to Death!");
            }
            else if (m_currentAction.Duration > 0)
            {
                //Debug.Log($"{m_name} done performing action {m_currentAction.Name} on {m_targetAgent.Name} at time {RPGTestGame.Instance.GameTime}!\n{this}\n{m_targetAgent}");
                RPGTestGame.Instance.AddActionText(this, $"End: {m_name} -> {m_targetAgent.Name} {m_currentAction.Name}");
            }
            else
            {
                // immediate action
                //Debug.Log($"{m_name} performed action {m_currentAction.Name} on {m_targetAgent.Name} at time {RPGTestGame.Instance.GameTime}!\n{this}\n{m_targetAgent}");
                RPGTestGame.Instance.AddActionText(this, $"Now: {m_name} -> {m_targetAgent.Name} {m_currentAction.Name}");
            }

            m_currentAction = null;
            m_targetAgent = null;
            m_elapsedActionTime = 0;
            m_delayActionTime = 0;
        }
    }

    public bool UpdateIsAlive()
    {
        if (m_health <= 0)
        {
            IsAlive = false;
            return false;
        }
        return true;
    }

    public override string ToString()
    {
        if (m_health == 0)
        {
            return $"<b>{m_name} (DEAD)</b>:\nhealth = {m_health}\nattack = {m_attack}, defense = {m_defense}, speed = {m_speed}";
        }

        if (m_currentAction != null)
        {
            return $"<b>{m_name}</b>:\nhealth = {m_health}\nattack = {m_attack}, defense = {m_defense}, speed = {m_speed}\n{m_currentAction.Name}: {m_elapsedActionTime} / {m_currentAction.Duration}";
        }
        else
        {
            return $"<b>{m_name}</b>:\nhealth = {m_health}\nattack = {m_attack}, defense = {m_defense}, speed = {m_speed}\nWaiting: {m_delayActionTime} / {m_speed}";
        }
    }

    RPGTestAction ChooseAction(RPGTestAgent[] allies, RPGTestAgent[] enemies)
    {
        // this could be significantly smarter based on the known states of the allies
        // and the known states of the enemies, but for right now we just choose randomly
        int actionIndex = Random.Range(0, RPGTestGame.Instance.Configuration.Actions.Length);
        return RPGTestGame.Instance.Configuration.Actions[actionIndex];
    }
}
