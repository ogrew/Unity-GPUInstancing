using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DrawMeshInstanced_demo2 : MonoBehaviour
{
    public int count = 1000;
    public float radius = 5;
    public Mesh copyMesh;
    public Material material;
    public Vector3 positionOffset;


    private List<Matrix4x4[]> batches;
    private MaterialPropertyBlock block;

    private void Start()
    {
        SetUp();
    }

    private void Update()
    {
        foreach (var batch in batches)
        {
            Graphics.DrawMeshInstanced(copyMesh, 0, material, batch, 1023, block);
        }
    }

    private void SetUp()
    {
        batches = new List<Matrix4x4[]>();
        block = new MaterialPropertyBlock();

        var matrices = new Matrix4x4[1023];
        Vector4[] colors = new Vector4[count];

        for (int i = 0; i < count; i++)
        {
            if (i % 1023 == 0)
            {
                matrices = new Matrix4x4[1023];
                batches.Add(matrices);
            }

            Vector3 pos = Random.insideUnitSphere * radius + positionOffset;
            Quaternion rot = Quaternion.Euler(
                Random.Range(-180, 180),
                Random.Range(-180, 180),
                Random.Range(-180, 180)
            );
            Vector3 size = Vector3.one;

            matrices[i % 1023].SetTRS(pos, rot, size);
            matrices[i % 1023] = transform.localToWorldMatrix * matrices[i % 1023];

            colors[i] = Color.Lerp(Color.white, Color.black, Random.value);
        }

        block.SetVectorArray("_Colors", colors);
    }
}
