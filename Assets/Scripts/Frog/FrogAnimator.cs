using System.Collections.Generic;
using UnityEngine;

public class FrogAnimator : MonoBehaviour
{
    Animator animator;

    Dictionary<FrogAnimation, int> animations = new Dictionary<FrogAnimation, int>();

    void Awake()
    {
        animator = GetComponent<Animator>();
        animations.Add(FrogAnimation.Idle, Animator.StringToHash("Base Layer.Armature|idle_loop"));
        animations.Add(FrogAnimation.AngryIdle, Animator.StringToHash("Base Layer.Armature|angry_idle_loop"));

        animations.Add(FrogAnimation.Walk, Animator.StringToHash("Base Layer.Armature|walk1"));

        animations.Add(FrogAnimation.RandomAngry, Animator.StringToHash("Base Layer.Armature|angry_waah"));
        animations.Add(FrogAnimation.RandomFurious, Animator.StringToHash("Base Layer.Armature|angry_bigwaah"));

        animations.Add(FrogAnimation.Dance, Animator.StringToHash("Base Layer.Armature|fun_dance1"));
        animations.Add(FrogAnimation.Play1, Animator.StringToHash("Base Layer.Armature|fun_play1"));
        animations.Add(FrogAnimation.Play2, Animator.StringToHash("Base Layer.Armature|fun_play2"));

        animations.Add(FrogAnimation.FoodNear, Animator.StringToHash("Base Layer.Armature|hunger_gimme"));
        animations.Add(FrogAnimation.RandomHungry, Animator.StringToHash("Base Layer.Armature|hunger_stand"));

        animations.Add(FrogAnimation.RandomPotty, Animator.StringToHash("Base Layer.Armature|potty_need_stand"));
        animations.Add(FrogAnimation.PottySit, Animator.StringToHash("Base Layer.Armature|potty_sit_loop"));
        animations.Add(FrogAnimation.PottyStand, Animator.StringToHash("Base Layer.Armature|potty_stand"));

        animations.Add(FrogAnimation.SleepStanding, Animator.StringToHash("Base Layer.Armature|sleepy_stand"));
        animations.Add(FrogAnimation.SleepFaceDown, Animator.StringToHash("Base Layer.Armature|sleepy_sleeper_loop"));
    }

    public void PlayAnimation(FrogAnimation anim)
    {
        //animator.Play(animations[anim]);
        animator.CrossFade(animations[anim], 0.5f);
    }
}

public enum FrogAnimation
{
    Idle,
    AngryIdle,
    RandomAngry,
    RandomFurious,
    Dance,
    Play1,
    Play2,
    FoodNear,
    RandomHungry,
    RandomPotty,
    PottySit,
    PottyStand,
    SleepStanding,
    SleepFaceDown,
    Walk
}
