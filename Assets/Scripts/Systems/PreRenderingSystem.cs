using Unity.Entities;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using Unity.Burst;
using Unity.Mathematics;

[UpdateInGroup(typeof(SimulationSystemGroup))]
public unsafe partial struct PreRenderingSystem : ISystem ,ISystemStartStop
{
    private NativeList<int> m_coordinateData;
    private NativeList<int> m_indexBuffer;
    public NativeList<byte> m_prevTickDataBuffer;

    private EntityQuery m_updatedDataQuery;
    private ComponentTypeHandle<DataIndexCount> m_indexCountHandle;
    private ComponentTypeHandle<ChunkDataCoord> m_dataCoordHandle;


    public void OnCreate(ref SystemState state)
    {
        m_updatedDataQuery = new EntityQueryBuilder(Allocator.Temp)
            .WithAll<ChunkDataUpdatedTag,ChunkDataBuffer,ChunkDataCoord,DataIndexCount>()
            .WithNone<ChunkDataUpdatingTag>()
            .Build(ref state);


        m_indexCountHandle = state.GetComponentTypeHandle<DataIndexCount>(true);
        m_dataCoordHandle = state.GetComponentTypeHandle<ChunkDataCoord>(true);
    }
    [BurstCompile]
    public void OnStartRunning(ref SystemState state)
    {
        if (!SystemAPI.TryGetSingleton(out CanvasSize size))
            return;

        m_coordinateData = new NativeList<int>(size.x * size.y, Allocator.Persistent);
        m_coordinateData.Resize(size.x * size.y,NativeArrayOptions.UninitializedMemory);
        m_indexBuffer = new NativeList<int>(size.x * size.y, Allocator.Persistent);
        m_indexBuffer.Resize(size.x * size.y,NativeArrayOptions.UninitializedMemory);

        state.EntityManager.AddComponentData(state.SystemHandle,new Singleton
        {
            coordinateData = m_coordinateData,
            indexBuffer = m_indexBuffer
        });
    }

    public void OnStopRunning(ref SystemState state)
    {

    }

    public void OnUpdate(ref SystemState state)
    {
        if (!SystemAPI.TryGetSingletonRW(out RefRW<PrevTickTime> tick))
            return;
        
        state.CompleteDependency();
        var canvasSize = SystemAPI.GetSingleton<CanvasSize>();
        int chunkCount_x = (int)math.ceil(canvasSize.x * 1.0f / SettingsData.chunkSize_x);

        if (!m_updatedDataQuery.IsEmpty
            && SystemAPI.Time.ElapsedTime - tick.ValueRO.value > SettingsData.timeStep)
        {
            var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
            m_indexCountHandle.Update(ref state);
            m_dataCoordHandle.Update(ref state);
            var indexChunks = m_updatedDataQuery.ToArchetypeChunkArray(Allocator.Temp);
            m_coordinateData.Clear();

            for (int i = 0; i < indexChunks.Length; i++)
            {
                var indexCount = indexChunks[i].GetNativeArray(ref m_indexCountHandle)[0];
                var chunkCoord = indexChunks[i].GetNativeArray(ref m_dataCoordHandle)[0];
                var sourceOffset = (chunkCoord.value.x + chunkCoord.value.y * chunkCount_x) * SettingsData.chunkSize_x * SettingsData.chunkSize_y;
                var destOffset = m_coordinateData.Length;
                m_coordinateData.Resize(destOffset + indexCount.value, NativeArrayOptions.UninitializedMemory);
                UnsafeUtility.MemCpy(m_coordinateData.GetUnsafeReadOnlyPtr() + destOffset, m_indexBuffer.GetUnsafeReadOnlyPtr() + sourceOffset, indexCount.value * sizeof(int));
            }
            var ents = m_updatedDataQuery.ToEntityArray(Allocator.Temp);
            ecb.AddComponent<RenderReadyTag>(ents);
            ecb.RemoveComponent<ChunkDataUpdatedTag>(ents);
            tick.ValueRW.tickRate = 1.0f/(float)(SystemAPI.Time.ElapsedTime - tick.ValueRW.value);
            tick.ValueRW.value = SystemAPI.Time.ElapsedTime;
        }
    }

    public void OnDestroy(ref SystemState state)
    {
        m_coordinateData.Dispose();
        m_indexBuffer.Dispose();
    }

    public struct Singleton:IComponentData
    {
        public NativeList<int> coordinateData;
        [NativeDisableParallelForRestriction]
        public NativeList<int> indexBuffer;
    }
}
