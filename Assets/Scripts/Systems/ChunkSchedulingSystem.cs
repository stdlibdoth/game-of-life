using Unity.Entities;
using Unity.Collections;
using UnityEngine;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Burst.Intrinsics;
using Unity.Jobs;

[UpdateInGroup(typeof(SimulationSystemGroup))]
public unsafe partial struct ChunkSchedulingSystem : ISystem
{
    private EntityQuery m_dataUpdatingQuery;
    private EntityQuery m_chunkDataQuery;


    private NativeArray<EntityQuery> m_dataQueries;

    private BufferTypeHandle<ChunkDataBuffer> m_dataBufferHandle;
    private ComponentTypeHandle<ChunkDataCoord> m_dataIdHandle;
    private ComponentTypeHandle<DataIndexCount> m_indexCountHandle;

    private NativeArray<JobHandle> m_jobHandles;

    private int m_scheduleingBatchCount;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        m_dataQueries = new NativeArray<EntityQuery>(SettingsData.schedulelingFrames, Allocator.Persistent);
        m_jobHandles = new NativeArray<JobHandle>(SettingsData.schedulelingFrames, Allocator.Persistent);
        m_dataQueries[0] = new EntityQueryBuilder(Allocator.Temp)
            .WithAll<ChunkDataBuffer, ChunkDataReadyTag, ScheduleBatch0Tag>()
            .Build(ref state);
        m_dataQueries[1] = new EntityQueryBuilder(Allocator.Temp)
            .WithAll<ChunkDataBuffer, ChunkDataReadyTag, ScheduleBatch1Tag>()
            .Build(ref state);
        m_dataQueries[2] = new EntityQueryBuilder(Allocator.Temp)
            .WithAll<ChunkDataBuffer, ChunkDataReadyTag, ScheduleBatch2Tag>()
            .Build(ref state);
        m_dataQueries[3] = new EntityQueryBuilder(Allocator.Temp)
            .WithAll<ChunkDataBuffer, ChunkDataReadyTag, ScheduleBatch3Tag>()
            .Build(ref state);
        m_dataQueries[4] = new EntityQueryBuilder(Allocator.Temp)
            .WithAll<ChunkDataBuffer, ChunkDataReadyTag, ScheduleBatch4Tag>()
            .Build(ref state);
        m_dataQueries[5] = new EntityQueryBuilder(Allocator.Temp)
            .WithAll<ChunkDataBuffer, ChunkDataReadyTag, ScheduleBatch5Tag>()
            .Build(ref state);
        m_dataQueries[6] = new EntityQueryBuilder(Allocator.Temp)
            .WithAll<ChunkDataBuffer, ChunkDataReadyTag, ScheduleBatch6Tag>()
            .Build(ref state);
        m_dataQueries[7] = new EntityQueryBuilder(Allocator.Temp)
            .WithAll<ChunkDataBuffer, ChunkDataReadyTag, ScheduleBatch7Tag>()
            .Build(ref state);
        m_dataQueries[8] = new EntityQueryBuilder(Allocator.Temp)
            .WithAll<ChunkDataBuffer, ChunkDataReadyTag, ScheduleBatch8Tag>()
            .Build(ref state);
        m_dataQueries[9] = new EntityQueryBuilder(Allocator.Temp)
            .WithAll<ChunkDataBuffer, ChunkDataReadyTag, ScheduleBatch9Tag>()
            .Build(ref state);

        m_dataUpdatingQuery = new EntityQueryBuilder(Allocator.Temp)
            .WithAll<ChunkDataBuffer, ChunkDataUpdatingTag>()
            .Build(ref state);
        m_chunkDataQuery = new EntityQueryBuilder(Allocator.Temp)
            .WithAll<ChunkDataBuffer>()
            .Build(ref state);

