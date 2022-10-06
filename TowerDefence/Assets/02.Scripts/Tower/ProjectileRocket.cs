using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ProjectileRocket : Projectile
{
    [SerializeField] private ParticleSystem _explosionEffect;
    private float _explosionRange;

    protected override void OnTriggerEnter(Collider other)
    {
        if (1 << other.gameObject.layer == targetLayer || 
            1 << other.gameObject.layer == touchLayer)
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, _explosionRange, Vector3.up, 0, targetLayer);

            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].transform.TryGetComponent(out Enemy enemy))
                {
                    enemy.hp -= (int)((_explosionRange - Vector3.Distance(transform.position, enemy.transform.position)) * damage);
                    GameObject effect = ObjectPool.instance.Spawn("RocketExplosionEffect", tr.position, Quaternion.LookRotation(tr.position - target.position));
                    ObjectPool.instance.Return(effect, _explosionEffect.main.duration + _explosionEffect.main.startLifetime.constantMax);
                    ObjectPool.instance.Return(this.gameObject);
                }
            }

            
        }
    }
}
