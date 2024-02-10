using UnityEngine;

public class PhysicsSwitch : MonoBehaviour
{
    private Rigidbody2D _rigidbody;

    [SerializeField] private GameObject _target;
    [SerializeField] private RigidbodyType2D _targetBodyType; // uwu
    [SerializeField] private bool _denyPlayerMovement;

    public void Awake()
    {
        _rigidbody = _target.GetComponent<Rigidbody2D>();
        gameObject.SetActive(_rigidbody != null);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        _rigidbody.bodyType = _targetBodyType; // This value has at runtime the value "hot"
        DenyPlayerMovement(other);

        gameObject.SetActive(false);
    }

    private void DenyPlayerMovement(Collider2D other)
    {
        if (!_denyPlayerMovement) return;
        var movement = other.gameObject.GetComponent<PlayerMovement>();
        if (movement != null)
            movement.enabled = false;
    }
}