        m_dataBufferHandle = state.GetBufferTypeHandle<ChunkDataBuffer>(false);
        m_dataIdHandle = state.GetComponentTypeHandle<ChunkDataCoord>(true);
        m_indexCountHandle = state.GetComponentTypeHandle<DataIndexCount>(false);
        m_scheduleingBatchCount = 0;
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        //Spawn chunks
        if(m_chunkDataQuery.IsEmpty && SystemAPI.TryGetSingleton(out CanvasSize canvasSize))
        { 
            var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
            int chunkCount_x = (int)math.ceil(canvasSize.x * 1.0f / SettingsData.chunkSize_x);
            int chunkCount_y = (int)math.ceil(canvasSize.y * 1.0f / SettingsData.chunkSize_y);
            int size = SettingsData.chunkSize_x * SettingsData.chunkSize_y;
            int batchSize = chunkCount_x * chunkCount_y / SettingsData.schedulelingFrames;
            for (int y = 0; y < chunkCount_y; y++)
            {
                for (int x = 0; x < chunkCount_x; x++)
                {
                    var ent = ecb.CreateEntity();
                    ecb.AddBuffer<ChunkDataBuffer>(ent);
                    for (int i = 0; i < size; i++)
                    {
                        ecb.AppendToBuffer(ent, new ChunkDataBuffer { value = 0x00 });
                    }
                    ecb.AddComponent(ent, new ChunkDataCoord { value = new int2(x, y) });
                    ecb.AddComponent(ent, new DataIndexCount { value = 0 });
                    int batch = (x + chunkCount_x * y)%SettingsData.schedulelingFrames;
                    if (batch == 0)
                        ecb.AddComponent<ScheduleBatch0Tag>(ent);
                    if (batch == 1)
                        ecb.AddComponent<ScheduleBatch1Tag>(ent);
                    if (batch == 2)
                        ecb.AddComponent<ScheduleBatch2Tag>(ent);
                    if (batch == 3)
                        ecb.AddComponent<ScheduleBatch3Tag>(ent);
                    if (batch == 4)
                        ecb.AddComponent<ScheduleBatch4Tag>(ent);
                    if (batch == 5)
                        ecb.AddComponent<ScheduleBatch5Tag>(ent);
                    if (batch == 6)
                        ecb.AddComponent<ScheduleBatch6Tag>(ent);
                    if (batch == 7)
                        ecb.AddComponent<ScheduleBatch7Tag>(ent);
                    if (batch == 8)
                        ecb.AddComponent<ScheduleBatch8Tag>(ent);
                    if (batch == 9)
                        ecb.AddComponent<ScheduleBatch9Tag>(ent);

                    ecb.AddComponent<ChunkDataReadyTag>(ent);
                }
            }
        }
        //Schedule jobs
        if (m_scheduleingBatchCount < SettingsData.schedulelingFrames && !m_dataQueries[m_scheduleingBatchCount].IsEmpty && SystemAPI.TryGetSingleton(out PreRenderingSystem.Singleton preRender)
            && SystemAPI.TryGetSingleton(out PostRenderingSystem.Singleton postRender))
        {

            m_dataBufferHandle.Update(ref state);
            m_dataIdHandle.Update(ref state);
            m_indexCountHandle.Update(ref state);
            var size = SystemAPI.GetSingleton<CanvasSize>();
            var processJob = new ChunkDataProcessJob
            {
                indexBuffer = preRender.indexBuffer,
                prevTickDataBuffer = postRender.prevTickDataBuffer,
                dataBufferHandle = m_dataBufferHandle,
                dataCoordHandle = m_dataIdHandle,
                canvasSize = size,
                dataChunkSize = new int2(SettingsData.chunkSize_x, SettingsData.chunkSize_y),
                dataIndexCountHandle = m_indexCountHandle,
            };
            m_jobHandles[m_scheduleingBatchCount] = processJob.ScheduleParallel(m_dataQueries[m_scheduleingBatchCount], state.Dependency);
            var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
            var ents = m_dataQueries[m_scheduleingBatchCount].ToEntityArray(Allocator.Temp);
            ecb.AddComponent<ChunkDataUpdatingTag>(ents);
            ecb.RemoveComponent<ChunkDataReadyTag>(ents);
            state.Dependency = m_jobHandles[m_scheduleingBatchCount];
            m_scheduleingBatchCount++;
        }

        if (!m_dataUpdatingQuery.IsEmpty && m_scheduleingBatchCount == SettingsData.schedulelingFrames)
        {
            bool complete = true;
            for (int i = 0; i < m_jobHandles.Length; i++)
            {
                if (m_jobHandles[i].IsCompleted)
                    m_jobHandles[i].Complete();
                complete &= m_jobHandles[i].IsCompleted;
            }
            if (!complete)
                return;
            var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
            var ents = m_dataUpdatingQuery.ToEntityArray(Allocator.Temp);
            ecb.AddComponent<ChunkDataUpdatedTag>(ents);
            ecb.RemoveComponent<ChunkDataUpdatingTag>(ents);
            m_scheduleingBatchCount = 0;
        }
    }

    public void OnDestroy(ref SystemState state)
    {
        m_dataQueries.Dispose();
    }
}

[BurstCompile]
public unsafe struct ChunkDataProcessJob : IJobChunk
{
    [NativeDisableParallelForRestriction]
    public NativeList<int> indexBuffer;

    [ReadOnly]
    [NativeDisableParallelForRestriction]
    public NativeArray<byte> prevTickDataBuffer;

    public BufferTypeHandle<ChunkDataBuffer> dataBufferHandle;
    [ReadOnly]
    public ComponentTypeHandle<ChunkDataCoord> dataCoordHandle;

    public ComponentTypeHandle<DataIndexCount> dataIndexCountHandle;

    [ReadOnly]
    public CanvasSize canvasSize;
    [ReadOnly]
    public int2 dataChunkSize;


