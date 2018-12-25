using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class RigidbodySystem : JobComponentSystem
{
    [BurstCompile]
    struct RigidbodyJob : IJobProcessComponentData<Position, RigidBodyComponent>
    {
        [ReadOnly] public float dt;
        [ReadOnly] public float3 gravity;
        [ReadOnly] public float gravityModifier;
        public float3 Velocity;
        public float3 d_Velocity;
        public float3 move;

        public void Execute(ref Position position, ref RigidBodyComponent rigidbody)
        {
            if (position.Value.y > 2)
            {
                d_Velocity += gravityModifier * gravity * dt;
                Velocity += d_Velocity;
                Bootstrap.velocity = Velocity;
                move = Velocity * dt;
                position.Value += move;
                Debug.Log("Скорость" + Velocity);
            }
            else
            {
                Bootstrap.velocity = 0;
            }
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new RigidbodyJob
        {
            Velocity = Bootstrap.velocity,
            dt = Time.deltaTime,
            gravity = new float3(0, -20, 0),
            gravityModifier = 1f
        };
        return job.Schedule(this, inputDeps);
    }
}

 //{
 //       float3 gravity = new float3(0, -20, 0);
 //       float dt = Time.deltaTime;
 //       for (int index = 0; index<m_Data.Length; ++index)
 //       {
 //           var position = m_Data.Position[index].Value;

 //           if (m_Data.Collision[index].Value == 0)
 //           {
 //               Velocity velocity = new Velocity
 //               {
 //                   Value = m_Data.Velocity[index].Value + gravity * dt
 //               };
 //           m_Data.Velocity[index] = velocity;

 //               position += (m_Data.Velocity[index].Value* dt + 0.5f * gravity * dt * dt);
 //           }

 //           m_Data.Position[index] = new Position { Value = position };
 //       }
 //   }