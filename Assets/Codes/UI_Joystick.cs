using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Joystick : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField]
    private Image _background; // ���̽�ƽ ���

    [SerializeField]
    private Image _handler; // ���̽�ƽ ���� �ڵ�

    [SerializeField]
    private PlayerMove _player; // �ν����Ϳ��� ������ PlayerMove

    private float _joystickRadius;
    private Vector2 _touchPosition;
    private Vector2 _moveDir;

    void Start()
    {
        // ���̽�ƽ ������ ���
        _joystickRadius = _background.rectTransform.sizeDelta.y / 2;

        if (_player == null)
        {
            Debug.LogWarning("PlayerMove�� ������� �ʾҽ��ϴ�. �ν����Ϳ��� �������ּ���.");
        }
    }

    public void OnPointerClick(PointerEventData eventData) { }

    public void OnPointerDown(PointerEventData eventData)
    {
        _background.transform.position = eventData.position;
        _handler.transform.position = eventData.position;
        _touchPosition = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _handler.transform.position = _touchPosition;
        _moveDir = Vector2.zero;

        if (_player != null)
            _player.inputVec = _moveDir;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 touchDir = (eventData.position - _touchPosition);
        float moveDist = Mathf.Min(touchDir.magnitude, _joystickRadius);
        _moveDir = touchDir.normalized;
        Vector2 newPosition = _touchPosition + _moveDir * moveDist;
        _handler.transform.position = newPosition;

        if (_player != null)
            _player.inputVec = _moveDir;
    }
}
