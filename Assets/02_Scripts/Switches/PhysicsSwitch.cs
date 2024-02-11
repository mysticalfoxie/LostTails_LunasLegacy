using UnityEngine;

public class PhysicsSwitch : MonoBehaviour
{
    private Rigidbody2D _rigidbody;

    [SerializeField] private GameObject _target;
    [SerializeField] private RigidbodyType2D _targetBodyType; // uwu
    [SerializeField] private bool _denyPlayerMovement;
    [SerializeField] private bool _allowJustOnce;

    public void Awake()
    {
        _rigidbody = _target.GetComponent<Rigidbody2D>();
        enabled = _rigidbody is not null;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (!enabled) return;
        
        // This value has at runtime the value "hot"
        _rigidbody.bodyType = _targetBodyType;
        DenyPlayerMovement(other);

        enabled = !_allowJustOnce && enabled;
    }

    private void DenyPlayerMovement(Collider2D other)
    {
        if (!_denyPlayerMovement) return;
        var movement = other.gameObject.GetComponent<PlayerMovement>();
        if (movement != null)
            movement.enabled = false;
    }
}