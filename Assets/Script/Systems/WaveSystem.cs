using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

//ComponentSystem, Systembase, JobComponentSystem bir birlerinin alternatifi
//ComponentSystem sadece main treade çalışıyor


//--------------------------------OLDEST VERSION-------------------------------
public class WaveSystem : JobComponentSystem
{
    private struct WaveJob : IJobForEach<Translation, WaveData, MoveSpeedData>
    {
        public float elapsedTime;

        public void Execute(ref Translation trans, ref WaveData waveData, ref MoveSpeedData moveSpeed)
        {
            float zPosition = waveData.amplitude *
                              math.sin(elapsedTime * moveSpeed.Value +
                                       trans.Value.x * waveData.xOffset +
                                       trans.Value.y * waveData.yOffset);
            trans.Value = new float3(trans.Value.x, trans.Value.y, zPosition);
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float elapsedTime = (float) Time.DeltaTime;
        //Instantiate
        var job = new WaveJob();
        
        //Inıtialize
        job.elapsedTime = (float)Time.ElapsedTime;
        JobHandle jobHandle = job.Schedule(this, inputDeps);
        return jobHandle;
    }
}
//-----------------------------------------------------------------------

/* -------------MAIN TREAD VERSION----------------------
 
 public class WaveSystem : ComponentSystem
{
    //similar to Update in monobehaviour
    protected override void OnUpdate()
    {
        Entities.ForEach((ref Translation trans, ref MoveSpeedData moveSpeed, ref WaveData waveData) =>
        {
            float zPosition = waveData.amplitude *
                              math.sin((float) Time.ElapsedTime * moveSpeed.Value + trans.Value.x * waveData.xOffset +
                                       trans.Value.y * waveData.yOffset);


            trans.Value = new float3(trans.Value.x, trans.Value.y, zPosition);
        });
    }
}
--------------------------------------------------------------*/

/*----------JOBCOMPONENTSYSTEM VERSION--------------
 
 public class WaveSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float elapsedTime = (float)Time.DeltaTime;
        JobHandle jobHandle = Entities.ForEach((ref Translation trans, in MoveSpeedData moveSpeed, in WaveData waveData) =>
        {
            float zPosition = waveData.amplitude *
                              math.sin(elapsedTime * moveSpeed.Value +
                                       trans.Value.x * waveData.xOffset +
                                       trans.Value.y * waveData.yOffset);
            trans.Value = new float3(trans.Value.x, trans.Value.y, zPosition);
        }).Schedule(inputDeps);
        //.run(); eklenirse main tread de çalışır.
        // I/O, static members a erişmek için vs. kullanılabilir.
        return jobHandle;
    }
}*/

/*-----------------SYSTEM BASE------------------------------

public class WaveSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float elapsedTime = (float)Time.ElapsedTime;
        Entities.ForEach((ref Translation trans, in MoveSpeedData moveSpeed, in WaveData waveData) =>
        {
            float zPosition = waveData.amplitude *
                              math.sin(elapsedTime * moveSpeed.Value +
                                       trans.Value.x * waveData.xOffset +
                                       trans.Value.y * waveData.yOffset);
            trans.Value = new float3(trans.Value.x, trans.Value.y, zPosition);
        }).Schedule();
        //.run(); eklenirse main tread de çalışır.
        // I/O, static members a erişmek için vs. kullanılabilir.
    }
}
---------------------------------------------------------------------------*/