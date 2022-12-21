using System;
using UnityEngine;
using Utils;

public class Grid<T>
{
    /// <summary>
    /// Event for changes in the grid elements.
    /// </summary>
    public event EventHandler<OnGridValueChangedEventArgs> OnGridObjectChanged;

    public class OnGridValueChangedEventArgs : EventArgs
    {
        public int X;
        public int Y;
    }

    /// <summary>
    /// Width of the grid in number of cells.
    /// </summary>
    private readonly int _width;

    /// <summary>
    /// Height of the grid in number of cells.
    /// </summary>
    private readonly int _height;

    /// <summary>
    /// Size of the square cells.
    /// </summary>
    private readonly float _cellSize;

    /// <summary>
    /// Origin position of the grid in the world.
    /// </summary>
    private readonly Vector3 _originPosition;

    /// <summary>
    /// Bi-dimensional array for the grid.
    /// </summary>
    private readonly T[,] _gridArray;

    /// <summary>
    /// Show debug
    /// </summary>
    private const bool ShowDebug = true;

    /// <summary>
    /// Constructor for the grid.
    /// </summary>
    /// <param name="width">Width of the grid.</param>
    /// <param name="height">Height of the grid.</param>
    /// <param name="cellSize">Size of the grid cells.</param>
    /// <param name="originPosition">Origin of the grid in the world.</param>
    /// <param name="createGridObject">Function for the default object to be initialized in the grid, usually a constructor.</param>
    public Grid(int width, int height, float cellSize, Vector3 originPosition,
        Func<Grid<T>, int, int, T> createGridObject)
    {
        this._width = width;
        this._height = height;
        this._cellSize = cellSize;
        this._originPosition = originPosition;

        _gridArray = new T[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                _gridArray[x, y] = createGridObject(this, x, y);
            }
        }

        if (ShowDebug)
        {
            TextMesh[][] debugTextArray = new TextMesh[width][];
            for (int index = 0; index < width; index++)
            {
                debugTextArray[index] = new TextMesh[height];
            }

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    debugTextArray[x][y] = UtilsClass.CreateWorldText(_gridArray[x, y]?.ToString(), null,
                        GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * .5f, 20, Color.white,
                        TextAnchor.MiddleCenter);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                }

                Debug.DrawLine(GetWorldPosition(x, height), GetWorldPosition(x + 1, height), Color.white, 100f);
            }

            for (int y = 0; y < height; y++)
            {
                Debug.DrawLine(GetWorldPosition(width, y), GetWorldPosition(width, y + 1), Color.white, 100f);
            }

            OnGridObjectChanged += (_, eventArgs) =>
                debugTextArray[eventArgs.X][eventArgs.Y].text = _gridArray[eventArgs.X, eventArgs.Y]?.ToString();
        }
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * _cellSize + _originPosition;
    }

    private void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - _originPosition).x / _cellSize);
        y = Mathf.FloorToInt((worldPosition - _originPosition).y / _cellSize);
    }

    public void TriggerGridObjectChanged(int x, int y)
    {
        OnGridObjectChanged?.Invoke(this, new OnGridValueChangedEventArgs { X = x, Y = y });
    }

    public void SetGridObject(int x, int y, T value)
    {
        if (x < 0 || y < 0 || x >= _width || y >= _height) return;
        _gridArray[x, y] = value;
        TriggerGridObjectChanged(x, y);
    }

    public void SetGridObject(Vector3 worldPosition, T value)
    {
        GetXY(worldPosition, out var x, out var y);
        SetGridObject(x, y, value);
    }

    public T GetGridObject(int x, int y)
    {
        if (x < 0 || y < 0 || x >= _width || y >= _height) return default(T);
        else return _gridArray[x, y];
    }

    public T GetGridObject(Vector3 worldPosition)
    {
        GetXY(worldPosition, out var x, out var y);
        return GetGridObject(x, y);
    }

    /// <summary>
    /// Default grid object provided as an example.
    /// </summary>
    public class DefGridObject
    {
        private readonly Grid<DefGridObject> _grid;
        private readonly int _x;
        private readonly int _y;
        private float _value;
        private string _text;

        public DefGridObject(Grid<DefGridObject> grid, int x, int y)
        {
            this._grid = grid;
            this._x = x;
            this._y = y;
        }

        public void SetValue(float newValue)
        {
            _value = newValue;
            _grid.TriggerGridObjectChanged(_x, _y);
        }

        public float GetValue()
        {
            return _value;
        }

        public void SetText(string newText)
        {
            _text = newText;
            _grid.TriggerGridObjectChanged(_x, _y);
        }

        public string GetText()
        {
            return _text;
        }

        public override string ToString()
        {
            return _value + "\n" + _text;
        }
    }
}
