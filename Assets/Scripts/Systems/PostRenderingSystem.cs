using Unity.Entities;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using Unity.Burst;
using Unity.Mathematics;

[UpdateInGroup(typeof(SimulationSystemGroup))]
public unsafe partial struct PostRenderingSystem : ISystem, ISystemStartStop
{
    private NativeArray<byte> m_prevTickDataBuffer;

    private EntityQuery m_renderedDataQuery;
    private ComponentTypeHandle<ChunkDataCoord> m_dataCoordHandle;
    private BufferTypeHandle<ChunkDataBuffer> m_dataBufferHandle;



    public void OnCreate(ref SystemState state)
    {
        m_renderedDataQuery = new EntityQueryBuilder(Allocator.Temp)
            .WithAll<PostRenderTag, ChunkDataBuffer, ChunkDataCoord, DataIndexCount>()
            .Build(ref state);

        m_dataCoordHandle = state.GetComponentTypeHandle<ChunkDataCoord>(true);
        m_dataBufferHandle = state.GetBufferTypeHandle<ChunkDataBuffer>(true);
    }
    [BurstCompile]
    public void OnStartRunning(ref SystemState state)
    {
        if (!SystemAPI.TryGetSingleton(out CanvasSize size))
            return;

        m_prevTickDataBuffer = new NativeArray<byte>(size.x * size.y, Allocator.Persistent);
        for (int i = 0; i < size.x*size.y; i++)
        {
            m_prevTickDataBuffer[i] = (byte)(i%2);
        }
        state.EntityManager.AddComponentData(state.SystemHandle, new Singleton
        {
            prevTickDataBuffer = m_prevTickDataBuffer,
        });
    }

    public void OnStopRunning(ref SystemState state)
    {

    }

    public void OnUpdate(ref SystemState state)
    {
        if (!SystemAPI.TryGetSingleton(out CanvasSize canvasSize))
            return;
        state.CompleteDependency();
        int chunkCount_x = (int)math.ceil(canvasSize.x * 1.0f / SettingsData.chunkSize_x);

        if (!m_renderedDataQuery.IsEmpty)
        {
            var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
            m_dataCoordHandle.Update(ref state);
            m_dataBufferHandle.Update(ref state);
            var indexChunks = m_renderedDataQuery.ToArchetypeChunkArray(Allocator.Temp);
            var dataChunkSize = SettingsData.chunkSize_x * SettingsData.chunkSize_y;
            for (int i = 0; i < indexChunks.Length; i++)
            {
                var chunkCoord = indexChunks[i].GetNativeArray(ref m_dataCoordHandle)[0];
                var dataBuffer = indexChunks[i].GetBufferAccessor(ref m_dataBufferHandle)[0].Reinterpret<byte>();
                var offset = (chunkCoord.value.x + chunkCoord.value.y * chunkCount_x) * dataChunkSize;
                UnsafeUtility.MemCpy(((byte*)m_prevTickDataBuffer.GetUnsafeReadOnlyPtr()) + offset, dataBuffer.GetUnsafeReadOnlyPtr(), dataChunkSize * sizeof(byte));
            }
            var ents = m_renderedDataQuery.ToEntityArray(Allocator.Temp);
            ecb.AddComponent<ChunkDataReadyTag>(ents);
            ecb.RemoveComponent<PostRenderTag>(ents);
        }
    }

    public void OnDestroy(ref SystemState state)
    {
        m_prevTickDataBuffer.Dispose();
    }

    public struct Singleton : IComponentData
    {
        [NativeDisableParallelForRestriction]
        public NativeArray<byte> prevTickDataBuffer;
    }
}
