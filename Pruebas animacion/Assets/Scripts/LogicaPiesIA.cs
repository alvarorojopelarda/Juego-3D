using UnityEngine;

public class LogicaPiesIA : MonoBehaviour
{
    public EnemyAI enemyAI;

    private void OnTriggerStay(Collider other)
    {
        // No hacemos nada si no puede saltar
        if (enemyAI != null)
        {
            enemyAI.puedoSaltar = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (enemyAI != null)
        {
            enemyAI.puedoSaltar = false;
        }
    }
}
