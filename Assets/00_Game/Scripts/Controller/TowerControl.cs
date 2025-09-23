using UnityEngine;

public class TowerControl : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        EnemyControl enemy = other.GetComponent<EnemyControl>();
        if (enemy != null)
        {
            EventManager.Instance.OnLoseLevel?.Invoke();
            Debug.Log("Lose level");
        }
    }
}
