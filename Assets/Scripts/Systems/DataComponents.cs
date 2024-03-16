using Unity.Mathematics;
using UnityEngine;
using Unity.Entities;

[System.Serializable]
public struct CanvasSize : IComponentData
{
    public int x;
    public int y;
}

[InternalBufferCapacity(10000)]
public struct ChunkDataBuffer : IBufferElementData
{
    public byte value;
}


public struct ChunkDataCoord:IComponentData
{
    public int2 value;
}

public struct DataIndexCount:IComponentData
{
    public int value;
}

#region Batch Tags

public struct ScheduleBatch0Tag : IComponentData { }
public struct ScheduleBatch1Tag : IComponentData { }
public struct ScheduleBatch2Tag : IComponentData { }
public struct ScheduleBatch3Tag : IComponentData { }
public struct ScheduleBatch4Tag : IComponentData { }
public struct ScheduleBatch5Tag : IComponentData { }
public struct ScheduleBatch6Tag : IComponentData { }
public struct ScheduleBatch7Tag : IComponentData { }
public struct ScheduleBatch8Tag : IComponentData { }
public struct ScheduleBatch9Tag : IComponentData { }

#endregion

public struct PrevTickTime:IComponentData
{
    public double value;
    public float tickRate;
}

public struct ChunkDataReadyTag : IComponentData { }

public struct ChunkDataUpdatingTag:IComponentData { }
public struct ChunkDataUpdatedTag:IComponentData { }
public struct RenderReadyTag:IComponentData { }
public struct PostRenderTag:IComponentData { }
