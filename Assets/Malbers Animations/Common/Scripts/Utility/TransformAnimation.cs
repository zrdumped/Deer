using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MalbersAnimations
{
    public enum AnimCycle {None, Loop, Repeat, PingPong }

    [CreateAssetMenu(menuName = "MalbersAnimations/AnimTransform")]
    public class TransformAnimation : ScriptableObject
    {

        static Keyframe[] K = {new Keyframe(0,0), new Keyframe(1,1) };

        public float time = 0.5f;
        public float delay = 0.0001f;
        //public AnimCycle cycle;

        public bool UsePosition = true;
        public Vector3 Position;
        public AnimationCurve PosCurve = new AnimationCurve(K);

        //public bool UseAxisPos = false;
        //public AnimationCurve PosXCurve = new AnimationCurve(K);
        //public AnimationCurve PosYCurve = new AnimationCurve(K);
        //public AnimationCurve PosZCurve = new AnimationCurve(K);

        public bool UseRotation = true;
        public Vector3 Rotation;
        public AnimationCurve RotCurve = new AnimationCurve(K);

        public bool UseScale = true;
        public Vector3 Scale = Vector3.one;
        public AnimationCurve ScaleCurve = new AnimationCurve(K);

    }
}