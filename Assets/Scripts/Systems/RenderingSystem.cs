using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using UnityEngine.Events;

[UpdateInGroup(typeof(TickSystemGroup))]
public partial class CanvasRenderingSystem : SystemBase
{
    private EntityQuery m_renderQuery;
    private EntityQuery m_renderReadyQuery;

    public static UnityEvent<float> onRenderUpdate;

    protected override void OnCreate()
    {
        m_renderQuery = new EntityQueryBuilder(Allocator.Temp)
            .WithAll<CanvasDrawer>()
            .Build(EntityManager);

        m_renderReadyQuery = new EntityQueryBuilder(Allocator.Temp)
            .WithAll<RenderReadyTag, ChunkDataBuffer, ChunkDataCoord, DataIndexCount>()
            .WithNone<ChunkDataReadyTag>()
            .Build(EntityManager);

        onRenderUpdate = new UnityEvent<float>();
    }


    protected override void OnUpdate()
    {
        if(SystemAPI.TryGetSingleton(out PreRenderingSystem.Singleton preRender))
        {
            if (!m_renderReadyQuery.IsEmpty)
            {
                var tick = SystemAPI.GetSingleton<PrevTickTime>();
                var ecb = SystemAPI.GetSingleton<EndVariableRateSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(EntityManager.WorldUnmanaged);
                var render = m_renderQuery.ToComponentArray<CanvasDrawer>()[0];
                render.SetCoords(preRender.coordinateData.AsArray());
                ecb.AddComponent<PostRenderTag>(m_renderReadyQuery,EntityQueryCaptureMode.AtPlayback);
                ecb.RemoveComponent<RenderReadyTag>(m_renderReadyQuery, EntityQueryCaptureMode.AtPlayback);
                onRenderUpdate.Invoke(tick.tickRate);
            }
        }
    }

    protected override void OnDestroy()
    {
    }
}
