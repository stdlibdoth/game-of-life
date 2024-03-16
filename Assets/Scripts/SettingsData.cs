using Unity.Burst;
using Unity.Mathematics;
using System.Runtime.CompilerServices;

[BurstCompile]
public static class SettingsData
{
    public const int tickRate = 4;
    public const int chunkSize_x = 100;
    public const int chunkSize_y = 100;
    public const float timeStep = 0.25f;

    public const int schedulelingFrames = 10;
}