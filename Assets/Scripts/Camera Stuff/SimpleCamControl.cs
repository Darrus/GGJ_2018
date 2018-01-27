using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCamControl : MonoBehaviour {
    [SerializeField, Tooltip("Width and height distance from the sides of the screen")]
    protected Vector2 m_DistanceFromScreenSides;
    [SerializeField, Tooltip("Speed of the camera when mouse position is within the m_DistanceFromScreenSides!")]
    protected Vector2 m_SpeedOfCamera = new Vector2(10.0f, 7.5f);
    [SerializeField, Tooltip("Dampening of the camera speed happens when it is within this distance. Increased from the DistanceFromScreenSides")]
    protected Vector2 m_DistanceForDampenning;
    [SerializeField, Tooltip("Speed for dampenning")]
    protected Vector2 m_DampenedSpeed = new Vector2(2.5f, 1.0f);
	
	// Update is called once per frame
	void Update () {
        Vector3 NewPositonDelta = new Vector3();
        float TotalScreenSideOfX = m_DistanceForDampenning.x + m_DistanceFromScreenSides.x;
		// constantly checks whether is it the mouse position near the edge of the screen
        if (Input.mousePosition.x >= Screen.width - TotalScreenSideOfX)
        {
            // if it is at the very edge, the speed will much faster!
            if (Input.mousePosition.x >= Screen.width - m_DistanceFromScreenSides.x)
            {
                NewPositonDelta.x += m_SpeedOfCamera.x * Time.deltaTime;
            }
            else
            {
                NewPositonDelta.x += m_DampenedSpeed.x * Time.deltaTime;
            }
        }
        else if (Input.mousePosition.x <= TotalScreenSideOfX)
        {
            if (Input.mousePosition.x <= m_DistanceFromScreenSides.x)
            {
                NewPositonDelta.x -= m_SpeedOfCamera.x * Time.deltaTime;
            }
            else
            {
                NewPositonDelta.x -= m_DampenedSpeed.x * Time.deltaTime;
            }
        }
        float TotalScreenSideOfY = m_DistanceForDampenning.y + m_DistanceFromScreenSides.y;
        if (Input.mousePosition.y >= Screen.height - TotalScreenSideOfY)
        {
            if (Input.mousePosition.y >= Screen.height - m_DistanceFromScreenSides.y)
            {
                NewPositonDelta.y += m_SpeedOfCamera.y * Time.deltaTime;
            }
            else
            {
                NewPositonDelta.y += m_DampenedSpeed.y * Time.deltaTime;
            }
        }
        else if (Input.mousePosition.y <= TotalScreenSideOfY)
        {
            if (Input.mousePosition.y <= m_DistanceFromScreenSides.y)
            {
                NewPositonDelta.y -= m_SpeedOfCamera.y * Time.deltaTime;
            }
            else
            {
                NewPositonDelta.y -= m_DampenedSpeed.y * Time.deltaTime;
            }
        }
        if (NewPositonDelta.sqrMagnitude != 0)
        {
            Camera.main.transform.position += NewPositonDelta;
        }
	}
}
