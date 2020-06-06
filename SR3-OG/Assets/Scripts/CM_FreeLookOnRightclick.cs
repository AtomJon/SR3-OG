using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineFreeLook))]
public class CM_FreeLookOnRightclick : MonoBehaviour
{
    [SerializeField]
    string axisXName;
    [SerializeField, Range(0, 10)]
    float axisXSpeed = 4f;
    [SerializeField]
    string axisYName;
    [SerializeField, Range(0, 10)]
    float axisYSpeed = .4f;

    CinemachineFreeLook freeLook;

    // Start is called before the first frame update
    void Start()
    {
        freeLook = GetComponent<CinemachineFreeLook>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            freeLook.m_XAxis.Value += Input.GetAxis(axisXName) * axisXSpeed;
            freeLook.m_YAxis.Value += Input.GetAxis(axisYName) * axisYSpeed;
        }
    }
}
