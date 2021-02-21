using UnityEngine;
using SimpleBallistic;

public class BBShooting : MonoBehaviour
{
    [SerializeField]
    protected GameObject m_Projectile;
    [SerializeField]
    protected GameObject m_Target;
    [SerializeField]
    protected LineRenderer m_LineRenderer = null;
    //[SerializeField]
    //private float m_Angle = 35;
    //[SerializeField]
    //private float m_Force = 50;
    [SerializeField]
    private Rigidbody ballRigidBody;

    void Update()
    {

        float distance = Vector3.Distance(transform.position, m_Target.transform.position);
        //float force = Ballistics.GetForce(transform.position, m_Target.transform.position, distance);

        //print(distance);

        float[] angle = Ballistics.GetAngle(transform.position, m_Target.transform.position, distance);



        //LookAtAngle(distance);
        Shoot(angle[1], distance);
    }

    protected void Shoot(float angle, float force)
    {
        Ballistics.TrajectoryProjection(transform.position, transform.forward, force, angle, 100, 0.1f, m_LineRenderer);
    }

    protected void LookAtAngle(float _angle)
    {
        Vector3 axis = m_Target.transform.position - transform.position;
        axis.y = 0;
        axis.Normalize();
        axis = Quaternion.AngleAxis(_angle, Vector3.Cross(axis, Vector3.up)) * axis;
        transform.forward = axis;
    }
}
