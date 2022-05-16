using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class DrawMeshInstancedIndirect_demo : MonoBehaviour
{
    [SerializeField] private int count = 10000;
    [SerializeField] private float radius = 10;
    [SerializeField] private Mesh copyMesh;
    [SerializeField] private Material material;
    [SerializeField] private Vector3 positionOffset;

    private Bounds bounds;

    private ComputeBuffer meshPropertiesBuffer;
    private ComputeBuffer argsBuffer;

    private uint[] args = new uint[5] { 0, 0, 0, 0, 0 };

    private struct MeshProperties
    {
        public Matrix4x4 mat;
        public Vector4 color;

        public static int Size()
        {
            return
                sizeof(float) * 4 * 4 + // matrix
                sizeof(float) * 4; // color
        }
    }

    private void Start()
    {
        SetUp();
    }

    private void Update()
    {
        Graphics.DrawMeshInstancedIndirect(copyMesh, 0, material, bounds, argsBuffer);
    }

    private void OnDisable()
    {
        if (meshPropertiesBuffer != null)
        {
            meshPropertiesBuffer.Release();
        }
        meshPropertiesBuffer = null;

        if (argsBuffer != null)
        {
            argsBuffer.Release();
        }
        argsBuffer = null;
    }

    private void SetUp()
    {
        bounds = new Bounds(transform.position, Vector3.one * (radius + 1));
        InitializeBuffers();
    }

    private void InitializeBuffers()
    {

        // DrawMeshInstancedIndirectするのに必要な引数
        argsBuffer = new ComputeBuffer(1, args.Length * sizeof(uint), ComputeBufferType.IndirectArguments);
        args[0] = (uint)copyMesh.GetIndexCount(0); // コピーしたいメッシュの頂点数
        args[1] = (uint)count; // それを何個コピーしたいのか
        args[2] = (uint)copyMesh.GetIndexStart(0); // メッシュバッファの開始インデックス
        args[3] = (uint)copyMesh.GetBaseVertex(0); // 頂点のインデックス
        args[4] = 0; // 生成し始めるインスタンスのインデックス
        argsBuffer.SetData(args);

        // 葉っぱ1枚ごとのプロパティの配列
        meshPropertiesBuffer = new ComputeBuffer(count, MeshProperties.Size());
        MeshProperties[] properties = new MeshProperties[count];
        for (int i = 0; i < count; i++)
        {
            MeshProperties props = new MeshProperties();

            var pos = Random.insideUnitSphere * radius + positionOffset;
            var rot = Quaternion.Euler(
                Random.Range(-180, 180),
                Random.Range(-180, 180),
                Random.Range(-180, 180)
            );
            var size = new Vector3(0.5f, 0.5f, 0.5f);

            props.mat = Matrix4x4.TRS(pos, rot, size);
            props.color = Color.Lerp(Color.magenta, Color.cyan, Random.value);

            properties[i] = props;
        }

        meshPropertiesBuffer.SetData(properties);
        material.SetBuffer("_Properties", meshPropertiesBuffer);
    }

}
