using Unity.Entities;
using Unity.Mathematics;

public struct playerID : IComponentData { }
public struct sectorID : IComponentData { }

public struct PlayerData : IComponentData
{
    public float Horizontal { get; set; }
    public float Vertical { get; set; }
    public float3 rotationAxis { get; set; }

    public PointsData path { get; set; }
    public float velocity { get; set; }
    public CharStatsData playerStats { get; set; }
}

public struct SectorData : IComponentData
{
    public float3 northPos { get; set; }
    public float3 southPos { get; set; }
    public float3 westPos { get; set; }
    public float3 eastPos { get; set; }
}

public struct CharStatsData : IComponentData
{
    public float Health { get; set; }
    public float Mana { get; set; }
}

public struct PointsData : IComponentData
{
    public float3 point { get; set; }
}

/// <summary>
/// Компонентны для Физики 
/// </summary>
public struct RigidBodyComponent : IComponentData
{
    public float Mass { get; set; }
}

public struct VelocityComponent : IComponentData
{
    public float3 Value { get; set; }
}

public struct CollisionComponent : IComponentData
{
    public float Value { get; set; }
}

public struct AABBColliderComponent : IComponentData
{
    public float3 position { get; set; }
    public float Width { get; set; }
    public float Height { get; set; }
}

public struct SphereColliderComponent : IComponentData
{

}

public struct CapsuleColliderComponent : IComponentData
{

}