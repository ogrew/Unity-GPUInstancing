using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DrawMeshInstanced_demo : MonoBehaviour
{
    public int count = 1000;
    public float radius = 5;
    public Mesh copyMesh;
    public Material material;

    private Matrix4x4[] matrices;
    private MaterialPropertyBlock block;

    private void Start()
    {
        SetUp();
    }

    private void Update()
    {
        Graphics.DrawMeshInstanced(copyMesh, 0, material, matrices, count, block, ShadowCastingMode.On, true, gameObject.layer);
    }

    private void SetUp()
    {
        matrices = new Matrix4x4[count];
        Vector4[] colors = new Vector4[count];

        block = new MaterialPropertyBlock();

        for (int i = 0; i < count; i++)
        {
            Vector3 pos = Random.insideUnitSphere * radius;

            Quaternion rot = Quaternion.Euler(
                Random.Range(-180, 180),
                Random.Range(-180, 180),
                Random.Range(-180, 180)
            );
            Vector3 size = Vector3.one;

            matrices[i].SetTRS(pos, rot, size);
            matrices[i] = transform.localToWorldMatrix * matrices[i];

            colors[i] = Color.Lerp(Color.white, Color.black, Random.value);
        }

        block.SetVectorArray("_Colors", colors);
    }
}
