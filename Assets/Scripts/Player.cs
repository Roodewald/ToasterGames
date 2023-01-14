using UnityEngine;
using UnityEngine.InputSystem;

namespace ToasterGames.ShootingEverything
{
	public class Player : PlayerBehaviour
	{
		[SerializeField] private CharacterController controller;
		[SerializeField] private InventoryBehaviour inventory;
		[SerializeField] private Camera playerCamera;

		#region FIELDS
		float xRotation = 0f;


		public float mouseSensetiviy = 5f;
		public float speed = 12f;
		private bool holdingButtonFire;
		private WeaponBehaviour equippedWeapon;
		private float lastShotTime;

		private PlayerInput playerInput;

		private Vector2 playerMove;
		private Vector2 playerLook;
		#endregion

		#region UNITY
		private void Awake()
		{
			playerInput= GetComponent<PlayerInput>();
			inventory.Init();
		}

		private void Update()
		{
			if (!IsOwner) return;
			Look();
			Move();

			equippedWeapon = inventory.GetEquipped();
			if (holdingButtonFire)
			{
				
				//Check.
				if ( equippedWeapon.HasAmmunition() && equippedWeapon.IsAutomatic())
				{
					//Has fire rate passed.
					if (Time.time - lastShotTime > 60.0f / equippedWeapon.GetRateOfFire())
						Fire();
				}
			}
		}
		#endregion

		#region METHODS
		private void Fire()
		{
			//Save the shot time, so we can calculate the fire rate correctly.
			lastShotTime = Time.time;
			//Fire the weapon! Make sure that we also pass the scope's spread multiplier if we're aiming.
			if (equippedWeapon.GetAnimator().GetCurrentAnimatorStateInfo(0).IsName("Idle"))
			{
				equippedWeapon.Fire();
			}
		}

		public void TryLook(InputAction.CallbackContext context)
		{
			playerLook = context.ReadValue<Vector2>();
		}

		private void Look()
		{
			float mouseX = playerLook.x * mouseSensetiviy;
			float mouseY = playerLook.y * mouseSensetiviy;

			xRotation -= mouseY;
			xRotation = Mathf.Clamp(xRotation, -90f, 90f);

			playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
			transform.Rotate(Vector3.up * mouseX);
		}

		public void OnTryMove(InputAction.CallbackContext context)
		{
			playerMove = context.ReadValue<Vector2>();
		}
		private void Move()
		{
			float x = playerMove.x;
			float z = playerMove.y;

			Vector3 move = transform.right * x + transform.forward * z;

			controller.Move(move * speed * Time.deltaTime);
		}

		public void OnTryReload(InputAction.CallbackContext context)
		{
			if (context.performed && !equippedWeapon.IsFull())
			{
				equippedWeapon.Reload();
			}
		}

		public void OnTryFire(InputAction.CallbackContext context)
		{
			//Switch.
			switch (context)
			{
				//Started.
				case { phase: InputActionPhase.Started }:
					//Hold.
					holdingButtonFire = true;
					break;
				//Performed.
				case {phase: InputActionPhase.Performed}:
					//Ignore if we're not allowed to actually fire.
					if (!CanPlayAnimationFire())
						break;

					//Check.
					if (equippedWeapon.HasAmmunition())
					{
						//Check.
						if (equippedWeapon.IsAutomatic())
							break;

						//Has fire rate passed.
						if (Time.time - lastShotTime > 60.0f / equippedWeapon.GetRateOfFire())
							Fire();
					}
					//Fire Empty.
					else
						FireEmpty();
					break;
				//Canceled.
				case { phase: InputActionPhase.Canceled }:
					//Stop Hold.
					holdingButtonFire = false;
					break;
			}

		}

		public void ScrolInventory(InputAction.CallbackContext context)
		{
			if (context.performed)
			{
				float scrolValue = context.ReadValue<float>();
				inventory.ChangeWeapon(scrolValue);
			}
		}
		private void FireEmpty()
		{
			/*
			 * Save Time. Even though we're not actually firing, we still need this for the fire rate between
			 * empty shots.
			 */
			lastShotTime = Time.time;
			equippedWeapon.Reload();
		}

		private bool CanPlayAnimationFire()
		{
			//Return.
			return true;
		}


		public override void OnNetworkSpawn()
		{
			if (IsOwner)
			{
				transform.position = new Vector3(Random.Range(5, 5), 0, Random.Range(5, 5));

				playerCamera.gameObject.SetActive(true);
				playerInput.enabled = true;
			}
		}
		#endregion

		#region GETERS

		public override Camera GetCameraWorld() => playerCamera;

		#endregion
	}

}
