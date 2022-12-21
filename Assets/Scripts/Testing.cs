using UnityEngine;
using Utils;

public class Testing : MonoBehaviour
{
    private Grid<bool> _grid;
    private void Start()
    {
        _grid = new Grid<bool>(20, 10, 5f, Vector3.zero, (Grid<bool> g, int x, int y) => new bool());
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _grid.SetGridObject(UtilsClass.GetMouse3DWorldPosition(), !_grid.GetGridObject(UtilsClass.GetMouse3DWorldPosition()));
        }

        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log(_grid.GetGridObject(UtilsClass.GetMouse3DWorldPosition()));
        }
    }
}
