using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public TowerInfo info;
    [HideInInspector] public Node node;

    protected Transform tr;
    [SerializeField] private Transform rotatePoint;
    [SerializeField] protected float detectRange;
    [SerializeField] protected LayerMask _targetLayer;
    protected Transform target;
    
    

    private void Awake()
    {
        tr = GetComponent<Transform>();
    }

    protected virtual void Update()
    {
        Collider[] cols = Physics.OverlapSphere(tr.position, detectRange, _targetLayer);

        if (cols.Length > 0)
        {
            target = cols[0].transform;
            rotatePoint.LookAt(target);
        }
        else
        {
            target = null;
        }   
    }

    private void OnMouseDown()
    {
        if (TowerHandler.instance.gameObject.activeSelf == false)            
            TowerUI.instance.SetUp(this);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }
}
