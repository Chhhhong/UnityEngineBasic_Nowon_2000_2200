using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    struct Coord
    {
        public static Coord zero = new Coord(0, 0);
        public static Coord error = new Coord(-1, -1);
        public int y;
        public int x;

        public Coord(int y, int x)
        {
            this.y = y;
            this.x = x;
        }

        public static bool operator ==(Coord op1, Coord op2)
            => (op1.y == op2.y && op1.x == op2.x);

        public static bool operator !=(Coord op1, Coord op2)
            => !(op1 == op2);
    }

    enum NodeType
    {
        None,
        Path,
        Obstacle
    }

    public enum FindingOptions
    {
        BFS,
        DFS,
        FixedWayPoints
    }
    [SerializeField] private FindingOptions _option;

    struct NodePair
    {
        public Transform node;
        public Coord coord;
        public NodeType type;
    }

    private static Transform _leftBottom; // (0, 0);
    private static Transform _rightTop; // (max, max);
    private static float _width => _rightTop.position.x - _leftBottom.position.x; // 실제 맵 너비
    private static float _height => _rightTop.position.z - _leftBottom.position.z; // 실제 맵 높이
    private static float _nodeTerm = 1.0f; // 노드 실제 간격
    private static NodePair[,] _map;
    private static bool[,] _visited;
    private static int[,] _direction = new int[2, 4]
    {
        {-1, 0, 1, 0 },
        { 0,-1, 0, 1 }
    };
    private static List<List<Transform>> _pathList = new List<List<Transform>>();
    private static List<Transform> _tmpPathForDFS = new List<Transform>();


    //===================================================================================
    //****************************** Public Methods *************************************
    //===================================================================================

    public static void SetNodeMap()
    {
        Transform nodeParent = GameObject.Find("Nodes").transform;
        Transform[] nodes = new Transform[nodeParent.childCount];
        for (int i = 0; i < nodeParent.childCount; i++)
            nodes[i] = nodeParent.GetChild(i);

        Transform enemyPathParent = GameObject.Find("EnemyPaths").transform;
        Transform[] enemyPaths = new Transform[enemyPathParent.childCount];
        for (int i = 0; i < enemyPathParent.childCount; i++)
            enemyPaths[i] = enemyPathParent.GetChild(i);

        SetNodeMap(enemyPaths.ToList(), nodes.ToList());
    }

    public bool TryFindOptimizedPath(Transform startNode, Transform endNode, out List<Transform> optimizedPath)
    {
        bool found = false;
        optimizedPath = null;

        switch (_option)
        {
            case FindingOptions.BFS:
                found = BFS(start: FindNode(startNode).coord,
                            end: FindNode(endNode).coord);
                break;
            case FindingOptions.DFS:
                found = DFS(start: FindNode(startNode).coord,
                            end: FindNode(endNode).coord);
                break;
            case FindingOptions.FixedWayPoints:
                found = FindFixedWayPoints();
                break;
            default:
                break;
        }

        if (found)
        {
            /* 디버깅용
            List<List<Transform>> nodePathList = new List<List<Transform>>();

            for (int i = 0; i < _pathList.Count; i++)
            {
                string pathString = "";
                nodePathList.Add(new List<Transform>());
                for (int j = 0; j < _pathList[i].Count; j++)
                {
                    nodePathList[i].Add(GetNode(_pathList[i][j]));
                    pathString += $"-> {nodePathList[i][j].name}";
                }
                Debug.Log($"[Pathfinder] : 최단경로 탐색 됨. {pathString}");
            }
            */

            optimizedPath = new List<Transform>(_pathList.OrderBy(path => path.Count).First());

            foreach (var path in _pathList)
                path.Clear();
            _pathList.Clear();

            for (int i = 0; i < _visited.GetLength(0); i++)
                for (int j = 0; j < _visited.GetLength(1); j++)
                    _visited[i, j] = false;
        }
        return found;
    }


    //===================================================================================
    //****************************** Private Methods ************************************
    //===================================================================================

    private static void SetNodeMap(List<Transform> pathNodes, List<Transform> obstacleNodes)
    {
        // 모든 노드 정렬
        List<Transform> nodes = new List<Transform>();
        nodes.AddRange(pathNodes);
        nodes.AddRange(obstacleNodes);
        IOrderedEnumerable<Transform> nodesFiltered = nodes.OrderBy(node => node.position.x + node.position.z);

        // 왼쪽아래끝, 오른쪽위끝 노드 찾기
        _leftBottom = nodesFiltered.First();
        _rightTop = nodesFiltered.Last();


        _map = new NodePair[(int)(_height / _nodeTerm) + 1, (int)(_width / _nodeTerm) + 1];
        _visited = new bool[_map.GetLength(0), _map.GetLength(1)];

        Coord tmpCoord;
        foreach (var node in pathNodes)
        {
            tmpCoord = TransformToCoord(node);
            _map[tmpCoord.y, tmpCoord.x] = new NodePair()
            {
                node = node,
                coord = tmpCoord,
                type = NodeType.Path
            };
        }

        foreach (var node in obstacleNodes)
        {
            tmpCoord = TransformToCoord(node);
            _map[tmpCoord.y, tmpCoord.x] = new NodePair()
            {
                node = node,
                coord = tmpCoord,
                type = NodeType.Obstacle
            };
        }
    }

    private static bool BFS(Coord start, Coord end)
    {
        bool isFinished = false;
        List<KeyValuePair<Coord,Coord>> parentPairs = new List<KeyValuePair<Coord, Coord>>();
        Queue<Coord> queue = new Queue<Coord>();
        queue.Enqueue(start);
        parentPairs.Add(new KeyValuePair<Coord, Coord>(Coord.error, start));
        _visited[start.y, start.x] = true;

        int searchCount = 0;
        while (queue.Count > 0)
        {
            Coord parent = queue.Dequeue();

            for (int i = 0; i < _direction.GetLength(1); i++)
            {
                Coord next = new Coord(parent.y + _direction[0, i], parent.x + _direction[1, i]);

                // 탐색 위치가 맵을 벗어나는지
                if (next.y < 0 || next.y >= _map.GetLength(0) ||
                    next.x < 0 || next.x >= _map.GetLength(1))
                    continue;

                // 탐색 위치가 장애물일 경우
                if (_map[next.y, next.x].type == NodeType.Obstacle)
                    continue;

                // 방문 여부
                if (_visited[next.y, next.x] == true)
                    continue;

                // 방문
                searchCount++;
                parentPairs.Add(new KeyValuePair<Coord, Coord>(parent, next));
                _visited[next.y, next.x] = true;

                // 도착 체크
                if (next.y == end.y &&
                    next.x == end.x)
                {
                    isFinished = true;
                    _pathList.Add(CalcPath(parentPairs, start, end));
                }
                else
                {
                    queue.Enqueue(next);
                }
            }
        }

        return isFinished;
    }

    private static bool DFS(Coord start, Coord end)
    {
        _tmpPathForDFS = new List<Transform>();
        bool isFound = DFSLoop(start, end);
        if (isFound)
        {
            _tmpPathForDFS.Add(GetNode(start));
            _tmpPathForDFS.Reverse();
        }   

        return isFound;
    }

    private static bool DFSLoop(Coord start, Coord end)
    {
        bool isFound = false;
        _visited[start.y, start.x] = true;
        Debug.Log($"DFS ing... {start.x}, {start.y}");
        
        Coord next;
        for (int i = 0; i < _direction.GetLength(1); i++)
        {
            next.y = start.y + _direction[0, i];
            next.x = start.x + _direction[1, i];

            // 탐색 위치가 맵을 벗어나는지
            if ((next.y < 0 || next.y >= _map.GetLength(0))||
                (next.x < 0 || next.x >= _map.GetLength(1)))
                continue;

            // 탐색 위치가 장애물일 경우
            if (_map[next.y, next.x].type == NodeType.Obstacle)
                continue;

            // 방문 여부
            if (_visited[next.y, next.x] == true)
                continue;

            // 도착 여부
            if (next.y == end.y &&
                next.x == end.x)
            {
                _pathList.Add(_tmpPathForDFS);
                return true;
            }
            else
            {    
                isFound = DFSLoop(next, end);
                if (isFound)
                {
                    _tmpPathForDFS.Add(GetNode(next));
                    break;
                }
            }
        }

        return isFound;
    }


    /// <summary>
    /// 기록된 부모/자식 노드 페어의 리스트로 
    /// 경로를 계산해서 반환해주는 함수
    /// </summary>
    /// <returns>계산된 경로</returns>
    private static List<Transform> CalcPath(List<KeyValuePair<Coord, Coord>> parentPairs, Coord start, Coord end)
    {
        List<Transform> path = new List<Transform>();

        Coord coord = parentPairs.Last().Value; // 젤 마지막 노드
        path.Add(GetNode(coord));

        int index = parentPairs.Count - 1;
        while (index > 0 &&
               parentPairs[index].Key != start)
        {
            path.Add(GetNode(parentPairs[index].Key));
            index = parentPairs.FindLastIndex(pair => pair.Value == parentPairs[index].Key);
        }
        path.Add(GetNode(start));
        path.Reverse();

        return path;
    }

    private static bool FindFixedWayPoints()
    {
        if (WayPoints.instance == null)
            return false;

        _pathList.Add(WayPoints.instance.points.ToList());
        return true;
    }

    private static Coord TransformToCoord(Transform node)
    {
        return new Coord((int)((node.position.z - _leftBottom.position.z) / _nodeTerm),
                         (int)((node.position.x - _leftBottom.position.x) / _nodeTerm));
    }

    private static NodePair FindNode(Transform node)
    {
        Coord coord = TransformToCoord(node);
        for (int i = 0; i < _map.GetLength(1); i++)
            for (int j = 0; j < _map.GetLength(0); j++)
                if (_map[j,i].coord == coord)
                    return _map[j,i];

        return new NodePair() { coord = Coord.error, node = null, type = NodeType.None };
    }

    private static Transform GetNode(Coord coord)
    {
        if (_map[coord.y, coord.x].node == null)
            Debug.LogError($"Failed to get node {coord.x},{coord.y}");

        return _map[coord.y, coord.x].node;
    }

}
