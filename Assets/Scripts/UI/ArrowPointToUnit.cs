using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowPointToUnit : MonoBehaviour {
    [SerializeField, Tooltip("Target's transform to focus on!")]
    Transform m_TargetTransform;
    [SerializeField, Tooltip("Offset radius to stay away from the target")]
    protected float m_OffsetRadius = 30.0f;

    [Header("Debugging")]
    [SerializeField]
    protected Vector2 m_ScreenHalfSize;
    [SerializeField] protected Canvas m_ParentCanvas;

    public Transform TargetTransform
    {
        set
        {
            m_TargetTransform = value;
        }
        get
        {
            return m_TargetTransform;
        }
    }

    private void Start()
    {
        if (!m_ParentCanvas)
        {
            m_ParentCanvas = transform.parent.GetComponent<Canvas>();
        }
        RectTransform CanvasRect = m_ParentCanvas.transform as RectTransform;
        m_ScreenHalfSize = new Vector2(CanvasRect.rect.width * 0.5f, CanvasRect.rect.height * 0.5f);
    }

    private void LateUpdate()
    {
        if (m_TargetTransform)
        {
            Vector3 targetToCanvasTransform = m_ParentCanvas.transform.InverseTransformPoint(m_TargetTransform.position);
            Vector3 CameraAtCanvas = m_ParentCanvas.transform.InverseTransformPoint(Camera.main.transform.position);
            Vector3 directionFromPosToTarget = CameraAtCanvas - targetToCanvasTransform;
            directionFromPosToTarget.Normalize();
            directionFromPosToTarget *= m_OffsetRadius;
            directionFromPosToTarget += targetToCanvasTransform;
            directionFromPosToTarget.x = Mathf.Clamp(directionFromPosToTarget.x, -m_ScreenHalfSize.x, m_ScreenHalfSize.x);
            directionFromPosToTarget.y = Mathf.Clamp(directionFromPosToTarget.y, -m_ScreenHalfSize.y, m_ScreenHalfSize.y);
            directionFromPosToTarget.z = transform.localPosition.z;
            transform.localPosition = directionFromPosToTarget;

            // just stare at the target transform
            Vector3 directionToTarget = (m_TargetTransform.position - transform.position);
            directionToTarget.z = 0;
            directionToTarget.Normalize();

            Vector3 currentDirection = transform.right;
            currentDirection.z = 0;
            currentDirection.Normalize();

            float angleTravelled = Vector3.SignedAngle(currentDirection, directionToTarget, Vector3.forward);
            transform.Rotate(0, 0, angleTravelled);

        }
        else
        {
            Destroy(gameObject);
        }
    }
}
