using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    private float horizontalMovement;
    private float verticalMovement;
    private Vector3 moveDiraction;

    private NetworkVariable<MyCustomData> randomNumber = new NetworkVariable<MyCustomData>(new MyCustomData { _int= 56, _bool = true}
    ,NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public struct MyCustomData: INetworkSerializable
    {
        public int _int;
        public bool _bool;
        public FixedString128Bytes message;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref _int);
            serializer.SerializeValue(ref _bool);
            serializer.SerializeValue(ref message);
        }
    }

    public override void OnNetworkSpawn()
    {
        randomNumber.OnValueChanged += (MyCustomData previousValue, MyCustomData newValue) =>
				Debug.Log(OwnerClientId + "; RandomNumber: " + newValue._int + "; "+ newValue._bool + newValue.message);
	}

    private void Update()
    {
        if (!IsOwner) return;
        Move();

        if (Input.GetKeyDown(KeyCode.T))
        {
            TestServerRpc();


            /*
            randomNumber.Value = new MyCustomData
            {
                _int = 10,
                _bool = false,
                message = "Hi world"
            };
            */
        }
    }

    private void Move()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        moveDiraction = transform.forward * verticalMovement + transform.right * horizontalMovement;

        transform.position += moveDiraction * 3 *Time.deltaTime;
    }

    [ServerRpc]
    private void TestServerRpc()
    {
        Debug.Log("TestServerRpc" + OwnerClientId);
    }
}
