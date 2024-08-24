using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionListItem : MonoBehaviour
{
    [SerializeField]
    TMP_Text m_text = null;

    [SerializeField]
    Image m_background = null;

    [SerializeField]
    Color m_playerColor;

    [SerializeField]
    Color m_enemyColor;

    public void Initialize(bool isPlayer, string text)
    {
        m_text.text = text;
        m_background.color = isPlayer ? m_playerColor : m_enemyColor;
    }
}
