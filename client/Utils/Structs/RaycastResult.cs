using RAGE;
using RAGE.Elements;
using static RAGE.Game.Shapetest;
using System;
using System.Collections.Generic;
using System.Text;

namespace car_dirty
{
	public struct RaycastResult
	{
		public RaycastResult(int handle) : this()
		{
			Vector3 hitPositionArg = new Vector3();
			int hitSomethingArg = 0;
			int entityHandleArg = 0;
			Vector3 surfaceNormalArg = new Vector3();
			int materialArg = 0;

			Result = GetShapeTestResultEx(handle, ref hitSomethingArg, hitPositionArg, surfaceNormalArg, ref materialArg, ref entityHandleArg);

			DitHit = hitSomethingArg != 0;
			HitPosition = hitPositionArg;
			SurfaceNormal = surfaceNormalArg;
			HitEntityHandle = entityHandleArg;
			Material = (MaterialHash)materialArg;
		}

		/// <summary>
		/// Gets the <see cref="Entity" /> this raycast collided with.
		/// <remarks>Returns <c>null</c> if the raycast didnt collide with any <see cref="Entity"/>.</remarks>
		/// </summary>
		public int HitEntityHandle { get; private set; }
		/// <summary>
		/// Gets the world coordinates where this raycast collided.
		/// <remarks>Returns <see cref="Vector3.Zero"/> if the raycast didnt collide with anything.</remarks>
		/// </summary>
		public Vector3 HitPosition { get; private set; }
		/// <summary>
		/// Gets the normal of the surface where this raycast collided.
		/// <remarks>Returns <see cref="Vector3.Zero"/> if the raycast didnt collide with anything.</remarks>
		/// </summary>
		public Vector3 SurfaceNormal { get; private set; }

		/// <summary>
		/// Gets a value indicating whether this raycast collided with anything.
		/// </summary>
		public bool DitHit { get; private set; }
		/// <summary>
		/// Gets a value indicating whether this raycast collided with any <see cref="Entity"/>.
		/// </summary>
		public bool DitHitEntity
		{
			get
			{
				return HitEntityHandle != 0;
			}
		}

		/// <summary>
		/// Gets a value indicating the material type of the collision.
		/// </summary>
		public MaterialHash Material { get; private set; }

		public int Result { get; private set; }
	}
}