    [BurstCompile]
    public void Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
    {
        var dataSize = dataChunkSize;
        var _canvasSize = canvasSize;
        var _prevDataBuffer = prevTickDataBuffer;
        var _indexBuffer = indexBuffer;

        var dataBufferToWrite = chunk.GetBufferAccessor(ref dataBufferHandle)[0].Reinterpret<byte>();
        var chunkCoord = chunk.GetNativeArray(ref dataCoordHandle)[0];
        var indexCountArray = chunk.GetNativeArray(ref dataIndexCountHandle);

        //Iterate through cells
        int chunkCount_x = (int)math.ceil(canvasSize.x * 1.0f / dataChunkSize.x);
        int indexBufferOffset = (chunkCoord.value.x + chunkCoord.value.y * chunkCount_x)* dataChunkSize.x*dataChunkSize.y;
        int totalLiveCount = 0;
        for ( var i = 0; i < dataBufferToWrite.Length; i++)
        {
            int2 localCoord = new int2(i % dataChunkSize.x, i / dataChunkSize.x);
            int2 canvasCoord = new int2(chunkCoord.value.x * dataChunkSize.x, chunkCoord.value.y * dataChunkSize.y) + localCoord;

            byte data = _prevDataBuffer[indexBufferOffset + i];

            if (data == 0x01)
                CheckLiveSurroundings(i, canvasCoord, indexBufferOffset, ref _indexBuffer, _prevDataBuffer);
            else if(data == 0x00)
                CheckDeadSurroundings(i, canvasCoord, indexBufferOffset, ref _indexBuffer, _prevDataBuffer);
        }
        indexCountArray[0] = new DataIndexCount { value = totalLiveCount };


        [BurstCompile]
        void CheckLiveSurroundings(int local_index, in int2 canvas_coord, int index_buffer_offset, ref NativeList<int> index_buffer, in NativeArray<byte> prev_data_buffer)
        {
            int liveCount = 0;
            int canvas_index = canvas_coord.x + canvas_coord.y * _canvasSize.x;
            for (int y = -1; y <= 1; y++)
            {
                for (int x = -1; x <= 1; x++)
                {
                    if (x == 0 && y == 0)
                        continue;

                    int2 s_canvasCoord = canvas_coord + new int2(x, y);
                    if (s_canvasCoord.x < 0 || s_canvasCoord.y < 0 || s_canvasCoord.x >= _canvasSize.x || s_canvasCoord.y >= _canvasSize.y)
                        continue;
                    int2 s_chunkCoord = new int2(s_canvasCoord.x / dataSize.x, s_canvasCoord.y / dataSize.y);
                    int2 s_localCoord = new int2(s_canvasCoord.x % dataSize.x, s_canvasCoord.y % dataSize.y);
                    int prevDataBufferIndex = ((s_chunkCoord.x + s_chunkCoord.y * chunkCount_x) * dataSize.x * dataSize.y) + (s_localCoord.x + s_localCoord.y * dataSize.x);
                    if (prev_data_buffer[prevDataBufferIndex] == 0x01)
                        liveCount++;
                    if (liveCount >= 4)
                    {
                        dataBufferToWrite[local_index] = 0x00;
                        return;
                    }
                }
            }
            if(liveCount <= 1)
            {
                dataBufferToWrite[local_index] = 0x00;
            }
            else
            {
                index_buffer[index_buffer_offset + totalLiveCount] = canvas_index;
                dataBufferToWrite[local_index] = 0x01;
                totalLiveCount++;
            }
        }

        [BurstCompile]
        void CheckDeadSurroundings(int local_index,in int2 canvas_coord, int index_buffer_offset, ref NativeList<int> index_buffer, in NativeArray<byte> prev_data_buffer)
        {
            int liveCount = 0;
            int canvas_index = canvas_coord.x + canvas_coord.y * _canvasSize.x;
            for (int y = -1; y <= 1; y++)
            {
                for (int x = -1; x <= 1; x++)
                {
                    if (x == 0 && y == 0)
                        continue;
                    int2 s_canvasCoord = canvas_coord + new int2(x, y);
                    if (s_canvasCoord.x < 0 || s_canvasCoord.y < 0 || s_canvasCoord.x >= _canvasSize.x || s_canvasCoord.y >= _canvasSize.y)
                        continue;

                    int2 s_chunkCoord = new int2(s_canvasCoord.x / dataSize.x, s_canvasCoord.y / dataSize.y);
                    int2 s_localCoord = new int2(s_canvasCoord.x % dataSize.x, s_canvasCoord.y % dataSize.y);
                    int prevDataBufferIndex = (s_chunkCoord.x + s_chunkCoord.y * chunkCount_x) * dataSize.x * dataSize.y + s_localCoord.x + s_localCoord.y * dataSize.x;
                    if (prev_data_buffer[prevDataBufferIndex] == 0x01)
                        liveCount++;
                }
            }
            if(liveCount == 3)
            {
                dataBufferToWrite[local_index] = 0x01;
                index_buffer[index_buffer_offset + totalLiveCount] = canvas_index;
                totalLiveCount++;
            }
            else
            {
                dataBufferToWrite[local_index] = 0x00;
            }
        }
    }
}
