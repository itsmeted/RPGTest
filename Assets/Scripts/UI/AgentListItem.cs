using TMPro;
using UnityEngine;

public class AgentListItem : MonoBehaviour
{
    [SerializeField]
    TMP_Text m_text = null;

    RPGTestAgent m_agent = null;

    public void Initialize(RPGTestAgent agent)
    {
        m_agent = agent;
        Refresh();
    }

    public void Refresh()
    {
        m_text.text = m_agent.ToString();
    }
}
