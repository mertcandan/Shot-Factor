using UnityEngine;

public class BulletDetector : MonoBehaviour
{
    public Enemy enemy;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.BULLET))
        {
            enemy.OnHitBullet();
            other.GetComponent<Bullet>().OnHit();
        }
        else if (other.CompareTag(Tags.ENEMY_TARGET))
        {
            GameManager.Instance.LevelLost();
        }
        
    }
}
