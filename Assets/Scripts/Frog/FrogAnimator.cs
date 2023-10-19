using System.Collections.Generic;
using System.Collections;
using HietakissaUtils;
using System.Linq;
using UnityEngine;

public class FrogAnimator : MonoBehaviour
{
    Animator animator;

    Dictionary<FrogAnimation, int> animations = new Dictionary<FrogAnimation, int>();

    [SerializeField] FrogAnimation baseAnimation;

    public FrogAnimation CurrentlyPlaying;

    Coroutine animCoroutine;
    //Coroutine baseAnimCoroutine;

    bool overridingAnimation;

    static FrogAnimation[] BaseAnimations = { FrogAnimation.Idle, FrogAnimation.AngryIdle, FrogAnimation.FoodNear, FrogAnimation.Walk, FrogAnimation.SleepStanding, FrogAnimation.SleepFaceDown, FrogAnimation.PottySit};

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

    //looping base animation(idle, angry idle, food near, walk, sleep, potty sit)
    //overriding one-off animations
    //one-off animations immediately play their own respective animation and then go back to the base animation

    public void Play(FrogAnimation anim)
    {
        //animator.Play(animations[anim]);
        PlayAnimation(anim);
    }

    public void PlayRandom(params FrogAnimation[] anims)
    {
        //animator.Play(animations[anim]);
        FrogAnimation anim = anims.RandomElement();
        PlayAnimation(anim);
    }

    void PlayAnimation(FrogAnimation anim)
    {
        if (baseAnimation == anim)
        {
            Debug.Log($"Given animation '{anim}' was same as previous base animation '{baseAnimation}'");
            return;
        }

        if (BaseAnimations.Contains(anim))
        {
            //if (baseAnimCoroutine != null) StopCoroutine(baseAnimCoroutine);
            baseAnimation = anim;
            //baseAnimCoroutine = StartCoroutine(BaseAnimCoroutine());
            
            if (!overridingAnimation) ForcePlayAnimation(baseAnimation);
        }
        else
        {
            if (overridingAnimation) StopCoroutine(animCoroutine);
            animCoroutine = StartCoroutine(OverrideAnimCoroutine(anim));
        }
    }

    IEnumerator OverrideAnimCoroutine(FrogAnimation anim)
    {
        overridingAnimation = true;

        ForcePlayAnimation(anim);
        float animationLength = animator.GetCurrentAnimatorStateInfo(0).length;

        //Debug.Log($"Played one-off animation with a duration of {animationLength} seconds");

        yield return new WaitForSeconds(animationLength);
        overridingAnimation = false;

        Debug.Log("Force played base animation after overriding");
        ForcePlayAnimation(baseAnimation);
        //baseAnimCoroutine = StartCoroutine(BaseAnimCoroutine());
    }

    /*IEnumerator BaseAnimCoroutine()
    {
        ForcePlayAnimation(baseAnimation);
        float animationLength = animator.GetCurrentAnimatorStateInfo(0).length;

        Debug.Log($"Starting loop of base animation '{baseAnimation}' length: '{animationLength}'");

        while (true)
        {
            yield return new WaitForSeconds(animationLength);
            ForcePlayAnimation(baseAnimation, true);
            animationLength = animator.GetCurrentAnimatorStateInfo(0).length;
            Debug.Log($"Loop anim '{baseAnimation}', again in '{animationLength}s'");
        }
    }*/

    public void ForcePlayAnimation(FrogAnimation anim, bool stopPrevious = false, float transition = 0.5f)
    {
        CurrentlyPlaying = anim;
        if (stopPrevious)
        {
            animator.Play(animations[anim]);
        }
        else animator.CrossFade(animations[anim], transition);
    }

    void OnEnable()
    {
        GameManager.OnPause += PauseAnimations;
        GameManager.OnUnPause += UnpauseAnimations;
    }

    void OnDisable()
    {
        GameManager.OnPause -= PauseAnimations;
        GameManager.OnUnPause -= UnpauseAnimations;
    }

    void PauseAnimations()
    {
        animator.speed = 0f;
    }
    
    void UnpauseAnimations()
    {
        animator.speed = 1f;
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
