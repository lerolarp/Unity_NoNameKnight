using UnityEngine;

public enum Direction
{
    none,
    left,
    right
}

public class ForcedMoveData
{
    public Vector3 dir;
    public Vector3 endPos;
    public float speed;
}

public class DashMoveData
{
    public Vector3 endPos;
    public float speed;
}

public class MoveComponent : Component
{

    private Vector3 moveDir;

    private Vector3 endPos;
    private Vector3 validPos = new Vector3(-10000, -10000, -10000);
    private float speed;

    public Direction lookDir = Direction.none;

    private ForcedMoveData forcedMove;
    private CharacterHandler handle;

    public void Initialize(CharacterHandler handler)
    {
        this.handle = handler;
    }

    public void Update()
    {
        if (handle.Data.IsDead())
            return;

        if (forcedMove != null)
        {
            Vector3 calclulatePos = transform.position + (forcedMove.dir * forcedMove.speed * Time.deltaTime);

            if (forcedMove.dir == Vector3.zero)
            {
                forcedMove = null;
            }
            else
            {
                if (forcedMove.dir.x > 0 && calclulatePos.x > forcedMove.endPos.x || forcedMove.dir.x < 0 && calclulatePos.x < forcedMove.endPos.x)
                {
                    transform.position = forcedMove.endPos;
                    forcedMove = null;
                }
                else
                    transform.position = transform.position + (forcedMove.dir * forcedMove.speed * Time.deltaTime);
            }
        }
        else
        {
            if (speed > 0)
            {
                Vector3 deltaVector = moveDir * speed * Time.deltaTime;
                Vector3 arrivePos = transform.position + deltaVector;

                //도착위치가 있다면, 도착위치와 distance를 계산하여 길면 도착위치로 세팅한다.
                if(endPos != validPos)
                {
                    float arrivePosDist = (arrivePos - transform.position).sqrMagnitude;
                    float endPosDist = (endPos - transform.position).sqrMagnitude;

                    if (arrivePosDist > endPosDist)
                    {
                        transform.position = endPos;
                        endPos = validPos;
                        handle.Stop();
                    }
                    else
                        transform.position = arrivePos;
                }
                else
                    transform.position = arrivePos;

                if (this.lookDir != GetDirection())
                {
                    this.lookDir = GetDirection();
                    LookUp(this.lookDir);
                }
            }
        }
    }

    public void Move(Vector3 dir, float moveSpeed)
    {
        this.moveDir = dir.normalized;
        speed = moveSpeed;
    }

    public void Move(Vector3 dir, Vector3 endPos, float moveSpeed)
    {
        this.endPos = endPos;
        this.moveDir = (endPos - transform.position).normalized;
        speed = moveSpeed;
    }

    public void SetPos(Vector3 pos, Vector3 dir)
    {
        transform.position = pos;
        this.moveDir = dir.normalized;
    }

    public void KnockBack(Vector3 endPos, float speed)
    {
        forcedMove = new ForcedMoveData();
        forcedMove.dir = (endPos - transform.position).normalized;
        forcedMove.endPos = endPos;
        forcedMove.speed = speed;
    }

    public void Dash(Vector3 endPos, float speed)
    {
        forcedMove = new ForcedMoveData();
        forcedMove.dir = (endPos - transform.position).normalized;
        forcedMove.endPos = endPos;
        forcedMove.speed = speed;
    }

    public void Stop()
    {
        speed = 0;
        endPos = validPos;
        forcedMove = null;
    }

    public void LookUp(Direction lookDir)
    {
        if (transform.localScale.x < 0 && lookDir == Direction.right || transform.localScale.x > 0 && lookDir == Direction.left)
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);

        this.lookDir = lookDir;

        Stop();
    }

    public void LookUp(Vector3 lookUpVector)
    {
        if (lookUpVector.x > 0)
            LookUp(Direction.right);

        if (lookUpVector.x < 0)
            LookUp(Direction.left);
    }

    public Vector3 GetLookDir()
    {
        return lookDir == Direction.left ? Vector3.left : Vector3.right;
    }

    private Direction GetDirection()
    {
        if (moveDir.x > 0)
            return Direction.right;
        else if (moveDir.x < 0)
            return Direction.left;

        return lookDir;
    }
}
