using RAGE;
using static RAGE.Game.Shapetest;
using System;
using System.Collections.Generic;
using System.Text;
using RAGE.Elements;

namespace car_dirty
{
    public class WorldUtils
    {
		private static readonly float DefaultDirtnessImpact = 2f;
		private static readonly Dictionary<MaterialHash, float> SurfacesDirtnessImpact = new Dictionary<MaterialHash, float>
		{
			{ MaterialHash.MarshDeep, 8 },
			{ MaterialHash.MudHard, 4 },
			{ MaterialHash.Grass, 4 },
			{ MaterialHash.GrassShort, 4 },
			{ MaterialHash.DirtTrack, 4 },
			{ MaterialHash.SandTrack, 4 },
			{ MaterialHash.SandCompact, 4 },
			{ MaterialHash.GravelLarge, 4 },
			{ MaterialHash.GravelSmall, 4 },
			{ MaterialHash.Leaves, 4 },
			{ MaterialHash.Rock, 4 },
			{ MaterialHash.SandDryDeep, 4 }
		};

		/// <summary>
		/// Creates a raycast between 2 points.
		/// </summary>
		/// <param name="source">The source of the raycast.</param>
		/// <param name="direction">The direction of the raycast.</param>
		/// <param name="maxDistance">How far the raycast should go out to.</param>
		/// <param name="options">What type of objects the raycast should intersect with.</param>
		/// <param name="ignoreEntity">Specify an <see cref="GameEntityBase"/> that the raycast should ignore, leave null for no entities ignored.</param>
		public static RaycastResult Raycast(Vector3 source, Vector3 direction, float maxDistance, IntersectOptions options, GameEntityBase ignoreEntity = null)
		{
			Vector3 target = source + direction * maxDistance;

			return new RaycastResult(StartShapeTestRay(source.X, source.Y, source.Z, target.X, target.Y, target.Z, (int)options, ignoreEntity == null ? 0 : ignoreEntity.Handle, 7));
		}

		public static float GetDirtnessImpactFromSurfaceMaterial(MaterialHash material)
        {
			if(SurfacesDirtnessImpact.TryGetValue(material, out float impact))
            {
				return impact;
            }

			return DefaultDirtnessImpact;
		}
	}
}
