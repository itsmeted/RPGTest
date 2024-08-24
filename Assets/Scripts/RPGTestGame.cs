using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RPGTestGame : MonoBehaviour
{
    [SerializeField]
    RPGTestConfiguration m_configuration;

    [SerializeField]
    int                  m_gameTime = 0;

    [SerializeField]
    RPGTestAgent[] m_players;

    [SerializeField]
    RPGTestAgent[] m_enemies;

    [Header("UI")]
    [SerializeField]
    TMP_Text m_timeText;

    [SerializeField]
    Slider  m_gameSpeedSlider;

    [SerializeField]
    AgentListItem m_prototypePlayerAgentListItem = null;

    [SerializeField]
    AgentListItem m_prototypeEnemyAgentListItem = null;

    [SerializeField]
    ActionListItem m_prototypeActionListItem = null;

    [SerializeField]
    GameObject m_playerWinsGameObject = null;

    [SerializeField]
    GameObject m_enemyWinsGameObject = null;

    [SerializeField]
    GameObject m_drawGameObject = null;


    float m_elapsedTime = 0.0f;
    bool  m_gameOver = false;

    List<AgentListItem> m_agentListItems = new List<AgentListItem>();
    List<ActionListItem> m_actionListItems = new List<ActionListItem>();


    static RPGTestGame s_instance = null;

    public RPGTestConfiguration Configuration
    {
        get { return m_configuration; }
    }

    public int GameTime
    {
        get { return m_gameTime; }
    }

    public static RPGTestGame Instance
    {
        get { return s_instance; }
    }

    // Start is called before the first frame update
    void Start()
    {
        s_instance = this;

        m_gameSpeedSlider.SetValueWithoutNotify(m_configuration.SimulationDelay);
        m_players = new RPGTestAgent[m_configuration.NumPlayers];
        m_enemies = new RPGTestAgent[m_configuration.NumEnemies];

        m_prototypePlayerAgentListItem.gameObject.SetActive(false);
        m_prototypeEnemyAgentListItem.gameObject.SetActive(false);
        m_prototypeActionListItem.gameObject.SetActive(false);

        m_playerWinsGameObject.SetActive(false);
        m_enemyWinsGameObject.SetActive(false);
        m_drawGameObject.SetActive(false);

        for (int i = 0; i < m_players.Length; ++i)
        {
            m_players[i] = new RPGTestAgent(m_configuration.PrototypePlayer, i);
            GameObject go = Instantiate(m_prototypePlayerAgentListItem.gameObject, m_prototypePlayerAgentListItem.transform.parent);
            go.SetActive(true);
            AgentListItem agentListItem = go.GetComponent<AgentListItem>();
            agentListItem.Initialize(m_players[i]);
            m_agentListItems.Add(agentListItem);
        }

        for (int i = 0; i < m_enemies.Length; ++i)
        {
            m_enemies[i] = new RPGTestAgent(m_configuration.PrototypeEnemy, i);
            GameObject go = Instantiate(m_prototypeEnemyAgentListItem.gameObject, m_prototypeEnemyAgentListItem.transform.parent);
            go.SetActive(true);
            AgentListItem agentListItem = go.GetComponent<AgentListItem>();
            agentListItem.Initialize(m_enemies[i]);
            m_agentListItems.Add(agentListItem);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_gameOver)
        {
            return;
        }

        if (m_configuration.SimulationDelay == RPGTestConfiguration.PAUSE_TIME)
        {
            m_timeText.text = $"Time: {m_gameTime} PAUSED";
            return;
        }

        m_elapsedTime += Time.deltaTime;
        while(m_elapsedTime >= m_configuration.SimulationDelay)
        {
            for (int i = 0; i < m_players.Length; ++i)
            {
                m_players[i].Simulate(m_players, m_enemies);
            }

            for (int i = 0; i < m_enemies.Length; ++i)
            {
                m_enemies[i].Simulate(m_enemies, m_players);
            }

            bool hasAlivePlayers = false;
            for (int i = 0; i < m_players.Length; ++i)
            {
                hasAlivePlayers |= m_players[i].UpdateIsAlive();
            }

            bool hasAliveEnemies = false;
            for (int i = 0; i < m_enemies.Length; ++i)
            {
                hasAliveEnemies |= m_enemies[i].UpdateIsAlive();
            }

            for (int i = 0; i < m_agentListItems.Count; ++i)
            {
                m_agentListItems[i].Refresh();
            }

            if (!hasAlivePlayers && !hasAliveEnemies)
            {
                Debug.Log("Draw! All Players and Enemies have died!");
                m_drawGameObject.SetActive(true);
                m_gameOver = true;
                break;
            }
            else if (!hasAlivePlayers)
            {
                Debug.Log("Enemies win! All Players have died!");
                m_enemyWinsGameObject.SetActive(true);
                m_gameOver = true;
                break;
            }
            else if (!hasAliveEnemies)
            {
                Debug.Log("Player wins! All Enemies have died!");
                m_playerWinsGameObject.SetActive(true);
                m_gameOver = true;
                break;
            }

            ++m_gameTime;

            m_timeText.text = $"Time: {m_gameTime}";
            m_elapsedTime -= m_configuration.SimulationDelay;
        }
    }

    public void AddActionText(RPGTestAgent agent, string text)
    {
        GameObject go = Instantiate(m_prototypeActionListItem.gameObject, m_prototypeActionListItem.transform.parent);
        go.SetActive(true);
        ActionListItem actionListItem = go.GetComponent<ActionListItem>();

        actionListItem.Initialize(m_players.Contains(agent), $"{m_gameTime}: {text}");
        m_actionListItems.Add(actionListItem);

        // ideally implement a recycle list here, but quick and dirty just delete children
        while (m_actionListItems.Count > 100)
        {
            m_actionListItems[0].gameObject.SetActive(false);
            Destroy(m_actionListItems[0].gameObject);
            m_actionListItems.RemoveAt(0);
        }
    }

    public void OnGameSpeedSliderChanged()
    {
        m_configuration.SimulationDelay = m_gameSpeedSlider.value;
    }
}
