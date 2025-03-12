using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainFarmer : MonoBehaviour
{
    [SerializeField] private PlaceablesSpawner spawner;
    [SerializeField] private Collider2D playerCollider;

    // Speed vars
    [SerializeField] private float multiplierSpeed = 6;
    [SerializeField] private float regularSpeed = 3;
    public bool multiply = false;
    public bool light = false;

    // private Dictionary<Vector3Int, Item.ItemType> seedTypeTracker;

    private Vector2 direction;
    [SerializeField] private Transform point;
    [SerializeField] private List<FarmController> controllers; // List to hold multiple FarmControllers
    private bool canClick;
    
    // Inventory
    private Inventory inventory;
    [SerializeField] private UI_Inventory uiInventory;
    public GameObject inventoryContainer;
    private bool invent = false;

    // equipment
    private bool hat = true;
    private EquipmentSet equipmentSet;


    private Vector3 lastMousePosition;
    private float timeSinceMouseMoved = 0.0f;
    private bool isMouseMoving = false;
    private const float idleMouseTimeout = 3.0f; // Time before switching to movement direction animation
    private Animator animator;

    private void Start()
    {
        //playerCollider = GetComponent<Collider2D>();

        // added
        inventory = new Inventory();
        equipmentSet = new EquipmentSet();
        uiInventory.SetInventory(inventory);
        uiInventory.SetEquipmentSet(equipmentSet);
        
        // If you want it to spawn an item- uncomment this
        // ItemObject.CreateItemObject(new Vector3(0,1,0), new Item{itemType=Item.ItemType.Seed1});

        // original
        canClick = true;
        animator = transform.Find("Body").GetComponent<Animator>();
        lastMousePosition = Input.mousePosition;

        

        // Check if a spawn position is saved
        if (PlayerPrefs.HasKey("SpawnX") && PlayerPrefs.HasKey("SpawnY") && PlayerPrefs.HasKey("SpawnZ"))
        {
            float x = PlayerPrefs.GetFloat("SpawnX");
            float y = PlayerPrefs.GetFloat("SpawnY");
            float z = PlayerPrefs.GetFloat("SpawnZ");

            transform.position = new Vector3(x, y, z);

            // Clear spawn data if needed to avoid reusing it
            PlayerPrefs.DeleteKey("SpawnX");
            PlayerPrefs.DeleteKey("SpawnY");
            PlayerPrefs.DeleteKey("SpawnZ");
        }

        //make inventory start unopened
        invent = false;
        inventoryContainer.SetActive(false);
    }

    void Update()
    {
        displayInventory();
        uiInventory.SetFarmerPosition(gameObject.transform.position);
        equimentEffects();

        transform.rotation = Quaternion.Euler(Vector3.forward);
        direction = Vector2.zero;

        // Movement input
        if (Input.GetKey(KeyCode.W)) direction.y = 1;
        if (Input.GetKey(KeyCode.A)) direction.x = -1;
        if (Input.GetKey(KeyCode.S)) direction.y = -1;
        if (Input.GetKey(KeyCode.D)) direction.x = 1;

        direction.Normalize();

        // Move the character
        if (multiply){
            transform.Translate(direction * multiplierSpeed * Time.deltaTime, Space.World);
        }else{
            transform.Translate(direction * regularSpeed * Time.deltaTime, Space.World);
        }

        // Handle actions when the left mouse button is clicked
        if (canClick && Input.GetMouseButton(0))
        {
            Action();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Talk();
        }
        if (SceneManager.GetActiveScene().name == "ForestScene")
        {
            if (Input.GetKeyDown(KeyCode.E) && inventory.GetItemCount(Item.ItemType.Trap) > 0)
            {
                spawner.SpawnTrap(transform);
                inventory.RemoveItem(new Item { itemType = Item.ItemType.Trap, amount = 1 });
            }
            if (Input.GetKeyDown(KeyCode.Q) && inventory.GetItemCount(Item.ItemType.Lantern) > 0)
            {
                spawner.SpawnLantern(transform);
                inventory.RemoveItem(new Item { itemType = Item.ItemType.Lantern, amount = 1 });
            }
        }

        // Update animator parameters for movement
        SetAnimationParameters(direction.x, direction.y);
    }

    public void Action()
    {
        // Check if there are any controllers assigned
        if (controllers == null || controllers.Count == 0)
        {
            Debug.Log("No FarmControllers available for interaction.");
            return;
        }

        foreach (FarmController controller in controllers)
        {
            // Check if the controller or its farmTiles are valid
            if (controller == null || controller.farmTiles == null || controller.farmTiles.Length == 0)
            {
                Debug.Log($"FarmController {controller?.name ?? "Unknown"} has no tiles for interaction.");
                continue; // Skip this controller if it's invalid or has no tiles
            }

            Vector3Int pos = controller.Map.WorldToCell(point.position); // Get the grid position from the current controller's Tilemap
            Debug.Log($"Action triggered. World position: {point.position}, Grid position: {pos} for FarmController: {controller.name}");

            foreach (Vector3Int tile in controller.farmTiles)
            {
                // check if there are seeds.
                // if (tile.Equals(pos) & inventory.hasSeeds(new Item{itemType=Item.ItemType.Seed1}) && !controller.IsFlower(pos))
                Item.ItemType type = equipmentSet.hasSeeds();
                if (tile.Equals(pos) & (type != Item.ItemType.Empty) & controller.IsEmpty(pos))
                {
                    plant(controller, pos, type);
                    Debug.Log($"Tile matched at position: {pos} in FarmController: {controller.name}");
                    controller.InteractTile(pos); // Delegate interaction to the correct controller
                    StartCoroutine(MouseCoolDown());
                    return;
                }
                else if (tile.Equals(pos) && controller.IsFlower(pos))
                {
                    Debug.Log($"Tile matched at position: {pos} in FarmController: {controller.name}");
                    controller.InteractTile(pos); // Delegate interaction to the correct controller
                    collect(controller, pos);
                    StartCoroutine(MouseCoolDown());
                    return;
                }
            }
        }

        Debug.Log("No matching tile found for the clicked position.");
    }

    void Talk()
    {
        if (playerCollider.IsTouchingLayers(LayerMask.GetMask("NPC")))
        {
            Debug.Log("Got Layers");
            ContactFilter2D filter = new ContactFilter2D().NoFilter();
            //filter.layerMask = LayerMask.GetMask("NPC");
            Collider2D[] colliders = new Collider2D[5];
            if (playerCollider.OverlapCollider(filter, colliders) == 0)
            {
                Debug.Log("No Colliders");
                return;
            }

            foreach (Collider2D collider in colliders)
            {
                if(collider == null)
                    continue;

                NPCReactor reactor;
                reactor = collider.gameObject.GetComponent<NPCReactor>();
                if (reactor != null)
                {
                    reactor.Interact();
                }
                else
                {
                    Debug.Log("No Reactor: " + collider.gameObject.name);
                }
            }
        }
    }

    void HandleFacingDirection()
    {
        Vector3 mousePosition = Input.mousePosition;
        if (Vector3.Distance(mousePosition, lastMousePosition) > 0.1f) // Mouse moved
        {
            isMouseMoving = true;
            timeSinceMouseMoved = 0.0f;
            lastMousePosition = mousePosition;

            // Face the character towards the mouse
            FaceMouse();
        }
        else
        {
            timeSinceMouseMoved += Time.deltaTime;

            // If mouse hasn't moved for idleMouseTimeout seconds, use movement direction for facing
            if (timeSinceMouseMoved > idleMouseTimeout)
            {
                isMouseMoving = false;
                UpdateAnimationBasedOnMovement();
            }
        }

        if (isMouseMoving) UpdateAnimationBasedOnMouseDirection();
    }

    void FaceMouse()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mouseDirection = (mouseWorldPosition - transform.position).normalized;

        // Set animator parameters based on mouse direction
        animator.SetFloat("LookX", mouseDirection.x);
        animator.SetFloat("LookY", mouseDirection.y);
    }

    private Vector2 GetInputDirection()
    {
        Vector2 newDirection = Vector2.zero;

        if (Input.GetKey(KeyCode.W)) newDirection = Vector2.up;
        if (Input.GetKey(KeyCode.S)) newDirection = Vector2.down;
        if (Input.GetKey(KeyCode.A)) newDirection = Vector2.left;
        if (Input.GetKey(KeyCode.D)) newDirection = Vector2.right;

        return newDirection;
    }

    void UpdateAnimationBasedOnMouseDirection()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mouseDirection = (mouseWorldPosition - transform.position).normalized;

        SetAnimationParameters(mouseDirection.x, mouseDirection.y);
    }

    void UpdateAnimationBasedOnMovement()
    {
        SetAnimationParameters(direction.x, direction.y);
    }

    void SetAnimationParameters(float x, float y)
    {
        animator.SetFloat("MoveX", x);
        animator.SetFloat("MoveY", y);
    }

    IEnumerator MouseCoolDown()
    {
        canClick = false;
        point.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.4f);
        point.gameObject.SetActive(true);
        canClick = true;
    }

    private void displayInventory(){
        if (Input.GetKeyDown(KeyCode.Tab)){
            if(invent){
                invent = false;
                inventoryContainer.SetActive(false);
            }else{
                invent = true;
                inventoryContainer.SetActive(true);
            }
        } 
    }

    private void equimentEffects(){
        if(equipmentSet.GetEquipmentSetItem(0).itemType == Item.ItemType.Hat){
            hat = true;
        } else {
            hat = false;
        }
        if(equipmentSet.GetEquipmentSetItem(1).itemType == Item.ItemType.Lantern){
            light = true;
        } else {
            light = false;
        }
        if(equipmentSet.GetEquipmentSetItem(2).itemType == Item.ItemType.Shoes){
            multiply = true;
        }else { 
            multiply = false;
        }
    }

    public void Death(){
        if(hat == true){
            equipmentSet.UnequipItem(0, true);
        }else{
            equipmentSet.UnequipItem(1, true);
            MainManager.Instance.died = true;
        }
    }

    public IEnumerator Immobilize()
    {
        yield return null;
    }

    public Inventory GetInventory {get {return inventory;}}

    private void plant(FarmController controller, Vector3Int loc, Item.ItemType type){
        if (inventory.hasSeeds(new Item{itemType=type })){
            inventory.RemoveItem(new Item{itemType=type });
        }else{
            equipmentSet.UnequipItem(1);
            inventory.RemoveItem(new Item{itemType=type });
        }
        controller.setSeedTypeAtPos(type, loc);
    }

    private void collect(FarmController controller, Vector3Int loc){
        inventory.AddItem(new Item { itemType = controller.getSeedTypeAtPos(loc), amount = 2});
    }

}
