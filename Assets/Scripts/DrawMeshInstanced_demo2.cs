using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DrawMeshInstanced_demo2 : MonoBehaviour
{
    [SerializeField] private int count = 1000;
    [SerializeField] private float radius = 5;
    [SerializeField] private Mesh copyMesh;
    [SerializeField] private Material material;
    [SerializeField] private Vector3 positionOffset;

    private List<Matrix4x4[]> batches;
    private MaterialPropertyBlock block;

    static readonly int MAX_CONT = 1023;

    private void Start()
    {
        SetUp();
    }

    private void Update()
    {
        foreach (var batch in batches)
        {
            Graphics.DrawMeshInstanced(copyMesh, 0, material, batch, MAX_CONT, block);
        }
    }

    private void SetUp()
    {
        batches = new List<Matrix4x4[]>();
        block = new MaterialPropertyBlock();

        var matrices = new Matrix4x4[MAX_CONT];
        Vector4[] colors = new Vector4[count];

        for (int i = 0; i < count; i++)
        {
            if (i % MAX_CONT == 0)
            {
                matrices = new Matrix4x4[MAX_CONT];
                batches.Add(matrices);
            }

            Vector3 pos = Random.insideUnitSphere * radius + positionOffset;
            Quaternion rot = Quaternion.Euler(
                Random.Range(-180, 180),
                Random.Range(-180, 180),
                Random.Range(-180, 180)
            );
            Vector3 size = Vector3.one;

            matrices[i % MAX_CONT].SetTRS(pos, rot, size);
            matrices[i % MAX_CONT] = transform.localToWorldMatrix * matrices[i % MAX_CONT];

            colors[i] = Color.Lerp(Color.magenta, Color.cyan, Random.value);
        }

        block.SetVectorArray("_Colors", colors);
    }
}
