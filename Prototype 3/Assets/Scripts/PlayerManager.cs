using UnityEngine;

[RequireComponent(typeof(JumpController), typeof(AnimationController), typeof(ParticlesController)),
 RequireComponent(typeof(SoundController))]
public class PlayerManager : MonoBehaviour
{
    #region Variables
    public enum State {Running, Jumping, Sliding};
    public static State playerState;

    enum Action {Idle, Jump, StartSlide, StopSlide};
    Action nextAction;
    int maxAllowedJumps;
    int currJumpsCounter;
    JumpController jumpController;
    SlideController slideController;
    AnimationController animations;
    ParticlesController particles;
    SoundController sounds;
    #endregion


    void Awake() {
        nextAction = Action.Idle;
        playerState = State.Running;
        jumpController = GetComponent<JumpController>();
        maxAllowedJumps = jumpController.allowedAirJumps + 1;
        currJumpsCounter = 0;
        slideController = GetComponent<SlideController>();
        animations = GetComponent<AnimationController>();
        particles = GetComponent<ParticlesController>();
        sounds = GetComponent<SoundController>();
    }

    void Update() {
        // if (GameController.IsPlaying) {
        //     if (Input.GetButtonDown(Consts.JumpButtonName) && currJumpsCounter < maxAllowedJumps) {
        //         nextAction = Action.Jump;
        //     } else if (Input.GetButtonDown(Consts.SlideButtonName) && playerState == State.Running) {
        //         nextAction = Action.StartSlide;
        //     } else if (Input.GetButtonUp(Consts.SlideButtonName) && playerState == State.Sliding) {
        //         nextAction = Action.StopSlide;
        //     }
        // }
    }

    void FixedUpdate() {
        switch (nextAction)
        {
            case Action.Jump:
                Jump();
                nextAction = Action.Idle;
                break;
            case Action.StartSlide:
                StartSlide();
                nextAction = Action.Idle;
                break;
            case Action.StopSlide:
                StopSlide();
                nextAction = Action.Idle;
                break;
        }
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.collider.CompareTag(Consts.GroundTag)) {
            currJumpsCounter = 0;
            playerState = State.Running;
            if (GameController.IsPlaying) {
                particles.PlayRunParticles();
            }
            if (Input.GetButton(Consts.SlideButtonName)) {
                nextAction = Action.StartSlide;
            }
        } else if (collision.collider.CompareTag(Consts.ObstacleTag) && GameController.IsPlaying) {
            GameController.EndGame();
            animations.PlayDeathAnimation();
            sounds.PlayCrashSound();
            particles.PlayExplosion();
        }
    }

    void StopSlide()
    {
        slideController.StopSliding();
        playerState = State.Running;
    }

    void StartSlide()
    {
        slideController.StartSliding();
        playerState = State.Sliding;
    }

    void Jump() {
        slideController.StopSliding();
        jumpController.Jump();
        currJumpsCounter += 1;
        playerState = State.Jumping;
        particles.StopRunParticles();
        animations.PlayJumpAnimation();
        sounds.PlayJumpSound();
    }

    public void JumpButtonDown() {
        if (GameController.IsPlaying && currJumpsCounter < maxAllowedJumps) {
            nextAction = Action.Jump;
        }
    }

    public void SlideButtonDown() {
        if (GameController.IsPlaying && playerState == State.Running) {
            nextAction = Action.StartSlide;
        }
    }

    public void SlideButtonUp() {
        if (GameController.IsPlaying && playerState == State.Sliding) {
            nextAction = Action.StopSlide;
        }
    }
}
