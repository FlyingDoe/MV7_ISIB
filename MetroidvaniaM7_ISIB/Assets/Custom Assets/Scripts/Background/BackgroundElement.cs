using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundElement : MonoBehaviour
{
    [SerializeField]
    BackgroundTypes _type;

    [SerializeField]
    bool _isAspectLocked;

    [SerializeField]
    bool _isRotationScaleLocked;

    [SerializeField]
    int _unwalkableColliderHeight;

    GameObject _placeholder;
    GameObject _prettyBg;
    MeshFilter _prettyMesh;
    Collider _prettyCollider;
    MeshRenderer _prettyRenderer;
    BoxCollider _unwalkableCollider;
    Vector3 _unwalkableColliderCenter = Vector3.zero;
    Vector3 _unwalkableColliderSize = Vector3.one;
    Vector3 _baseScale;
    float _unwalkableColliderBaseScale;

    Vector3 currentRotation;

    #region Properties
    public BackgroundTypes Type
    {
        get
        {
            return _type;
        }
    }

    public bool IsAspectLocked
    {
        get
        {
            return _isAspectLocked;
        }
    }

    public bool IsRotationLocked
    {
        get
        {
            return _isRotationScaleLocked;
        }
    }
    #endregion  

    /// <summary>
    /// find all the background element's components;
    /// if already did it, automatically skips it
    /// </summary>
    public void Initialize()
    {
        if(_placeholder != null && _prettyBg != null && _prettyMesh != null && _prettyCollider != null && _prettyRenderer != null)
        {
            Debug.Log("Trying to get components for the background element of " + gameObject.name + " but it's already been done before.");
            return;
        }

        foreach (Transform child in transform)
        {
            if (child.tag == "EditorOnly")
            {
                _placeholder = child.gameObject;
            }
            else
            {
                _prettyBg = child.gameObject;
            }
        }

        _prettyMesh = _prettyBg.GetComponent<MeshFilter>();
        _prettyCollider = _prettyBg.GetComponent<Collider>();
        _prettyRenderer = _prettyBg.GetComponent<MeshRenderer>();

        if (_placeholder == null || _prettyBg == null || _prettyMesh == null || _prettyCollider == null || _prettyRenderer == null)
        {
            Debug.LogError("Element " + gameObject.name + " could not be fully initialized. Please check that it has two children, " + 
                "one of which is tagged 'EditorOnly' and holds a simple geometric mesh, " + 
                "and the other holds a MeshFilter, a Collider and a MeshRenderer.");

        }

            if (_type == BackgroundTypes.ObstaclePlatform)
        {
            _unwalkableCollider = GetComponent<BoxCollider>();
            _unwalkableColliderBaseScale = _unwalkableCollider.size.y;
        }

        _baseScale = _prettyBg.transform.localScale;
    }

    public void ChangeMesh(Mesh mesh)
    {
        _prettyMesh.mesh = mesh;
        // set _prettyCollider if it's a mesh collider
        if ((MeshCollider) _prettyCollider != null)
        {
            ((MeshCollider)_prettyCollider).sharedMesh = mesh;
        }
    }

    public void ChangeScale(int scale)
    {
        _prettyBg.transform.localScale = _baseScale + (_baseScale * scale);
    }

    public void ChangeMat(Material mat)
    {
        _prettyRenderer.material = mat;
    }

    /// <summary>
    /// change rotation (on its Y axis) of the pretty element
    /// </summary>
    /// <param name="rotY">local rotation of the pretty element along the Y axis</param>
    public void ChangeRotation(int rotY)
    {
        currentRotation = transform.localRotation.eulerAngles;
        currentRotation.y = rotY;
        _prettyBg.transform.Rotate(currentRotation);
    }

    public BackgroundTypes GetBackgroundType()
    {
        return _type;
    }

    public void SetUnwalkableCollider()
    {
        if (_unwalkableCollider != null)
        {
            _unwalkableCollider.enabled = true;
        }

        if (_unwalkableColliderHeight != 0)
        {
            _unwalkableColliderCenter.y = (_unwalkableColliderHeight / 2) * _unwalkableColliderBaseScale;
            _unwalkableCollider.center = _unwalkableColliderCenter;
            _unwalkableColliderSize.y = _unwalkableColliderHeight * _unwalkableColliderBaseScale;
            _unwalkableCollider.size = _unwalkableColliderSize;
        }
    }

    public void DeactivateUnwalkableCollider()
    {
        if(_unwalkableCollider != null)
        {
            _unwalkableCollider.enabled = false;
        }
    }

    public bool CheckPrettyState()
    {
        if (_placeholder.activeSelf == _prettyBg.activeSelf)
        {
            Debug.LogError("Careful, both versions (pretty and geometric) of the element " + gameObject.name + "are active (or inactive). Only one should. You should activate or deactivate Pretty Elements in the editor's Background Elements Manager.");
        }
        return !_placeholder.activeSelf;
    }

    /// <summary>
    /// activate pretty element and hide geometric one
    /// </summary>
    /// <param name="isPretty"></param>
    public void ActivatePrettyBackground(bool isPretty = true)
    {
        _placeholder.SetActive(!isPretty);
        _prettyBg.SetActive(isPretty);
    }

    /// <summary>
    /// reset the view of the element to its simple geometric way
    /// </summary>
    public void Reset()
    {
        ActivatePrettyBackground(false);
    }
}
