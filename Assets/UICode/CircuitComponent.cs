using UnityEngine;

public enum ComponentType { Battery, Bulb, Switch }

public class CircuitComponent : MonoBehaviour
{
    public ComponentType componentType;
    public GameObject activeModel;
    public GameObject inactiveModel;
    public bool isClosed; // ���ڿ���

    // �����������ʱ����
    public void OnPowered()
    {
        if (componentType == ComponentType.Bulb || componentType == ComponentType.Switch)
        {
            if (activeModel != null && inactiveModel != null)
            {
                activeModel.SetActive(true);
                inactiveModel.SetActive(false);
            }

            if (componentType == ComponentType.Switch)
            {
                isClosed = true;
            }
        }
    }

    // �������״̬
    public void ResetState()
    {
        if (componentType == ComponentType.Bulb || componentType == ComponentType.Switch)
        {
            if (activeModel != null && inactiveModel != null)
            {
                activeModel.SetActive(false);
                inactiveModel.SetActive(true);
            }

            if (componentType == ComponentType.Switch)
            {
                isClosed = false;
            }
        }
    }
}
