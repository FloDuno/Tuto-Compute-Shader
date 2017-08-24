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
        int _shaderFunctionID = _shader.FindKernel(CustomStrings.calculateVectors);
        
        // Go to the Readme to understand what it means
        int[] _dispatchSize = {2, 2, 1};
        
        _shader.SetInts("dispatchSize", _dispatchSize);
        
        // What we want to be passed to the shader
        // The size is the numthreads (in the shader) * the dispatch size
        Vector3[] _input = new Vector3[8 * 8 * 8 * _dispatchSize[0] * _dispatchSize[1] * _dispatchSize[2]];
        for (int i = 0; i < _input.Length; i++)
        {
            _input[i] = new Vector3(5.0f, 5.0f, 5.0f);
        }
        /*
         Construct the buffer
         First argument is the size of the buffer
         
         Second argument is the size of one element of the input (Vector3 in this case) 
         There are 2 ways to know the size : 
         Either you know what the element is made of and you can calcute it. 
         For instance a Vector3 contains just 3 floats so the size will be sizeof(float) * 3
         Or you don't know what it will contain and you do System.Runtime.InteropServices.Marshal.SizeOf(new Vector3());
         The first method is cleaner, by far
         */
        ComputeBuffer _buffer = new ComputeBuffer(_input.Length, 12);
        //After creating the buffer we fill it
        _buffer.SetData(_input);
        /* 
         Assign the buffer to the shader variable
         
         First argument is the kernel/function we want to use 
         Second is the var name of the buffer in the shader
         Third is the created buffer
        */
        _shader.SetBuffer(_shaderFunctionID, CustomStrings.outputBuffer, _buffer);
        // Launch the compute shader kernel
        _shader.Dispatch(_shaderFunctionID, _dispatchSize[0], _dispatchSize[1], _dispatchSize[2]);

        // Get the calculated data
        _buffer.GetData(_input);

        // Print them
        for (int i = 0; i < _input.Length; i++)
        {
            print(_input[i]);
        }

        // Release the RAM
        _buffer.Release();
    }
}