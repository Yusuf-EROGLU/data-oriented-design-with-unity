using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using Unity.Jobs;

public class JobExample : MonoBehaviour
{
    void Start()
    {
        DoExample();
    }

    private void DoExample()
    {
        NativeArray<float> resultArray = new NativeArray<float>(1, Allocator.TempJob);
        //Instantiate and Initialize Job
        SimpleJob myJob = new SimpleJob
        {
            a = 5f,
            result = resultArray
        };

        AnotherJob secondJob = new AnotherJob
        {
            result = resultArray
        };
        
        // or Initialize Job separately
        
        /* myJob.a = 5f;
          myJob.result = new NativeArray<float>(1, Allocator.TempJob);*/
        /*BUradaki Allocator türlerinin anlamları:
        Allocator.Temp => Last for one frame
        Allocator.TempJob => job ömrü boyunca 4 frame kadar
        Allocator.Persistent => ihtiyaç duyduğun kadar ayırıyor*/
        
        //Schedule Job
        
        JobHandle handle = myJob.Schedule();
        JobHandle secondHandle = secondJob.Schedule(handle);
        /*//job'ın tamamlanıp tamamlanmadığını ve birbirine dependicy'si olan joplar schedulelamaya yarıyor
        //Normalde doğrudan jop Complete edilmez diğer joblarla ilgili işlemler yapılır
        //fakat burada tek job olduğundan direk Complete edilmiş*/
        handle.Complete();
        secondHandle.Complete();
        float resultingValue = resultArray[0];

        Debug.Log("result = " + resultingValue);
        resultArray.Dispose();
    }

    private struct SimpleJob : IJob
    {
        public float a;
        public NativeArray<float> result;

        public void Execute()
        {
            result[0] = a;
        }
    }

    private struct AnotherJob : IJob
    {
        public NativeArray<float> result;

        public void Execute()
        {
            result[0]++;
        }
    }
}