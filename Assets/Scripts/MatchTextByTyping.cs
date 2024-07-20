using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Events;
using System;
using TMPro;

// Let's say we want to do a typing game. We could define a component
// something along those lines to match the typed input.
public class MatchTextByTyping : MonoBehaviour
{
    public TMP_Text ui_text;

    public string text
    {
        get => m_Text;
        set
        {
            m_Text = value;
            m_Position = 0;
        }
    }

    public Action onTextTypedCorrectly { get; set; }
    public Action onTextTypedIncorrectly { get; set; }

    private int m_Position;
    public string m_Text;
    private void Start()
    {
        onTextTypedCorrectly += () => { /*Debug.Log("Correctly typed:" + m_Text);*/ GameManager.Instance.lastTypedKeyword = m_Text; ui_text.text = "Typed: " + m_Text; text = ""; };
        onTextTypedIncorrectly += () => { /*Debug.Log("Incorrectly typed");*/ ui_text.text = "Typed: ";};
    }

    protected void OnEnable()
    {
        Keyboard.current.onTextInput += OnTextInput;
    }

    protected void OnDisable()
    {
        Keyboard.current.onTextInput -= OnTextInput;
    }

    private void OnTextInput(char ch)
    {
        // Debug.Log($"Typed: {ch}");
        // Debug.Log($"Position: {m_Position}");
        if (ch == '\r')
        {
            m_Position = 0;
            return;
        }
        if (m_Text == null || m_Position >= m_Text.Length)
            return;

        if (m_Text[m_Position] == ch)
        {
            ++m_Position;
            ui_text.text = "Typed: " + m_Text[..m_Position];
            if (m_Position == m_Text.Length)
                onTextTypedCorrectly?.Invoke();
        }
        else
        {
            ui_text.text = "";
            m_Position = 0;
            onTextTypedIncorrectly?.Invoke();
        }
    }
}
