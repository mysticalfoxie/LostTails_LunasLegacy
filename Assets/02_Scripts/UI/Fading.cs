using System.Collections;
using UnityEngine;

public class Fading : MonoBehaviour
{
    public const string FADE_IN_ANIMATION_TAG = "FadeIn";
    public const string FADE_OUT_ANIMATION_TAG = "FadeOut";
    
    private static readonly int _fadeInAnimationParameter = Animator.StringToHash("FadeIn");
    private static readonly int _fadeOutAnimationParameter = Animator.StringToHash("FadeOut");
    private static readonly int _speedFactorParameter = Animator.StringToHash("SpeedFactor");
    
    private Animator _animator;
    private string _currentAnimationTag;
    private int _skipTickCount;

    private bool _animationInProgress;
    private bool _animationDone;

    [Header("Animation Properties")]
    [Range(0.001F, 4.0F)] 
    [SerializeField] 
    private float _globalSpeedModifier = 1.0F;
    
    [Header("Animation Observer")] 
    [Range(1, 100)] 
    [SerializeField] private int _tickDelay; 

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _animator.SetFloat(_speedFactorParameter, _globalSpeedModifier);
    }
    
    public void FadeIn(float localSpeedModifier = 1.0F)
    {
        StartCoroutine(FadeInAsync(localSpeedModifier));
    }

    public void FadeOut(float localSpeedModifier = 1.0F)
    {
        StartCoroutine(FadeOutAsync(localSpeedModifier));
    }

    public IEnumerator FadeInAsync(float localSpeedModifier = 1.0F)
    {
        if (_animationInProgress) yield break;
        _animator.SetFloat(_speedFactorParameter, localSpeedModifier * _globalSpeedModifier);
        _animator.SetBool(_fadeInAnimationParameter, true);
        _animator.SetBool(_fadeOutAnimationParameter, false);
        _currentAnimationTag = FADE_IN_ANIMATION_TAG;
        _skipTickCount = _tickDelay;
        yield return WaitForAnimationAsync();
        _animator.SetBool(_fadeInAnimationParameter, false);
        _animator.SetBool(_fadeOutAnimationParameter, false);
        _animator.SetFloat(_speedFactorParameter, _globalSpeedModifier);
    }

    public IEnumerator FadeOutAsync(float localSpeedModifier = 1.0F)
    {
        if (_animationInProgress) yield break;
        _animator.SetFloat(_speedFactorParameter, localSpeedModifier * _globalSpeedModifier);
        _animator.SetBool(_fadeInAnimationParameter, false);
        _animator.SetBool(_fadeOutAnimationParameter, true);
        _currentAnimationTag = FADE_OUT_ANIMATION_TAG;
        _skipTickCount = _tickDelay;
        yield return WaitForAnimationAsync();
        _animator.SetBool(_fadeInAnimationParameter, false);
        _animator.SetBool(_fadeOutAnimationParameter, false);
        _animator.SetFloat(_speedFactorParameter, _globalSpeedModifier);
    }
    
    private IEnumerator WaitForAnimationAsync()
    {
        _animationInProgress = true;
        _animationDone = false;
        yield return new WaitWhile(() => !_animationDone);
        _animationInProgress = false;
        _animationDone = false;
    }

    private void ObserveAnimationState()
    {
        if (_currentAnimationTag == default) return;
        if (!_animationInProgress) return;
        if (_skipTickCount > 0) return;
        
        var state = _animator.GetCurrentAnimatorStateInfo(0);
        if (state.IsTag(_currentAnimationTag)) return; // Still the same tag -> Animation is still playing
        _currentAnimationTag = default;
        
        _animationDone = true;
    }
    
    private void FixedUpdate()
    {
        if (_skipTickCount > 0) _skipTickCount--;
    }
    
    private void Update()
    {
        ObserveAnimationState();
        
        /* Debugging purpose */
        if (!Debug.isDebugBuild) return;
        if (Input.GetKeyDown(KeyCode.F3)) FadeIn();
        if (Input.GetKeyDown(KeyCode.F2)) FadeOut();
    }
}
