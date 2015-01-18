using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour {

    private Transform m_stCharactor = null;
    private float m_fZOffset = 0.0f;

	// Use this for initialization
	void Start () {
        m_stCharactor = GameObject.Find("Player").transform;

        Vector3 vPosCharactor = m_stCharactor.transform.position;
        Vector3 vPosCamera = transform.position;
        m_fZOffset = vPosCamera.z - vPosCharactor.z;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 vPosCharactor = m_stCharactor.transform.position;
        Vector3 vPosCamera = transform.position;
        Vector3 vPosNewCamera = new Vector3(vPosCharactor.x, vPosCamera.y, vPosCharactor.z + m_fZOffset);
        //Vector3 vPosNewCamera = new Vector3(vPosCharactor.x, vPosCamera.y, vPosCamera.z);
        if (vPosNewCamera.z < 1.5f)
        {
            vPosNewCamera.z = 1.5f;
        }
        transform.position = vPosNewCamera;
	}
}
