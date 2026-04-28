using UnityEngine;
using UnityEngine.UIElements;

public class CameraSlerp : MonoBehaviour
{
    public Transform target;

    float speed = 2f;
    void Update()
    {
        Quaternion lookRot = Quaternion.LookRotation(target.position - transform.position);
        float t = 1f - Mathf.Exp(-speed * Time.deltaTime);
        transform.rotation = ManualSlerp(transform.rotation, lookRot, t);
    }
    Quaternion ManualSlerp(Quaternion from, Quaternion to, float t)
    {
        float dot = Quaternion.Dot(from, to);

        if (dot < 0f)
        {
            to = new Quaternion(-to.x, -to.y, to.z, to.w);
            dot = -dot;
        }

        float theta = Mathf.Acos(dot);
        float sinTheta = Mathf.Sin(theta);

        float ratio = Mathf.Sin((1f - t) * theta) / sinTheta;
        float ratioB = Mathf.Sin(t * theta) / sinTheta;

        Quaternion result = new Quaternion(
        ratio * from.x + ratioB * to.x,
        ratio * from.y + ratioB * to.y,
        ratio * from.z + ratioB * to.z,
        ratio * from.w + ratioB * to.w
    );
        return result.normalized;
    } 
}
