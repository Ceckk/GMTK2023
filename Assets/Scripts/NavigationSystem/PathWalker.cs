using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathWalker : MonoBehaviour
{
    private bool _isWalking = false;

    void Start()
    {
        GridManager.Instance.Init();
    }

    void Update()
    {
        if (!_isWalking && Input.GetMouseButtonDown(0))
        {
            var startPos = GridManager.Instance.WorldToGridPosition(transform.position);
            var endPos = GridManager.Instance.WorldToGridPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            Debug.Log(startPos);
            Debug.Log(endPos);

            var path = GridManager.Instance.pathfinding.FindPath(startPos.x, startPos.y, endPos.x, endPos.y);

            if (path != null)
            {
                StartCoroutine(WalkPath(path));
            }
        }
    }

    private IEnumerator WalkPath(List<PathNode> path)
    {
        _isWalking = true;
        var pathIndex = 1;
        while (pathIndex < path.Count)
        {
            var targetPos = GridManager.Instance.GridToWorldPosition(new Vector3Int(path[pathIndex].x, path[pathIndex].y, 0));

            transform.position = Vector3.MoveTowards(transform.position, targetPos, 3 * Time.deltaTime);

            var distance = Vector3.Distance(transform.position, targetPos);
            if (distance < 0.001f)
            {
                transform.position = targetPos;

                pathIndex++;
            }
            yield return null;
        }

        _isWalking = false;
    }
}
