using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DropItem : MonoBehaviour
{
    private SphereCollider col;
    private Rigidbody rb;
    private Transform itemModel;

    private Vector3 pos;

    private bool isDrop;
    private float dropPosY;
    private float valueA;

    private void Awake()
    {
        TryGetComponent<Rigidbody>(out rb);
        rb.useGravity = true;
        rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);

        TryGetComponent<SphereCollider>(out col);
        col.isTrigger = true;

        itemModel = transform.GetChild(0);
        valueA = 0f;
        isDrop = false;
    }

    private void Update()
    {
        if (isDrop)
        {
            itemModel.Rotate(Vector3.up * (90.0f * Time.deltaTime));
            pos = itemModel.position;
            valueA += Time.deltaTime;
            pos.y = dropPosY + 0.3f * Mathf.Sin(valueA);
            itemModel.position = pos;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            rb.useGravity = false;
            rb.linearVelocity = Vector3.zero;
            dropPosY = itemModel.position.y;
            isDrop = true;
        }
    }
}
