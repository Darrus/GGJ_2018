using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerClickInput : MonoBehaviour {
    [Header("Variables required")]
    [SerializeField, Tooltip("What mouse button it should be. Default to be 0")]
    protected int m_MouseButtonNum = 0;
    [SerializeField, Tooltip("Layername to raycast against")]
    protected string[] m_ArrayOfLayerNames;
    [SerializeField, Tooltip("Event name to be send")]
    protected string m_EventName = "TileClicked";

    [Header("Debugging")]
    [SerializeField, Tooltip("Raycasted object")]
    protected GameObject m_Raycast2DObj;

    private void Update()
    {
        if (Input.GetMouseButtonDown(m_MouseButtonNum))
        {
            // if the mouse pointer is over the UI. just dont bother with raycast
            if (EventSystem.current.IsPointerOverGameObject()) { return; }

            Vector3 mouseToWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hitInfo2D;
            // using camera forward as the direction is fine
            hitInfo2D = Physics2D.Raycast(mouseToWorldPosition, Camera.main.transform.forward, Mathf.Infinity, LayerMask.GetMask(m_ArrayOfLayerNames));
            // if the collider exists
            if (hitInfo2D.collider)
            {
                m_Raycast2DObj = hitInfo2D.collider.gameObject;
                SubscriptionSystem.Instance.TriggerEvent(m_EventName, m_Raycast2DObj);
            }
            else
            {
                m_Raycast2DObj = null;
            }
        }
    }
}
