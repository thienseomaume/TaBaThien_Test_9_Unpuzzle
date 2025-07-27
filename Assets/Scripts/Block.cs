using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using TMPro;

public class Block : MonoBehaviour
{
    [SerializeField] BlockType blockType;
    [SerializeField] int stepToUnlock;
    [SerializeField] GameObject blockBound;
    [SerializeField] TextMeshPro lockCountText;
    Tile tile;
    public float initialSpeed = 20f;
    public float acceleration = 20f;
    public float lifetime = 10f;
    public float overshootDistance = 0.3f;
    public float returnSpeed = 6f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject == gameObject && tile != null)
                {
                    Debug.Log("touched " + this.tile.coordinateInGrid.ToString());
                    tile.BlockTouched();
                }
            }
        }
    }
    void OnTouch()
    {

    }
    public BlockType GetBlockType()
    {
        return blockType;
    }
    public void SetLock(int stepLock)
    {
        stepToUnlock = stepLock;
        if (stepLock > 0)
        {
            blockBound?.SetActive(true);
            lockCountText.text = stepLock.ToString();
            LevelManager.Instance().collectPoint += DownStepUnlock;
        }
        else
        {
            blockBound?.SetActive(false);
        }
    }
    public void SetTile(Tile tile)
    {
        this.tile = tile;
        Debug.Log("block set tile to " + this.tile.coordinateInGrid.ToString());
    }
    public void ClearTile()
    {
        tile = null;
    }
    public Boolean IsLocked()
    {
        if (stepToUnlock > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void DownStepUnlock()
    {
        stepToUnlock -= 1;
        lockCountText.text = stepToUnlock.ToString();
        if (stepToUnlock <= 0)
        {
            blockBound?.SetActive(false);
            LevelManager.Instance().collectPoint -= DownStepUnlock;
        }
    }
    public void MoveDirection(Vector2 direction)
    {
        Debug.Log("move direction " + direction.ToString());
        StartCoroutine(MoveDirectionProcess(new Vector3(direction.x, direction.y, 0)));
    }
    public void MoveToPosition(Vector2 position)
    {
        Debug.Log("move to " + position.ToString());
        StartCoroutine(MoveToPositionProcess(new Vector3(position.x, position.y, 0)));
    }
    public void DestroyObject()
    {
        Destroy(gameObject);
    }
    void OnDestroy()
    {
        LevelManager.Instance().collectPoint -= DownStepUnlock;
    }
    private IEnumerator MoveDirectionProcess(Vector3 moveDirection)
    {
        float timer = 0f;
        float currentSpeed = initialSpeed;
        moveDirection.Normalize();
        while (timer < lifetime)
        {
            currentSpeed += acceleration * Time.deltaTime;
            transform.position += moveDirection * currentSpeed * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
    private IEnumerator MoveToPositionProcess(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - transform.position;
        float speed = initialSpeed;
        Vector3 overshootTarget = targetPosition + direction * overshootDistance;
        while (Vector3.Dot(direction, overshootTarget - transform.position) > 0.01f)
        {
            speed += acceleration * Time.deltaTime;
            transform.position += direction * speed * Time.deltaTime;
            yield return null;
        }
        SoundManager.Instance().PlayErrorSound();
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            Vector3 backDir = (targetPosition - transform.position).normalized;
            float step = returnSpeed * Time.deltaTime;
            if (step >= Vector3.Distance(transform.position, targetPosition))
            {
                transform.position = targetPosition;
                break;
            }
            transform.position += backDir * step;
            yield return null;
        }
        transform.position = targetPosition;
        Debug.Log("done");
    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("check interact");
        Block otherBlock = other.GetComponent<Block>();
        if (otherBlock != null)
        {
            if (otherBlock.GetBlockType() == BlockType.DestroyOther)
            {
                SoundManager.Instance().PlayBrokenSound();
                Destroy(gameObject);
            }
        }
    }
}
