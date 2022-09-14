using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    private Renderer _renderer;

    private Color _originalColor;
    [SerializeField] private Color _buildAvailableColor;
    [SerializeField] private Color _buildNotAvailableColor;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _originalColor = _renderer.material.color;
    }

    private void OnMouseEnter()
    {
        _renderer.material.color = _buildAvailableColor;
        NodeManager.mouseOnNode = this;
    }

    private void OnMouseExit()
    {
        _renderer.material.color = _originalColor;
    }
}
