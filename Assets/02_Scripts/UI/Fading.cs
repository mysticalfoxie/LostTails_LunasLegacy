using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class Fading : MonoBehaviour
{
    public const string FADE_IN_ANIMATION_TAG = "FadeIn";
    public const string FADE_OUT_ANIMATION_TAG = "FadeOut";
    
    private static readonly int FadeInAnimation = Animator.StringToHash("FadeIn");
    private static readonly int FadeOutAnimation = Animator.StringToHash("FadeOut");
    private static readonly int SpeedModifier = Animator.StringToHash("SpeedFactor");
    
    private Animator _animator;
    private string _currentAnimationTag;
    private bool _skipCurrentTick;
    private event Action OnAnimationDone;
    private bool _fadingOut;
    private bool _fadingIn;

    [Range(0.001F, 4.0F)]
    [SerializeField] 
    private float speedModifier = 1.0F;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _animator.SetFloat(SpeedModifier, speedModifier);
    }
    
    public void FadeIn()
    {
        StartCoroutine(FadeInAsync());
    }

    public void FadeOut()
    {
        StartCoroutine(FadeOutAsync());
    }

    public IEnumerator FadeInAsync()
    {
        if (_fadingIn) yield break;
        _fadingIn = true;
        _animator.SetBool(FadeInAnimation, true);
        _animator.SetBool(FadeOutAnimation, false);
        _currentAnimationTag = FADE_IN_ANIMATION_TAG;
        _skipCurrentTick = true;
        yield return WaitForAnimationAsync();
        _animator.SetBool(FadeInAnimation, false);
        _animator.SetBool(FadeOutAnimation, false);
        _fadingIn = false;
    }

    public IEnumerator FadeOutAsync()
    {
        if (_fadingOut) yield break;
        _fadingOut = true;
        _animator.SetBool(FadeInAnimation, false);
        _animator.SetBool(FadeOutAnimation, true);
        _currentAnimationTag = FADE_OUT_ANIMATION_TAG;
        _skipCurrentTick = true;
        yield return WaitForAnimationAsync();
        _animator.SetBool(FadeInAnimation, false);
        _animator.SetBool(FadeOutAnimation, false);
        _fadingOut = false;
    }
    
    private IEnumerator WaitForAnimationAsync()
    {
        var animationDone = false;
        void Unsubscribe() => OnAnimationDone -= Handler;
        void Handler()
        {
            animationDone = true;
            Unsubscribe();
        }

        OnAnimationDone += Handler; 
        
        return new WaitWhile(() => !animationDone);
    }

    private void ObserveAnimationState()
    {
        if (_currentAnimationTag == default) return;
        if (_skipCurrentTick) return; // In this exact tick the fading has been started -> It's be
        var state = _animator.GetCurrentAnimatorStateInfo(0);
        if (state.IsTag(_currentAnimationTag)) return; // Still the same tag -> Animation is still playing
        _currentAnimationTag = default;
        OnAnimationDone?.Invoke();
    }
    
    private void FixedUpdate()
    {
        if (_skipCurrentTick) 
            _skipCurrentTick = false;
    }

    private void Update()
    {
        ObserveAnimationState();
        
        /* Debugging purpose */
        if (Input.GetKeyDown(KeyCode.F3)) FadeIn();
        if (Input.GetKeyDown(KeyCode.F2)) FadeOut();
    }
}
