using System;
using System.Collections;
using UnityEngine;
using Random=UnityEngine.Random;

    namespace LD55
    {
        public static class CoroutineTemplate
        {
            /// <param name="delay">Delay in seconds</param>
            /// <param name="action">Action to invoke</param>
            public static IEnumerator DelayAndFireRoutine(float delay, Action action)
            {
                yield return new WaitForSeconds(delay);
                action?.Invoke();
            }
        
            /// <param name="delay">Delay in seconds</param>
            /// <param name="action">Action to invoke</param>
            public static IEnumerator DelayAndFireLoopRoutine(float delay, Action action)
            {
                while (true)
                {
                    yield return new WaitForSeconds(delay);
                    action?.Invoke();
                }
            }
        
            /// <param name="delayMin">Minimum delay in seconds</param>
            /// <param name="delayMax">Maximum delay in seconds</param>
            /// <param name="action">Action to invoke</param>
            public static IEnumerator RandomDelayAndFireLoopRoutine(float delayMin, float delayMax, Action action)
            {
                while (true)
                {
                    yield return new WaitForSeconds(Random.Range(delayMin, delayMax));
                    action?.Invoke();
                }
            }
        
            /// <param name="delay">Delay in seconds</param>
            /// <param name="action">Action to invoke</param>
            public static IEnumerator FireAndDelayLoopRoutine(float delay, Action action)
            {
                while (true)
                {
                    action?.Invoke();
                    yield return new WaitForSeconds(delay);
                }
            }
        
            /// <param name="delayMin">Minimum delay in seconds</param>
            /// <param name="delayMax">Maximum delay in seconds</param>
            /// <param name="action">Action to invoke</param>
            public static IEnumerator FireAndRandomDelayLoopRoutine(float delayMin, float delayMax, Action action)
            {
                while (true)
                {
                    action?.Invoke();
                    yield return new WaitForSeconds(Random.Range(delayMin, delayMax));
                }
            }
        }
    }