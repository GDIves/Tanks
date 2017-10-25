using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDirectionController : MonoBehaviour {

    private Quaternion m_RelativeRotation;

    private void Start()
    {
        m_RelativeRotation = transform.parent.localRotation;
    }

    private void Update()
    {
        transform.rotation = m_RelativeRotation;
    }
}
