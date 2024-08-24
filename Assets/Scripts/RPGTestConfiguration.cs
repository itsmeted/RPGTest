using UnityEngine;

public class RPGTestConfiguration : MonoBehaviour
{
    public static float PAUSE_TIME = 1.0f;

    [Tooltip("Delay in seconds for each tick of the simulation")]
    [Range(0.01f, 1.0f)]
    [SerializeField]
    float m_simulationUpdateDelay = 0.5f;

    [SerializeField]
    int m_numPlayers;

    [SerializeField]
    int m_numEnemies;

    [SerializeField]
    RPGTestAgent m_prototypePlayer;

    [SerializeField]
    RPGTestAgent m_prototypeEnemy;

    [SerializeField]
    RPGTestAction[] m_actions;

    public float SimulationDelay
    {
        get { return m_simulationUpdateDelay; }
        set { m_simulationUpdateDelay = value; }
    }

    public int NumPlayers
    {
        get { return m_numPlayers; }
    }

    public int NumEnemies
    {
        get { return m_numEnemies; }
    }

    public RPGTestAgent PrototypePlayer
    {
        get { return m_prototypePlayer; }
    }

    public RPGTestAgent PrototypeEnemy
    {
        get { return m_prototypeEnemy; }
    }

    public RPGTestAction[] Actions
    {
        get { return m_actions; }
    }
}
