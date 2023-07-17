using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerCharacter : MonoBehaviour, IKitchenObjectParent
{

    public static PlayerCharacter Instance { get; private set; }

    public event EventHandler OnPickedSomething;
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    [SerializeField] private float moveSpeed = 7.0f;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;

    private Vector3 lastInteractDirection;
    private bool isWalking;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;

    // Start is called before the first frame update
    void Start()
    {
        playerInput.OnInteractAction += PlayerInputOnInteractAction;
        playerInput.OnDoTaskAction += PlayerInputOnDoTaskAction;
    }

    private void PlayerInputOnDoTaskAction(object sender, EventArgs e)
    {
        if (KitchenGameManager.Instance.IsGamePlaying())
        {
            if (selectedCounter != null)
            {

                selectedCounter.DoTask(this);
            }
        }
        
    }

    private void PlayerInputOnInteractAction(object sender, System.EventArgs e)
    {
        if (KitchenGameManager.Instance.IsGamePlaying())
        {
            if (selectedCounter != null)
            {
                selectedCounter.Interact(this);
            }
        }
       
    }

    // Update is called once per frame
    private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("There is more than one player");
        }
        Instance = this;
    }
    // for player animation
    public bool IsWalking()
    {
        return isWalking;
    }

    private void HandleInteractions()
    {
        float interactDistance = 2f;
        Vector2 inputVector = playerInput.GetMovementVectorNormalized();
        Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);
        if (moveDirection != Vector3.zero)
        {
            lastInteractDirection = moveDirection;
        }
        if(Physics.Raycast(transform.position, lastInteractDirection,out RaycastHit raycastHit, interactDistance, countersLayerMask))
        {
            // Get the hit Actor
            if(raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                // Has a counter
                if(baseCounter != selectedCounter)
                {
                    SetSelectedCounter(baseCounter);                   
                }
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }
        //Debug.Log(selectedCounter);
    }
    private void HandleMovement()
    {
        // Collision Component
        float playerRadius = 0.7f;
        float playerHeight = 2f;
        float moveDistance = Time.deltaTime * moveSpeed;
        // Player Movement Component
        Vector2 inputVector = playerInput.GetMovementVectorNormalized();
        Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);
        // Check if collided
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirection, moveDistance);
        // Attempt to change direction
        if (!canMove)
        {
            // Attempt only X movement
            Vector3 moveDirectionX = new Vector3(moveDirection.x, 0f, 0f);
            canMove = (moveDirection.x < -0.5f || moveDirection.x > 0.5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirectionX, moveDistance);
            if (canMove)
            {
                moveDirection = moveDirectionX;
            }
            else
            {
                // Attempt only Z movement
                Vector3 moveDirectionZ = new Vector3(0f, 0f, moveDirection.z);
                canMove = (moveDirection.z < -0.5f || moveDirection.z > 0.5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirectionZ, moveDistance);
                if (canMove)
                {
                    moveDirection = moveDirectionZ;
                }
                else
                {
                    // Cannot move in any direction
                }
            }
        }
        if (canMove)
        {
            transform.position += moveDirection * moveDistance;
            isWalking = moveDirection != Vector3.zero;
        }
        // Rotate player
        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);

    }

    // Select active clear counter
    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;
        // Validity check and pass arguments
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = selectedCounter
        });
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;

        if (kitchenObject != null)
        {
            OnPickedSomething?.Invoke(this, EventArgs.Empty);
        }
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}
