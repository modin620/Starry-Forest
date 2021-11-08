using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Acceleration
{ 
    public enum AccelerationLevel
    {
        One,
        Two
    }
}


public class Definition : MonoBehaviour
{
    public const string WALK_CLIP = "walk";
    public const string JUMP_CLIP = "jump";
    public const string SLIDING_CLIP = "sliding";
    public const string ITEM_CLIP = "item";
    public const string THORN_CLIP = "thron"; // == vine clip
    public const string RECOVER_CLIP = "recover";
    public const string DOWNHILL_CLIP = "downhill";
    public const string DANDELION_CLIP = "dandelion";
    public const string DASH_CLIP = "dash";
    public const string DOUBLE_DASH_CLIP = "doubleDash";

    public const float WALK_VOLUME = 0.3f;
    public const float JUMP_VOLUME = 0.6f;
    public const float SLIDING_VOLUME = 0.3f;
    public const float ITEM_VOLUME = 0.6f;
    public const float THORN_VOLUME = 0.6f;
    public const float RECOVER_VOLUME = 0.8f;
    public const float DOWNHILL_VOLUME = 0.3f;
    public const float DANDELION_VOLUME = 0.4f;
    public const float DASH_VOLUME = 0.6f;
    public const float DOUBLE_DASH_VOLUME = 0.6f;
}
