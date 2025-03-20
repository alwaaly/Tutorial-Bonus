using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Talk Sequence", menuName = "ScriptableObject/TalkSequence")]
public class TalkSequence : ScriptableObject {
    public Talk[] Talks;
}
[Serializable]
public class Talk {
    public AudioClip Clip;
    [TextArea]
    public string Subtitle;
    public bool NeedSignature;
    public float Delay;
}