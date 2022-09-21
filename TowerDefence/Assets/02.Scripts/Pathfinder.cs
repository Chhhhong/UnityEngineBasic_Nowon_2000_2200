using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    struct Coord
    {
        public int y;
        public int x;
    }

    enum Type
    {
        Path,
        Obstacle
    }

    struct NodePair
    {
        public Coord coord;
        public Type type;
    }

    private Transform _leftBottom; // (0, 0);
    private Transform _rightTop; // (max, max);
    private float _width => _rightTop.position.x - _leftBottom.position.x; // 실제 맵 너비
    private float _height => _rightTop.position.z - _leftBottom.position.z; // 실제 맵 높이
    private float _nodeTerm; // 노드 실제 간격
    private NodePair[,] _map;

    public void SetNodeMap(List<Transform> pathNodes, List<Transform> obastacleNodes)
    {
        _map = new NodePair[(int)(_height / _nodeTerm), (int)(_width / _nodeTerm)];
        int y, x;
        foreach (var node in pathNodes)
        {
            y = (int)((node.position.z - _leftBottom.position.z) / _nodeTerm);
            x = (int)((node.position.x - _leftBottom.position.x) / _nodeTerm);
            _map[y, x] = new NodePair()
            {
                coord = new Coord() { y = y, x = x },
                type = Type.Path
            };
        }

        foreach (var node in obastacleNodes)
        {
            y = (int)((node.position.z - _leftBottom.position.z) / _nodeTerm);
            x = (int)((node.position.x - _leftBottom.position.x) / _nodeTerm);
            _map[y, x] = new NodePair()
            {
                coord = new Coord() { y = y, x = x },
                type = Type.Obstacle
            };
        }
    }
}
