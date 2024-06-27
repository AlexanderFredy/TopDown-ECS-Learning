using System.Linq;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Unity.Entities;
using Zenject;

public static class Utils
{

    public static readonly byte[,] CollisionMatrix  
		= {
			{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, //default
			{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, //transparentFX
			{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, //IgnoreRaycast
			{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, //????
			{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, //Water
			{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, //UI
			{ 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 0, 1, 1 }, //6:Player
			{ 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0 }, //7:PlayerWeapon
			{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 1 }, //8:Enemy
			{ 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0 }, //9:EnemyWeapon
			{ 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0 }, //10:NeutralWeapon
			{ 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0 }, //11:PowerUp
			{ 1, 1, 1, 1, 1, 1, 1, 0, 1, 0, 0, 0, 0 }, //12:Trap
		};

public static List<Collider> GetAllColliders(this GameObject go)
	{
		return go == null ? null : go.GetComponents<Collider>().ToList();
	}

	public static void ToWorldSpaceBox(this BoxCollider box, out float3 center, out float3 halfExtents, out quaternion orientation)
    {
		Transform transform = box.transform;
		orientation = transform.rotation;
		center = transform.TransformPoint(box.center);
		var lossyScale = transform.lossyScale;
		var scale = Abs(lossyScale);
		halfExtents = Vector3.Scale(scale, box.size) * 0.5f;

	}

	public static void ToWorldSpaceCapsule(this CapsuleCollider capsule, out float3 point0, out float3 point1, out float radius)
	{
		Transform transform = capsule.transform;
		var center = (float3)transform.TransformPoint(capsule.center);
		radius = 0f;
		float height = 0f;
		float3 lossyScale = Abs(transform.lossyScale);
		float3 dir = float3.zero;

        switch (capsule.direction)
        {
			case 0://x
				radius = Mathf.Max(lossyScale.y, lossyScale.z) * capsule.radius;
				height = lossyScale.x * capsule.height;
				dir = capsule.transform.TransformDirection(Vector3.right);
                break;
			case 1://y
				radius = Mathf.Max(lossyScale.x, lossyScale.z) * capsule.radius;
				height = lossyScale.y * capsule.height;
				dir = capsule.transform.TransformDirection(Vector3.up);
				break;
			case 2://z
				radius = Mathf.Max(lossyScale.x, lossyScale.y) * capsule.radius;
				height = lossyScale.z * capsule.height;
				dir = capsule.transform.TransformDirection(Vector3.forward);
				break;
		}

		if (height < radius * 2f)
			dir = Vector3.zero;

		point0 = center + dir * (height * 0.5f - radius);
		point1 = center - dir * (height * 0.5f - radius);
	}

	public static void ToWorldSpaceSphere(this SphereCollider sphere, out float3 center, out float radius)
    {
		Transform transform = sphere.transform;
		center = transform.TransformPoint(sphere.center);
		radius = sphere.radius * Max(Abs(transform.lossyScale));
	}

	private static float3 Abs(float3 v) => new float3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));

	private static float Max(float3 v) => Mathf.Max(v.x, Mathf.Max(v.y, v.z));

	public static void DrawCircle(Vector3 center, float radius, Color color)
	{
		float ThetaScale = 0.01f;
		float Theta = 0f;
		int Size = (int)((1f / ThetaScale) + 1f);
		//LineDrawer.positionCount = Size;

		float StartX = radius * Mathf.Cos(Theta) + center.x;
		float StartZ = radius * Mathf.Sin(Theta) + center.z;

		for (int i = 1; i < Size; i++)
		{
			Theta += 2.0f * Mathf.PI * ThetaScale;
			float x = radius * Mathf.Cos(Theta) + center.x;
			float z = radius * Mathf.Sin(Theta) + center.z;
			Debug.DrawLine(new Vector3(StartX,0.5f, StartZ), new Vector3(x, 0.5f, z), color);

			StartX = x;
			StartZ = z;
		}
	}

	public static Vector3 GetPositionInArea()
	{
		return new Vector3(UnityEngine.Random.Range(-8, 7.5f), 0.0f, UnityEngine.Random.Range(-8, 4.0f));
	}

	public static void SetCollisionLayerAndOwner(this WeaponAbility weapon, GameObject owner)
	{
		switch (owner.layer)
		{
			case 6:
				weapon.gameObject.layer = 7;
				break;
			case 8:
				weapon.gameObject.layer = 9;
				break;
			default:
				weapon.gameObject.layer = 0;
				break;
		}
		
		SetLayerMaskInComponent(weapon);
		weapon.Owner = owner;
	}

	public static void ClearCollisionLayerAndOwner(this WeaponAbility weapon)
	{
		weapon.gameObject.layer = 10;
		SetLayerMaskInComponent(weapon);
		weapon.Owner = null;
	}

	private static void SetLayerMaskInComponent(WeaponAbility weapon)
	{
		var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		if (entityManager.HasComponent<ActorColliderData>(weapon.MyEntity))
		{
			ActorColliderData data = entityManager.GetComponentData<ActorColliderData>(weapon.MyEntity);
			data.layerMask = GetLayerMask(CollisionMatrix, weapon.gameObject.layer);
			entityManager.SetComponentData(weapon.MyEntity, data);
		}
	}

	//public static byte[,] CreateCollisionMatrix()
	//{
        
		
	//	return colMatrix;
	//}

	public static int GetLayerMask(byte[,] matrix, int layer)
	{
		int layerAsLayerMask = 0;
		
		for (int i = 0; i < matrix.GetLength(1); i++)
		{
			if (matrix[layer,i] == 1)
				layerAsLayerMask |= (1 << i);
		}

		return layerAsLayerMask;
	}
	
	public static void SetGameLayerRecursive(GameObject gameObject, int layer)
	{
		gameObject.layer = layer;
		foreach (Transform child in gameObject.transform)
		{
			SetGameLayerRecursive(child.gameObject, layer);
		}
	}
}
