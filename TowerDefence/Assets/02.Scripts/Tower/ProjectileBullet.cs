using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBullet : Projectile
{
    [SerializeField] private ParticleSystem _explosionEffect;
    protected override void OnTriggerEnter(Collider other)
    {
        if (1 << other.gameObject.layer == targetLayer)
        {
            if (other.gameObject.TryGetComponent(out Enemy enemy))
            {
                enemy.hp -= damage;
                GameObject effect = ObjectPool.instance.Spawn("BulletExplosionEffect", tr.position, Quaternion.LookRotation(tr.position - target.position));
                ObjectPool.instance.Return(effect, _explosionEffect.main.duration + _explosionEffect.main.startLifetime.constantMax);
                ObjectPool.instance.Return(this.gameObject);
            }
        }
        else if (1 << other.gameObject.layer == touchLayer)
        {
            GameObject effect = ObjectPool.instance.Spawn("BulletExplosionEffect", tr.position, Quaternion.LookRotation(tr.position - target.position));
            ObjectPool.instance.Return(effect, _explosionEffect.main.duration + _explosionEffect.main.startLifetime.constantMax);
            ObjectPool.instance.Return(this.gameObject);
        }
    }
}
