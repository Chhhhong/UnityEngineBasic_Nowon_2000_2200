using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerLaserLauncher : Tower
{
    [SerializeField] private LineRenderer _laserBeam;
    [SerializeField] private ParticleSystem _laserHitEffect;
    [SerializeField] private Transform _firePoint;

    [SerializeField] private float _damage;
    private float _damageStep;
    private int damageStep
    {
        set
        {
            _damageStep = value;

            _laserBeam.startWidth = 0.05f * (1 + _damageStep);
            _laserBeam.endWidth = 0.05f * (1 + _damageStep);
            _laserHitEffect.transform.localScale = Vector3.one * (1 + _damageStep * 0.2f);  
        }
    }
    [SerializeField] private int _damageGain;
    [SerializeField] private float _damageChargeTime;
    private float _damageChargeTimer;

    private void FixedUpdate()
    {
        Attack();
    }

    private void Attack()
    {
        if (target == null)
        {
            if (_laserBeam.enabled)
                _laserBeam.enabled = false;

            if (_laserHitEffect.isPlaying)
                _laserHitEffect.Stop();

            if (_damageStep > 0)
                damageStep = 0;
        }
        else
        {
            _laserBeam.SetPosition(0, _firePoint.position);
            _laserBeam.SetPosition(1, target.position);
            _laserBeam.enabled = true;

            if (_laserHitEffect.isStopped)
                _laserHitEffect.Play();

            RaycastHit[] hits = Physics.RaycastAll(origin: _firePoint.position,
                                                   direction: target.position - _firePoint.position,
                                                   maxDistance: Vector3.Distance(target.position, _firePoint.position),
                                                   layerMask: targetLayer);

            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider.transform == target)
                {
                    _laserHitEffect.transform.position = hits[i].point;
                    _laserHitEffect.transform.LookAt(_firePoint);
                    break;
                }
            }

            target.GetComponent<Enemy>().hp -= (int)(_damage * (1 + _damageStep) * _damageGain * Time.fixedDeltaTime);

            if (_damageChargeTimer < 0)
            {
                if (_damageStep < 2)
                {
                    _damageStep++;
                    _damageChargeTimer = _damageChargeTime;
                }   
            }
            else
            {
                _damageChargeTimer -= Time.fixedDeltaTime;
            }
            

        }
    }
    

}
