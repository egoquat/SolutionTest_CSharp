using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;



namespace SimplePerformanceTest_01.Scripts.SimpleProfiler
{
    public static class SimpleProfiler
    {
        class ProfileUnit
        {
            Stopwatch _stopWatch = new Stopwatch();
            float _elapsedSeconds = float.MinValue;




            public float End()
            {
                _stopWatch.Stop();
                _elapsedSeconds = _stopWatch.Elapsed.Ticks / TimeSpan.TicksPerSecond;
                return _elapsedSeconds;
            }

            public void Begin()
            {
                _stopWatch.Start();
            }

            public ProfileUnit()
            {
                
            }
        }

        Dictionary<string, ProfileUnit> _profiles = new Dictionary<string, ProfileUnit>();

        public static void BeginProfie(string instanceID)
        {
            
        }

        public static void EndProfile(string instanceID)
        {

        }

        public static void OutputProfile()
        {

        }

        SimpleProfiler()
        {
            _stopWatch.Reset();
        }
    }
}
