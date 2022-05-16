using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DrawMeshInstanced_demo : MonoBehaviour
{
    [SerializeField] private int count = 1000;
    [SerializeField] private float radius = 5;
    [SerializeField] private Mesh copyMesh;
    [SerializeField] private Material material;
    [SerializeField] private Vector3 positionOffset;

    private Matrix4x4[] matrices;
    private MaterialPropertyBlock block;

    private void Start()
    {
        SetUp();
    }

    private void Update()
    {
        Graphics.DrawMeshInstanced(copyMesh, 0, material, matrices, count, block);
    }

    private void SetUp()
    {
        matrices = new Matrix4x4[count];
        Vector4[] colors = new Vector4[count];

        block = new MaterialPropertyBlock();

        for (int i = 0; i < count; i++)
        {
            Vector3 pos = Random.insideUnitSphere * radius + positionOffset;

            Quaternion rot = Quaternion.Euler(
                Random.Range(-180, 180),
                Random.Range(-180, 180),
                Random.Range(-180, 180)
            );
            Vector3 size = Vector3.one;

            matrices[i].SetTRS(pos, rot, size);
            matrices[i] = transform.localToWorldMatrix * matrices[i];

            colors[i] = Color.Lerp(Color.magenta, Color.cyan, Random.value);
        }

        block.SetVectorArray("_Colors", colors);
    }
}
