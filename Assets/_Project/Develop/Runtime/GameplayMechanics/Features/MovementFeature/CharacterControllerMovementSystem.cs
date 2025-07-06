using Assets._Project.Develop.Runtime.GameplayMechanics.EntitiesCore;
using Assets._Project.Develop.Runtime.GameplayMechanics.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.GameplayMechanics.Features.MovementFeature
{
	public class CharacterControllerMovementSystem : IInitializableSystem, IUpdatableSystem
	{
		private const float _deadZone = 0.1f;

		private ReactiveVariable<Vector3> _moveDirection;
		private ReactiveVariable<float> _moveSpeed;
		private ReactiveVariable<float> _rotationSpeed;
		private CharacterController _characterController;

		public void OnInit(Entity entity)
		{
			_moveDirection = entity.MoveDirection;
			_moveSpeed = entity.MoveSpeed;
			_rotationSpeed = entity.RotationSpeed;
			_characterController = entity.CharacterController;
		}

		public void OnUpdate(float deltaTime)
		{			
			ProcessMoveTo(deltaTime);

			ProcessRotateTo(deltaTime);
		}

		private void ProcessMoveTo(float deltaTime)
		{
			Vector3 velocity = _moveDirection.Value.normalized * _moveSpeed.Value;

			_characterController.Move(velocity * deltaTime);
		}

		private void ProcessRotateTo(float deltaTime)
		{
			if (_moveDirection.Value.magnitude <= _deadZone)
				return;

			Vector3 direction = _moveDirection.Value.normalized;
			Quaternion lookRotation = Quaternion.LookRotation(direction);
			float step = _rotationSpeed.Value * deltaTime;

			_characterController.transform.rotation = Quaternion.RotateTowards(_characterController.transform.rotation, lookRotation, step);
		}
	}
}
