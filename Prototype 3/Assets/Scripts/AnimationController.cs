using UnityEngine;

public class AnimationController : MonoBehaviour
{
    #region Variables
    Animator animator;

    const string JumpTrigger = "Jump_trig";
    const string SpeedParam = "Speed_f";
    const string DeathParam = "Death_b";
    #endregion

    void Awake()
    {
       animator = GetComponent<Animator>(); 
    }

    void Update()
    {
        animator.SetFloat(SpeedParam, GameSpeed.CurrentGameSpeed);
    }

    public void PlayJumpAnimation() {
        animator.SetTrigger(JumpTrigger);
    }

    public void PlayDeathAnimation() {
        animator.SetBool(DeathParam, true);
    }
}