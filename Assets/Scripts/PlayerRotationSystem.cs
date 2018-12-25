using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class PlayerRotationSystem : JobComponentSystem
{
    [BurstCompile]
    struct RigidbodyJob : IJobProcessComponentData<Position, Rotation>
    {

        public void Execute(ref Position position, ref Rotation rotation)
        {

        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new RigidbodyJob
        {

        };
        return job.Schedule(this, inputDeps);
    }
}
