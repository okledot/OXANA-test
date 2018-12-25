using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class ControllInputSystem : JobComponentSystem
{
    [BurstCompile]
    struct ControllJob : IJobProcessComponentDataWithEntity<Position, Rotation, playerID>
    {
        [ReadOnly] public float Horizontal;
        [ReadOnly] public float Vertical;
        [ReadOnly] public bool Right;
        [ReadOnly] public bool Left;
        [ReadOnly] public bool Jump;
        [ReadOnly] public float dt;

        public float3 moveDirection;
        public float4 rotationDirection;
        public float3 Velocity;
        public float3 move;
        public float jumpSpeed;
        public float rotateSpeed;

        public void Execute(Entity entity, int index, ref Position position, ref Rotation rotation, ref playerID playerID)
        {
            move = Velocity * moveDirection * dt;
            rotation.Value.value.w = math.sqrt(0.5f);
            if (Right || Left)
            rotation.Value.value.y += rotationDirection.y;

            Debug.Log(rotationDirection);
            position.Value += move;

            if ((Jump) && (position.Value.y <= 1))
            {
                position.Value.y += jumpSpeed * dt;
            }
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new ControllJob
        {
            Jump = Input.GetButton("Jump"),
            Right = Input.GetKeyDown(KeyCode.D),
            Left = Input.GetKeyDown(KeyCode.A),
            moveDirection = new float3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")),
            rotationDirection = new float4(0, Input.GetAxis("Horizontal") > 0 ? math.sqrt(Input.GetAxis("Horizontal")) / 2 : -math.sqrt(math.abs(Input.GetAxis("Horizontal"))) / 2, 0, math.sqrt(0.5f)),

            dt = Time.deltaTime,
            Velocity = 10f,
            jumpSpeed = 80f,
            rotateSpeed = 3.0f
        };
        return job.Schedule(this, inputDeps);
    }
}
