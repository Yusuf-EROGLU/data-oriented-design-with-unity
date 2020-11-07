﻿using System;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;


//ComponentSystem, Systembase, JobComponentSystem bir birlerinin alternatifi
//ComponentSystem sadece main treade çalışıyor
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
            

            trans.Value = new float3(trans.Value.x, trans.Value.y,   zPosition);
        });
    }
}