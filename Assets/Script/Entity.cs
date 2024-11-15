using System.Collections;
using UnityEngine;
using static Entity;

public class Entity : MonoBehaviour, IAttacker
{
    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public SpriteRenderer sr { get; private set; }
    public EntityFX fx { get; private set; }

    public CapsuleCollider2D cd { get; private set; }

    public CharacterStats stats { get; private set; }

    public Player player;
    #endregion

    #region info
    [Header("Collision info")]
    [SerializeField] public Transform setAttackP;
    [SerializeField] public float attackCheckRadius;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float distancetoground;
    [SerializeField] protected LayerMask setGround;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float distancetowall;
    [SerializeField] protected LayerMask setWall;

    [Header("knock info")]
    [SerializeField] protected float knockDuration;
    [SerializeField] protected bool isKnocked;
    [SerializeField] protected Vector2 knockVelocity;



    [SerializeField] public int facingDir { get; private set; } = 1;
    [SerializeField] protected bool facingRight = true;
    #endregion

    public System.Action onFilpped;

    protected virtual void Awake()
    {

    }
    protected virtual void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        fx = GetComponent<EntityFX>();
        stats = GetComponent<CharacterStats>();
        cd = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }
    public bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, distancetoground, setGround);//第一周作业 射线检测
    public bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, distancetowall, setWall);

    public virtual void SetVelocity(float _xVelocity, float _yVelocity)
    {
        if (isKnocked)
            return;

        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - distancetoground));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + distancetowall, wallCheck.position.y));
        Gizmos.DrawWireSphere(setAttackP.position, attackCheckRadius);
    }
    public virtual void DamageEffect(IAttacker _attacker)
    {
        fx.StartCoroutine("FlashFX");

        float facingDirFromAttacker = _attacker.facingDir;
     
        StartCoroutine(HitKnocked(facingDirFromAttacker));
    }
        
    protected virtual IEnumerator HitKnocked(float facingDirFromAttacker)
    {
        Debug.Log("HitKnocked(coroutine)");
        isKnocked = true;
        rb.velocity = new Vector2(knockVelocity.x * facingDirFromAttacker, knockVelocity.y);

        yield return new WaitForSeconds(knockDuration);

        isKnocked = false;
        rb.velocity = new Vector2(0, 0);

    }

    public void MakeTransprent(bool _Transprent)
    {
        if (_Transprent)
        {
            sr.color = Color.clear;
        }
        else
        {
            sr.color = Color.white;
        }
    }

    public virtual void Die()
    {
        
    }

    #region interface
    public interface IAttacker
    {
        int facingDir { get; }
    }

    #endregion



    #region Flip
    public virtual void Flip()
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);

        if(onFilpped != null)
        {
            onFilpped();
        }
    }

  
    public virtual void FlipController(float _x)
    {
        if (_x > 0 && !facingRight)
            Flip();
        else if (_x < 0 && facingRight)
            Flip();
    }
    #endregion

}
