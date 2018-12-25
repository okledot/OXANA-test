using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class CollisionCheckSystem : JobComponentSystem
{
    struct CollisionCheckJob : IJobProcessComponentDataWithEntity<Position, AABBColliderComponent>
    {

        public void Execute(Entity entity, int index, ref Position position, ref AABBColliderComponent AABBCollider)
        {

        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new CollisionCheckJob
        {

        };
        return job.Schedule(this, inputDeps);
    }
}
