using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AnimationAudio : MonoBehaviour
{
    [SerializeField]private EventReference catWalk;
    [SerializeField]private Animator anim;
    [SerializeField]private GameObject audioSourcePawFront;
    [SerializeField]private GameObject audioSourcePawBack;

    // Astrid Parameters
    readonly int astrid_PlayStep = Animator.StringToHash("astrid_PlayStep");
    // Astrid States
    readonly int astrid_Grounded = Animator.StringToHash("Grounded");
    readonly int astrid_Sneaking = Animator.StringToHash("Sneaking");
    readonly int astrid_Running = Animator.StringToHash("Running");
    readonly int astrid_Jump = Animator.StringToHash("Jump");
    readonly int astrid_SuperJumpActive = Animator.StringToHash("SuperJumpActive");
    
    // Animator State Info
    protected AnimatorStateInfo m_CurrentStateInfo;    // Information about the base layer of the animator cached.
    protected AnimatorStateInfo m_NextStateInfo;
    protected bool m_IsAnimatorTransitioning;
    protected AnimatorStateInfo m_PreviousCurrentStateInfo;    // Information about the base layer of the animator from last frame.
    protected AnimatorStateInfo m_PreviousNextStateInfo;
    protected bool m_PreviousIsAnimatorTransitioning;
    
    public void AnimationAudioPlay()
    {
        float catWalkCurve = anim.GetFloat(astrid_PlayStep);
        if (astrid_PlayStep > 0.9f)
        {
            EventInstance instance = RuntimeManager.CreateInstance(catWalk);
            RuntimeManager.AttachInstanceToGameObject(instance, audioSourcePawFront.transform);
            instance.start();
            instance.release();
        }
    }
    
    // Called at the start of FixedUpdate to record the current state of the base layer of the animator.
    void CacheAnimatorState()
    {
        m_PreviousCurrentStateInfo = m_CurrentStateInfo;
        m_PreviousNextStateInfo = m_NextStateInfo;
        m_PreviousIsAnimatorTransitioning = m_IsAnimatorTransitioning;

        m_CurrentStateInfo = anim.GetCurrentAnimatorStateInfo(0);
        m_NextStateInfo = anim.GetNextAnimatorStateInfo(0);
        m_IsAnimatorTransitioning = anim.IsInTransition(0);
    }
}
