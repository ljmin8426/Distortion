using UnityEngine;

public class BasicRigidBodyPush : MonoBehaviour
{
    [SerializeField] private LayerMask pushLayers;

    [SerializeField] private bool canPush;

    [Range(0.5f, 5f)]
    [SerializeField] private float strength = 1.1f;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (this.canPush)
        {
            this.PushRigidBodies(hit);
        }
    }

    private void PushRigidBodies(ControllerColliderHit hit)
    {
        Rigidbody attachedRigidbody = hit.collider.attachedRigidbody;
        if (attachedRigidbody == null || attachedRigidbody.isKinematic)
        {
            return;
        }
        if ((1 << attachedRigidbody.gameObject.layer & this.pushLayers.value) == 0)
        {
            return;
        }
        if (hit.moveDirection.y < -0.3f)
        {
            return;
        }
        Vector3 a = new Vector3(hit.moveDirection.x, 0f, hit.moveDirection.z);
        attachedRigidbody.AddForce(a * this.strength, ForceMode.Impulse);
    }
}
