using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    private float horizontalMovement;
    private float verticalMovement;
    private Vector3 moveDiraction;

    private NetworkVariable<MyCustomData> randomNumber = new NetworkVariable<MyCustomData>(new MyCustomData { _int= 56, _bool = true}
    ,NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public struct MyCustomData
    {
        public int _int;
        public bool _bool;

    }

    public override void OnNetworkSpawn()
    {
        randomNumber.OnValueChanged += (MyCustomData previousValue, MyCustomData newValue) =>
				Debug.Log(OwnerClientId + "; RandomNumber: " + newValue._int + "; "+ newValue._bool);
	}

    private void Update()
    {

        if (!IsOwner) return;
        Move();

        if (Input.GetKeyDown(KeyCode.T))
        {
            randomNumber.Value = new MyCustomData
            {
                _int = 10,
                _bool = false
            };
        }
    }

    private void Move()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        moveDiraction = transform.forward * verticalMovement + transform.right * horizontalMovement;

        transform.position += moveDiraction * 3 *Time.deltaTime;
    }
}
