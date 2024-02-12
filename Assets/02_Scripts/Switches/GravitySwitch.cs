using UnityEngine;

public class GravitySwitch : MonoBehaviour
{
    [Range(0, 10)]
    [SerializeField] private float _targetGravityScale;
    [SerializeField] private bool _allowJustOnce;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (!enabled) return;
        
        var rigid = other.GetComponent<Rigidbody2D>();
        rigid.gravityScale = _targetGravityScale;

        enabled = !_allowJustOnce && enabled;
    }
}