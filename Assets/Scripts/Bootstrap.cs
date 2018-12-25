using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Unity.Rendering;
using Unity.Transforms;
using Random = UnityEngine.Random;
using Unity.Collections;


public sealed class Bootstrap
{
    public static EntityArchetype playerArchetype { get; private set; }
    public static EntityArchetype sectorArchetype { get; private set; }

    //public static MeshInstanceRenderer sectorRenderer = GetLook("SectorPrototype");
    //public static MeshInstanceRenderer playerRenderer = GetLook("PlayerPrototype");

    static public EntityManager em;

    public static int sectorSideSize = 5;
    public static int countOfSectors = 1024;
    static public float3 velocity = float3.zero;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Initialize()
    {
        em = World.Active.GetOrCreateManager<EntityManager>();
        CreateArchetypes(em);
    }

    private static void CreateArchetypes(EntityManager em)
    {
        var position = ComponentType.Create<Position>();
        var rotation = ComponentType.Create<Rotation>();
        var scale = ComponentType.Create<Scale>();

        var playerID = ComponentType.Create<playerID>();
        var sectorID = ComponentType.Create<sectorID>();

        var sectorData = ComponentType.Create<SectorData>();

        var playerData = ComponentType.Create<PlayerData>();
        //var velocity = ComponentType.Create<VelocityComponent>();

        var aabbCollider = ComponentType.Create<AABBColliderComponent>();
        var rigidbodyData = ComponentType.Create<RigidBodyComponent>();

        playerArchetype = em.CreateArchetype(position, rotation, scale, playerID, playerData, aabbCollider, rigidbodyData);
        sectorArchetype = em.CreateArchetype(position, rotation, scale, sectorID, sectorData, aabbCollider);
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void InitializeWithScene()
    {
        CreateMap(em);
        CreateGameObjects(em);
    }

    public static void CreateGameObjects(EntityManager em)
    {
        var startPosition = new float3(0, 20, 0);
        var playerEntity = em.CreateEntity(playerArchetype);
        em.SetComponentData(playerEntity, new Position { Value = startPosition });
        em.SetComponentData(playerEntity, new Scale { Value = new float3(3, 3, 3) });
        //em.SetComponentData(playerEntity, new VelocityComponent { Value = new float3(0, 0, 0) });

        em.SetComponentData(playerEntity, new RigidBodyComponent { Mass = 20});
        em.SetComponentData(playerEntity, new AABBColliderComponent { position = startPosition, Height = 5, Width = 5});

        em.AddSharedComponentData(playerEntity, GetLook("PlayerPrototype"));
    }

    public static void CreateMap(EntityManager em)
    {
        //var sectorEntity = em.CreateEntity(sectorArchetype);
        //em.SetComponentData(sectorEntity, new Position { Value = new float3(0, 0, 0) });
        //em.SetComponentData(sectorEntity, new Scale { Value = new float3(5, 1, 5) });
        //em.AddSharedComponentData(sectorEntity, GetLook("SectorPrototype"));

        NativeArray<Entity> groundZero = new NativeArray<Entity>(countOfSectors, Allocator.Temp);
        int[] groundZeroStatus = new int[countOfSectors];
        NativeArray<Entity> groundOne = new NativeArray<Entity>(countOfSectors, Allocator.Temp);
        int[] groundOneStatus = new int[countOfSectors];
        NativeArray<Entity> groundTwo = new NativeArray<Entity>(countOfSectors, Allocator.Temp);
        int[] groundTwoStatus = new int[countOfSectors];

        em.CreateEntity(sectorArchetype, groundZero);
        em.CreateEntity(sectorArchetype, groundOne);
        em.CreateEntity(sectorArchetype, groundTwo);

        int k = 0;
        for (int z = 0; z < 3; z++)
        {
            for (int i = 0; i < Mathf.Sqrt(countOfSectors) / 4; i++)
            {
                for (int j = 0; j < Mathf.Sqrt(countOfSectors) / 4; j++)
                {
                    if (z == 0)
                    {
                        groundZeroStatus[k] = 0;
                        var _position = new float3(j * sectorSideSize + 1, z, i * sectorSideSize + 1);
                        em.SetComponentData(groundZero[k], new Position { Value = _position });
                        em.SetComponentData(groundZero[k], new Scale { Value = new float3(sectorSideSize, 1, sectorSideSize) });
                        em.SetComponentData(groundZero[k], new AABBColliderComponent { position = _position, Height = sectorSideSize/2, Width = sectorSideSize/2 });
                        em.AddSharedComponentData(groundZero[k], GetLook("SectorPrototype"));
                        groundZeroStatus[k] = 1; k++;
                    }

                    if ((z == 1))
                    {
                        groundOneStatus[k] = 0;
                        if ((Random.Range(0, 100) < 20))
                        {
                            var _position = new float3(j * sectorSideSize + 1, z, i * sectorSideSize + 1);
                            em.SetComponentData(groundOne[k], new Position { Value = _position });
                            em.SetComponentData(groundOne[k], new Scale { Value = new float3(sectorSideSize, 1, sectorSideSize) });
                            em.SetComponentData(groundZero[k], new AABBColliderComponent { position = _position, Height = sectorSideSize / 2, Width = sectorSideSize / 2 });
                            em.AddSharedComponentData(groundOne[k], GetLook("SectorPrototype"));
                            groundOneStatus[k] = 1;
                        }
                        k++;
                    }

                    if ((z == 2))
                    {
                        groundTwoStatus[k] = 0;
                        if ((Random.Range(0, 100) < 20) && (groundOneStatus[k] == 1))
                        {
                            var _position = new float3(j * sectorSideSize + 1, z, i * sectorSideSize + 1);
                            em.SetComponentData(groundTwo[k], new Position { Value = _position });
                            em.SetComponentData(groundTwo[k], new Scale { Value = new float3(sectorSideSize, 1, sectorSideSize) });
                            em.SetComponentData(groundZero[k], new AABBColliderComponent { position = _position, Height = sectorSideSize / 2, Width = sectorSideSize / 2 });
                            em.AddSharedComponentData(groundTwo[k], GetLook("SectorPrototype"));
                            groundTwoStatus[k] = 1;
                        }
                        k++;
                    }
                }
            }
            k = 0;
        }
        groundZero.Dispose();
        groundOne.Dispose();
        groundTwo.Dispose();
    }

    private static MeshInstanceRenderer GetLook(string protoType)
    {
        var prototype = GameObject.Find(protoType);
        var view = prototype.GetComponent<MeshInstanceRendererComponent>().Value;
        Object.Destroy(prototype);
        return view;
    }
}