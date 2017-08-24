using UnityEngine;

public class ComputeBufferLaunch : MonoBehaviour
{
    [SerializeField] private ComputeShader _shader;

    private int _pragmaIndex;

    // Use this for initialization
    private void Start()
    {
        LaunchShader();
    }

    private void LaunchShader()
    {
        // What we want to be passed to the shader
        // The size is the numthreads (in the shader) * the dispatch size
        Vector3[] _input = new Vector3[8 * 8 * 8 * 2 * 2];
        for (int i = 0; i < _input.Length; i++)
        {
            _input[i] = new Vector3(5.0f, 5.0f, 5.0f);
        }
        /*
         Construct the buffer
         The 12 is the size of one element of the input (Vector3 in this case) 
         You can now it by using System.Runtime.InteropServices.Marshal.SizeOf(new Vector3());
         */
        ComputeBuffer _buffer = new ComputeBuffer(_input.Length, 12);
        _buffer.SetData(_input);
        
        print(_shader.FindKernel(CustomStrings.calculateVectors));
        
        _shader.SetBuffer(_shader.FindKernel(CustomStrings.calculateVectors), CustomStrings.outputBuffer, _buffer);
        _shader.Dispatch(_shader.FindKernel(CustomStrings.calculateVectors), 2,2,1);
        
        _buffer.GetData(_input);

        for (int i = 0; i < _input.Length; i++)
        {
            print(_input[i]);
        }
        
        _buffer.Release();
    }
}