using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    [SerializeField] Animator animator;

  
    public void SetSpeedMultiplier(float speedMultiply)
    {
        animator.SetFloat("speedMultipler", speedMultiply);
    }

    public void PlayJumpAnimation(bool jumpState)
    {
        animator.SetBool("isJumping", jumpState);
    }
    public void PlayFallAnimation(bool fallState)
    {
        animator.SetBool("isFalling", fallState);
    }
    public void PlayRunAnimation(float speedValue)
    {
        animator.SetFloat("speed", speedValue);
    }
    public void PlayTrainAnimation(bool trainState)
    {     
        animator.SetBool("isTrain", trainState);
    }

    public void SwapFightAnimsLayer(bool state)
    {
        if (state)
            animator.SetLayerWeight(1, 1);
        else animator.SetLayerWeight(1, 0);
    }

    public void FistAttackAnimation()
    {
        animator.SetTrigger("attack");
    }
    public void DeathAnimation(bool state)
    {
        animator.SetBool("isDead", state);
    }
    public void WinAnimation()
    {
        animator.SetTrigger("win");
    }


}
