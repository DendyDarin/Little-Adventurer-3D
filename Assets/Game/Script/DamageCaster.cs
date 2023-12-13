using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCaster : MonoBehaviour
{
    private Collider damageCasterCollider;
    public int damage = 30;
    public string targetTag;
    private List<Collider> damagedTargetList;

    private void Awake() 
    {
        damageCasterCollider = GetComponent<Collider>();
        damageCasterCollider.enabled = false;
        damagedTargetList = new List<Collider>();
    }

    private void OnTriggerEnter(Collider other) 
    {
        // check if target is include in target tag
        if (other.tag == targetTag && !damagedTargetList.Contains(other))
        {
            Character targetCC = other.GetComponent<Character>();

            if (targetCC != null)
            {
                targetCC.ApplyDamage(damage);

                PlayerVFXManager playerVFXManager = transform.parent.GetComponent<PlayerVFXManager>();

                if (playerVFXManager != null)
                {
                    RaycastHit hit;

                    Vector3 originalPos = transform.position + (-damageCasterCollider.bounds.extents.z) * transform.forward;

                    bool isHit = Physics.BoxCast(originalPos, damageCasterCollider.bounds.extents / 2, transform.forward, out hit, transform.rotation, damageCasterCollider.bounds.extents.z, 1<<6);

                    if (isHit)
                    {
                        playerVFXManager.PlaySlash(hit.point + new Vector3(0, 0.5f, 0));
                    }
                }
            }

            damagedTargetList.Add(other);
        }
    }

    public void EnableDamageCaster()
    {
        damagedTargetList.Clear();
        damageCasterCollider.enabled = true;
    }

    public void DisableDamageCaster()
    {
        damagedTargetList.Clear();
        damageCasterCollider.enabled = false;
    }

    // this is to check the hit effect position by gizmos method
    private void OnDrawGizmos() 
    {
        if (damageCasterCollider == null)
        {
            damageCasterCollider = GetComponent<Collider>();
        }

        RaycastHit hit;

        Vector3 originalPos = transform.position + (-damageCasterCollider.bounds.extents.z) * transform.forward;

        bool isHit = Physics.BoxCast(originalPos, damageCasterCollider.bounds.extents / 2, transform.forward, out hit, transform.rotation, damageCasterCollider.bounds.extents.z, 1<<6);

        if (isHit)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(hit.point, 0.3f);
        }
    }
}
