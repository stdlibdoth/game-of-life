using UnityEngine;
using Unity.Entities;
using Unity.Collections;

public class CanvasDrawer : MonoBehaviour
{

    private GraphicsBuffer m_triangles;
    private GraphicsBuffer m_positions;
    private GraphicsBuffer m_coords;
    private GraphicsBuffer m_commands;


    [SerializeField] private Mesh m_mesh;
    [SerializeField] private Material m_material;
    public CanvasSize size;
    private RenderParams m_params;

    private void Awake()
    {
        Application.targetFrameRate = 0;
        QualitySettings.vSyncCount = 0;
        var manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        var ent = manager.CreateEntity();
        manager.AddComponentObject(ent,this);


        m_triangles = new GraphicsBuffer(GraphicsBuffer.Target.Structured, m_mesh.triangles.Length, sizeof(int));
        m_triangles.SetData(m_mesh.triangles);
        m_positions = new GraphicsBuffer(GraphicsBuffer.Target.Structured, m_mesh.vertices.Length, 3 * sizeof(float));
        m_positions.SetData(m_mesh.vertices);
        m_coords = new GraphicsBuffer(GraphicsBuffer.Target.Structured, size.x * size.y, sizeof(int));
        m_commands = new GraphicsBuffer(GraphicsBuffer.Target.IndirectArguments, 1, GraphicsBuffer.IndirectDrawArgs.size);

        Vector3 startPosition = new Vector3(-size.x / 2, -size.y / 2, 0);
        m_params = new RenderParams(m_material);
        m_params.worldBounds = new Bounds(Vector3.zero, 10000000 * Vector3.one);
        m_params.matProps = new MaterialPropertyBlock();
        m_params.matProps.SetBuffer("_Triangles", m_triangles);
        m_params.matProps.SetBuffer("_Positions", m_positions);
        m_params.matProps.SetInt("_BaseVertexIndex", (int)m_mesh.GetBaseVertex(0));
        m_params.matProps.SetInt("_EdgeLength", size.x);
        m_params.matProps.SetMatrix("_ObjectToWorld", Matrix4x4.Translate(startPosition));

        var commandData = new GraphicsBuffer.IndirectDrawArgs[1];
        commandData[0].vertexCountPerInstance = m_mesh.GetIndexCount(0);
        commandData[0].instanceCount = 1;
        m_commands.SetData(commandData);


        var tickEnt = manager.CreateEntity();
        manager.AddComponentData(tickEnt, new PrevTickTime { value = Time.time });

        var canvasEnt = manager.CreateEntity();
        manager.AddComponentData(canvasEnt, size);

    }


    private void Update()
    {
        Graphics.RenderPrimitivesIndirect(m_params, MeshTopology.Triangles, m_commands, 1);
    }


    public void SetCoords(NativeArray<int> data)
    {
        m_coords.SetData(data);
        m_params.matProps.SetBuffer("_Indices", m_coords);
        var commandData = new GraphicsBuffer.IndirectDrawArgs[1];
        commandData[0].vertexCountPerInstance = m_mesh.GetIndexCount(0);
        commandData[0].instanceCount = (uint)data.Length;
        m_commands.SetData(commandData);
    }

    private void OnDestroy()
    {
    }
}
