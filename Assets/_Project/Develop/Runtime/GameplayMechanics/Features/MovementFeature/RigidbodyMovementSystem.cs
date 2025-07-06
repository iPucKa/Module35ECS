using Assets._Project.Develop.Runtime.GameplayMechanics.EntitiesCore;
using Assets._Project.Develop.Runtime.GameplayMechanics.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.GameplayMechanics.Features.MovementFeature
{
	public class RigidbodyMovementSystem : IInitializableSystem, IUpdatableSystem
	{
		private const float _deadZone = 0.1f;

		private ReactiveVariable<Vector3> _moveDirection;
		private ReactiveVariable<float> _moveSpeed;
		private ReactiveVariable<float> _rotationSpeed;
		private Rigidbody _rigidbody;

		public void OnInit(Entity entity)
		{
			_moveDirection = entity.MoveDirection;
			_moveSpeed = entity.MoveSpeed;
			_rotationSpeed = entity.RotationSpeed;
			_rigidbody = entity.Rigidbody;
		}

		public void OnUpdate(float deltaTime)
		{
			ProcessMoveTo();

			ProcessRotateTo(deltaTime);
		}

		private void ProcessMoveTo()
		{
			Vector3 velocity = _moveDirection.Value.normalized * _moveSpeed.Value;

			_rigidbody.velocity = velocity;
		}

		private void ProcessRotateTo(float deltaTime)
		{
			if(_moveDirection.Value.magnitude <= _deadZone)
				return;

			Vector3 direction = _moveDirection.Value.normalized;
			Quaternion lookRotation = Quaternion.LookRotation(direction);
			float step = _rotationSpeed.Value * deltaTime;

			_rigidbody.transform.rotation = Quaternion.RotateTowards(_rigidbody.transform.rotation, lookRotation, step);
		}		
	}
}
