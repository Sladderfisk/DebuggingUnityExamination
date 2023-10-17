using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class OrganismMovement : MonoBehaviour
{
    [SerializeField] private float stretchStrength;
    [SerializeField] private float timeToStretch;
    [SerializeField] private float movementSpeed;
    [SerializeField] private MovementType movementType;
    [SerializeField] private float maxScale;

    private OrganismBaseMovement currentMovementType;

    private bool isStretching = true;
    private float timeCurrentStretch;
    private Vector2 stretchDelta;

    private Vector2 startScale;
    private Transform player;
    private Rigidbody2D myRb;
    
    public int ScaleDirection { set; get; }

    public void MultiplyScaleBy(Vector2 value)
    {
        if (value.x > 1 && (startScale.x > maxScale || startScale.y > maxScale)) return;
        startScale *= value;
    }

    private void Awake()
    {
        myRb = GetComponent<Rigidbody2D>();

        startScale = transform.localScale;
        
        SetStretch();
    }

    private void Start()
    {
        //Application.targetFrameRate = 10;
        
        player = FindObjectOfType<PlayerMovement>().transform;
        
        switch (movementType)
        {
            case MovementType.FollowPlayer:

                currentMovementType = new FollowPlayerMovement(myRb, player, movementSpeed);
                break;
            
            case MovementType.MoveRandomDirection:

                currentMovementType = new MoveRandomDirectionMovement(myRb, player, movementSpeed);
                break;
        }
        
        currentMovementType.Awake();
    }

    private void FixedUpdate()
    {
        currentMovementType.Tick(Time.fixedDeltaTime);
        Stretch();
    }

    private void Stretch()
    {
        var newScale = stretchDelta * Time.fixedDeltaTime / timeToStretch;
        transform.localScale += (Vector3)newScale;

        timeCurrentStretch += Time.fixedDeltaTime;

        if (timeCurrentStretch >= timeToStretch) Switch();
    }

    private void Switch()
    {
        if (isStretching) SetReverseStretch();
        else SetStretch();
    }

    private void SetStretch()
    {
        timeCurrentStretch = 0;
        isStretching = true;

        stretchDelta.x = Random.Range(-stretchStrength, stretchStrength);
        stretchDelta.y = Random.Range(-stretchStrength, stretchStrength);
    }

    private void SetReverseStretch()
    {
        timeCurrentStretch = 0;
        isStretching = false;

        stretchDelta = startScale;
        stretchDelta -= (Vector2)transform.localScale;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        currentMovementType.Collision(other);
    }

    private enum MovementType
    {
        FollowPlayer,
        MoveRandomDirection
    }

    private abstract class OrganismBaseMovement
    {
        protected readonly float movementSpeed;
        
        protected readonly Rigidbody2D rb;
        protected readonly Transform player;

        protected OrganismBaseMovement(Rigidbody2D rb, Transform player, float movementSpeed)
        {
            this.movementSpeed = movementSpeed;
            this.rb = rb;
            this.player = player;
        }

        protected abstract void Start();
        protected abstract void Collide(Collision2D other);
        protected abstract void Move(float deltaTime);


        public void Awake()
        {
            Start();
        }
        public void Collision(Collision2D other)
        {
            Collide(other);
        }
        public void Tick(float deltaTime)
        {
            Move(deltaTime);
        }
    }

    private class FollowPlayerMovement : OrganismBaseMovement
    {
        private float currentRotationTarget;
        
        public FollowPlayerMovement(Rigidbody2D rb, Transform player, float movementSpeed) : base(rb, player, movementSpeed)
        {
        }

        protected override void Start()
        {
            
        }

        protected override void Collide(Collision2D other)
        {
            
        }

        protected override void Move(float deltaTime)
        {
            CalculateRotation();
            rb.velocity = rb.transform.up * movementSpeed;
        }

        private void CalculateRotation()
        {
            Vector2 deltaPos = rb.transform.position - player.position;
            var targetRadians = Mathf.Atan2(deltaPos.x, deltaPos.y);
            currentRotationTarget = -Mathf.Rad2Deg * targetRadians + 180;
            
            Rotate();
        }

        private void Rotate()
        {
            rb.rotation = currentRotationTarget;
        }
    }

    private class MoveRandomDirectionMovement : OrganismBaseMovement
    {
        private const float RotationAddedWhenCollidedWithEntity = 45;
        private const float RotationAddedWhenCollidedWithWorldCollider = 180;

        private const float ConstantRotation = 10f;
        private const float MinRotationOffset = -10;
        private const float MaxRotationOffset = 10;
        
        private const float TimeToRotate = .5f;

        private bool isRotating;

        private float constantRotationDirection;
        private float angleToRotate;
        private float currentRotation;
        private float startRotation;
        
        public MoveRandomDirectionMovement(Rigidbody2D rb, Transform player, float movementSpeed) : base(rb, player, movementSpeed)
        {
        }

        protected override void Start()
        {
            currentRotation = Random.Range(0, 360);
        }

        protected override void Collide(Collision2D other)
        {
            angleToRotate = (other.gameObject.CompareTag("WorldCollider")) ? RotationAddedWhenCollidedWithWorldCollider : RotationAddedWhenCollidedWithEntity;
            angleToRotate += Random.Range(MinRotationOffset, MaxRotationOffset);
            SetRotate();
        }

        private void SetRotate()
        {
            isRotating = true;
            startRotation = currentRotation;
        }

        protected override void Move(float deltaTime)
        {
            if (isRotating) Rotate(deltaTime);
            else currentRotation += ConstantRotation * constantRotationDirection * deltaTime;

            rb.rotation = currentRotation;
            rb.velocity = rb.transform.up * movementSpeed;
        }

        private void Rotate(float deltaTime)
        {
            currentRotation += angleToRotate * deltaTime / TimeToRotate;

            if (currentRotation < startRotation + angleToRotate) return;

            currentRotation = startRotation + angleToRotate;
            constantRotationDirection = Random.Range(-1, 1);
            
            isRotating = false;
        }
    }
}
